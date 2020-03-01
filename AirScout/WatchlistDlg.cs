using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScoutBase.Stations;

namespace AirScout
{
    public partial class WatchlistDlg : Form
    {
        List<LocationDesignator> AllLocations = new List<LocationDesignator>();

        DataTable AllCallsigns = new DataTable();
        DataTable SelectedCallsigns = new DataTable();

        public WatchlistDlg()
        {
            InitializeComponent();
            AllCallsigns.Columns.Add("Call");
            AllCallsigns.Columns.Add("Locator");
            AllCallsigns.DefaultView.Sort = "Call ASC";
            SelectedCallsigns.Columns.Add("Call");
            SelectedCallsigns.Columns.Add("Locator");
            SelectedCallsigns.DefaultView.Sort = "Call ASC";
        }

        private void WatchlistDlg_Load(object sender, EventArgs e)
        {

        }

        private void WatchlistDlg_Shown(object sender, EventArgs e)
        {
            // initially fill tables
            bw_Watchlist_Fill.RunWorkerAsync();
        }

        private void FillCallsigns (string filter)
        {
            try
            {
                AllCallsigns.Rows.Clear();
                dgv_Watchlist_Callsigns.DataSource = null;
                foreach (LocationDesignator ld in AllLocations)
                {
                    DataRow row = AllCallsigns.NewRow();
                    row[0] = ld.Call;
                    row[1] = ld.Loc;
                    AllCallsigns.Rows.Add(row);
                }
                dgv_Watchlist_Callsigns.DataSource = AllCallsigns;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Watchlist.FillItems: " + ex.ToString());
            }
        }

        private void FillSelected(string filter)
        {
            try
            {
                SelectedCallsigns.Rows.Clear();
                dgv_Watchlist_Selected.DataSource = null;
                foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
                {
                    DataRow row = SelectedCallsigns.NewRow();
                    row[0] = item.Call;
                    row[1] = item.Loc;
                    SelectedCallsigns.Rows.Add(row);
                }
                dgv_Watchlist_Selected.DataSource = SelectedCallsigns;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Watchlist.FillItems: " + ex.ToString());
            }
        }

        private void btn_Watchlist_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Watchlist_Callsigns.SelectedRows != null)
                {
                    foreach (DataGridViewRow selectedrow in dgv_Watchlist_Callsigns.SelectedRows)
                    {
                        // search row content in selected calls first
                        string sql = "Call = '" + selectedrow.Cells[0].Value.ToString() + "' AND Locator = '" + selectedrow.Cells[1].Value.ToString() + "'";
                        DataRow[] result = SelectedCallsigns.Select(sql);
                        if ((result == null) || (result.Length == 0))
                        {
                            DataRow row = SelectedCallsigns.NewRow();
                            row[0] = selectedrow.Cells[0].Value;
                            row[1] = selectedrow.Cells[1].Value;
                            SelectedCallsigns.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Watchlist_Remove_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_Watchlist_Selected.SelectedRows != null)
                {
                    // keep the selected rows in a separate List as the selection changes dynamically when deleting rows from table
                    List<DataRow> SelectedRows = new List<DataRow>();
                    foreach (DataGridViewRow selectedrow in dgv_Watchlist_Selected.SelectedRows)
                    {
                        DataRow row = SelectedCallsigns.NewRow();
                        row[0] = selectedrow.Cells[0].Value.ToString();
                        row[1] = selectedrow.Cells[1].Value.ToString();
                        SelectedRows.Add(row);
                    }
                    SelectedCallsigns.AcceptChanges();
                    foreach (DataRow selectedrow in SelectedRows)
                    {
                        foreach (DataRow row in SelectedCallsigns.Rows)
                        {
                            if ((row[0].ToString() == selectedrow[0].ToString()) && (row[1].ToString() == selectedrow[1].ToString()))
                            {
                                // row found --> delete row in table, commit changes immediately and exit iteration
                                row.Delete();
                                SelectedCallsigns.AcceptChanges();
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void btn_Watchlist_RemoveAll_Click(object sender, EventArgs e)
        {
            SelectedCallsigns.Rows.Clear();
        }

        private void tb_Watchlist_Callsigns_TextChanged(object sender, EventArgs e)
        {
            try
            {
                AllCallsigns.DefaultView.RowFilter = "[Call] LIKE '" + tb_Watchlist_Callsigns.Text + "%'";
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        private void tb_Watchlist_Selected_TextChanged(object sender, EventArgs e)
        {
            SelectedCallsigns.DefaultView.RowFilter = "[Call] LIKE '" + tb_Watchlist_Selected.Text + "%'";
        }

        private void bw_Watchlist_Fill_DoWork(object sender, DoWorkEventArgs e)
        {
            bw_Watchlist_Fill.ReportProgress(0, "Lists are being populated. Please wait...");
            // get all available callsigns from database
            lock (AllLocations)
            {
                AllLocations = StationData.Database.LocationGetAll(bw_Watchlist_Fill, Properties.Settings.Default.MinLat, Properties.Settings.Default.MinLon, Properties.Settings.Default.MaxLat, Properties.Settings.Default.MaxLon);
            }
        }

        private void bw_Watchlist_Fill_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // report message
            if (e.ProgressPercentage <= 0)
            {
                tsl_Watchlist_Main.Text = (string)e.UserState;
                ss_Watchlist_Main.Refresh();
            }
        }

        private void bw_Watchlist_Fill_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tsl_Watchlist_Main.Text = "Adding callsigns to list. Please wait...";
            ss_Watchlist_Main.Refresh();
            FillCallsigns(tb_Watchlist_Callsigns.Text);
            FillSelected(tb_Watchlist_Selected.Text);
            tsl_Watchlist_Main.Text = "";
            btn_Watchlist_Add.Enabled = true;
            btn_Watchlist_Remove.Enabled = true;
            btn_Watchlist_RemoveAll.Enabled = true;
            btn_Watchlist_OK.Enabled = true;
            tsl_Watchlist_Main.Text = "Ready.";
            ss_Watchlist_Main.Refresh();
        }

        private void dgv_Watchlist_Callsigns_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;
            if (e.RowIndex < 0)
                return;
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            var r = e.CellBounds;
            e.Graphics.DrawLine(Pens.LightGray, r.Left, r.Top, r.Right, r.Top);
            e.Graphics.DrawLine(Pens.LightGray, r.Left, r.Top, r.Left, r.Bottom);
            e.Graphics.DrawLine(Pens.LightGray, r.Left, r.Bottom, r.Right, r.Bottom);
            e.Graphics.DrawLine(Pens.LightGray, r.Right, r.Top, r.Right, r.Bottom);
            e.Handled = true;
        }

        private void dgv_Watchlist_Selected_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;
            if (e.RowIndex < 0)
                return;
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            var r = e.CellBounds;
            e.Graphics.DrawLine(Pens.LightGray, r.Left, r.Top, r.Right, r.Top);
            e.Graphics.DrawLine(Pens.LightGray, r.Left, r.Top, r.Left, r.Bottom);
            e.Graphics.DrawLine(Pens.LightGray, r.Left, r.Bottom, r.Right, r.Bottom);
            e.Graphics.DrawLine(Pens.LightGray, r.Right, r.Top, r.Right, r.Bottom);
            e.Handled = true;
        }

        private void dgv_Watchlist_Callsigns_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_Watchlist_Add_Click(this, null);
        }

        private void dgv_Watchlist_Selected_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_Watchlist_Remove_Click(this, null);
        }
    }
}
