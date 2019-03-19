using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Reflection;
using WinTest;
using System.Diagnostics;
using ScoutBase.Core;

public static class Planefinder
{
    public static string Read()
    {
        String getURL = "http://planefinder.net/endpoints/update.php?faa=1&bounds=37.729817%2C-98.840955%2C52.451268%2C-76.868299";
        HttpWebRequest get = (HttpWebRequest)HttpWebRequest.Create(getURL);
        get.Referer = "http://planefinder.net/";
        get.UserAgent = "Mozilla/5.0 (X11; Linux x86_64; rv:17.0) Gecko/20130807 Firefox/17.0";
        HttpWebResponse responseGet = (HttpWebResponse)get.GetResponse();
        string response;
         using (StreamReader sr = new StreamReader(responseGet.GetResponseStream()))
        {
            response = sr.ReadToEnd();
            // more stuff
        }
//        HttpEntity resEntityGet = responseGet.getEntity();
        return response;
    }
}