using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    static class Builds
    {
        public static List<Build> builds { get; set; } = new List<Build>();
        private static bool isLoadingBuild = false;

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

        public static void AddBuild(Faction faction)
        {
            Build newBuild = new Build();
            newBuild.faction = faction;
            int newBuildId = 0;
            while(true)
            {
                int origionalNewBuildId = newBuildId;
                foreach (Build build in builds)
                {
                    if(build.uniqueBuildId == newBuildId)
                    {
                        newBuildId++;
                        break;
                    }
                }
                if(origionalNewBuildId == newBuildId)
                {
                    newBuild.uniqueBuildId = newBuildId;
                    break;
                }
            }
            builds.Add(newBuild);
            SaveBuilds();
        }

        public static void SaveBuilds()
        {
            if (isLoadingBuild) { return; }
            string buildInfo = "";
            foreach (Build build in builds)
            {
                buildInfo += build.uniqueBuildId + "|";
                buildInfo += (int)build.faction + "|";
                foreach(KeyValuePair<int, Pilot> pilot in build.pilots)
                {
                    buildInfo += pilot.Value.uniquePilotId + "£" + pilot.Value.id + "£";
                    foreach (Upgrade upgrade in pilot.Value.upgrades)
                    {
                        buildInfo += upgrade.id + "$";
                    }
                    buildInfo = buildInfo.TrimEnd('$');
                    buildInfo += "&";
                }
                buildInfo = buildInfo.TrimEnd('&');
                buildInfo += System.Environment.NewLine;
            }
            FileHandler.SaveFile("build.txt", buildInfo);
        }
        public static void LoadBuilds()
        {
            isLoadingBuild = true;
            string[] allBuilds = FileHandler.LoadFile("build.txt");
            if (allBuilds.Count() > 0)
            {
                foreach (string buildString in allBuilds)
                {
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
                            int uniquePilotId = Int32.Parse(pilotInfo[0]);
                            int pilotId = Int32.Parse(pilotInfo[1]);
                            Pilot pilot = Pilots.GetPilotClone(pilotId);
                            pilot.uniquePilotId = uniquePilotId;
                            build.AddPilot(uniquePilotId, pilot);
                            string[] upgrades = pilotInfo[2].Split('$');
                            upgrades = upgrades.Where(s => s != "").ToArray();
                            if (upgrades.Count() > 0)
                            {
                                foreach (string upgradeIdString in upgrades)
                                {
                                    int upgradeId;
                                    if (int.TryParse(upgradeIdString, out upgradeId))
                                    {
                                        build.AddUpgrade(uniquePilotId, Upgrades.upgrades[upgradeId]);
                                    }
                                }
                            }
                        }
                    }
                    builds.Add(build);
                }
            }
            isLoadingBuild = false;
        }
    }
}
