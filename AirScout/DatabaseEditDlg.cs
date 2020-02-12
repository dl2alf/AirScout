using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScoutBase.Core;
using ScoutBase.Database;

namespace AirScout
{
    public partial class DatabaseEditDlg : Form
    {
        DataTable Table;
        int StartIndexI = 0;
        int StartIndexJ = 0;
        bool SomethingFound = false;
        public DatabaseEditDlg(DataTable table)
        {
            InitializeComponent();
            Table = table;
            dgv_Main.DataSource = Table;
        }

        private void tb_Search_TextChanged(object sender, EventArgs e)
        {
            // reset search
            StartIndexI = 0;
            StartIndexJ = 0;
            SomethingFound = false;
        }

        private void FindNext()
        {
            if (Table == null)
                return;
            if (String.IsNullOrEmpty(tb_Search.Text))
                return;
            StartIndexJ++;
            if (StartIndexJ > dgv_Main.Columns.Count - 1)
            {
                StartIndexJ = 0;
                StartIndexI = StartIndexI + 1;
                if (StartIndexI > dgv_Main.Rows.Count - 1)
                {
                    // end of table reached
                    if (SomethingFound)
                    {
                        if (MessageBox.Show("Reached the end of table.\nNo (more) occurencies found for: " + tb_Search.Text + "\n\nContinue search at the beginning of table?",
                            "Find Next", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;
                        // start at the beginning
                        StartIndexI = 0;
                        StartIndexJ = 0;
                    }
                    else
                    {
                        MessageBox.Show("No occurencies found for: " + tb_Search.Text, "Find Next", MessageBoxButtons.OK);
                        StartIndexI = 0;
                        StartIndexJ = 0;
                        SomethingFound = false;
                        return;
                    }
                }
            }
            for (int i = StartIndexI; i < dgv_Main.Rows.Count; i++)
            {
                for (int j = StartIndexJ; j < dgv_Main.Columns.Count; j++)
                {
                    if ((dgv_Main.Rows[i].Cells[j].Value != null) && (dgv_Main.Rows[i].Cells[j].Value.ToString().ToUpper().IndexOf(tb_Search.Text.ToUpper()) >= 0))
                    {
                        dgv_Main.CurrentCell = dgv_Main.Rows[i].Cells[j];
                        StartIndexI = dgv_Main.CurrentCell.RowIndex;
                        StartIndexJ = dgv_Main.CurrentCell.ColumnIndex;
                        SomethingFound = true;
                        return;
                    }
                }
                StartIndexJ = 0;
            }
            // set current cell to null if nothing found
            dgv_Main.CurrentCell = null;
            StartIndexI = dgv_Main.Rows.Count - 1;
            StartIndexJ = dgv_Main.Columns.Count - 1;
            FindNext();
        }
        private void btn_FindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void DatabaseEditDlg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                FindNext();
        }

        private void dgv_Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                FindNext();
        }

        private void tb_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                FindNext();
        }

        private void btn_FindNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
                FindNext();
        }

        private void dgv_Main_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                // update timestamp, if column "LastUpdated" is available
                dgv_Main.Rows[dgv_Main.CurrentCell.RowIndex].Cells["LastUpdated"].Value = SupportFunctions.DateTimeToUNIXTime(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void dgv_Main_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // currently not implemented
        }

        private void dgv_Main_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            // currently not implemented
        }

        private void dgv_Main_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // occurs when a BLOB was read from database
            // do nothing
        }
    }
}
