using ScoutBase.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AirScout.Core
{
    public class BandSetting
    {
        public double K_Factor;
        public double F1_Clearance;
        public double GroundClearance;
        public double MaxDistance;
        public double MaxSquint;
        public double MaxElevation;

        public BandSetting()
        {
            K_Factor = 1.33;
            F1_Clearance = 0.6;
            GroundClearance = 0;
            MaxDistance = 10;
            MaxSquint = double.MaxValue;
            MaxElevation = double.MaxValue;
        }
    }

    [Serializable]
    public class BandSettings : DataTable
    {

        public BandSettings() : base("BANDSETTINGS")
        {
            this.Columns.Add("BAND");
            this.Columns.Add("K-FACTOR", typeof(double));
            this.Columns.Add("F1-CLEARANCE", typeof(double));
            this.Columns.Add("GROUNDCLEARANCE", typeof(double));
            this.Columns.Add("MAXDISTANCE", typeof(double));
            this.Columns.Add("MAXSQUINT", typeof(double));
            this.Columns.Add("MAXELEVATION", typeof(double));
            DataColumn[] keys = new DataColumn[1];
            keys[0] = this.Columns["BAND"];
            this.PrimaryKey = keys;
        }

        public BandSettings(bool generatedefault) : base("BANDSETTINGS")
        {
            this.Columns.Add("BAND");
            this.Columns.Add("K-FACTOR", typeof(double));
            this.Columns.Add("F1-CLEARANCE", typeof(double));
            this.Columns.Add("GROUNDCLEARANCE", typeof(double));
            this.Columns.Add("MAXDISTANCE", typeof(double));
            this.Columns.Add("MAXSQUINT", typeof(double));
            this.Columns.Add("MAXELEVATION", typeof(double));
            DataColumn[] keys = new DataColumn[1];
            keys[0] = this.Columns["BAND"];
            this.PrimaryKey = keys;
            if (generatedefault)
                GenerateDefault();
        }

        private void GenerateDefault()
        {
            if (this.Rows.Count > 0)
                return;
            // generate default rows
            BAND[] bands = Bands.GetValuesExceptNoneAndAll();
            foreach (BAND band in bands)
            {
                if (band != BAND.BNONE)
                {
                    DataRow row;
                    row = this.NewRow();
                    row["BAND"] = Bands.GetStringValue(band);
                    row["GROUNDCLEARANCE"] = Properties.Settings.Default.Path_Default_Ground_Clearance;
                    row["MAXDISTANCE"] = Properties.Settings.Default.Path_Default_Max_Distance;
                    row["MAXSQUINT"] = Properties.Settings.Default.Path_Default_Max_Squint;
                    row["MAXELEVATION"] = Properties.Settings.Default.Path_Default_Max_Elevation;
                    // adjust values starting with V1.3.0.4 
                    switch (band)
                    {
                        case BAND.B50M:
                            {
                                row["K-FACTOR"] = 1.6;
                                row["F1-CLEARANCE"] = 0.1;
                                break;
                            }
                        case BAND.B70M:
                            {
                                row["K-FACTOR"] = 1.6;
                                row["F1-CLEARANCE"] = 0.1;
                                break;
                            }
                        case BAND.B144M:
                            {
                                row["K-FACTOR"] = 1.5;
                                row["F1-CLEARANCE"] = 0.2;
                                break;
                            }
                        case BAND.B432M:
                            {
                                row["K-FACTOR"] = 1.4;
                                row["F1-CLEARANCE"] = 0.4;
                                break;
                            }
                        default:
                            {
                                row["K-FACTOR"] = Properties.Settings.Default.Path_Default_K_Factor;
                                row["F1-CLEARANCE"] = Properties.Settings.Default.Path_Default_F1_Clearance;
                                break;
                            }
                    }
                    this.Rows.Add(row);
                }
            }
        }

        public BandSetting this[BAND band]
        {
            get
            {
                DataRow row = this.Rows.Find(Bands.GetStringValue(band));
                if (row != null)
                {
                    BandSetting setting = new BandSetting();
                    try
                    {
                        // fill in the values from the bandsettings table
                        setting.K_Factor = (double)row["K-FACTOR"];
                        setting.F1_Clearance = (double)row["F1-CLEARANCE"];
                        setting.GroundClearance = (double)row["GROUNDCLEARANCE"];
                        setting.MaxDistance = (double)row["MAXDISTANCE"];
                        setting.MaxSquint = (double)row["MAXSQUINT"];
                        setting.MaxElevation = (double)row["MAXELEVATION"];
                    }
                    catch
                    {
                    }
                    return setting;
                }
                else
                {
                    BandSetting setting = new BandSetting();
                    return setting;
                }
            }
        }
    }
}
