using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using IniParser;
using IniParser.Model;

namespace ScoutBase.CAT
{
    public enum CommandType
    {
        ctNone = 0,
        ctASCII = 1,
        ctBinary = 2
    }

    public class RigCommands
    {
        // public properties
        public string RigType { get; internal set; } = "";
        public CommandType CmdType = CommandType.ctNone;
        public bool WasError
        {
            get
            {
                return ((ErrorList != null) && ErrorList.Count > 0);
            }
        }

        public List<RigCommand> InitCmd = new List<RigCommand>();
        public Dictionary<RigParam, RigCommand> WriteCmd = new Dictionary<RigParam, RigCommand>();
        public List<RigCommand> StatusCmd = new List<RigCommand>();

        public RigParamSet ReadableParams = new RigParamSet();
        public RigParamSet WriteableParams = new RigParamSet();

        // private 
        private List<string> ErrorList = new List<string>();

        public RigCommands()
        {
            RigType = "";
        }

        // --------------------------------------------------------------------
        // Logging
        // --------------------------------------------------------------------

        private void Log(string msg, string section = "", string entry = "", string value = "", bool showvalue = false)
        {
            if (showvalue && !String.IsNullOrEmpty(section) && !String.IsNullOrEmpty(entry))
            {
                value = " in " + value;
            }

            // only console output is yet implemented
            msg = "[" + section + "]." + entry + ": " + msg + " " + value;
            Console.WriteLine(msg);
            ErrorList.Add(msg);
        }

        //------------------------------------------------------------------------------
        // Clear record
        //------------------------------------------------------------------------------

        public void Clear(ref RigCommand rec)
        {
            rec.Code = null;
            Clear(ref rec.Value);
            rec.ReplyLength = 0;
            rec.ReplyEnd = null;
            Clear(ref rec.Validation);
            rec.Values = null;
            rec.Flags = null;
        }

        public void Clear(ref ParamValue rec)
        {
            rec.Start = 0;
            rec.Len = 0;
            rec.Format = ValueFormat.vfNone;
            rec.Mult = 0;
            rec.Add = 0;
            rec.Param = RigParam.pmNone;
        }

        public void Clear(ref BitMask rec)
        {
            rec.Mask = null;
            rec.Flags = null;
            rec.Param = RigParam.pmNone;
        }


        //------------------------------------------------------------------------------
        // Create
        //------------------------------------------------------------------------------

        public static RigCommands FromIni(string filename)
        {
            RigCommands commands = new RigCommands();

            if (!File.Exists(filename))
                return commands;

            // set rig type
            commands.RigType = Path.GetFileNameWithoutExtension(filename);

            // read ini
            IniData inidata = new FileIniDataParser().ReadFile(filename);

            // iterate throug all sections and append command, if valid
            foreach (SectionData section in inidata.Sections)
            {
                if (section.SectionName.ToUpper().StartsWith("INIT"))
                {
                    commands.LoadInitCmd(section);
                }
                else if (section.SectionName.ToUpper().StartsWith("STATUS"))
                {
                    commands.LoadStatusCmd(section);
                }
                else
                {
                    commands.LoadWriteCmd(section);
                }
            }

            // list supported params
            commands.ListSupportedParams();

            // return result
            return commands;
        }

        //------------------------------------------------------------------------------
        // Load
        //------------------------------------------------------------------------------

