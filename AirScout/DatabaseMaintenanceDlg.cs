using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using ScoutBase.Core;
using ScoutBase.Database;
using ScoutBase.Elevation;
using ScoutBase.Propagation;

namespace AirScout
{
    public partial class DatabaseMaintenanceDlg : Form
    {
        ScoutBaseDatabase Database;
        System.Data.SQLite.SQLiteDatabase db = null;
        ELEVATIONMODEL Model;
        public DatabaseMaintenanceDlg( ScoutBaseDatabase database, ELEVATIONMODEL model = ELEVATIONMODEL.NONE)
        {
            InitializeComponent();
            lv_Tables.FullRowSelect = true;
            ListViewExtender extender = new ListViewExtender(lv_Tables);
            // extend 4th & 5th column for actions
            ListViewButtonColumn buttonAction1 = new ListViewButtonColumn(3);
            buttonAction1.Click += OnButtonAction1Click;
            buttonAction1.FixedWidth = true;
            // add extender
            extender.AddColumn(buttonAction1);

            ListViewButtonColumn buttonAction2 = new ListViewButtonColumn(4);
            buttonAction2.Click += OnButtonAction2Click;
            buttonAction1.FixedWidth = true;
            // add extender
            extender.AddColumn(buttonAction2);
            // add items
            Database = database;
            // get SQLite database to use for, use null füor default
            if (database.GetType() == typeof(ElevationDatabase))
                db = ((ElevationDatabase)Database).GetElevationDatabase(model);
            if (database.GetType() == typeof(PropagationDatabase))
                db = ((PropagationDatabase)Database).GetPropagationDatabase(model);
            Model = model;
            this.Text = Database.Name;
            this.lbl_Description.Text = Database.Description;
            DataTable dt = Database.GetTableList(db);
            foreach (DataRow row in dt.Rows)
            {
                // add items
                string tablename = row[0].ToString();
                ListViewItem lvi_Location = lv_Tables.Items.Add(tablename);
                lvi_Location.SubItems.Add(Database.GetTableDescription(tablename));
                lvi_Location.SubItems.Add(Database.GetTableRowCount(tablename, db).ToString());
                lvi_Location.SubItems.Add("Delete All");
                lvi_Location.SubItems.Add("View");
            }
        }
        private void OnButtonAction1Click(object sender, ListViewColumnMouseEventArgs e)
        {
            if (e.Item != null)
            {
                if (MessageBox.Show("Do you really want to delete all entries of table " + e.Item.Text + "?", "Delete All Entries", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Database.ClearTable(e.Item.Text, db);
                }
            }
        }
        private void OnButtonAction2Click(object sender, ListViewColumnMouseEventArgs e)
        {
            if (e.Item != null)
            {
                if (MessageBox.Show("CAUTION! Pressing \"View\" on large databases may take a long time to open the edit dialog.\n\nDo you really want to continue?", "Database Edit", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                DataTable dt;
                string tablename = e.Item.Text;
                string columns = "";
                // fill DataTable with all fields except BLOBs
                // get table structure, very basic approach 
                SQLiteConnection conn = Database.GetDBConnection(db);
                Console.WriteLine("Reading columns for table: " + tablename);
                try
                {
                    dt = conn.GetSchema("Columns");
                    foreach (DataColumn col in dt.Columns)
                    {
                        Console.Write("[" + col.ColumnName + "] ");
                    }
                    Console.WriteLine("\nDumping content:");
                    foreach (DataRow row in dt.Rows)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                            Console.Write(row[j].ToString() + ",");
                        Console.WriteLine();
                        // extract all column names and types from given table
                        if (row["TABLE_NAME"].ToString().ToLower() == tablename.ToLower())
                        {
                            if (row["DATA_TYPE"].ToString().ToLower() != "blob")
                            {
                                columns = columns + row["COLUMN_NAME"].ToString() + ",";
                            }
                        }
                    }
                    // remove trailing ","
                    if (columns.EndsWith(","))
                        columns = columns.Remove(columns.Length - 1);
                    // Fill DataTable when at least one column is found
                    if (!string.IsNullOrEmpty(columns))
                    {
                        // fill DataTable
                        dt = new DataTable();
                        string sql = "SELECT " + columns + " FROM " + tablename;
                        Console.WriteLine("Initializing DataAdapter: " + sql);
                        using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, conn))
                        {
                            dataAdapter.Fill(dt);
                            DatabaseEditDlg Dlg = new DatabaseEditDlg(dt);
                            Dlg.ShowDialog();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

    }
}
