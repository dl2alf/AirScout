using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data.SQLite
{
    public class SQLitePrimaryKey
    {
        public string Name { get; set; }
        public Type KeyType { get; set; }
        public DbType DBType { get; set; }

        public SQLitePrimaryKey()
        {
            Name = String.Empty;
            KeyType = null;
            DBType = DbType.Object;
        }

        public SQLitePrimaryKey(string name, Type type, DbType dbtype)
        {
            Name = name;
            KeyType = KeyType;
            DBType = dbtype;
        }
    }

}