        //Value=5|5|vfBcdL|1|0[|pmXXX]
        private ParamValue LoadValue(SectionData section, string entry)
        {
            ParamValue result = new ParamValue();

            string value = "";
            string[] a;

            try
            {
                value = section.Keys[entry];
                if (String.IsNullOrEmpty(value))
                    return result;

                a = value.Split('|');
            }
            catch
            {
                throw new ArgumentException("Invalid parameter >" + value + "<");
            }

            switch (a.Length)
            {
                case 0:
                    return result;
                case 5:
                    break;
                case 6:
                    try
                    {
                        result.Param = StrToParam(a[5]);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                    break;
                default:
                    throw new ArgumentException("Invalid syntax >" + value + "<");
            }

            try
            {
                result.Start = int.Parse(a[0]);
            }
            catch
            {
                throw new ArgumentException("Invalid integer >" + value + "<");
            }

            try
            {
                result.Len = int.Parse(a[1]);
            }
            catch
            {
                throw new ArgumentException("Invalid integer >" + value + "<");
            }

            try
            {
                result.Format = StrToFmt(a[2]);
            }
            catch
            {
                throw new ArgumentException("Invalid format string >" + value + "<");
            }

            try
            {
                result.Mult = double.Parse(a[3], CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException("Invalid Multiplier value >" + value + "<");
            }

            try
            {
                result.Add = double.Parse(a[4], CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new ArgumentException("Invalid Add value >" + value + "<");
            }

            return result;
        }

        private RigCommand LoadCommon(SectionData section)
        {
            RigCommand command = new RigCommand();
            string value = "";
            string entry = "Command";
            try
            {
                value = section.Keys[entry];
                if (!String.IsNullOrEmpty(value))
                {
                    command.Code = StrToBytes(value);
                }
            }
            catch
            {
                Log("Invalid byte string", section.SectionName, entry, value);
                throw new ArgumentException();
            }

            entry = "ReplyLength";
            try
            {
                value = section.Keys[entry];
                if (!String.IsNullOrEmpty(value))
                {
                    command.ReplyLength = int.Parse(value);
                }
            }
            catch
            {
                Log("Invalid integer value", section.SectionName, entry, value);
                throw new ArgumentException();
            }

            entry = "ReplyEnd";
            try
            {
                value = section.Keys[entry];
                if (!String.IsNullOrEmpty(value))
                {
                    command.ReplyEnd = StrToBytes(value);
                }
            }
            catch
            {
                Log("Invalid byte string", section.SectionName, entry, value);
                throw new ArgumentException();
            }

            entry = "Validate";
            try
            {
                value = section.Keys[entry];
                if (!String.IsNullOrEmpty(value))
                {
                    command.Validation = StrToMask(value);
                }
            }
            catch
            {
                Log("Invalid byte string", section.SectionName, entry, value);
                throw new ArgumentException();
            }

            return command;
        }

        private void LoadInitCmd(SectionData section)
        {
            // empty section
            if ((section == null) || (section.Keys.Count == 0))
                return;

            RigCommand command = new RigCommand();

            // validate all entries
            // log but continue on errors
            try
            {
                ValidateEntryNames(section, new string[] { "COMMAND", "REPLYLENGTH", "REPLYEND", "VALIDATE" });
            }
            catch (Exception ex)
            {
                Log(ex.Message, section.SectionName);
            }

            // load command
            // stop on errors
            try
            {
                command = LoadCommon(section);

                // check if values are set --> not allowed in INIT
                if ((command.Value != null) && (command.Value.Format != ValueFormat.vfNone))
                {
                    Log("Value is not allowed in INIT", section.SectionName);
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                // do nothing but return on error
                return;
            }

            // add command
            InitCmd.Add(command);
        }


        private void LoadStatusCmd(SectionData section)
        {
            // empty section
            if ((section == null) || (section.Keys.Count == 0))
                return;

            RigCommand command = new RigCommand();

            // validate all entries
            // log but continue on errors
            try
            {
                ValidateEntryNames(section, new string[] { "COMMAND", "REPLYLENGTH", "REPLYEND", "VALIDATE", "VALUE*", "FLAG*" });
            }
            catch (Exception ex)
            {
                Log(ex.Message, section.SectionName);
            }

            // load command
            // stop on errors
            try
            {
                command = LoadCommon(section);

                // check if reply length or reply end is set
                if ((command.ReplyLength == 0) && (command.ReplyEnd == null))
                {
                    Log("Either ReplyLength or ReplyEnd must be specified", section.SectionName);
                    throw new ArgumentException();
                }

                // iterate through entries and get values
                foreach (KeyData key in section.Keys)
                {
                    string entry = key.KeyName;
                    string value = key.Value;
                    if (entry.ToUpper().StartsWith("VALUE"))
                    {
                        try
                        {
                            ParamValue paramvalue = LoadValue(section,entry);
                            ValidateValue(paramvalue, Math.Max(command.ReplyLength, command.Validation.Mask.Length));
                            // report error but continue
                            if (paramvalue.Param == RigParam.pmNone)
                            {
                                Log("Parameter name is missing >" + value + "<", section.SectionName, entry);
                            }
                            if (!RigParams.NumericParams.Contains(paramvalue.Param))
                            {
                                Log("Parameter must be of numeric type <" + value + "<", section.SectionName, entry);
                            }

                            // add value
                            if (command.Values == null)
                            {
                                command.Values = new List<ParamValue>();
                            }
                            command.Values.Add(paramvalue);
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message,section.SectionName, entry);
                            throw new ArgumentException();
                        }
                    }
                    else if (entry.ToUpper().StartsWith("FLAG"))
                    {
                        try
                        {
                            BitMask flag = StrToMask(value);
                            ValidateMask(entry, flag, command.ReplyLength, command.ReplyEnd);

                            // add value
                            if (command.Flags == null)
                            {
                                command.Flags = new List<BitMask>();
                            }
                            command.Flags.Add(flag);
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message, section.SectionName, entry);
                            throw new ArgumentException();
                        }
                    }
                }

                if ((command.Values == null) && (command.Flags == null))
                {
                    Log("At least one ValueNN or FlagNN must be defined", section.SectionName);
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                // do nothing but return on error
                return;
            }

            // add command
            StatusCmd.Add(command);
        }

        private void LoadWriteCmd(SectionData section)
        {
            // empty section
            if ((section == null) || (section.Keys.Count == 0))
                return;

            bool error = false;

            RigCommand command = new RigCommand();
            RigParam rigparam = RigParam.pmNone;

            try
            {
                rigparam = StrToParam(section.SectionName);
            }
            catch (Exception ex)
            {
                Log(ex.Message, section.SectionName);
                throw new ArgumentException();
            }

            // validate all entries
            // log but continue on errors
            try
            {
                ValidateEntryNames(section, new string[] { "COMMAND", "REPLYLENGTH", "REPLYEND", "VALIDATE", "VALUE" });
            }
            catch (Exception ex)
            {
                Log(ex.Message, section.SectionName);
            }

            try
            {

                command = LoadCommon(section);

                string entry = "Value";
                try
                {
                    command.Value = LoadValue(section, entry);
                    ValidateValue(command.Value, command.Code.Length);

                    // verify
                    if (command.Value.Param != RigParam.pmNone)
                    {
                        Log("Parameter name is not allowed.", section.SectionName, entry);
                        error = true;
                    }
                    if (RigParams.NumericParams.Contains(rigparam) && (command.Value.Len == 0))
                    {
                        Log("Value is missing.", section.SectionName, entry);
                        error = true;
                    }
                    if (!RigParams.NumericParams.Contains(rigparam) && (command.Value.Len > 0))
                    {
                        Log("Parameter does not require a value.", section.SectionName, entry);
                        error = true;
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message, section.SectionName);
                    throw new ArgumentException();
                }

            }
            catch (Exception ex)
            {
                // do nothing but return on error
                return;
            }

            // add/update command if valid
            if (!error)
            {
                WriteCmd[rigparam] = command;
            }
        }

        //------------------------------------------------------------------------------
        // Conversion functions
        //------------------------------------------------------------------------------

        private ValueFormat StrToFmt(string s)
        {
            ValueFormat result = ValueFormat.vfNone;
            if (!Enum.TryParse<ValueFormat>(s, out result))
            {
                throw new ArgumentException("Invalid format name: >" + s + "<");
            }
            return result;
        }

        private RigParam StrToParam(string s)
        {
            RigParam result = RigParam.pmNone;
            if (!Enum.TryParse<RigParam>(s, out result))
            {
                throw new ArgumentException("Invalid parameter name: >" + s + "<");
            }
            return result;
        }

        public string ParamToStr(RigParam param)
        {
            return param.ToString();
        }

        private byte[] StrToBytes(string s)
        {
            // blank
            s = s.Trim();
            if (s.Length < 2)
            {
                return null;
            }

            // ASCII
            if (s.StartsWith("("))
            {
                if (s.EndsWith(")"))
                {
                    // set command type
                    CmdType = CommandType.ctASCII;
                    var MyEncoding = Encoding.GetEncoding("Windows-1252");
                    return MyEncoding.GetBytes(s.Substring(1, s.Length - 2));
                }
                else
                {
                    throw new ArgumentException("Invalid ASCII parameter >" + s + "<");
                }
            }

            // Binary (Hex)
            if (s[1].ToString().All("0123456789abcdefABCDEF".Contains))
            {
                // set command type
                CmdType = CommandType.ctBinary;

                // remove all dots
                s = s.Replace(".", "");
                if (s.Length % 2 > 0)
                {
                    throw new ArgumentException("Invalid HEX parameter >" + s + "<");
                }
                int len = s.Length / 2;
                byte[] result = new byte[len];
                for (int i = 0; i < len; i++)
                {
                    result[i] = byte.Parse(s.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                }
                return result;
            }

            // string is not valid
            throw new ArgumentException("Invalid string parameter >" + s + "<");
        }

        private byte[] FlagsFromMask(byte[] a, char c)
        {
            byte[] result = new byte[a.Length];
            Array.Copy(a, result, a.Length);
            if (c == '(')
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] == (byte)'.')
                    {
                        a[i] = 0;
                        result[i] = 0;
                    }
                    else
                    {
                        a[i] = 0xFF;
                    }
                }
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] != 0)
                    {
                        a[i] = 0xFF;
                    }
                }
            }

            return result;
        }

        //Flag1 =".......................0.............."|pmRitOff
        //Flag1 =13.00.00.00.00.00.00.00|00.00.00.00.00.00.00.00|pmVfoAA
        //Validation=FEFEE05EFBFD
        //Validation=FFFFFFFF.FF.0000000000.FF|FEFEE05E.03.0000000000.FD

        private BitMask StrToMask(string s)
        {
            BitMask result = new BitMask();

            if (String.IsNullOrEmpty(s))
                return result;

            // split the string
            string[] a = s.Split('|');

            if ((a.Length < 1) || (a.Length > 3))
                throw new ArgumentException("Invalid numer of parameters >" + s + "<");

            result.Mask = StrToBytes(a[0]);
            if (result.Mask == null)
            {
                throw new ArgumentException("Invalid mask >" + s + "<");
            }

            switch (a.Length)
            {
                case 1:
                    // just mask, infer flags
                    result.Flags = FlagsFromMask(result.Mask, a[0][0]);
                    break;
                case 2:
                    // mask|param or mask|flags
                    try
                    {
                        result.Param = StrToParam(a[1]);
                        if (result.Param != RigParam.pmNone)
                        {
                            result.Flags = FlagsFromMask(result.Mask, a[0][0]);
                        }
                    }
                    catch
                    {
                        result.Flags = StrToBytes(a[1]);
                    }
                    break;
                case 3:
                    // mask|flags|param
                    result.Flags = StrToBytes(a[1]);
                    result.Param = StrToParam(a[2]);
                    break;
            }

            return result;
        }

        //------------------------------------------------------------------------------
        // Validation
        //------------------------------------------------------------------------------

        private void ValidateMask(string entry, BitMask mask, int len, byte[] end)
        {
            if ((mask.Mask == null) && (mask.Flags == null) && (mask.Param == RigParam.pmNone))
            {
                return;
            }

            if ((mask.Mask == null) || (mask.Flags == null))
            {
                throw new ArgumentException("Incorrect mask length (" + ((CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(mask.Mask) : "\"" + ByteFuns.BytesToStr(mask.Mask) + "\"") + ")");
            }
            else if (mask.Mask.Length != mask.Flags.Length)
            {
                throw new ArgumentException("Incorrect mask length (" + ((CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(mask.Mask) : "\"" + ByteFuns.BytesToStr(mask.Mask) + "\"") + ")");
            }
            else if ((len > 0) && (mask.Mask.Length != len))
            {
                throw new ArgumentException("Mask length (" + mask.Mask.Length.ToString() + ") <> ReplyLength (" + mask.Flags.Length.ToString() + ")");
            }
            else if (!ByteFuns.BytesEqual(ByteFuns.BytesAnd(mask.Flags, mask.Flags), mask.Flags))
            {
                throw new ArgumentException("Mask hides valid bits (" + 
                    ((CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(mask.Mask) : "\"" + ByteFuns.BytesToStr(mask.Mask) + "\"") + ")" + 
                    " & "
                     + ((CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(mask.Flags) : "\"" + ByteFuns.BytesToStr(mask.Flags) + "\"") + ")");
            }
            //syntax is different for validation masks and flag masks   
            else if (entry.ToUpper() == "VALIDATE")
            {
                if (mask.Param != RigParam.pmNone)
                {
                    throw new ArgumentException("Parameter name is not allowed >" + ParamToStr(mask.Param) + "<");
                }
                byte[] ending = new byte[mask.Flags.Length - len];
                Array.Copy(mask.Flags, mask.Flags.Length - len, ending, 0, len);
                if (!ByteFuns.BytesEqual(ending, end))
                {
                    throw new ArgumentException("Mask does not end with ReplyEnd " +
                         ((CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(mask.Mask) : "\"" + ByteFuns.BytesToStr(mask.Mask) + "\"") + ")" + 
                         " <> " +
                         ((CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(end) : "\"" + ByteFuns.BytesToStr(end) + "\"") + ")");
                }
            }
            else
            {
                if (mask.Param == RigParam.pmNone)
                {
                    throw new ArgumentException("Parameter name is missing");
                }
                if (mask.Mask == null)
                {
                    throw new ArgumentException("Mask is blank");
                }
            }
        }

        private void ValidateValue(ParamValue paramvalue, int len)
        {
            // empty parameter
            if (paramvalue.Param == RigParam.pmNone)
                return;
            // set length to max when zero
            if (len == 0)
                len = int.MaxValue;

            if ((paramvalue.Start < 0) || (paramvalue.Start > len))
            {
                throw new ArgumentException("Invalid Start value" + paramvalue.Start.ToString());
            }
            if ((paramvalue.Len < 0) || (paramvalue.Start + paramvalue.Len > len))
            {
                throw new ArgumentException("Invalid Length value" + paramvalue.Len.ToString());
            }
            if (paramvalue.Mult <= 0)
            {
                throw new ArgumentException("Invalid Multiplier value" + paramvalue.Mult.ToString("F8"));
            }
        }

        private void ValidateEntryNames(SectionData section, string[] validnames)
        {
            // empty section or validnames
            if ((section == null) || (section.Keys.Count == 0) || (validnames == null) || (validnames.Length == 0))
                return;

            foreach (KeyData key in section.Keys)
            {
                bool ok = false;
                foreach (string validname in validnames)
                {
                    // make strings upper and cut strings in case of '*'
                    string s1 = key.KeyName.ToUpper();
                    string s2 = validname.ToUpper();
                    if (s2.EndsWith("*"))
                    {
                        s2 = s2.Substring(0, s2.Length - 1);
                        s1 = s1.Substring(0, s2.Length);
                    }
                    if (s1 == s2)
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                {
                    throw new ArgumentException("Invalid entry name >" + key.KeyName + "<");
                }
            }
        }

        //------------------------------------------------------------------------------
        // Supported params
        //------------------------------------------------------------------------------

        private void ListSupportedParams()
        {
            ReadableParams = new RigParamSet();
            WriteableParams = new RigParamSet();

            foreach (RigCommand cmd in StatusCmd)
            {
                if (cmd.Values != null)
                {
                    foreach (ParamValue value in cmd.Values)
                    {
                        ReadableParams.Add(value.Param);
                    }
                }
                if (cmd.Flags != null)
                {
                    foreach (BitMask flags in cmd.Flags)
                    {
                        ReadableParams.Add(flags.Param);
                    }
                }
            }

            if ((WriteCmd != null) && (WriteCmd.Keys != null))
            {
                foreach (RigParam p in WriteCmd.Keys)
                {
                    WriteableParams.Add(p);
                }
            }
        }

        private string ParamListToString(RigParamSet Params)
        {
            string result = "";
            foreach (RigParam p in Params)
            {
                if (String.IsNullOrEmpty(result))
                {
                    result = p.ToString();
                }
                else
                {
                    result = result + "," + p.ToString();
                }
            }

            return result;
        }

        private int ParamsToInt(RigParamSet Params)
        {
            int result = 0;

            foreach (RigParam p in Params)
            {
                result = result | 1 << (int)p;
            }

            return result;

        }
    }


}
