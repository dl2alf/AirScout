using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ScoutBase.Core;
using ScoutBase.CAT;

namespace CATCheck
{
    public partial class MainDlg : Form
    {

        private bool Loaded = false;
        private bool Logging = true;

        private RigParam LastMode = RigParam.pmNone;
        private RigParam LastTx = RigParam.pmNone;
        private RigParam LastVfo = RigParam.pmNone;
        private RigParam LastSplit = RigParam.pmNone;
        private RigParam LastXit = RigParam.pmNone;
        private RigParam LastRit = RigParam.pmNone;

        private long LastRitOffset = 0;

        Rig Rig1 = null;

        private Queue<LogNotifyEventArgs> MsgQueue = new Queue<LogNotifyEventArgs>();
 
        public MainDlg()
        {
            InitializeComponent();

            this.Text = this.Text.Replace("xxx", Application.ProductVersion);
            // set OmniRig rig settings directory
            if (String.IsNullOrEmpty(Properties.Settings.Default.RigDefinitionsFolder))
            {
                Properties.Settings.Default.RigDefinitionsFolder = Path.Combine(Application.StartupPath, "Rigs");
            }
            OmniRig.RigDefinitionsDirectory = Properties.Settings.Default.RigDefinitionsFolder;

            // start timer
            ti_Main.Start();

            // subscribe to events
            OmniRig.LogNotify += OmniRig_LogNotify;
            OmniRig.StatusChange += OmniRig_ComNotifyStatus;
            OmniRig.ParamsChange += OmniRig_ComNotifyParams;

        }

        private void MainDlg_Load(object sender, EventArgs e)
        {

            // load user settings
            LoadUserSettings();

            // set the "Loaded" flag and start rig
            Loaded = true;

            Rig1 = new Rig();
            RestartRig();
        }


        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserSettings();
        }

        #region User Settings

        private void LoadUserSettings()
        {

            Console.WriteLine("Loading user settings.");


            // generate empty RigSettings object if null
            if (Properties.Settings.Default.RigSettings == null)
            {
                Console.WriteLine("Creating empty rig settings...");
                Properties.Settings.Default.RigSettings = new RigSettings();
            }

            // initially fill dropdowns
            cb_Rig_DropDown(this, null);
            cb_PortName_DropDown(this, null);
            cb_Baudrate_DropDown(this, null);
            cb_Databits_DropDown(this, null);
            cb_Parity_DropDown(this, null);
            cb_Stopbits_DropDown(this, null);
            cb_RTS_DropDown(this, null);
            cb_DTR_DropDown(this, null);

            // try to select settings
            try
            {
                cb_Rig.DataSource = OmniRig.SupportedRigs();
                cb_Rig.SelectedItem = Properties.Settings.Default.RigSettings.RigType;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up rig dropdown: " + ex.Message));
            }
            try
            {
                cb_PortName.SelectedItem = Properties.Settings.Default.RigSettings.PortName;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up port dropdown: " + ex.Message));
            }
            try
            {
                cb_Baudrate.SelectedItem = Properties.Settings.Default.RigSettings.Baudrate.ToString();
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up baudrate dropdown: " + ex.Message));
            }
            try
            {
                cb_Databits.SelectedItem = Properties.Settings.Default.RigSettings.DataBits.ToString();
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up databits dropdown: " + ex.Message));
            }
            try
            {
                Helpers.BindToEnum<Parity>(cb_Parity);
                cb_Parity.SelectedValue = (int)Properties.Settings.Default.RigSettings.Parity;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up parity dropdown: " + ex.Message));
            }
            try
            {
                Helpers.BindToEnum<StopBits>(cb_Stopbits);
                cb_Stopbits.SelectedValue = (int)Properties.Settings.Default.RigSettings.StopBits;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up stopbits dropdown: " + ex.Message));
            }
            try
            {
                Helpers.BindToEnum<FlowControl>(cb_RTS);
                cb_RTS.SelectedValue = (int)Properties.Settings.Default.RigSettings.RtsMode;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up rts mode dropdown: " + ex.Message));
            }
            try
            {
                Helpers.BindToEnum<FlowControl>(cb_DTR);
                cb_DTR.SelectedValue = (int)Properties.Settings.Default.RigSettings.DtrMode;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up dtr mode dropdown: " + ex.Message));
            }
            try
            {
                Helpers.BindToEnum<LOGLEVEL>(cb_LogVerbosity);
                cb_LogVerbosity.SelectedValue = (int)Properties.Settings.Default.LogVerbosity;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up log verbosity dropdown: " + ex.Message));
            }
            try
            {
                ud_PollMs.Value = Properties.Settings.Default.RigSettings.PollMs;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up PollMs updown: " + ex.Message));
            }
            try
            {
                ud_TimeoutMs.Value = Properties.Settings.Default.RigSettings.TimeoutMs;
            }
            catch (Exception ex)
            {
                // do nothing if fails
                OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Error while setting up PollMs updown: " + ex.Message));
            }

            Console.WriteLine("Loading user settings finished.");

        }

