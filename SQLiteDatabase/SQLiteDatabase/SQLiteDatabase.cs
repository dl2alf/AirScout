// based on the SQLiteHelper poject
// Version 1.2
// Date: 2014-03-27
// http://sh.codeplex.com
// Dedicated to Public Domain
//
// modified by DL2ALF 2016

using System.Collections.Generic;
//
// SqliteDatabase.cs
//
// SQLite3 support simultaneous for Windows/Linux (Mono) without code change/recompilation
// Derived from Mono.Data.SqliteClient data access components for .Net
//
// Copyright (C) 2018  DL2ALF
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//using System.Data.Common;

using System.IO;
using System.Text;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Data;
using System.Runtime.InteropServices;
using System.Data.SQLite;
using System.Linq;

namespace System.Data.SQLite
{

    public class SQLiteDatabase
    {
        // contains real filename on disk if InMemory database
        public string DiskFileName = "";

        string dblocation;

        /// <summary>
        /// local database location
        /// ":memory:", if in-memory database
        /// </summary>
        public string DBLocation
        {
            get
            {
                if (isinmemory)
                    return ":memory:";
                else
                    return dblocation;
            }
        }

        bool isinmemory = false;

        /// <summary>
        /// indicates a in-memory database
        /// </summary>
        public bool IsInMemory
        {
            get
            {
                return isinmemory;
            }
        }


        /// <summary>
        /// local database connection string
        /// </summary>
        public string DBConnectionString
        {
            get
            {
                SQLiteConnectionStringBuilder b = new SQLiteConnectionStringBuilder();
                b.Version = 3;
                b.PageSize = 4096;
                b.UseUTF16Encoding = true;
                b.DataSource = DBLocation;
                return b.ConnectionString;
            }
        }

