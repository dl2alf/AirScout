﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirScoutViewClient.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.5.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("localhost")]
        public string Server_URL {
            get {
                return ((string)(this["Server_URL"]));
            }
            set {
                this["Server_URL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9880")]
        public decimal Server_Port {
            get {
                return ((decimal)(this["Server_Port"]));
            }
            set {
                this["Server_Port"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int RefreshRate {
            get {
                return ((int)(this["RefreshRate"]));
            }
            set {
                this["RefreshRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OpenStreetMap")]
        public string Map_Provider {
            get {
                return ((string)(this["Map_Provider"]));
            }
            set {
                this["Map_Provider"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\Tmp")]
        public string Tmp_Directory {
            get {
                return ((string)(this["Tmp_Directory"]));
            }
            set {
                this["Tmp_Directory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\\Log")]
        public string Log_Directory {
            get {
                return ((string)(this["Log_Directory"]));
            }
            set {
                this["Log_Directory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DL2ALF")]
        public string MyCall {
            get {
                return ((string)(this["MyCall"]));
            }
            set {
                this["MyCall"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double MyLat {
            get {
                return ((double)(this["MyLat"]));
            }
            set {
                this["MyLat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double MyLon {
            get {
                return ((double)(this["MyLon"]));
            }
            set {
                this["MyLon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GB3MHZ")]
        public string DXCall {
            get {
                return ((string)(this["DXCall"]));
            }
            set {
                this["DXCall"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double DXLat {
            get {
                return ((double)(this["DXLat"]));
            }
            set {
                this["DXLat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double DXLon {
            get {
                return ((double)(this["DXLon"]));
            }
            set {
                this["DXLon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double MinLat {
            get {
                return ((double)(this["MinLat"]));
            }
            set {
                this["MinLat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double MinLon {
            get {
                return ((double)(this["MinLon"]));
            }
            set {
                this["MinLon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double MaxLat {
            get {
                return ((double)(this["MaxLat"]));
            }
            set {
                this["MaxLat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double MaxLon {
            get {
                return ((double)(this["MaxLon"]));
            }
            set {
                this["MaxLon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("B1_2G")]
        public global::ScoutBase.Core.BAND Band {
            get {
                return ((global::ScoutBase.Core.BAND)(this["Band"]));
            }
            set {
                this["Band"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Locator_SmallLettersForSubsquares {
            get {
                return ((bool)(this["Locator_SmallLettersForSubsquares"]));
            }
            set {
                this["Locator_SmallLettersForSubsquares"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Locator_AutoLength {
            get {
                return ((bool)(this["Locator_AutoLength"]));
            }
            set {
                this["Locator_AutoLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public decimal Locator_MaxLength {
            get {
                return ((decimal)(this["Locator_MaxLength"]));
            }
            set {
                this["Locator_MaxLength"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Map_SmallMarkers {
            get {
                return ((bool)(this["Map_SmallMarkers"]));
            }
            set {
                this["Map_SmallMarkers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("12200")]
        public int Planes_MaxAlt {
            get {
                return ((int)(this["Planes_MaxAlt"]));
            }
            set {
                this["Planes_MaxAlt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("plane.png")]
        public string Planes_IconFileName {
            get {
                return ((string)(this["Planes_IconFileName"]));
            }
            set {
                this["Planes_IconFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("airport.png")]
        public string Airports_IconFileName {
            get {
                return ((string)(this["Airports_IconFileName"]));
            }
            set {
                this["Airports_IconFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Position {
            get {
                return ((bool)(this["InfoWin_Position"]));
            }
            set {
                this["InfoWin_Position"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Metric {
            get {
                return ((bool)(this["InfoWin_Metric"]));
            }
            set {
                this["InfoWin_Metric"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Track {
            get {
                return ((bool)(this["InfoWin_Track"]));
            }
            set {
                this["InfoWin_Track"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Speed {
            get {
                return ((bool)(this["InfoWin_Speed"]));
            }
            set {
                this["InfoWin_Speed"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Alt {
            get {
                return ((bool)(this["InfoWin_Alt"]));
            }
            set {
                this["InfoWin_Alt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Type {
            get {
                return ((bool)(this["InfoWin_Type"]));
            }
            set {
                this["InfoWin_Type"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Time {
            get {
                return ((bool)(this["InfoWin_Time"]));
            }
            set {
                this["InfoWin_Time"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Dist {
            get {
                return ((bool)(this["InfoWin_Dist"]));
            }
            set {
                this["InfoWin_Dist"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool InfoWin_Angle {
            get {
                return ((bool)(this["InfoWin_Angle"]));
            }
            set {
                this["InfoWin_Angle"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool InfoWin_Epsilon {
            get {
                return ((bool)(this["InfoWin_Epsilon"]));
            }
            set {
                this["InfoWin_Epsilon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool InfoWin_Squint {
            get {
                return ((bool)(this["InfoWin_Squint"]));
            }
            set {
                this["InfoWin_Squint"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int Planes_Filter_Min_Alt {
            get {
                return ((int)(this["Planes_Filter_Min_Alt"]));
            }
            set {
                this["Planes_Filter_Min_Alt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NONE")]
        public global::AirScout.Core.PLANECATEGORY Planes_Filter_Min_Category {
            get {
                return ((global::AirScout.Core.PLANECATEGORY)(this["Planes_Filter_Min_Category"]));
            }
            set {
                this["Planes_Filter_Min_Category"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::AirScout.Core.BandSettings Path_Band_Settings {
            get {
                return ((global::AirScout.Core.BandSettings)(this["Path_Band_Settings"]));
            }
            set {
                this["Path_Band_Settings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5000")]
        public int Planes_MinAlt {
            get {
                return ((int)(this["Planes_MinAlt"]));
            }
            set {
                this["Planes_MinAlt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Planes_Filter_Max_Circumcircle {
            get {
                return ((int)(this["Planes_Filter_Max_Circumcircle"]));
            }
            set {
                this["Planes_Filter_Max_Circumcircle"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Map_AutoCenter {
            get {
                return ((bool)(this["Map_AutoCenter"]));
            }
            set {
                this["Map_AutoCenter"] = value;
            }
        }
    }
}