        private string GetUserSettingsPath()
        {
            if (!SupportFunctions.IsMono)
                return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;
            // try to build a path to user specific settings under Linux/Mono
            string usersettingspath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            usersettingspath = Path.Combine(usersettingspath, Application.CompanyName, AppDomain.CurrentDomain.FriendlyName);
            usersettingspath += "_Url_";
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }
            byte[] pkt = assembly.GetName().GetPublicKeyToken();
            byte[] hash = SHA1.Create().ComputeHash((pkt != null && pkt.Length > 0) ? pkt : Encoding.UTF8.GetBytes(assembly.EscapedCodeBase));
            StringBuilder evidence_string = new StringBuilder();
            byte[] array = hash;
            for (int i = 0; i < array.Length; i++)
            {
                byte b = array[i];
                evidence_string.AppendFormat("{0:x2}", b);
            }
            usersettingspath += evidence_string.ToString();
            if (!Directory.Exists(usersettingspath))
            {
                Directory.CreateDirectory(usersettingspath);
            }
            usersettingspath = Path.Combine(usersettingspath, "user.config");
            return usersettingspath;
        }

        private XmlElement CreateUserSection(XmlDocument doc, SettingsBase settings)
        {
            XmlElement usersection = doc.CreateElement(string.Empty, "section", string.Empty);
            XmlAttribute sectionname = doc.CreateAttribute(string.Empty, "name", string.Empty);
            sectionname.Value = settings.GetType().FullName;
            usersection.Attributes.Append(sectionname);
            XmlAttribute sectiontype = doc.CreateAttribute(string.Empty, "type", string.Empty);
            Assembly assembly = Assembly.GetAssembly(typeof(System.Configuration.ClientSettingsSection));
            //            sectiontype.Value = "System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            sectiontype.Value = typeof(System.Configuration.ClientSettingsSection).FullName + ", " + assembly.FullName;
            usersection.Attributes.Append(sectiontype);
            XmlAttribute sectionallowexedefinition = doc.CreateAttribute(string.Empty, "allowExeDefinition", string.Empty);
            sectionallowexedefinition.Value = "MachineToLocalUser";
            usersection.Attributes.Append(sectionallowexedefinition);
            XmlAttribute sectionrequirepermission = doc.CreateAttribute(string.Empty, "requirePermission", string.Empty);
            sectionrequirepermission.Value = "false";
            usersection.Attributes.Append(sectionrequirepermission);
            return usersection;
        }

        private XmlElement SerializeSettings(XmlDocument doc, SettingsBase settings)
        {
            XmlElement properties = doc.CreateElement(string.Empty, settings.ToString(), string.Empty);
            foreach (SettingsPropertyValue p in settings.PropertyValues)
            {
                if ((p != null) && (p.Name != null) && (p.PropertyValue != null) && !p.UsingDefaultValue)
                {
                    //                    Console.WriteLine("Appending " + p.Name + " = " + p.PropertyValue.ToString());
                    XmlElement setting = doc.CreateElement(string.Empty, "setting", string.Empty);
                    XmlAttribute name = doc.CreateAttribute(string.Empty, "name", string.Empty);
                    name.Value = p.Name.ToString();
                    setting.Attributes.Append(name);
                    XmlAttribute serializeas = doc.CreateAttribute(string.Empty, "serializeAs", string.Empty);
                    serializeas.Value = p.Property.SerializeAs.ToString();
                    setting.Attributes.Append(serializeas);
                    XmlElement value = doc.CreateElement(string.Empty, "value", string.Empty);
                    if (p.PropertyValue != null && p.Property.SerializeAs == SettingsSerializeAs.String)
                    {
                        XmlText text = doc.CreateTextNode(p.SerializedValue.ToString());
                        value.AppendChild(text);
                    }
                    else
                    {
                        if (p.PropertyValue != null && p.Property.SerializeAs == SettingsSerializeAs.Xml)
                        {
                            MemoryStream ms = new MemoryStream();
                            XmlWriter writer = XmlWriter.Create(ms, new XmlWriterSettings
                            {
                                NewLineOnAttributes = true,
                                OmitXmlDeclaration = true
                            });
                            XmlSerializer serializer = new XmlSerializer(p.PropertyValue.GetType());
                            serializer.Serialize(writer, p.PropertyValue);
                            byte[] text2 = new byte[ms.ToArray().Length - 3];
                            Array.Copy(ms.ToArray(), 3, text2, 0, text2.Length);
                            XmlText xml = doc.CreateTextNode(Encoding.UTF8.GetString(text2.ToArray<byte>()));
                            value.AppendChild(xml);
                            value.InnerXml = WebUtility.HtmlDecode(value.InnerXml);
                        }
                    }
                    setting.AppendChild(value);
                    properties.AppendChild(setting);
                }
            }
            return properties;
        }


        private void SaveUserSettings()
        {
            try
            {
                if (!SupportFunctions.IsMono)
                {
                    Properties.Settings.Default.Save();
                    return;
                }

                // Linux/Mono hack to save all properties in a correct manner
                Console.WriteLine("Saving user settings...");
                Console.WriteLine("Creating XML document...");
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);
                XmlElement configuration = doc.CreateElement(string.Empty, "configuration", string.Empty);
                doc.AppendChild(configuration);
                XmlElement configsections = doc.CreateElement(string.Empty, "configSections", string.Empty);
                configuration.AppendChild(configsections);
                XmlElement usersettingsgroup = doc.CreateElement(string.Empty, "sectionGroup", string.Empty);
                XmlAttribute usersettingsname = doc.CreateAttribute(string.Empty, "name", string.Empty);
                usersettingsname.Value = "userSettings";
                usersettingsgroup.Attributes.Append(usersettingsname);
                usersettingsgroup.AppendChild(CreateUserSection(doc, ScoutBase.CAT.Properties.Settings.Default));
                configsections.AppendChild(usersettingsgroup);
                XmlElement usersettings = doc.CreateElement(string.Empty, "userSettings", string.Empty);
                configuration.AppendChild(usersettings);
                Console.WriteLine("Writing user settings...");
                // append AirScout.CAT properties
                Console.WriteLine("Appending AirScout.CAT.Settings.Default node...");
                usersettings.AppendChild(SerializeSettings(doc, ScoutBase.CAT.Properties.Settings.Default));
                // append properties
                Console.WriteLine("Appending Properties.Settings.Default node...");
                usersettings.AppendChild(SerializeSettings(doc, Properties.Settings.Default));
                doc.Save(GetUserSettingsPath());
                Console.WriteLine("Saving user settings finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save settings: " + ex.ToString());
            }
        }

        #endregion

        private void cb_Rig_DropDown(object sender, EventArgs e)
        {
        }

        private void cb_Rig_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                Console.WriteLine("RigType changed to : " + (string)cb_Rig.SelectedItem);
                Properties.Settings.Default.RigSettings.RigType = (string)cb_Rig.SelectedItem;
                SaveUserSettings();

                //restart rig
                RestartRig();
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_PortName_DropDown(object sender, EventArgs e)
        {
            cb_PortName.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cb_PortName.Items.Add(s);
            }
        }

        private void cb_PortName_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                Console.WriteLine("PortName changed to : " + (string)cb_PortName.SelectedItem);
                Properties.Settings.Default.RigSettings.PortName = (string)cb_PortName.SelectedItem;
                SaveUserSettings();

                //restart rig
                RestartRig();
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_Baudrate_DropDown(object sender, EventArgs e)
        {
            cb_Baudrate.Items.Clear();
            string[] baudrates = new string[] { "110", "300", "600", "1200", "2400", "4800", "9600", "14400", "19200", "38400", "56000", "57600", "115200", "128000", "256000"};
            cb_Baudrate.Items.AddRange(baudrates);
        }

        private void cb_Baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                Properties.Settings.Default.RigSettings.Baudrate = System.Convert.ToInt32(cb_Baudrate.SelectedItem);
                SaveUserSettings();

                //restart rig
                RestartRig();
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_Databits_DropDown(object sender, EventArgs e)
        {
            cb_Databits.Items.Clear();
            string[] databits = new string[] { "5", "6", "7", "8"};
            cb_Databits.Items.AddRange(databits);
        }

        private void cb_Databits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.RigSettings.DataBits = System.Convert.ToInt32(cb_Databits.SelectedItem);
                SaveUserSettings();

                //restart rig
                RestartRig();
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_Parity_DropDown(object sender, EventArgs e)
        {
        }

        private void cb_Parity_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                if (cb_Parity.SelectedValue != null)
                {
                    Properties.Settings.Default.RigSettings.Parity = (Parity)cb_Parity.SelectedValue;
                    SaveUserSettings();
                    //restart rig
                    RestartRig();
                }
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_Stopbits_DropDown(object sender, EventArgs e)
        {
        }

        private void cb_Stopbits_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                if (cb_Stopbits.SelectedValue != null)
                {
                    Properties.Settings.Default.RigSettings.StopBits = (StopBits)cb_Stopbits.SelectedValue;
                    SaveUserSettings();
                    //restart rig
                    RestartRig();
                }
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_RTS_DropDown(object sender, EventArgs e)
        {
        }

        private void cb_RTS_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                if (cb_RTS.SelectedValue != null)
                {
                    Properties.Settings.Default.RigSettings.RtsMode = (FlowControl)cb_RTS.SelectedValue;
                    SaveUserSettings();
                    //restart rig
                    RestartRig();
                }
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_DTR_DropDown(object sender, EventArgs e)
        {
        }

        private void cb_DTR_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                if (cb_DTR.SelectedValue != null)
                {
                    Properties.Settings.Default.RigSettings.DtrMode = (FlowControl)cb_DTR.SelectedValue;
                    SaveUserSettings();
                    //restart rig
                    RestartRig();
                }
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void cb_LogVerbosity_SelectedIndexChanged(object sender, EventArgs e)
        {
            // do nothing while loading
            if (!Loaded)
                return;

            try
            {
                if (cb_LogVerbosity != null)
                {
                    Properties.Settings.Default.LogVerbosity = (LOGLEVEL)cb_LogVerbosity.SelectedValue;
                    SaveUserSettings();
                }
            }
            catch
            {
                // do nothing if fails
            }
        }

        private void ud_PollMs_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)
                return;

            try
            {
                Properties.Settings.Default.RigSettings.PollMs = (int)ud_PollMs.Value;
            }
            catch
            {
                // do nothing if fails
            }

            RestartRig();
        }

        private void ud_TimeoutMs_ValueChanged(object sender, EventArgs e)
        {
            if (!Loaded)
                return;

            try
            {
                Properties.Settings.Default.RigSettings.TimeoutMs = (int)ud_TimeoutMs.Value;
            }
            catch
            {
                // do nothing if fails
            }
            RestartRig();
        }

        private void btn_Configurations_Folder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog Dlg = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(Properties.Settings.Default.RigDefinitionsFolder))
            {
                Dlg.SelectedPath = Properties.Settings.Default.RigDefinitionsFolder;
            }

            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(Dlg.SelectedPath))
                {
                    Properties.Settings.Default.RigDefinitionsFolder = Dlg.SelectedPath;
                    SaveUserSettings();
                    OmniRig.RigDefinitionsDirectory = Properties.Settings.Default.RigDefinitionsFolder;
                }
                else
                {
                    MessageBox.Show("Selected path does not exist!", "Select Rig Definitions Folder");
                }
            }
        }

        private void btn_Restart_Click(object sender, EventArgs e)
        {
            RestartRig();
        }

        private void btn_Log_StartStop_Click(object sender, EventArgs e)
        {
            if (Logging)
            {
                btn_Log_StartStop.Text = "Start Log";
                Logging = false;
            }
            else
            {
                btn_Log_StartStop.Text = "Stop Log";
                Logging = true;
            }
        }

        private void btn_Log_Clear_Click(object sender, EventArgs e)
        {
            lv_Messages.Items.Clear();
        }

        private void btn_Log_Show_Click(object sender, EventArgs e)
        {
            // get log directory, set application dir if not specified
            string dir = Path.GetDirectoryName(OmniRig.LogFileName);
            if (String.IsNullOrEmpty(dir))
            {
                dir = Application.StartupPath;
            }

            Process.Start("explorer.exe", dir);
        }

        private void lbl_Freq_Click(object sender, EventArgs e)
        {
            // return if param is not writeable
            if (!Rig1.RigCommands.WriteableParams.Contains(RigParam.pmFreq))
                return;
            SetFrequencyDlg Dlg = new SetFrequencyDlg();
            Dlg.ud_Frequency.Value = Rig1.Freq;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Rig1.Freq = (int)Dlg.ud_Frequency.Value;
            }
        }

        private void lbl_FreqA_Click(object sender, EventArgs e)
        {
            // return if param is not writeable
            if (!Rig1.RigCommands.WriteableParams.Contains(RigParam.pmFreqA))
                return;
            SetFrequencyDlg Dlg = new SetFrequencyDlg();
            Dlg.ud_Frequency.Value = Rig1.FreqA;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Rig1.FreqA = (int)Dlg.ud_Frequency.Value;
            }
        }

        private void lbl_FreqB_Click(object sender, EventArgs e)
        {
            // return if param is not writeable
            if (!Rig1.RigCommands.WriteableParams.Contains(RigParam.pmFreqB))
                return;
            SetFrequencyDlg Dlg = new SetFrequencyDlg();
            Dlg.ud_Frequency.Value = Rig1.FreqB;
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                Rig1.FreqB = (int)Dlg.ud_Frequency.Value;
            }
        }

        private void lbl_CW_U_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmCW_U;
        }

        private void lbl_CW_L_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmCW_L;
        }

        private void lbl_SSB_U_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmSSB_U;
        }

        private void lbl_SSB_L_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmSSB_L;
        }

        private void lbl_DIG_U_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmDIG_U;
        }

        private void lbl_DIG_L_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmDIG_L;
        }

        private void lbl_AM_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmAM;
        }

        private void lbl_FM_Click(object sender, EventArgs e)
        {
            Rig1.Mode = RigParam.pmFM;
        }

        private void lbl_TX_Click(object sender, EventArgs e)
        {
            Rig1.Tx = RigParam.pmTx;
        }

        private void lbl_RX_Click(object sender, EventArgs e)
        {
            Rig1.Tx = RigParam.pmRx;
        }

        private void lbl_VFO_AA_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoAA;
        }

        private void lbl_VFO_AB_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoAB;
        }

        private void lbl_VFO_BA_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoBA;
        }

        private void lbl_VFO_BB_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoBB;
        }

        private void lbl_VFO_A_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoA;
        }

        private void lbl_VFO_B_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoB;
        }

        private void lbl_VFO_Equal_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoEqual;
        }

        private void lbl_VFO_Swap_Click(object sender, EventArgs e)
        {
            Rig1.Vfo = RigParam.pmVfoSwap;
        }

        private void lbl_SPLIT_ON_Click(object sender, EventArgs e)
        {
            Rig1.Split = RigParam.pmSplitOn;
        }

        private void lbl_SPLIT_OFF_Click(object sender, EventArgs e)
        {
            Rig1.Split = RigParam.pmSplitOff;
        }

        private void lbl_XIT_ON_Click(object sender, EventArgs e)
        {
            Rig1.Xit = RigParam.pmXitOn;
        }

        private void lbl_XIT_OFF_Click(object sender, EventArgs e)
        {
            Rig1.Xit = RigParam.pmXitOff;
        }

        private void lbl_RIT_ON_Click(object sender, EventArgs e)
        {
            Rig1.Rit = RigParam.pmRitOn;
        }

        private void lbl_RIT_OFF_Click(object sender, EventArgs e)
        {
            Rig1.Rit = RigParam.pmRitOff;
        }

        private void lbl_RIT_0_Click(object sender, EventArgs e)
        {
            Rig1.Rit = RigParam.pmRit0;
        }

        private void lbl_RTS_Click(object sender, EventArgs e)
        {
            Rig1.PortBits.Rts = !Rig1.PortBits.Rts;
        }

        private void lbl_DTR_Click(object sender, EventArgs e)
        {
            Rig1.PortBits.Dtr = !Rig1.PortBits.Dtr;
        }


        // special functions
        private void mni_ClearRIT_Click(object sender, EventArgs e)
        {
            Rig1.ClearRit();
        }

        private void mni_FrequencyOfTone_Click(object sender, EventArgs e)
        {
            FrequencyOfToneDlg Dlg = new FrequencyOfToneDlg(Rig1);
            Dlg.Show();
        }

        private void RestartRig()
        {
            // suppress restarts until fully loaded
            if (Loaded)
            {
                // stop rig if running
                if (Rig1.Enabled)
                {
                    Rig1.Enabled = false;
                }

                DateTime timeout = DateTime.Now.AddMilliseconds((int)Properties.Settings.Default.RigSettings.TimeoutMs);
                do
                {
                    Application.DoEvents();
                    if (DateTime.Now > timeout)
                    {
                        OmniRig_LogNotify(new LogNotifyEventArgs(DateTime.UtcNow, LOGLEVEL.llError, "Cannot not stop rig!"));
                        break;
                    }
                }
                while (Rig1.Enabled);

                // clear log
                if (File.Exists(OmniRig.LogFileName))
                {
                    try
                    {
                        File.Delete(OmniRig.LogFileName);
                    }
                    catch
                    {
                        // do nothing
                    }
                }
                // refresh settings
                Rig1.RigNumber = 1;
                Properties.Settings.Default.RigSettings.ToRig(Rig1);
                // start rig
                Rig1.Enabled = true;

            }
        }

        private void OmniRig_LogNotify(LogNotifyEventArgs args)
        {
            if (!Logging)
                return;

            // queue message
            MsgQueue.Enqueue(args);
        }

        private void OmniRig_ComNotifyParams(int rignumber, int Params)
        {

            // set frequencies
            tb_Freq.Text = Rig1.Freq.ToString();
            tb_FreqA.Text = Rig1.FreqA.ToString();
            tb_FreqB.Text = Rig1.FreqB.ToString();

            // set mode
            if (LastMode != Rig1.Mode)
            {
                SetLabelStyle(lbl_CW_L, RigParam.pmCW_L, false);
                SetLabelStyle(lbl_CW_U, RigParam.pmCW_U, false);
                SetLabelStyle(lbl_SSB_L, RigParam.pmSSB_L, false);
                SetLabelStyle(lbl_SSB_U, RigParam.pmSSB_U, false);
                SetLabelStyle(lbl_DIG_L, RigParam.pmDIG_L, false);
                SetLabelStyle(lbl_DIG_U, RigParam.pmDIG_U, false);
                SetLabelStyle(lbl_AM, RigParam.pmAM, false);
                SetLabelStyle(lbl_FM, RigParam.pmFM, false);

                switch (Rig1.Mode)
                {
                    case RigParam.pmCW_U: SetLabelStyle(lbl_CW_U, RigParam.pmCW_U, true, 1); break;
                    case RigParam.pmCW_L: SetLabelStyle(lbl_CW_L, RigParam.pmCW_L, true, 1); break;
                    case RigParam.pmSSB_U: SetLabelStyle(lbl_SSB_U, RigParam.pmSSB_U, true, 1);break;
                    case RigParam.pmSSB_L: SetLabelStyle(lbl_SSB_L, RigParam.pmSSB_L, true, 1); break;
                    case RigParam.pmDIG_U: SetLabelStyle(lbl_DIG_U, RigParam.pmDIG_U, true, 1); break;
                    case RigParam.pmDIG_L: SetLabelStyle(lbl_DIG_L, RigParam.pmDIG_L, true, 1); break;
                    case RigParam.pmAM: SetLabelStyle(lbl_AM, RigParam.pmAM, true, 1); break;
                    case RigParam.pmFM: SetLabelStyle(lbl_FM, RigParam.pmFM, true, 1); break;
                }

                LastMode = Rig1.Mode;
            }

            // set TX
            if (LastTx != Rig1.Tx)
            {
                SetLabelStyle(lbl_RX, RigParam.pmRx, false);
                SetLabelStyle(lbl_TX, RigParam.pmTx, false);

                switch (Rig1.Tx)
                {
                    case RigParam.pmRx: SetLabelStyle(lbl_RX, RigParam.pmRx, true, 1); break;
                    case RigParam.pmTx: SetLabelStyle(lbl_TX, RigParam.pmTx, true, 2); break;
                }

                LastTx = Rig1.Tx;
            }

            // set VFO
            if (LastVfo != Rig1.Vfo)
            {
                SetLabelStyle(lbl_VFO_AA, RigParam.pmVfoAA, false);
                SetLabelStyle(lbl_VFO_AB, RigParam.pmVfoAB, false);
                SetLabelStyle(lbl_VFO_BA, RigParam.pmVfoBA, false);
                SetLabelStyle(lbl_VFO_BB, RigParam.pmVfoBB, false);
                SetLabelStyle(lbl_VFO_A, RigParam.pmVfoA, false);
                SetLabelStyle(lbl_VFO_B, RigParam.pmVfoB, false);
                SetLabelStyle(lbl_VFO_Equal, RigParam.pmVfoEqual, false);
                SetLabelStyle(lbl_VFO_Swap, RigParam.pmVfoSwap, false);

                switch (Rig1.Vfo)
                {
                    case RigParam.pmVfoAA: SetLabelStyle(lbl_VFO_AA, RigParam.pmVfoAA, true, 1); break;
                    case RigParam.pmVfoAB: SetLabelStyle(lbl_VFO_AB, RigParam.pmVfoAB, true, 1); break;
                    case RigParam.pmVfoBA: SetLabelStyle(lbl_VFO_BA, RigParam.pmVfoBA, true, 1); break;
                    case RigParam.pmVfoBB: SetLabelStyle(lbl_VFO_BB, RigParam.pmVfoBB, true, 1); break;
                    case RigParam.pmVfoA: SetLabelStyle(lbl_VFO_A, RigParam.pmVfoA, true, 1); break;
                    case RigParam.pmVfoB: SetLabelStyle(lbl_VFO_B, RigParam.pmVfoB, true, 1); break;
                    case RigParam.pmVfoEqual: SetLabelStyle(lbl_VFO_Equal, RigParam.pmVfoEqual, true, 1); break;
                    case RigParam.pmVfoSwap: SetLabelStyle(lbl_VFO_Swap, RigParam.pmVfoSwap, true, 1); break;
                }

                LastVfo = Rig1.Vfo;
            }

            // set split
            if (LastSplit != Rig1.Split)
            {
                SetLabelStyle(lbl_SPLIT_ON, RigParam.pmSplitOn, false);
                SetLabelStyle(lbl_SPLIT_OFF, RigParam.pmSplitOff, false);

                switch (Rig1.Split)
                {
                    case RigParam.pmSplitOn: SetLabelStyle(lbl_SPLIT_ON, RigParam.pmSplitOn, true, 1); break;
                    case RigParam.pmSplitOff: SetLabelStyle(lbl_SPLIT_OFF, RigParam.pmSplitOff, true, 1); break;
                }

                LastSplit = Rig1.Split;
            }

            // set xit
            if (LastXit != Rig1.Xit)
            {
                SetLabelStyle(lbl_XIT_ON, RigParam.pmXitOn, false);
                SetLabelStyle(lbl_XIT_OFF, RigParam.pmXitOff, false);

                switch (Rig1.Xit)
                {
                    case RigParam.pmXitOn: SetLabelStyle(lbl_XIT_ON, RigParam.pmXitOn, true, 1); break;
                    case RigParam.pmXitOff: SetLabelStyle(lbl_XIT_OFF, RigParam.pmXitOff, true, 1); break;
                }

                LastXit = Rig1.Xit;
            }

            // set rit
            if (LastRit != Rig1.Rit)
            {
                SetLabelStyle(lbl_RIT_ON, RigParam.pmRitOn, false);
                SetLabelStyle(lbl_RIT_OFF, RigParam.pmRitOff, false);

                switch (Rig1.Rit)
                {
                    case RigParam.pmRitOn: SetLabelStyle(lbl_RIT_ON, RigParam.pmRitOn, true, 1); break;
                    case RigParam.pmRitOff: SetLabelStyle(lbl_RIT_OFF, RigParam.pmRitOff, true, 1); break;
                }

                LastRit = Rig1.Rit;
            }

            // set rit offset
            if (LastRitOffset != Rig1.RitOffset)
            {
                // show RIT offset in kHz
                lbl_RIT_OFS.Text = (Rig1.RitOffset/1000.0).ToString("0.00");
                LastRitOffset = Rig1.RitOffset;
            }


            Application.DoEvents();
        }

        private void SetLabelStyle(Label label, RigParam param, bool on, int color = 1)
        {
            // calculate color
            Color c = Color.Transparent;
            switch (color)
            {
                case 1: c = Color.Chartreuse; break;
                case 2: c = Color.LightSalmon; break;
                default: c = Color.LightGray; break;
            }

            // set colors according to read/write caps
            if (!Rig1.RigCommands.ReadableParams.Contains(param) && !Rig1.RigCommands.WriteableParams.Contains(param))
            {
                label.BorderStyle = BorderStyle.Fixed3D;
                label.ForeColor = Color.Gray;
                label.BackColor = Color.LightGray;
                tt_Set.SetToolTip(label, "Not writeable!");
            }
            else if (Rig1.RigCommands.ReadableParams.Contains(param) && !Rig1.RigCommands.WriteableParams.Contains(param))
            {
                label.BorderStyle = BorderStyle.FixedSingle;
                label.ForeColor = Color.Black;
                label.BackColor = on ? c : Color.FloralWhite;
                tt_Set.SetToolTip(label, "Not writeable!");
            }
            else if (!Rig1.RigCommands.ReadableParams.Contains(param) && Rig1.RigCommands.WriteableParams.Contains(param))
            {
                label.BorderStyle = BorderStyle.FixedSingle;
                label.BackColor = Color.LightGray;
                label.ForeColor = Color.Black;
                tt_Set.SetToolTip(label, "Click to send command!");
            }
            else if (Rig1.RigCommands.ReadableParams.Contains(param) && Rig1.RigCommands.WriteableParams.Contains(param))
            {
                label.ForeColor = Color.Black;
                label.BorderStyle = BorderStyle.FixedSingle;
                label.BackColor = on ? c : Color.White;
                tt_Set.SetToolTip(label, "Click to send command!");
            }
        }

        private void SetTextBoxStyle(TextBox textbox, RigParam param, bool on, int color = 1)
        {
            // calculate color
            Color c = Color.Transparent;
            switch (color)
            {
                case 1: c = Color.Chartreuse; break;
                case 2: c = Color.LightSalmon; break;
                default: c = Color.LightGray; break;
            }

            // set colors according to read/write caps
            if (!Rig1.RigCommands.ReadableParams.Contains(param))
            {
                textbox.BorderStyle = BorderStyle.Fixed3D;
                textbox.ForeColor = Color.Gray;
                textbox.BackColor = Color.LightGray;
            }
            else
            {
                textbox.BorderStyle = BorderStyle.FixedSingle;
                textbox.ForeColor = c;
                textbox.BackColor = Color.Gray;
                tt_Set.SetToolTip(textbox, "Not writeable!");
            }
        }

        private void OmniRig_ComNotifyStatus(int rignumber)
        {
            RigCtlStatus status = Rig1.Status;

            // reset all last values to None if offline
            if (status != RigCtlStatus.stOnLine)
            {
                LastMode = RigParam.pmNone;
                LastTx = RigParam.pmNone;
                LastVfo = RigParam.pmNone;
                LastSplit = RigParam.pmNone;
                LastXit = RigParam.pmNone;
                LastRit = RigParam.pmNone;

                LastRitOffset = 0;

            }

            // show status text in text box
            tb_Status.Text = Rig1.StatusStr;

            // enable/disable elements according to rig capabilities
            SetTextBoxStyle(tb_Freq, RigParam.pmFreq,false);
            SetTextBoxStyle(tb_FreqA, RigParam.pmFreqA, false);
            SetTextBoxStyle(tb_FreqB, RigParam.pmFreqB, false);

            SetLabelStyle(lbl_Freq, RigParam.pmFreq, false);
            SetLabelStyle(lbl_FreqA, RigParam.pmFreqA, false);
            SetLabelStyle(lbl_FreqB, RigParam.pmFreqB, false);

            // initially set all buttons
            SetLabelStyle(lbl_CW_L, RigParam.pmCW_L, false);
            SetLabelStyle(lbl_CW_U, RigParam.pmCW_U, false);
            SetLabelStyle(lbl_SSB_L, RigParam.pmSSB_L, false);
            SetLabelStyle(lbl_SSB_U, RigParam.pmSSB_U, false);
            SetLabelStyle(lbl_DIG_L, RigParam.pmDIG_L, false);
            SetLabelStyle(lbl_DIG_U, RigParam.pmDIG_U, false);
            SetLabelStyle(lbl_AM, RigParam.pmAM, false);
            SetLabelStyle(lbl_FM, RigParam.pmFM, false);

            SetLabelStyle(lbl_RX, RigParam.pmRx, false);
            SetLabelStyle(lbl_TX, RigParam.pmTx, false);

            SetLabelStyle(lbl_VFO_AA, RigParam.pmVfoAA, false);
            SetLabelStyle(lbl_VFO_AB, RigParam.pmVfoAB, false);
            SetLabelStyle(lbl_VFO_BA, RigParam.pmVfoBA, false);
            SetLabelStyle(lbl_VFO_BB, RigParam.pmVfoBB, false);
            SetLabelStyle(lbl_VFO_A, RigParam.pmVfoA, false);
            SetLabelStyle(lbl_VFO_B, RigParam.pmVfoB, false);
            SetLabelStyle(lbl_VFO_Equal, RigParam.pmVfoEqual, false);
            SetLabelStyle(lbl_VFO_Swap, RigParam.pmVfoSwap, false);

            SetLabelStyle(lbl_SPLIT_ON, RigParam.pmSplitOn, false);
            SetLabelStyle(lbl_SPLIT_OFF, RigParam.pmSplitOff, false);

            SetLabelStyle(lbl_XIT_ON, RigParam.pmXitOn, false);
            SetLabelStyle(lbl_XIT_OFF, RigParam.pmXitOff, false);

            SetLabelStyle(lbl_RIT_ON, RigParam.pmRitOn, false);
            SetLabelStyle(lbl_RIT_OFF, RigParam.pmRitOff, false);
            SetLabelStyle(lbl_RIT_OFS, RigParam.pmRitOffset, false);

            // show status lines
            if (status != RigCtlStatus.stPortBusy)
            {
                lbl_RTS.BackColor = (Rig1.PortBits.Rts) ? Color.Red : Color.Chartreuse;
                lbl_DTR.BackColor = (Rig1.PortBits.Dtr) ? Color.Red : Color.Chartreuse;
                lbl_CTS.BackColor = (Rig1.PortBits.Cts) ? Color.Red : Color.Chartreuse;
                lbl_DSR.BackColor = (Rig1.PortBits.Dsr) ? Color.Red : Color.Chartreuse;
                tt_Set.SetToolTip(lbl_RTS, "Click to toggle!");
                tt_Set.SetToolTip(lbl_DTR, "Click to toggle!");
            }
            else
            {
                lbl_RTS.BackColor = Color.LightGray;
                lbl_DTR.BackColor = Color.LightGray;
                lbl_CTS.BackColor = Color.LightGray;
                lbl_DSR.BackColor = Color.LightGray;
                tt_Set.SetToolTip(lbl_RTS, "");
                tt_Set.SetToolTip(lbl_DTR, "");
            }
        }

        private void mni_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mni_About_Click(object sender, EventArgs e)
        {
            AboutDlg Dlg = new AboutDlg();
            Dlg.lbl_Version.Text = Dlg.lbl_Version.Text.Replace("x.x.x", Application.ProductVersion.ToString());
            Dlg.ShowDialog();
        }

        private void lv_Messages_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            // Draw the header background
            e.Graphics.FillRectangle(Brushes.LightCyan, e.Bounds);

            // Draw the header text
            Font font = new Font("Courier New", 8, FontStyle.Bold);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString(e.Header.Text, font, Brushes.Black, e.Bounds, sf);
        }

        private void lv_Messages_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lv_Messages_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void ti_Main_Tick(object sender, EventArgs e)
        {
            // stop timer
            ti_Main.Stop();

            try
            {
                // show msg queue count
                lbl_Queue.Text = MsgQueue.Count.ToString();
                lbl_Queue.Refresh();

                lock (MsgQueue)
                {
                    while (MsgQueue.Count > 0)
                    {
                        // get message
                        LogNotifyEventArgs args = MsgQueue.Dequeue();

                        if (args.LogLevel >= Properties.Settings.Default.LogVerbosity)
                        {
                            lv_Messages.BeginUpdate();

                            // maintain size
                            while (lv_Messages.Items.Count > 100)
                            {
                                lv_Messages.Items.RemoveAt(0);
                            }

                            // add new entry
                            ListViewItem item = new ListViewItem();
                            item.ImageIndex = ((int)args.LogLevel) - 1;
                            item.SubItems.Add(args.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss,fff"));
                            item.SubItems.Add(args.Message);
                            lv_Messages.Items.Add(item);

                            lv_Messages.EndUpdate();

                            lv_Messages.Items[lv_Messages.Items.Count - 1].EnsureVisible();

                            // check for application events
                            Application.DoEvents();
                        
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while adding message to list: " + ex.Message);
            }

            // restart timer
            ti_Main.Start();
        }

    }
}