        /// <summary>
        /// local database logical file size in MB
        /// </summary>
        public double DBSize
        {
            get
            {
                if (isinmemory)
                    return 0;
                else
                {
                    try
                    {
                        FileInfo fi = new FileInfo(dblocation);
                        return fi.Length / (long)1024.0 / (long)1024.0;
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// Holds the database status
        /// Is set to UNDEFINED by default
        /// // Can be used by external procedure to manage database update process 
        /// </summary>
        public DATABASESTATUS Status { get; set; }

        public SQLiteConnection DBConnection = null;
        public SQLiteCommand DBCommand = null;

        public SQLiteDatabase() : this(null) { }
        public SQLiteDatabase(string filename) 
        {
            if (String.IsNullOrEmpty(filename))
            {
                // create a default database file name
                filename = "db.db3";
            }
            // handle special filenames
            if (filename == ":memory:")
            {
                isinmemory = true;
                dblocation = filename;
                return;
            }
            // check if filename contains path
            string directory = Path.GetDirectoryName(filename);
            if (String.IsNullOrEmpty(directory))
            {
                // create default database directory name
                // collect entry assembly info
                Assembly ass = Assembly.GetCallingAssembly();
                string name = ass.GetName().Name;
                string product = "";
                string company = "";
                object[] attribs;
                attribs = ass.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
                if (attribs.Length > 0)
                {
                    company = ((AssemblyCompanyAttribute)attribs[0]).Company;
                }
                attribs = ass.GetCustomAttributes(typeof(AssemblyProductAttribute), true);
                if (attribs.Length > 0)
                {
                    product = ((AssemblyProductAttribute)attribs[0]).Product;
                }
                // create database path
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!String.IsNullOrEmpty(company))
                    dir = Path.Combine(dir, company);
                if (!String.IsNullOrEmpty(product))
                    dir = Path.Combine(dir, product);
                directory = Path.Combine(dir, "Database");
                filename = Path.Combine(directory, filename);
            }
            dblocation = filename;
            // create an empty database if not file found
            if (!File.Exists(dblocation))
            {
                // check for existing directory first
                string dir = Path.GetDirectoryName(dblocation);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            // set status to UNDEFINED after initialization
            Status = DATABASESTATUS.UNDEFINED;
        }

        public void Open()
        {
            if (DBConnection != null)
                throw new InvalidOperationException("Cannot open database twice.");
            // check if database is initailly created
            DBConnection = new SQLiteConnection(DBConnectionString);
            DBCommand = new SQLiteCommand();
            DBCommand.Connection = DBConnection;
            Console.WriteLine("SQLiteDatabase.Open: connectionstring=" + DBConnection.ConnectionString);
            DBConnection.Open();
        }

        public void Close()
        {
            if (DBConnection == null)
                throw new InvalidOperationException("Database is closed already.");
            DBConnection.Close();
            DBConnection = null;
            DBCommand = null;
        }

        #region DB Info

        public DataTable GetTableStatus()
        {
            return Select("SELECT * FROM sqlite_master;");
        }

        public DataTable GetTableList()
        {
            DataTable dt = GetTableStatus();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Tables");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                /*
                string t = dt.Rows[i]["name"] + "";
                if (t != "sqlite_sequence")
                    dt2.Rows.Add(t);
                */
                if (dt.Rows[i]["type"].ToString() == "table")
                {
                    dt2.Rows.Add(dt.Rows[i]["name"].ToString());
                }
            }
            return dt2;
        }

        public bool TableExists (string tablename)
        {
            DataTable dt = Select("SELECT * FROM sqlite_master WHERE NAME = '" + tablename + "'");
            return ((dt != null) && (dt.Rows.Count > 0));
        }

        public int TableRowCount(string tablename)
        {
            try
            {
                DataTable dt = Select("SELECT Count(*) FROM " + tablename);
                return System.Convert.ToInt32(dt.Rows[0][0]);
            }
            catch
            {
            }
            return 0;
        }

        public DataTable GetColumnStatus(string tableName)
        {
            return Select(string.Format("PRAGMA table_info(`{0}`);", tableName));
        }

        public DataTable ShowDatabase()
        {
            return Select("PRAGMA database_list;");
        }

        public int GetUserVersion()
        {
            DataTable dt = Select("PRAGMA user_version;");
            if ((dt != null) && (dt.Rows.Count > 0))
                return System.Convert.ToInt32(dt.Rows[0][0]);
            return 0;
        }

        public void SetUserVerion(int version)
        {
            Execute("PRAGMA user_version = " + version.ToString());
        }

        public int GetSchemaVersion()
        {
            DataTable dt = Select("PRAGMA schema_version;");
            if ((dt != null) && (dt.Rows.Count > 0))
                return System.Convert.ToInt32(dt.Rows[0][0]);
            return 0;
        }

        public void SetSchemaVerion(int version)
        {
            Execute("PRAGMA schema_version = " + version.ToString());
        }

        public string GetTextEncoding()
        {
            DataTable dt = Select("PRAGMA encoding;");
            if ((dt != null) && (dt.Rows.Count > 0))
                return (dt.Rows[0][0].ToString());
            else return String.Empty;
        }

        public void SetTextEncoding(string encoding)
        {
            Execute("PRAGMA encoding = '" + encoding + "'");
        }

        public AUTOVACUUMMODE GetAutoVacuum()
        {
            DataTable dt = Select("PRAGMA auto_vacuum;");
            if ((dt != null) && (dt.Rows.Count > 0))
                return (AUTOVACUUMMODE)System.Convert.ToInt32(dt.Rows[0][0]);
            return 0;
        }

        public void SetAutoVacuum (AUTOVACUUMMODE mode)
        {
            // changes the auto vacuum pragma
            // command requires a VACUUM of the database to change layout
            // CAUTION: this requires time and disk space!
            try
            {
                Execute("PRAGMA auto_vacuum = " + ((int)mode).ToString() + "; VACUUM");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region Query

        public void BeginTransaction()
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = "BEGIN TRANSACTION;";
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }

        public void Commit()
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = "COMMIT;";
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }

        public void Rollback()
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = "ROLLBACK";
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }

        // simple sql select command returns a DataTable
        public DataTable Select (string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                lock (DBCommand)
                {
                    DBCommand.CommandText = sql;
                    DBCommand.Parameters.Clear();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(DBCommand);
                    da.Fill(dt);
                }
                // Linux/Mono hack --> cut column names to its right length
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    int j = 0;
                    while ((j < dt.Columns[i].ColumnName.Length) && (Char.IsLetter(dt.Columns[i].ColumnName[j]) || Char.IsDigit(dt.Columns[i].ColumnName[j])))
                        j++;
                    dt.Columns[i].ColumnName = dt.Columns[i].ColumnName.Substring(0, j);
                    Console.Write("'" + dt.Columns[i].ColumnName + "[" + dt.Columns[i].ColumnName.Length + "]', ");
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLiteDatabase.Select: fatal error while executing command <" + sql + ">\n\n" + ex.ToString());
            }
            return dt;
        }

        public DataTable Select (SQLiteCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                lock (cmd)
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLiteDatabase.Select: fatal error while executing command <" + cmd.CommandText + ">\n\n" + ex.ToString());
            }
            return dt;
        }

