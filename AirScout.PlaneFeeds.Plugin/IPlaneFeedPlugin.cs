using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AirScout.PlaneFeeds.Plugin.MEFContract
{
    // defines the PLaneFeed plugin basic interface
    public interface IPlaneFeedPlugin
    {
        string Name { get; }
        string Version { get; }
        string Info { get; }
        bool HasSettings { get; }
        bool CanImport { get; }
        bool CanExport { get; }
        string Disclaimer { get; }
        string DisclaimerAccepted { get; set; }
        void ResetSettings();
        void LoadSettings();
        object GetSettings();
        void SaveSettings();
        void ImportSettings();
        void ExportSettings();
        void Start(PlaneFeedPluginArgs args);
        AirScout.PlaneFeeds.Plugin.PlaneFeedPluginPlaneInfoList GetPlanes(PlaneFeedPluginArgs args);
        void Stop(PlaneFeedPluginArgs args);
    }

    // defines the PlaneFeed plugin metadata
    public interface IPlaneFeedPluginMetaData
    {
        string Name { get; }
    }
}