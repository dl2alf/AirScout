using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ScoutBase.CAT
{
    public class RigData
    {
        static RigDatabase rigs = new RigDatabase();
        public static RigDatabase Database
        {
            get
            {
                return rigs;
            }
        }
    }

    // holds the rig definitions database
    // this is a simple directory structure so far
    public class RigDatabase
    {
        public string DefaultDatabaseDirectory()
        {
            // create default database directory name
            // fully qualify path and adjust it to Windows/Linux notation
            // create directory if not exists
            // return directory string if needed
            string dir = Properties.Settings.Default.Database_Directory;
            // set default value if empty
            if (String.IsNullOrEmpty(dir))
                dir = "RigData";
            // fully qualify path if not rooted
            if (!System.IO.Path.IsPathRooted(dir))
            {
                // empty or incomplete settings --> create fully qulified standard path
                // collect entry assembly info
                Assembly ass = Assembly.GetExecutingAssembly();
                string company = "";
                string product = "";
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
                string rootdir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (!String.IsNullOrEmpty(company))
                    rootdir = Path.Combine(rootdir, company);
                if (!String.IsNullOrEmpty(product))
                    rootdir = Path.Combine(rootdir, product);
                dir = Path.Combine(rootdir, dir);
            }
            // replace Windows/Linux directory spearator chars
            dir = dir.Replace('\\', Path.DirectorySeparatorChar);
            dir = dir.Replace('/', Path.DirectorySeparatorChar);
            // create directory if not exists
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            return dir;
        }

    }


}
