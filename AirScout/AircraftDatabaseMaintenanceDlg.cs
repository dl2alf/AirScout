using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using AirScout.AircraftPositions;
using AirScout.Aircrafts;
using ScoutBase.Core;
using ScoutBase.Stations;

namespace AirScout
{
    public partial class AircraftDatabaseMaintenanceDlg : Form
    {
        public AircraftDatabaseMaintenanceDlg()
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
