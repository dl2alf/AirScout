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
        public WatchlistDlg()
        {
            InitializeComponent();
        }

        private void WatchlistDlg_Load(object sender, EventArgs e)
        {

        }

        private void WatchlistDlg_Shown(object sender, EventArgs e)
        {
            tsl_Watchlist_Main.Text = "Please wait while lists are being populated...";
            ss_Watchlist_Main.Refresh();
            List<LocationDesignator> l = StationData.Database.LocationGetAll();
            lv_Watchlist_Callsigns.BeginUpdate();
            foreach (LocationDesignator ld in l)
            {
                ListViewItem item = new ListViewItem(ld.Call);
                item.Name = ld.Call;
                lv_Watchlist_Callsigns.Items.Add(item);
            }
            lv_Watchlist_Callsigns.EndUpdate();
            lv_Watchlist_Selected.BeginUpdate();
            lv_Watchlist_Selected.Items.Clear();
            foreach (WatchlistItem item in Properties.Settings.Default.Watchlist)
            {
                ListViewItem lvi = new ListViewItem(item.Call);
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, item.Loc));
                if (item.Checked)
                    lvi.Checked = true;
                if (item.Selected)
                    lvi.Selected = true;
                lv_Watchlist_Selected.Items.Add(lvi);
                lv_Watchlist_Selected.Sort();
            }
            lv_Watchlist_Selected.EndUpdate();
            tsl_Watchlist_Main.Text = "";
        }

        private void btn_Watchlist_Add_Click(object sender, EventArgs e)
        {
            if(lv_Watchlist_Callsigns.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lv_Watchlist_Callsigns.SelectedItems)
                {
                    ListViewItem newitem = new ListViewItem(item.Name);
                    newitem.Name = item.Name;
                    lv_Watchlist_Selected.Items.Add(newitem);
                }
            }
        }

        private void btn_Watchlist_Remove_Click(object sender, EventArgs e)
        {
            if (lv_Watchlist_Selected.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lv_Watchlist_Selected.SelectedItems)
                {
                    try
                    {
                        lv_Watchlist_Selected.Items.RemoveByKey(item.Name);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void btn_Watchlist_RemoveAll_Click(object sender, EventArgs e)
        {
            lv_Watchlist_Selected.Items.Clear();
        }

        private void lv_Watchlist_Callsigns_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_Watchlist_Add_Click(this, null);
        }

        private void lv_Watchlist_Selected_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_Watchlist_Remove_Click(this, null);
        }
    }
}
