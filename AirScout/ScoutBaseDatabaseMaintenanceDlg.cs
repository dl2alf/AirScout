using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;using ScoutBase.Core;
using ScoutBase.Stations;

namespace AirScout
{
    public partial class ScoutBaseDatabaseMaintenanceDlg : Form
    {
        public ScoutBaseDatabaseMaintenanceDlg()
        {
            InitializeComponent();
            lv_Tables.FullRowSelect = true;
            ListViewExtender extender = new ListViewExtender(lv_Tables);
            // extend 4th column
            ListViewButtonColumn buttonAction = new ListViewButtonColumn(3);
            buttonAction.Click += OnButtonActionClick;
            buttonAction.FixedWidth = true;
            // add extender
            extender.AddColumn(buttonAction);
            // add items
            ListViewItem lvi_Location = lv_Tables.Items.Add(LocationDesignator.TableName);
            lvi_Location.SubItems.Add("Location info");
            lvi_Location.SubItems.Add(StationData.Database.LocationCount().ToString());
            lvi_Location.SubItems.Add("Delete All Entries");
            ListViewItem lvi_QRV = lv_Tables.Items.Add(QRVDesignator.TableName);
            lvi_QRV.SubItems.Add("QRV info");
            lvi_QRV.SubItems.Add(StationData.Database.QRVCount().ToString());
            lvi_QRV.SubItems.Add("Delete All");

        }

        private void OnButtonActionClick(object sender, ListViewColumnMouseEventArgs e)
        {
            if ((e.Item != null) && (e.Item.Text == LocationDesignator.TableName))
                StationData.Database.LocationDeleteAll();
            else if ((e.Item != null) && (e.Item.Text == QRVDesignator.TableName))
                StationData.Database.QRVDeleteAll();
        }
    }


}
