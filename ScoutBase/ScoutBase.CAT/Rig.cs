using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ScoutBase.CAT
{
    public class Rig : CustomRig
    {

        protected RigParamSet ChangedParams = new RigParamSet();

        public Rig(int rignumber = 0)
        {
            // set rig number
            RigNumber = rignumber;
        }

        //------------------------------------------------------------------------------
        // Interpret reply
        //------------------------------------------------------------------------------

        private bool ValidateReply(byte[] data, BitMask mask)
        {
            bool result = false;

            if (mask == null)
                return true;
            if (data.Length != mask.Mask.Length)
                return false;
            if (data.Length != mask.Flags.Length)
                return false;

            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            string datastr = MyEncoding.GetString(data);
            string flagstr = MyEncoding.GetString(mask.Flags);

            result = ByteFuns.BytesEqual(ByteFuns.BytesAnd(data, mask.Mask), mask.Flags);
            return result;
        }

        protected override void ProcessInitReply(int number, byte[] data)
        {
            ValidateReply(data, RigCommands.InitCmd[number].Validation);
        }

        protected override void ProcessStatusReply(int number, byte[] data)
        {
            try
            {
                // validate reply
                RigCommand cmd = RigCommands.StatusCmd[number];
                if (!ValidateReply(data, cmd.Validation))
                {
                    throw new ArgumentException("Reply validation failed (" + ((RigCommands.CmdType == CommandType.ctBinary)? ByteFuns.BytesToHex(data) : "\"" + ByteFuns.BytesToStr(data) + "\"") + ")");
                }

                // extract numeric values
                for (int i = 0; i < cmd.Values.Count; i++)
                {
                    StoreParam(cmd.Values[i].Param, UnformatValue(data, cmd.Values[i]));
                }

                // extract bit flags
                for (int i = 0; i < cmd.Flags.Count; i++)
                {
                    if ((data.Length == cmd.Flags[i].Mask.Length) && (data.Length == cmd.Flags[i].Flags.Length))
                    {
                        if (ByteFuns.BytesEqual(ByteFuns.BytesAnd(data, cmd.Flags[i].Mask), cmd.Flags[i].Flags))
                        {
                            StoreParam(cmd.Flags[i].Param);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Incorrect reply length (" + ((RigCommands.CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(data) : "\"" + ByteFuns.BytesToStr(data) + "\"") + ")");
                    }
                }

                // tell clients
                if (ChangedParams.Count > 0)
                {
                    OmniRig.FireComNotifyParams(RigNumber, ChangedParams.Count);
                    ChangedParams.Clear();
                }
            }
            catch (Exception ex)
            {
                // log error
                OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " Error processing status reply: " + ex.Message));
            }
        }

        protected override void ProcessWriteReply(RigParam param, byte[] data)
        {
            ValidateReply(data, RigCommands.WriteCmd[param].Validation);
        }

        protected override void ProcessCustomReply(object sender, byte[] code, byte[] data)
        {
            // different implementation as there is no ActiveX object
            OmniRig.FireComNotifyCustom(RigNumber, this, code, data);
        }

        //------------------------------------------------------------------------------
        // Add command to queue
        //------------------------------------------------------------------------------

        protected override void AddWriteCommand(RigParam param, long value = 0)
        {
            // return on empty rig commands
            if (RigCommands == null)
                return;

            // generate cmd
            try
            {

                // is cmd supported?
                RigCommand cmd = RigCommands.WriteCmd[param];
                if (cmd.Code == null)
                {
                    throw new ArgumentException("Write command not supported for " + RigCommands.ParamToStr(param));
                }

                byte[] newcode = cmd.Code;
                byte[] fmtvalue = null;

                // command with value?
                if (cmd.Value.Format != ValueFormat.vfNone)
                {
                    // format value and handle exception
                    fmtvalue = FormatValue(value, cmd.Value);

                    // check available space 
                    if (cmd.Value.Start + cmd.Value.Len > newcode.Length)
                    {
                        throw new ArgumentException("Value too long (" + value.ToString() + ")");
                    }

                    // copy value to the right place of destination
                    Array.Copy(fmtvalue, 0, newcode, cmd.Value.Start, cmd.Value.Len);
                }

                //add command to queue
                lock (ComPort.FQueue)
                {
                    QueueItem item = new QueueItem();
                    item.Code = newcode;
                    item.Param = param;
                    item.Kind = CommandKind.ckWrite;
                    item.ReplyLength = cmd.ReplyLength;
                    item.ReplyEnd = ByteFuns.BytesToStr(cmd.ReplyEnd);
                    ComPort.FQueue.AddBeforeStatusCommands(item);
                }

                // log
                if (cmd.Value.Format != ValueFormat.vfNone)
                {
                    OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llInfo, "RIG" + RigNumber.ToString() + " Generating write command for " + RigCommands.ParamToStr(param) + " = " + value.ToString()));
                }
                else
                {
                    OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llInfo, "RIG" + RigNumber.ToString() + " Generating write command for " + RigCommands.ParamToStr(param) + " set"));
                }

            }
            catch (Exception ex)
            {
                OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " Error while generating write command: " + ex.Message));
            }
        }

        protected override void AddCustomCommand(object sender, byte[] code, int len, string end)
        {
            if (code == null)
                return;

            try
            {
                lock (ComPort.FQueue)
                {
                    QueueItem item = new QueueItem();
                    item.Code = code;
                    item.Kind = CommandKind.ckCustom;
                    item.CustSender = sender;
                    item.ReplyLength = len;
                    item.ReplyEnd = end;
                    ComPort.FQueue.Add(item);
                }

                // not implemented
                // PostMessage(MainForm.Handle, WM_TXQUEUE, RigNumber, 0);
            }
            catch (Exception ex)
            {
                OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "RIG" + RigNumber.ToString() + " Error while generating custom command: " + ex.Message));
            }
        }



        //------------------------------------------------------------------------------
        // Format
        //------------------------------------------------------------------------------

        private byte[] FormatValue(long value,  ParamValue info)
        {
            // scale value according to param info
            value = (long)Math.Round(value * info.Mult + info.Add);

            // create result array
            byte[] result = new byte[info.Len];

            // check for invalid values
            if (((info.Format == ValueFormat.vfBcdLU) || (info.Format == ValueFormat.vfBcdBU)) && (value < 0))
            {
                throw new ArgumentException("User passed invalid value (" + value.ToString() + ")");
            }

            // convert format
            switch (info.Format)
            {
                case ValueFormat.vfText: ToText(ref result, value); break;
                case ValueFormat.vfBinL: ToBinL(ref result, value); break;
                case ValueFormat.vfBinB: ToBinB(ref result, value); break;
                case ValueFormat.vfBcdLU: ToBcdLU(ref result, value); break;
                case ValueFormat.vfBcdLS: ToBcdLS(ref result, value); break;
                case ValueFormat.vfBcdBU: ToBcdBU(ref result, value); break;
                case ValueFormat.vfBcdBS: ToBcdBS(ref result, value); break;
                case ValueFormat.vfYaesu: ToYaesu(ref result, value); break;
                // Added by RA6UAZ for Icom Marine Radio NMEA Command
                case ValueFormat.vfDPIcom: ToDPIcom(ref result, value); break;
                case ValueFormat.vfTextUD: ToTextUD(ref result, value); break;
            }

            // return result
            return result;
        }

        // ASCII codes of digits
        private void ToText(ref byte[] arr, long value)
        {
            int len = arr.Length;
            string s = value.ToString();
            if (s.Length <= len)
            {
                s = s.PadLeft(len, '0');
            }
            else
            {
                throw new ArgumentException("Value is too long (" + value.ToString() + ")");
            }
            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            arr = MyEncoding.GetBytes(s);
        }

        private void ToTextUD(ref byte[] arr, long value)
        {
            int len = arr.Length;
            string s = Math.Abs(value).ToString();
            if (s.Length <= len - 1)
            {
                s = s.PadLeft(arr.Length, '0');
                if (value > 0)
                {
                    s = "U" + s.Remove(0, 1);
                }
                else
                {
                    s = "D" + s.Remove(0, 1);
                }
            }
            else
            {
                throw new ArgumentException("Value is too long (" + value.ToString() + ")");
            }
            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            arr = MyEncoding.GetBytes(s);
        }

        // Added by RA6UAZ for Icom Marine Radio NMEA Command
        private void ToDPIcom(ref byte[] arr, long value)
        {
            double f= value / 1000000.0;
            string s = f.ToString("F6", CultureInfo.InvariantCulture).PadLeft(arr.Length,'0');
            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            arr = MyEncoding.GetBytes(s);
        }

        // integer, little endian
        private void ToBinL(ref byte[] arr, long value)
        {
            // get length of array to switch between int and long
            int len = arr.Length;
            if (len < 8)
            {
                // assuming int
                // check bounds and cast value to integer
                if ((value <= int.MaxValue) && (value >= int.MinValue))
                {
                    int v = (int)value;
                    byte[] a = BitConverter.GetBytes(v);
                    Array.Copy(a, arr, len);
                }
                else
                {
                    throw new ArgumentException("Value is out of bounds (" + value.ToString() + ")");
                }
            }
            else
            {
                // assuming long or even longer
                byte[] a = BitConverter.GetBytes(value);
                Array.Copy(a, arr, len);
            }
        }

        // integer, big endian
        private void ToBinB(ref byte[] arr, long value)
        {
            arr = BitConverter.GetBytes(value);
            Array.Reverse(arr);
        }

        // BCD big endian unsigned
        private void ToBcdBU(ref byte[] arr, long value)
        {
            byte[] chars = new byte[arr.Length * 2];
            ToText(ref chars, value);
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (byte)(((chars[i * 2] - (byte)('0')) << 4) | (chars[i * 2 + 1] - (byte)('0')));
            }
        }

        // BCD little endian unsigned
        private void ToBcdLU(ref byte[] arr, long value)
        {
            ToBcdBU(ref arr, value);
            Array.Reverse(arr);
        }

        // BCD little endian signed; sign in high byte (00 or FF)
        private void ToBcdLS(ref byte[] arr, long value)
        {
            ToBcdLU(ref arr, Math.Abs(value));
            if (value < 0)
            {
                arr[arr.Length] = 0xFF;
            }
        }

        // BCD big endian signed
        private void ToBcdBS(ref byte[] arr, long value)
        {
            ToBcdBU(ref arr, Math.Abs(value));
            if (value < 0)
            {
                arr[0] = 0xFF;
            }

        }

        // 16 bits. high bit of the 1-st byte is sign,
        // the rest is integer, absolute value, big endian (not complementary!)
        private void ToYaesu(ref byte[] arr, long value)
        {
            ToBinB(ref arr, Math.Abs(value));
            if (value < 0)
            {
                arr[0] = (byte)(arr[0] | 0x80);
            }
        }

        //------------------------------------------------------------------------------
        // Unformat
        //------------------------------------------------------------------------------

        // bytes to value
        private long UnformatValue(byte[] data, ParamValue info)
        {
            // create result variable and temp arrray 
            long result = 0;
            byte[] value = new byte[info.Len];
            // copy over bytes from answer to temp array
            Array.Copy(data, info.Start, value, 0, info.Len);

            // check for successful copy
            if ((value == null) || (value.Length != info.Len))
            {
                throw new ArgumentException("Reply too short or malformatted(" + ((RigCommands.CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(data) : "\"" + ByteFuns.BytesToStr(data) + "\"") + ")");
            }

            // convert bytes to value
            switch (info.Format)
            {
                case ValueFormat.vfText: result = FromText(value); break;
                case ValueFormat.vfBinL: result = FromBinL(value); break;
                case ValueFormat.vfBinB: result = FromBinB(value); break;
                case ValueFormat.vfBcdLU: result = FromBcdLU(value); break;
                case ValueFormat.vfBcdLS: result = FromBcdLS(value); break;
                case ValueFormat.vfBcdBU: result = FromBcdBU(value); break;
                case ValueFormat.vfBcdBS: result = FromBcdBS(value); break;
                // Added by RA6UAZ for Icom Marine Radio NMEA Command
                case ValueFormat.vfDPIcom: result = FromDPIcom(value); break;
                case ValueFormat.vfYaesu: result = FromYaesu(value); break;
            }

            // scale value according to param info
            result = (long)Math.Round(result * info.Mult + info.Add);

            // return result
            return result;
        }

        private long FromText(byte[] data)
        {
            long result = 0;
            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            string s = MyEncoding.GetString(data);
            if (!long.TryParse(s, out result))
            {
                throw new ArgumentException("Invalid reply(" + ((RigCommands.CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(data) : "\"" + ByteFuns.BytesToStr(data) + "\"") + ")");
            }

            return result;
        }

        // Added by RA6UAZ for Icom Marine Radio NMEA Command
        private long FromDPIcom(byte[] data)
        {
            var MyEncoding = Encoding.GetEncoding("Windows-1252");
            string s = MyEncoding.GetString(data);
            // cut string at first non-numeric char
            for (int i = 0; i < s.Length; i++)
            { 
                if (!s[i].ToString().All("0123456789.".Contains))
                {
                    s = s.Substring(0, i-1);
                    break;
                }
            }
            double result = 0;
            double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
            result = 1000000.0 * result;
            return (long)Math.Round(result);
        }

        private long FromBinL(byte[] data)
        {
            byte[] b;

            //propagate sign if data is less than 8 bytes
            if ((data[data.Length - 1] & 0x80) > 0)
            {
                b = new byte[sizeof(long)] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }
            else
            {
                b = new byte[sizeof(long)] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80 };
            }

            //copy data
            Array.Copy(data, b, Math.Min(data.Length, sizeof(long)));

            return BitConverter.ToInt64(b, 0);

        }

        private long FromBinB(byte[] data)
        {
            Array.Reverse(data);
            return FromBinL(data);
        }

        private long FromBcdBU(byte[] data)
        {
            char[] chars = new char[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                chars[i * 2] = (char)((byte)('0') + ((data[i] >> 4) & 0x0F));
                chars[i * 2 + 1] = (char)((byte)('0') + (data[i] & 0x0F));
            }

            long result = 0;
            if (!long.TryParse(new string(chars), out result))
            {
                throw new FormatException("Invalid BCD value (" + ((RigCommands.CmdType == CommandType.ctBinary) ? ByteFuns.BytesToHex(data) : "\"" + ByteFuns.BytesToStr(data) + "\"") + ")");
            }

            return result;
        }

        private long FromBcdLU(byte[] data)
        {
            Array.Reverse(data);
            return FromBcdBU(data);
        }

        private long FromBcdBS(byte[] data)
        {
            int result = 0;
            if (data[0] == 0)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }
            data[0] = 0;
            return result * FromBcdBU(data);
        }

        private long FromBcdLS(byte[] data)
        {
            Array.Reverse(data);
            return FromBcdBS(data);
        }

        //16 bits. high bit of the 1-st byte is sign,
        //the rest is integer, absolute value, big endian (not complementary!)
        private long FromYaesu(byte[] data)
        {
            long result = 0;
            if ((data[0] & 0x80) == 0)
            {
                result = 1;
            }
            else
            {
                result = -1;
            }
            data[0] = (byte)(data[0] & 0x7F);
            return result * FromBinB(data);
        }

        //------------------------------------------------------------------------------
        // Store extracted param
        //------------------------------------------------------------------------------

        private void StoreParam(RigParam param)
        {
            RigParam p = RigParam.pmNone;
            if (RigParams.VfoParams.Contains(param))
                p = FVfo;
            else if (RigParams.SplitOnParams.Contains(param))
                p = FSplit;
            else if (RigParams.RitOnParams.Contains(param))
                p = FRit;
            else if (RigParams.XitOnParams.Contains(param))
                p = FXit;
            else if (RigParams.TxParams.Contains(param))
                p = FTx;
            else if (RigParams.ModeParams.Contains(param))
                p = FMode;
            else return;

            // return on no change
            if (p == param)
                return;

            // set param
            if (RigParams.VfoParams.Contains(param))
                FVfo = param;
            else if (RigParams.SplitOnParams.Contains(param))
                FSplit = param;
            else if (RigParams.RitOnParams.Contains(param))
                FRit = param;
            else if (RigParams.XitOnParams.Contains(param))
                FXit = param;
            else if (RigParams.TxParams.Contains(param))
                FTx = param;
            else if (RigParams.ModeParams.Contains(param))
                FMode = param;
            else return;

            // add parameter to set of changes
            ChangedParams.Add(p);

            // unsolved problem:
            // there is no command to read the mode of the other VFO,
            // its change goes undetected.
            if (RigParams.ModeParams.Contains(param) && (param != LastWrittenMode))
            {
                LastWrittenMode = RigParam.pmNone;
            }

            // log
            OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llInfo, "RIG" + RigNumber.ToString() + " Param changed: " + RigCommands.ParamToStr(param) + " set"));

        }

        private void StoreParam(RigParam param, long value)
        {
            long v = 0;
            switch (param)
            {
                case RigParam.pmFreqA: v = FFreqA; break;
                case RigParam.pmFreqB: v = FFreqB; break;
                case RigParam.pmFreq: v = FFreq; break;
                case RigParam.pmPitch: v = FPitch; break;
                case RigParam.pmRitOffset: v = FRitOffset; break;
                default:
                    return;
            }

            // return on no change
            if (v == value)
                return;
            
            // set value
            switch (param)
            {
                case RigParam.pmFreqA: FFreqA = value; break;
                case RigParam.pmFreqB: FFreqB = value; break;
                case RigParam.pmFreq: FFreq = value; break;
                case RigParam.pmPitch: FPitch = value; break;
                case RigParam.pmRitOffset: FRitOffset = value; break;
                default:
                    return;
            }

            // add parameter to set of changes
            ChangedParams.Add(param);

            // log
            OmniRig.Log(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llInfo, "RIG" + RigNumber.ToString() + " param changed: " + RigCommands.ParamToStr(param) + " = " + value.ToString()));
        }


        public void SendCustomCommand(dynamic command, int replylength, dynamic replyend)
        {
            byte[] cmd = new byte[0];
            byte[] t = new byte[0];
            string trm = "";

            if (command.GetType() == typeof(byte))
            {
                cmd = new byte[1];
                cmd[0] = (byte)command;
            }
            else if (command.GetType() == typeof(byte[]))
            {
                cmd = (byte[])command;
            }
            else if (command.GetType() == typeof(string))
            {
                cmd = ByteFuns.StrToBytes((string)command);
            }

            if (replyend.GetType() == typeof(byte))
            {
                t = new byte[1];
                t[0] = (byte)replyend;
                trm = ByteFuns.BytesToStr(t);
            }
            else if (replyend.GetType() == typeof(byte[]))
            {
                trm = ByteFuns.BytesToStr(replyend);
            }
            else if (replyend.GetType() == typeof(string))
            {
                trm = replyend;
            }

            AddCustomCommand(this, cmd, replyend, trm);
        }
    }
}
