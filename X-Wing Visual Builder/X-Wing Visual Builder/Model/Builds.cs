using System;
using System.Collections.Generic;
using System.IO;
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

        public static void DuplicateBuild(int uniqueBuildId)
        {
            Build oldBuild = GetBuild(uniqueBuildId);
            int newBuildId = AddBuild(oldBuild.faction);
            Build newBuild = GetBuild(newBuildId);

            //foreach
        }

        private static void SortBuilds()
        {
            int order = 20;
            foreach (Build build in Builds.builds.OrderBy(build => build.displayOrder).ToList())
            {
                build.displayOrder = order;
                order += 10;
            }
            SaveBuilds();
        }

        public static void MoveBuildUp(int uniqueBuildId)
        {
            foreach(Build build in builds)
            {
                if(build.uniqueBuildId == uniqueBuildId) { build.displayOrder += 15; }
            }
            SortBuilds();
        }

        public static void MoveBuildDown(int uniqueBuildId)
        {
            foreach (Build build in builds)
            {
                if (build.uniqueBuildId == uniqueBuildId) { build.displayOrder -= 15; }
            }
            SortBuilds();
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

        public static int AddBuild(Faction faction)
        {
            Build newBuild = new Build();
            newBuild.faction = faction;
            int newBuildId = 1;
            int displayOrder = 20;
            if (builds.Count > 0)
            {
                newBuildId = builds.OrderByDescending(build => build.uniqueBuildId).ToArray()[0].uniqueBuildId + 1;
                displayOrder = builds.OrderByDescending(build => build.displayOrder).ToList()[0].displayOrder + 10;
            }
            newBuild.uniqueBuildId = newBuildId;
            newBuild.displayOrder = displayOrder;
            builds.Add(newBuild);
            SaveBuilds();
            return newBuildId;
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
                buildInfo += build.displayOrder + "|";
                foreach (UniquePilot uniquePilot in build.pilots.Values.ToList())
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
            if (File.Exists("build.txt") == false) { return; }
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
                    build.displayOrder = Int32.Parse(buildInfo[2]);

                    string[] pilotBuilds = buildInfo[3].Split('&');
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
