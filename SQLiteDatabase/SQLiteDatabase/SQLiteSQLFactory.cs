using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SQLite;

namespace System.Data.SQLite
{
    public static class SQLiteSQLFactory
    {

        public static string TableNameParameter = "@TableName";

        public static string SQLCreateTable(Type T)
        {
            // build static sqlcreatetable command
            // get member variables according to type
            FieldInfo field;
            //            field = T.GetField(nameof(SQLiteEntry.TableName));
            //            string tablename = (string)field.GetValue(T);
            string tablename = SQLiteSQLFactory.TableNameParameter;
            field = T.GetField(nameof(SQLiteEntry.PrimaryKeys));
            List <SQLitePrimaryKey> primarykeys = (List<SQLitePrimaryKey>)field.GetValue(T);
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("CREATE TABLE `");
            sb.Append(tablename);
            sb.Append("`(");
            int i = 1;
            PropertyInfo[] properties = T.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                // append property names as column names
                sb.Append(p.Name);
                sb.Append(" ");
                // append data type --> replace STRING with TEXT and DOUBLE with REAL
                if (p.PropertyType.Name.ToUpper() == "STRING")
                    sb.Append("TEXT");
                else if (p.PropertyType.Name.ToUpper() == "DOUBLE")
                    sb.Append("REAL");
                else if (p.PropertyType.Name.ToUpper() == "INT32")
                    sb.Append("INT32");
                else if (p.PropertyType.Name.ToUpper() == "DATETIME")
                    //                sb.Append("TEXT");
                    sb.Append("INT32");
                else sb.Append("BLOB");
                foreach (SQLitePrimaryKey key in primarykeys)
                {
                    if (key.Name == p.Name)
                    {
                        // append not null statement
                        sb.Append(" NOT NULL");
                        // maintain default value --> null is not allowed at primary key
                        Type t = p.PropertyType;
                        if (t.IsValueType)
                        {
                            sb.Append(" DEFAULT " + Activator.CreateInstance(t).ToString());
                        }
                        else if (t == typeof(string))
                        {
                            sb.Append(" DEFAULT ''");
                        }
                        break;
                    }
                }
                if (i < properties.Length)
                    sb.Append(", ");
                i++;
            }
            //append primary keys if any
            if (primarykeys.Count > 0)
            {
                i = 1;
                sb.Append(", PRIMARY KEY (");
                foreach (SQLitePrimaryKey key in primarykeys)
                {
                    sb.Append(key.Name);
                    if (i < primarykeys.Count)
                        sb.Append(", ");
                    else
                        sb.Append(")");
                    i++;
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static string SQLInsert (Type T)
        {
            FieldInfo field;
            //            field = T.GetField(nameof(SQLiteEntry.TableName));
            //            string tablename = (string)field.GetValue(T);
            string tablename = SQLiteSQLFactory.TableNameParameter;
            field = T.GetField(nameof(SQLiteEntry.PrimaryKeys));
            List<SQLitePrimaryKey> primarykeys = (List<SQLitePrimaryKey>)field.GetValue(T);
            string sql = "";
            // build INSERT command
            int i = 1;
            StringBuilder sbi = new StringBuilder();
            sbi.Append("INSERT INTO " + tablename + " (");
            StringBuilder sbv = new StringBuilder();
            sbv.Append("(");
            PropertyInfo[] properties = T.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                // append property names as column names
                sbi.Append(p.Name);
                // append property names as parameter names --> @PropertyName
                sbv.Append("@" + p.Name);
                if (i < properties.Length)
                {
                    sbi.Append(", ");
                    sbv.Append(", ");
                }
                i++;
            }
            sbi.Append(")");
            sbv.Append(")");
            // first line
            sql = sbi.ToString() + " VALUES " + sbv.ToString();
            return sql;
        }

        public static string SQLUpdate (Type T)
        {
            FieldInfo field;
            //            field = T.GetField(nameof(SQLiteEntry.TableName));
            //            string tablename = (string)field.GetValue(T);
            string tablename = SQLiteSQLFactory.TableNameParameter;
            field = T.GetField(nameof(SQLiteEntry.PrimaryKeys));
            List<SQLitePrimaryKey> primarykeys = (List<SQLitePrimaryKey>)field.GetValue(T);
            string sql = "";
            StringBuilder sbu = new StringBuilder();
            sbu.Append("UPDATE " + tablename + " SET ");
            StringBuilder sbv = new StringBuilder();
            sbv.Append("(");
            PropertyInfo[] properties = T.GetProperties();
            int i = 1;
            foreach (PropertyInfo p in properties)
            {
                // append property names as column names
                sbu.Append(p.Name + " = @" + p.Name);
                // append property names as parameter names --> @PropertyName
                sbv.Append("@" + p.Name);
                if (i < properties.Length)
                {
                    sbv.Append(", ");
                    sbu.Append(", ");
                }
                i++;
            }
            sbv.Append(")");
            // build where clause from primary keys
            StringBuilder sbw = new StringBuilder();
            i = 1;
            foreach (SQLitePrimaryKey key in primarykeys)
            {
                sbw.Append(key.Name + " = @" + key.Name);
                if (i < primarykeys.Count)
                    sbw.Append(" AND ");
                i++;
            }
            sql = sbu.ToString() + " WHERE " + sbw.ToString();
            return sql;
        }

        public static string SQLFindLastUpdated (Type T)
        {
            // build static sqlfind command
            // get member variables according to type
            FieldInfo field;
            //            field = T.GetField(nameof(SQLiteEntry.TableName));
            //            string tablename = (string)field.GetValue(T);
            string tablename = SQLiteSQLFactory.TableNameParameter;
            field = T.GetField(nameof(SQLiteEntry.PrimaryKeys));
            List<SQLitePrimaryKey> primarykeys = (List<SQLitePrimaryKey>)field.GetValue(T);
            if (primarykeys.Count <= 0)
                return "";
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT LastUpdated FROM ");
            sb.Append(tablename);
            sb.Append(" WHERE ");
            int i = 1;
            foreach (SQLitePrimaryKey key in primarykeys)
            {
                // append primary key as column name
                sb.Append(key.Name);
                sb.Append(" = ");
                // append @ + primary key as compare criterion
                sb.Append("@" + key.Name);
                if (i < primarykeys.Count)
                    sb.Append(" AND ");
                i++;
            }
            return sb.ToString();
        }

        public static string SQLExists(Type T)
        {
            // build static sqlfind command
            // get member variables according to type
            FieldInfo field;
            //            field = T.GetField(nameof(SQLiteEntry.TableName));
            //            string tablename = (string)field.GetValue(T);
            string tablename = SQLiteSQLFactory.TableNameParameter;
            field = T.GetField(nameof(SQLiteEntry.PrimaryKeys));
            List<SQLitePrimaryKey> primarykeys = (List<SQLitePrimaryKey>)field.GetValue(T);
            if (primarykeys.Count <= 0)
                return "";
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT EXISTS (SELECT LastUpdated FROM ");
            sb.Append(tablename);
            sb.Append(" WHERE ");
            int i = 1;
            foreach (SQLitePrimaryKey key in primarykeys)
            {
                // append primary key as column name
                sb.Append(key.Name);
                sb.Append(" = ");
                // append @ + primary key as compare criterion
                sb.Append("@" + key.Name);
                if (i < primarykeys.Count)
                    sb.Append(" AND ");
                i++;
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static string SQLFind(Type T)
        {
            // build static sqlfind command
            // get member variables according to type
            FieldInfo field;
            //            field = T.GetField(nameof(SQLiteEntry.TableName));
            //            string tablename = (string)field.GetValue(T);
            string tablename = SQLiteSQLFactory.TableNameParameter;
            field = T.GetField(nameof(SQLiteEntry.PrimaryKeys));
            List<SQLitePrimaryKey> primarykeys = (List<SQLitePrimaryKey>)field.GetValue(T);
            if (primarykeys.Count <= 0)
                return "";
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("SELECT * FROM ");
            sb.Append(tablename);
            sb.Append(" WHERE ");
            int i = 1;
            foreach (SQLitePrimaryKey key in primarykeys)
            {
                // append primary key as column name
                sb.Append(key.Name);
                sb.Append(" = ");
                // append @ + primary key as compare criterion
                sb.Append("@" + key.Name);
                if (i < primarykeys.Count)
                    sb.Append(" AND ");
                i++;
            }
            return sb.ToString();
        }

        public static List<SQLitePrimaryKey> FillPrimaryKeys(Type T, List<string> primarykeys)
        {
            List<SQLitePrimaryKey> l = new List<SQLitePrimaryKey>();
            // return an empty list if no primary key
            if (primarykeys.Count <= 0)
                return l;
            // find property according to each primary key
            foreach (string key in primarykeys)
            {
                SQLitePrimaryKey par = new SQLitePrimaryKey();
                par.Name = key;
                // try to find a property first
                PropertyInfo prop = T.GetProperty(key);
                if (prop != null)
                {
                    par.KeyType = prop.PropertyType;
                }
                else
                {
                    // not found --> try to finsd a field
                    FieldInfo field = T.GetField(key);
                    par.KeyType = field.FieldType;
                }
                if (par.KeyType == typeof(int))
                    par.DBType = DbType.Int32;
                else if (par.KeyType == typeof(double) || (par.KeyType == typeof(float)))
                    par.DBType = DbType.Single;
                else if (par.KeyType == typeof(string))
                    par.DBType = DbType.String;
                else if (par.KeyType == typeof(DateTime))
                    par.DBType = DbType.Int32;
                else
                    throw new ArgumentException("Invalid primary key type, conversion not supported: " + key + "[" + par.KeyType.ToString() + "]");
                l.Add(par);
            }
            return l;
        }
    }
}
