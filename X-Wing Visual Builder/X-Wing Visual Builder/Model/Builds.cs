using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    static class Builds
    {
        public static List<Build> builds = new List<Build>();
        private static bool isLoadingBuild = false;
        private static int buildSaveVersion = 2;

        static Builds()
        {
            LoadBuilds();
        }

        public static Build GetBuild(int uniqueBuildId)
        {
            foreach(Build build in builds)
            {
                if(build.uniqueBuildId == uniqueBuildId)
                {
                    return build;
                }
            }
            return null;
        }

        public static void DeleteBuild(int uniqueBuildId)
        {
            builds.Remove(GetBuild(uniqueBuildId));
            SaveBuilds();
        }

        public static void AddBuild(Faction faction)
        {
            Build newBuild = new Build();
            newBuild.faction = faction;
            int newBuildId = builds.OrderByDescending(build => build.uniqueBuildId).ToArray()[0].uniqueBuildId + 1;
            newBuild.uniqueBuildId = newBuildId;
            builds.Add(newBuild);
            SaveBuilds();
        }

        public static void SaveBuilds()
        {
            if (isLoadingBuild) { return; }
            string buildInfo = buildSaveVersion.ToString();
            buildInfo += System.Environment.NewLine;
            int uniqueBuildId = 0;
            foreach (Build build in builds.OrderBy(build => build.uniqueBuildId).ToList())
            {
                buildInfo += uniqueBuildId + "|";
                buildInfo += (int)build.faction + "|";
                foreach(UniquePilot uniquePilot in build.pilots.Values.ToList())
                {
                    buildInfo += uniquePilot.id + "£" + uniquePilot.pilot.id + "£";
                    foreach (KeyValuePair<int, Upgrade> upgrade in uniquePilot.upgrades)
                    {
                        buildInfo += upgrade.Key + "~" + upgrade.Value.id + "$";
                    }
                    buildInfo = buildInfo.TrimEnd('$');
                    buildInfo += "&";
                }
                buildInfo = buildInfo.TrimEnd('&');
                buildInfo += System.Environment.NewLine;
                uniqueBuildId++;
            }
            FileHandler.SaveFile("build.txt", buildInfo);
        }
        public static void LoadBuilds()
        {
            int saveVersion = 0;
            isLoadingBuild = true;
            string[] allBuilds = FileHandler.LoadFile("build.txt");
            if (allBuilds.Count() > 0)
            {
                bool isVersion = true;
                foreach (string buildString in allBuilds)
                {
                    if(isVersion) { saveVersion = Int32.Parse(buildString); isVersion = false; continue; }
                    bool upgradeHasUniqueId = (saveVersion == 1) ? false : true;
                    Build build = new Build();

                    string[] buildInfo = buildString.Split('|');
                    build.uniqueBuildId = Int32.Parse(buildInfo[0]);
                    build.faction = (Faction)Int32.Parse(buildInfo[1]);

                    string[] pilotBuilds = buildInfo[2].Split('&');
                    pilotBuilds = pilotBuilds.Where(s => s != "").ToArray();
                    if (pilotBuilds.Count() > 0)
                    {
                        foreach (string pilotString in pilotBuilds)
                        {
                            string[] pilotInfo = pilotString.Split('£');
                            //int uniquePilotId = Int32.Parse(pilotInfo[0]);
                            int pilotId = Int32.Parse(pilotInfo[1]);
                            int uniquePilotId = build.AddPilot(pilotId);
                            string[] upgrades = pilotInfo[2].Split('$');
                            upgrades = upgrades.Where(s => s != "").ToArray();
                            if (upgrades.Count() > 0)
                            {
                                foreach (string upgradeUniqueIdAndId in upgrades)
                                {
                                    string[] upgradeUniqueIdAndIdSplit = upgradeUniqueIdAndId.Split('~');
                                    int upgradeId;
                                    //int uniqueUpgradeId;
                                    if (/*int.TryParse(upgradeUniqueIdAndIdSplit[0], out uniqueUpgradeId) && */int.TryParse(upgradeUniqueIdAndIdSplit[1], out upgradeId))
                                    {
                                        build.AddUpgrade(uniquePilotId, upgradeId);
                                    }
                                }
                            }
                        }
                    }
                    
                    builds.Add(build);
                }
            }
            isLoadingBuild = false;
            if (saveVersion != buildSaveVersion) { SaveBuilds(); }
        }
    }
}