        public void Execute(SQLiteCommand cmd)
        {
            cmd.ExecuteNonQuery();
        }

        public void Execute(string sql)
        {
            Execute(sql, new List<SQLiteParameter>());
        }

        public void Execute(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = GetParametersList(dicParameters);
            Execute(sql, lst);
        }

        public void Execute(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = sql;
                if (parameters != null)
                {
                    DBCommand.Parameters.Clear();
                    foreach (var param in parameters)
                    {
                        DBCommand.Parameters.Add(param);
                    }
                }
                DBCommand.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery (SQLiteCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string sql)
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = sql;
                DBCommand.Parameters.Clear();
                return DBCommand.ExecuteScalar();
            }
        }

        public object ExecuteScalar (SQLiteCommand cmd)
        {
            List<SQLiteParameter> lst = new List<SQLiteParameter>();
            foreach (SQLiteParameter par in cmd.Parameters)
                lst.Add(par);
            return ExecuteScalar(cmd.CommandText, lst);
        }

        public object ExecuteScalar(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = GetParametersList(dicParameters);
            return ExecuteScalar(sql, lst);
        }

        public object ExecuteScalar(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            lock (DBCommand)
            { 
                DBCommand.CommandText = sql;
                if (parameters != null)
                {
                    DBCommand.Parameters.Clear();
                    foreach (var parameter in parameters)
                    {
                        DBCommand.Parameters.Add(parameter);
                    }
                }
                return DBCommand.ExecuteScalar();
            }
        }

        public dataType ExecuteScalar<dataType>(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = null;
            if (dicParameters != null)
            {
                lst = new List<SQLiteParameter>();
                foreach (KeyValuePair<string, object> kv in dicParameters)
                {
                    lst.Add(new SQLiteParameter(kv.Key, kv.Value));
                }
            }
            return ExecuteScalar<dataType>(sql, lst);
        }

