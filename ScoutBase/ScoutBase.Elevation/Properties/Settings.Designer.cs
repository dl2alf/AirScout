﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScoutBase.Elevation.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.0.3.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Database_Directory {
            get {
                return ((string)(this["Database_Directory"]));
            }
            set {
                this["Database_Directory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Database_InMemory {
            get {
                return ((bool)(this["Database_InMemory"]));
            }
            set {
                this["Database_InMemory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("GLOBE")]
        public string Elevation_GLOBE_DataPath {
            get {
                return ((string)(this["Elevation_GLOBE_DataPath"]));
            }
            set {
                this["Elevation_GLOBE_DataPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.airscout.eu/downloads/ScoutBase/1/ElevationData/GLOBE")]
        public string Elevation_GLOBE_UpdateURL {
            get {
                return ((string)(this["Elevation_GLOBE_UpdateURL"]));
            }
            set {
                this["Elevation_GLOBE_UpdateURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("globe.json")]
        public string Elevation_GLOBE_JSONFile {
            get {
                return ((string)(this["Elevation_GLOBE_JSONFile"]));
            }
            set {
                this["Elevation_GLOBE_JSONFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SRTM3")]
        public string Elevation_SRTM3_DataPath {
            get {
                return ((string)(this["Elevation_SRTM3_DataPath"]));
            }
            set {
                this["Elevation_SRTM3_DataPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.airscout.eu/downloads/ScoutBase/1/ElevationData/SRTM3")]
        public string Elevation_SRTM3_UpdateURL {
            get {
                return ((string)(this["Elevation_SRTM3_UpdateURL"]));
            }
            set {
                this["Elevation_SRTM3_UpdateURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("srtm3.json")]
        public string Elevation_SRTM3_JSONFile {
            get {
                return ((string)(this["Elevation_SRTM3_JSONFile"]));
            }
            set {
                this["Elevation_SRTM3_JSONFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SRTM1")]
        public string Elevation_SRTM1_DataPath {
            get {
                return ((string)(this["Elevation_SRTM1_DataPath"]));
            }
            set {
                this["Elevation_SRTM1_DataPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.airscout.eu/downloads/ScoutBase/1/ElevationData/SRTM1")]
        public string Elevation_SRTM1_UpdateURL {
            get {
                return ((string)(this["Elevation_SRTM1_UpdateURL"]));
            }
            set {
                this["Elevation_SRTM1_UpdateURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("srtm1.json")]
        public string Elevation_SRTM1_JSONFile {
            get {
                return ((string)(this["Elevation_SRTM1_JSONFile"]));
            }
            set {
                this["Elevation_SRTM1_JSONFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public int Datatbase_BackgroundUpdate_Period {
            get {
                return ((int)(this["Datatbase_BackgroundUpdate_Period"]));
            }
            set {
                this["Datatbase_BackgroundUpdate_Period"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int Database_BackgroundUpdate_ThreadWait {
            get {
                return ((int)(this["Database_BackgroundUpdate_ThreadWait"]));
            }
            set {
                this["Database_BackgroundUpdate_ThreadWait"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_GLOBE_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_GLOBE_TimeStamp"]));
            }
            set {
                this["Elevation_GLOBE_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_SRTM3_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_SRTM3_TimeStamp"]));
            }
            set {
                this["Elevation_SRTM3_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_SRTM1_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_SRTM1_TimeStamp"]));
            }
            set {
                this["Elevation_SRTM1_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UNDEFINED")]
        public global::System.Data.SQLite.DATABASESTATUS Elevation_GLOBE_Status {
            get {
                return ((global::System.Data.SQLite.DATABASESTATUS)(this["Elevation_GLOBE_Status"]));
            }
            set {
                this["Elevation_GLOBE_Status"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UNDEFINED")]
        public global::System.Data.SQLite.DATABASESTATUS Elevation_SRTM3_Status {
            get {
                return ((global::System.Data.SQLite.DATABASESTATUS)(this["Elevation_SRTM3_Status"]));
            }
            set {
                this["Elevation_SRTM3_Status"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UNDEFINED")]
        public global::System.Data.SQLite.DATABASESTATUS Elevation_SRTM1_Status {
            get {
                return ((global::System.Data.SQLite.DATABASESTATUS)(this["Elevation_SRTM1_Status"]));
            }
            set {
                this["Elevation_SRTM1_Status"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_GLOBE_Update_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_GLOBE_Update_TimeStamp"]));
            }
            set {
                this["Elevation_GLOBE_Update_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_SRTM3_Update_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_SRTM3_Update_TimeStamp"]));
            }
            set {
                this["Elevation_SRTM3_Update_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_SRTM1_Update_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_SRTM1_Update_TimeStamp"]));
            }
            set {
                this["Elevation_SRTM1_Update_TimeStamp"] = value;
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
        [global::System.Configuration.DefaultSettingValueAttribute("ASTER3")]
        public string Elevation_ASTER3_DataPath {
            get {
                return ((string)(this["Elevation_ASTER3_DataPath"]));
            }
            set {
                this["Elevation_ASTER3_DataPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.airscout.eu/downloads/ScoutBase/1/ElevationData/ASTER3")]
        public string Elevation_ASTER3_UpdateURL {
            get {
                return ((string)(this["Elevation_ASTER3_UpdateURL"]));
            }
            set {
                this["Elevation_ASTER3_UpdateURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("aster3.json")]
        public string Elevation_ASTER3_JSONFile {
            get {
                return ((string)(this["Elevation_ASTER3_JSONFile"]));
            }
            set {
                this["Elevation_ASTER3_JSONFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_ASTER3_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_ASTER3_TimeStamp"]));
            }
            set {
                this["Elevation_ASTER3_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_ASTER3_Update_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_ASTER3_Update_TimeStamp"]));
            }
            set {
                this["Elevation_ASTER3_Update_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ASTER1")]
        public string Elevation_ASTER1_DataPath {
            get {
                return ((string)(this["Elevation_ASTER1_DataPath"]));
            }
            set {
                this["Elevation_ASTER1_DataPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.airscout.eu/downloads/ScoutBase/1/ElevationData/ASTER1")]
        public string Elevation_ASTER1_UpdateURL {
            get {
                return ((string)(this["Elevation_ASTER1_UpdateURL"]));
            }
            set {
                this["Elevation_ASTER1_UpdateURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("aster1.json")]
        public string Elevation_ASTER1_JSONFile {
            get {
                return ((string)(this["Elevation_ASTER1_JSONFile"]));
            }
            set {
                this["Elevation_ASTER1_JSONFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_ASTER1_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_ASTER1_TimeStamp"]));
            }
            set {
                this["Elevation_ASTER1_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1970-01-01")]
        public global::System.DateTime Elevation_ASTER1_Update_TimeStamp {
            get {
                return ((global::System.DateTime)(this["Elevation_ASTER1_Update_TimeStamp"]));
            }
            set {
                this["Elevation_ASTER1_Update_TimeStamp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UNDEFINED")]
        public global::System.Data.SQLite.DATABASESTATUS Elevation_ASTER3_Status {
            get {
                return ((global::System.Data.SQLite.DATABASESTATUS)(this["Elevation_ASTER3_Status"]));
            }
            set {
                this["Elevation_ASTER3_Status"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UNDEFINED")]
        public global::System.Data.SQLite.DATABASESTATUS Elevation_ASTER1_Status {
            get {
                return ((global::System.Data.SQLite.DATABASESTATUS)(this["Elevation_ASTER1_Status"]));
            }
            set {
                this["Elevation_ASTER1_Status"] = value;
            }
        }
    }
}
