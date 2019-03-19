using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace System.Data.SQLite
{
    /// <summary>
    /// Holds basic functionaltity
    /// </summary>
    [System.ComponentModel.DesignerCategory("")]
    [Serializable]
    public class SQLiteEntry
    {

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // be sure to have a copy of these static members in each derived class !!
        // individual static SQL strings per class will be created on first use
        // add a "new" statement on each derived class to confirm hiding of the base class members
        // update the tbale name to the table name according to the class
        // update the PrimaryKeys collection according to the class, crreate an empty list if no primary key
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [JsonIgnore]
        public static readonly string TableName = "Table";

        [JsonIgnore]
        public static List<SQLitePrimaryKey> PrimaryKeys = SQLiteSQLFactory.FillPrimaryKeys(MethodBase.GetCurrentMethod().DeclaringType, new List<string>());

        [JsonIgnore]
        public static readonly string SQLCreateTable = SQLiteSQLFactory.SQLCreateTable(MethodBase.GetCurrentMethod().DeclaringType);

        [JsonIgnore]
        public static readonly string SQLExists = SQLiteSQLFactory.SQLExists(MethodBase.GetCurrentMethod().DeclaringType);

        [JsonIgnore]
        public static readonly string SQLFind = SQLiteSQLFactory.SQLFind(MethodBase.GetCurrentMethod().DeclaringType);

        [JsonIgnore]
        public static readonly string SQLFindLastUpdated = SQLiteSQLFactory.SQLFindLastUpdated(MethodBase.GetCurrentMethod().DeclaringType);

        [JsonIgnore]
        public static readonly string SQLInsert = SQLiteSQLFactory.SQLInsert(MethodBase.GetCurrentMethod().DeclaringType);

        [JsonIgnore]
        public static readonly string SQLUpdate = SQLiteSQLFactory.SQLUpdate(MethodBase.GetCurrentMethod().DeclaringType);

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public DateTime LastUpdated { get; set; }

        
        public SQLiteEntry()
        {
            LastUpdated = DateTime.MinValue.ToUniversalTime();
        }

        public SQLiteEntry(DataRow row) : this()
        {
            FillFromDataRow(row);
        }

        public SQLiteEntry(IDataRecord record) : this()
        {
            FillFromDataRecord(record);
        }

        /// <summary>
        /// Creates an object of type T from a DataRow. Tries to fill all property values from the according columns.
        /// </summary>
        /// <param name="row">The DataRow to create object from.</param>
        protected void FillFromDataRow(DataRow row)
        {
            try
            {
                int i = 0;
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    // skip read only properties
                    if (!p.CanWrite)
                        continue;
                    // get column index
                    int index = row.Table.Columns.IndexOf(p.Name);
                    // NASTY!!! Linux/Mono hack: check if column name is present in table 
                    // --> use incremental index instead which should do the same job basically because columns are arranged the same way as type's properties
                    // found, that Windows arranges the properties not always in the same manner
                    if (index < 0)
                        index = i;
                    // Console.WriteLine("[" + T.Name + ".FillFromRow] DataColumn not found: " + p.Name);
                    if (p.PropertyType.Name.ToUpper() == "STRING")
                        p.SetValue(this, (row[index].GetType().Name != "DBNull") ? row[index] : null, null);
                    else if ((p.PropertyType.Name.ToUpper() == "FLOAT") || (p.PropertyType.Name.ToUpper() == "DOUBLE"))
                        p.SetValue(this, (row[index].GetType().Name != "DBNull") ? row[index] : null, null);
                    else if (p.PropertyType.Name.ToUpper() == "INT32")
                        p.SetValue(this, (row[index].GetType().Name != "DBNull") ? row[index] : null, null);
                    else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                    {

                        if ((row[index].GetType() == typeof(int)) || (row[index].GetType() == typeof(long)))
                            p.SetValue(this, UNIXTimeToDateTime(System.Convert.ToInt32(row[index])), null);
                        else if (row[index].GetType() == typeof(string))
                            p.SetValue(this, DateTime.ParseExact(row[index].ToString(), "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime(), null);
                        else
                            p.SetValue(this, row[index], null);
                    }
                    else if (p.PropertyType.BaseType.Name.ToUpper() == "ENUM")
                        p.SetValue(this, System.Convert.ToInt32(row[index]), null);
                    else
                        p.SetValue(this, ByteArrayToObject((byte[])row[index]), null);
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Creates an object of type T from a IDataRecord interface (e.g. SQLiteDataReader). Tries to fill all property values from the according record fields.
        /// </summary>
        /// <param name="row">The IDataRecord to create object from.</param>
        protected void FillFromDataRecord(IDataRecord record)
        {
            try
            {
                int i = 0;
                PropertyInfo[] properties = this.GetType().GetProperties();
                foreach (PropertyInfo p in properties)
                {
                    // skip read only properties
                    if (!p.CanWrite)
                        continue;
                    // get column index
                    int index = record.GetOrdinal(p.Name);
                    // NASTY!!! Linux/Mono hack: check if column name is present in table 
                    // --> use incremental index instead which should do the same job basically because columns are arranged the same way as type's properties
                    // found, that Windows arranges the properties not always in the same manner
                    if (index < 0)
                        index = i;
                    // Console.WriteLine("[" + T.Name + ".FillFromRow] DataColumn not found: " + p.Name);
                    if (p.PropertyType.Name.ToUpper() == "STRING")
                        p.SetValue(this, (record[index].GetType().Name != "DBNull") ? record[index] : null, null);
                    else if ((p.PropertyType.Name.ToUpper() == "FLOAT") || (p.PropertyType.Name.ToUpper() == "DOUBLE"))
                        p.SetValue(this, (record[index].GetType().Name != "DBNull") ? record[index] : null, null);
                    else if (p.PropertyType.Name.ToUpper() == "INT32")
                        p.SetValue(this, (record[index].GetType().Name != "DBNull") ? record[index] : null, null);
                    else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                    {

                        if ((record[index].GetType() == typeof(int)) || (record[index].GetType() == typeof(long)))
                            p.SetValue(this, UNIXTimeToDateTime(System.Convert.ToInt32(record[index])), null);
                        else if (record[index].GetType() == typeof(string))
                            p.SetValue(this, DateTime.ParseExact(record[index].ToString(), "yyyy-MM-dd HH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime(), null);
                        else
                            p.SetValue(this, record[index], null);
                    }
                    else if (p.PropertyType.BaseType.Name.ToUpper() == "ENUM")
                        p.SetValue(this, System.Convert.ToInt32(record[index]), null);
                    else
                        p.SetValue(this, ByteArrayToObject((byte[])record[index]), null);
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Converts object to JSON string
        /// </summary>
        /// <returns>JSON string representing the object.</returns>
        public string ToJSON()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            settings.Culture = CultureInfo.InvariantCulture;
            string json = JsonConvert.SerializeObject(this, settings);
            return json;
        }

        /// <summary>
        /// Creates an object of .NET type from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <returns>Object of .NET type created from JSON string</returns>
        public static T FromJSON<T>(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.FloatFormatHandling = FloatFormatHandling.String;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            settings.Culture = CultureInfo.InvariantCulture;
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Creates an object of .NET type from a JSON file.
        /// </summary>
        /// <param name="json">The JSON file.</param>
        /// <returns>Object of .NET type created from JSON file.</returns>
        /// <summary>
        public static T FromJSONFile<T>(string filename)
        {
            string json = "";
            using (StreamReader sr = new StreamReader(File.OpenRead(filename)))
            {
                json = sr.ReadToEnd();
            }
            return FromJSON<T>(json);
        }

        /// Converts an object to an array of bytes.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>Array of bytes representing the object.</returns>
        private static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts an array of bytes into an object.
        /// </summary>
        /// <param name="arr">The array of bytes representing the object.</param>
        /// <returns>The object.</returns>
        private static Object ByteArrayToObject(byte[] arr)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arr, 0, arr.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        /// <summary>
        /// Converts a DateTime into UNIX Epoch time
        /// Handles MinValue and MaxValue correctly
        /// </summary>
        /// <param name="dt">The DateTime to be converted.</param>
        /// <returns>The UNIX Epoch time. Fractional seconds will be lost.</returns>
        public static int DateTimeToUNIXTime(DateTime dt)
        {
            if (dt == DateTime.MinValue)
                return int.MinValue;
            else if (dt == DateTime.MaxValue)
                return int.MaxValue;
            return (int)(dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// Converts a UNIX Epoch time into DateTime
        /// Handles MinValue and MaxValue correctly
        /// </summary>
        /// <param name="dt">The UNIX Epoch time to be converted.</param>
        /// <returns>The DateTime (in UTC).</returns>
        public static DateTime UNIXTimeToDateTime(int ut)
        {
            if (ut == int.MinValue)
                return DateTime.MinValue;
            else if (ut == int.MaxValue)
                return DateTime.MaxValue;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(ut);
        }

        public SQLiteParameter AsString(string name)
        {
            object obj = this.GetType().GetProperty(name).GetValue(this, null);
            SQLiteParameter par = new SQLiteParameter(DbType.String, obj);
            par.ParameterName = "@" + name;
            return par;
        }

        public SQLiteParameter AsInt32(string name)
        {
            object obj = this.GetType().GetProperty(name).GetValue(this, null);
            SQLiteParameter par = new SQLiteParameter(DbType.Int32, obj);
            par.ParameterName = "@" + name;
            return par;
        }

        public SQLiteParameter AsSingle(string name)
        {
            object obj = System.Convert.ToSingle(this.GetType().GetProperty(name).GetValue(this, null));
            SQLiteParameter par = new SQLiteParameter(DbType.Single, obj);
            par.ParameterName = "@" + name;
            return par;
        }

        public SQLiteParameter AsDouble(string name)
        {
            object obj = System.Convert.ToDouble(this.GetType().GetProperty(name).GetValue(this, null));
            SQLiteParameter par = new SQLiteParameter(DbType.Double, obj);
            par.ParameterName = "@" + name;
            return par;
        }

        public SQLiteParameter AsUNIXTime(string name)
        {
            object obj = this.GetType().GetProperty(name).GetValue(this, null);
            int time = SQLiteEntry.DateTimeToUNIXTime((DateTime)obj);
            // don't forget to cast integer back to object, otherwise we will get a wrong function call SQLiteParameter(DBType type, int Parametersize)!
            SQLiteParameter par = new SQLiteParameter(DbType.Int32,(object)time);
            par.ParameterName = "@" + name;
            return par;
        }

        public SQLiteParameter AsBinary (string name)
        {
            object obj = this.GetType().GetProperty(name).GetValue(this, null);
            byte[] b = ObjectToByteArray(obj);
            SQLiteParameter par = new SQLiteParameter(name, DbType.Binary);
            par.Size = b.Length;
            par.Value = b;
            return par;
        }
    }

}