        public dataType ExecuteScalar<dataType>(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = sql;
                if (parameters != null)
                {
                    DBCommand.Parameters.Clear();
                    foreach (var parameter in parameters)
                    {
                        DBCommand.Parameters.Add(parameter);
                    }
                }
                return (dataType)Convert.ChangeType(DBCommand.ExecuteScalar(), typeof(dataType));
            }
        }

        public dataType ExecuteScalar<dataType>(string sql)
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = sql;
                DBCommand.Parameters.Clear();
                return (dataType)Convert.ChangeType(DBCommand.ExecuteScalar(), typeof(dataType));
            }
        }

        private List<SQLiteParameter> GetParametersList(Dictionary<string, object> dicParameters)
        {
            List<SQLiteParameter> lst = new List<SQLiteParameter>();
            if (dicParameters != null)
            {
                foreach (KeyValuePair<string, object> kv in dicParameters)
                {
                    lst.Add(new SQLiteParameter(kv.Key, kv.Value));
                }
            }
            return lst;
        }

        public string Escape(string data)
        {
            data = data.Replace("'", "''");
            data = data.Replace("\\", "\\\\");
            return data;
        }

        public void InsertDataRow(string tableName, DataRow row)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DataColumn col in row.Table.Columns)
            {
                dict.Add(col.ColumnName, row[col]);
            }
            Insert(tableName, dict);
        }

        public void InsertOrReplaceDataRow(string tableName, DataRow row)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                dict.Add(row.Table.Columns[i].ColumnName, row[i]);
            }
            InsertOrReplace(tableName, dict);
        }

        public void Insert(string tableName, Dictionary<string, object> dic)
        {
            StringBuilder sbCol = new System.Text.StringBuilder();
            StringBuilder sbVal = new System.Text.StringBuilder();

            foreach (KeyValuePair<string, object> kv in dic)
            {
                if (sbCol.Length == 0)
                {
                    sbCol.Append("INSERT INTO ");
                    sbCol.Append(tableName);
                    sbCol.Append("(");
                }
                else
                {
                    sbCol.Append(",");
                }

                sbCol.Append("`");
                sbCol.Append(kv.Key);
                sbCol.Append("`");

                if (sbVal.Length == 0)
                {
                    sbVal.Append(" VALUES(");
                }
                else
                {
                    sbVal.Append(", ");
                }

                sbVal.Append("@v");
                sbVal.Append(kv.Key);
            }

            sbCol.Append(") ");
            sbVal.Append(");");

            lock (DBCommand)
            {
                DBCommand.CommandText = sbCol.ToString() + sbVal.ToString();
                DBCommand.Parameters.Clear();
                foreach (KeyValuePair<string, object> kv in dic)
                {
                    SQLiteParameter par = new SQLiteParameter("@v" + kv.Key, kv.Value);
                    DBCommand.Parameters.Add(par);
                }
                DBCommand.ExecuteNonQuery();
            }
        }

        public void InsertOrReplace(string tableName, Dictionary<string, object> dic)
        {
            StringBuilder sbCol = new System.Text.StringBuilder();
            StringBuilder sbVal = new System.Text.StringBuilder();

            foreach (KeyValuePair<string, object> kv in dic)
            {
                if (sbCol.Length == 0)
                {
                    sbCol.Append("INSERT OR REPLACE INTO ");
                    sbCol.Append(tableName);
                    sbCol.Append("(");
                }
                else
                {
                    sbCol.Append(",");
                }

                sbCol.Append("`");
                sbCol.Append(kv.Key);
                sbCol.Append("`");

                if (sbVal.Length == 0)
                {
                    sbVal.Append(" VALUES(");
                }
                else
                {
                    sbVal.Append(", ");
                }

                sbVal.Append("@v");
                sbVal.Append(kv.Key);
            }

            sbCol.Append(") ");
            sbVal.Append(");");
            lock (DBCommand)
            {
                DBCommand.CommandText = sbCol.ToString() + sbVal.ToString();
                DBCommand.Parameters.Clear();
                foreach (KeyValuePair<string, object> kv in dic)
                {
                    SQLiteParameter par = new SQLiteParameter("@v" + kv.Key, kv.Value);
                    DBCommand.Parameters.Add(par);
                }

                DBCommand.ExecuteNonQuery();
            }
        }

        public void Update(string tableName, Dictionary<string, object> dicData, string colCond, object varCond)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic[colCond] = varCond;
            Update(tableName, dicData, dic);
        }

        public void Update(string tableName, Dictionary<string, object> dicData, Dictionary<string, object> dicCond)
        {
            if (dicData.Count == 0)
                throw new Exception("dicData is empty.");

            StringBuilder sbData = new System.Text.StringBuilder();

            Dictionary<string, object> _dicTypeSource = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kv1 in dicData)
            {
                _dicTypeSource[kv1.Key] = null;
            }

            foreach (KeyValuePair<string, object> kv2 in dicCond)
            {
                if (!_dicTypeSource.ContainsKey(kv2.Key))
                    _dicTypeSource[kv2.Key] = null;
            }

            sbData.Append("UPDATE `");
            sbData.Append(tableName);
            sbData.Append("` SET ");

            bool firstRecord = true;

            foreach (KeyValuePair<string, object> kv in dicData)
            {
                if (firstRecord)
                    firstRecord = false;
                else
                    sbData.Append(",");

                sbData.Append("`");
                sbData.Append(kv.Key);
                sbData.Append("` = ");

                sbData.Append("@v");
                sbData.Append(kv.Key);
            }

            sbData.Append(" WHERE ");

            firstRecord = true;

            foreach (KeyValuePair<string, object> kv in dicCond)
            {
                if (firstRecord)
                    firstRecord = false;
                else
                {
                    sbData.Append(" AND ");
                }

                sbData.Append("`");
                sbData.Append(kv.Key);
                sbData.Append("` = ");

                sbData.Append("@c");
                sbData.Append(kv.Key);
            }

            sbData.Append(";");
            lock (DBCommand)
            {
                DBCommand.CommandText = sbData.ToString();
                DBCommand.Parameters.Clear();
                foreach (KeyValuePair<string, object> kv in dicData)
                {
                    SQLiteParameter par = new SQLiteParameter("@v" + kv.Key, kv.Value);
                    DBCommand.Parameters.Add(par);
                }

                foreach (KeyValuePair<string, object> kv in dicCond)
                {
                    SQLiteParameter par = new SQLiteParameter("@v" + kv.Key, kv.Value);
                    DBCommand.Parameters.Add(par);
                }
                DBCommand.ExecuteNonQuery();
            }
        }

        public long LastInsertRowId()
        {
            return ExecuteScalar<long>("SELECT last_insert_rowid();");
        }

        #endregion

        #region Utilities

        public void CreateTable(DataTable dt)
        {
            if (String.IsNullOrEmpty(dt.TableName))
                throw new InvalidOperationException("Table name cannot be empty.");
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("CREATE TABLE IF NOT EXISTS `");
            sb.Append(dt.TableName);
            sb.AppendLine("`(");

            bool firstRecord = true;

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName.Trim().Length == 0)
                {
                    throw new Exception("Column name cannot be blank.");
                }

                if (firstRecord)
                    firstRecord = false;
                else
                    sb.AppendLine(",");

                sb.Append(col.ColumnName);
                sb.Append(" ");

                if (col.AutoIncrement)
                {

                    sb.Append("INTEGER PRIMARY KEY AUTOINCREMENT");
                    continue;
                }
                if ((col.DataType.IsPrimitive || col.DataType.IsValueType) || (col.DataType == typeof(string)))
                {
                    // append data type --> replace STRING with TEXT and DOUBLE with REAL
                    sb.Append(col.DataType.Name.ToUpper().Replace("STRING", "TEXT").Replace("DOUBLE", "REAL"));
                }
                else
                {
                    // complex type --> append as BLOB
                    sb.Append("BLOB");
                }
                if (col.Unique)
                    sb.Append(" UNIQUE");
                if (!col.AllowDBNull)
                    sb.Append(" NOT NULL");
                if (col.DefaultValue.GetType() != typeof(DBNull))
                {
                    sb.Append(" DEFAULT ");

                    if (col.DataType == typeof(string) || col.DataType == typeof(DateTime))
                    {
                        sb.Append("'");
                        sb.Append(col.DefaultValue.ToString());
                        sb.Append("'");
                    }
                    else
                    {
                        sb.Append(col.DefaultValue.ToString());
                    }
                }
            }

            //append primary keys if any
            if ((dt.PrimaryKey != null) && (dt.PrimaryKey.Length > 0))
            {
                sb.Append(", PRIMARY KEY (");
                firstRecord = true; 
                foreach (DataColumn col in dt.PrimaryKey)
                {
                    if (col.ColumnName.Trim().Length == 0)
                    {
                        throw new Exception("Column name cannot be blank.");
                    }

                    if (firstRecord)
                        firstRecord = false;
                    else
                        sb.AppendLine(",");

                    sb.Append(col.ColumnName);
                    sb.Append(" ");
                }
                sb.Append(")");
            }
            sb.AppendLine(");");
            lock (DBCommand)
            {
                DBCommand.CommandText = sb.ToString();
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }

        public void ClearTable(string tablename)
        {
            BeginTransaction();
            Execute("DELETE FROM " + tablename);
            Commit();
        }

        public void InsertOrReplaceTable(DataTable dt)
        {
            // inserts all records from a data table or updates if exists
            // does not remove data rows if not in source table
            // will work with a unique key only
            if (String.IsNullOrEmpty(dt.TableName))
                throw new InvalidOperationException("Table name cannot be empty.");
            // check for primary key in database table
            DataTable info = GetColumnStatus(dt.TableName);
            bool hasprimarykey = false;
            foreach (DataRow row in info.Rows)
            {
                if (System.Convert.ToInt32(row["pk"]) > 0)
                {
                    hasprimarykey = true;
                    break;
                }
            }
            if (!hasprimarykey)
            if ((dt.PrimaryKey == null) || (dt.PrimaryKey.Length == 0))
                throw new InvalidOperationException("Data table must have a primary key.");
            BeginTransaction();
            foreach (DataRow row in dt.Rows)
                InsertOrReplaceDataRow(dt.TableName, row);
            Commit();
        }

        public void RenameTable(string tableFrom, string tableTo)
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = string.Format("ALTER TABLE `{0}` RENAME TO `{1}`;", tableFrom, tableTo);
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }


        public void CopyAllData(string tableFrom, string tableTo)
        {
            // get at least one row from tables
            DataTable src = Select(string.Format("SELECT * FROM `{0}` LIMIT 1;", tableFrom));
            // empty source table?? 
            if (src.Rows.Count == 0)
                return;
            DataTable dest;
            // empty dest table??
            if (TableRowCount(tableTo) == 0)
            {
                // add just one dummy row 
                Execute("INSERT INTO " + tableTo + " DEFAULT VALUES");
                // get the structure in a DataTable
                dest = Select(string.Format("SELECT * FROM `{0}` LIMIT 1;", tableTo));
                // remove the dummy row
                Execute("DELETE * FROM " + tableTo);
            }
            else
            {
                dest = Select(string.Format("SELECT * FROM `{0}` LIMIT 1;", tableTo));
            }
            Dictionary<string, bool> dic = new Dictionary<string, bool>();

            foreach (DataColumn dc in src.Columns)
            {
                if (dest.Columns.Contains(dc.ColumnName))
                {
                    if (!dic.ContainsKey(dc.ColumnName))
                    {
                        dic[dc.ColumnName] = true;
                    }
                }
            }

            foreach (DataColumn dc in dest.Columns)
            {
                if (src.Columns.Contains(dc.ColumnName))
                {
                    if (!dic.ContainsKey(dc.ColumnName))
                    {
                        dic[dc.ColumnName] = true;
                    }
                }
            }

            StringBuilder sb = new System.Text.StringBuilder();

            foreach (KeyValuePair<string, bool> kv in dic)
            {
                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append("`");
                sb.Append(kv.Key);
                sb.Append("`");
            }

            StringBuilder sb2 = new System.Text.StringBuilder();
            sb2.Append("INSERT INTO `");
            sb2.Append(tableTo);
            sb2.Append("`(");
            sb2.Append(sb.ToString());
            sb2.Append(") SELECT ");
            sb2.Append(sb.ToString());
            sb2.Append(" FROM `");
            sb2.Append(tableFrom);
            sb2.Append("`;");
            lock (DBCommand)
            { 
                DBCommand.CommandText = sb2.ToString();
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }

        public void DropTable(string table)
        {
            lock (DBCommand)
            {
                DBCommand.CommandText = string.Format("DROP TABLE IF EXISTS `{0}`", table);
                DBCommand.Parameters.Clear();
                DBCommand.ExecuteNonQuery();
            }
        }

        public void UpdateTableStructure(string targetTable, DataTable newStructure)
        {
            newStructure.TableName = targetTable + "_temp";

            CreateTable(newStructure);

            CopyAllData(targetTable, newStructure.TableName);

            DropTable(targetTable);

            RenameTable(newStructure.TableName, targetTable);
        }

        public void AttachDatabase(string database, string alias)
        {
            Execute(string.Format("ATTACH '{0}' AS {1};", database, alias));
        }

        public void DetachDatabase(string alias)
        {
            Execute(string.Format("DETACH {0};", alias));
        }

        public void BackupDatabase(string filename)
        {
            //backups a database to file
            // destination database must be open
            if (DBConnection == null)
                throw new InvalidOperationException("Database must be open for backup.");
            try
            {
                // return on empty filename
                if (String.IsNullOrEmpty(filename))
                    return;
                // check for existing directory first --> create on if not
                string dir = Path.GetDirectoryName(filename);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                // simply delete destination db if already exists
                if (File.Exists(filename))
                    File.Delete(filename);
                // get sql for creating structures
                string sql = "select sql from sqlite_master where name not like 'sqlite_%'";
                DataTable structure = this.Select(sql);
                // get user tables
                sql = "select name from sqlite_master where type = 'table' and name not like 'sqlite_%'";
                DataTable usertables = this.Select(sql);
                // crate identically structures in backup db
                SQLiteDatabase backupdb = new SQLiteDatabase(filename);
                backupdb.Open();
                foreach (DataRow row in structure.Rows)
                {
                    backupdb.DBCommand.CommandText = row[0].ToString();
                    backupdb.DBCommand.ExecuteNonQuery();
                }
                // close the backup db
                backupdb.Close();
                // attach backup db to source db 
                this.AttachDatabase(filename, "dest");
                this.BeginTransaction();
                // copy all user tables
                foreach (DataRow row in usertables.Rows)
                {
                    this.DBCommand.CommandText = "insert into dest." + row[0].ToString() + " select * from main." + row[0].ToString();
                    this.DBCommand.ExecuteNonQuery();
                }
                this.Commit();
                this.DetachDatabase("dest");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static SQLiteDatabase CreateInMemoryDB(string filename)
        {
            // returns a copy of a database in memory
            // create an empty memory db first
            SQLiteDatabase memdb = new SQLiteDatabase(":memory:");
            try
            {
                // open an existing on disk db
                SQLiteDatabase ondiskdb = new SQLiteDatabase(filename);
                ondiskdb.Open();
                // get user  & schema version
                int user_version = ondiskdb.GetUserVersion();
                int schema_version = ondiskdb.GetSchemaVersion();
                // get text encoding
                string text_encoding = ondiskdb.GetTextEncoding();
                // get sql for creating structures
                string sql = "select sql from sqlite_master where name not like 'sqlite_%'";
                DataTable structure = ondiskdb.Select(sql);
                // get user tables
                sql = "select name from sqlite_master where type = 'table' and name not like 'sqlite_%'";
                DataTable usertables = ondiskdb.Select(sql);
                // crate identically structures in memmory db
                memdb.Open();
                foreach (DataRow row in structure.Rows)
                {
                    memdb.DBCommand.CommandText = row[0].ToString();
                    memdb.DBCommand.ExecuteNonQuery();
                }
                // set user & schema version
                memdb.SetUserVerion(user_version);
                memdb.SetSchemaVerion(schema_version);
                // set text encoding
                memdb.SetTextEncoding(text_encoding);
                user_version = memdb.GetUserVersion();
                text_encoding = memdb.GetTextEncoding();
                // close on disk db
                ondiskdb.Close();
                // attach on disk db to memory db
                memdb.AttachDatabase(filename, "ondisk");
                memdb.BeginTransaction();
                foreach (DataRow row in usertables.Rows)
                {
                    memdb.DBCommand.CommandText = "insert into main." + row[0].ToString() + " select * from ondisk." + row[0].ToString();
                    memdb.DBCommand.ExecuteNonQuery();
                }
                memdb.Commit();
                memdb.DetachDatabase("ondisk");
                memdb.DiskFileName = filename;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return memdb;
        }

        #endregion

    }
}
