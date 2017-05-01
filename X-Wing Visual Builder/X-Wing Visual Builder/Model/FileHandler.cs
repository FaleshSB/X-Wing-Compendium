using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    static class FileHandler
    {
        private static string filteredLocation;

        static FileHandler()
        {
            string baseLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            filteredLocation = System.IO.Path.GetDirectoryName(baseLocation).Replace("file:\\", "") + "\\";
        }

        public static void SaveFile(string fileName, string data)
        {
            File.WriteAllText(filteredLocation + fileName, data);
        }
        public static string[] LoadFile(string fileName)
        {
            if (File.Exists(filteredLocation + fileName))
            {
                return File.ReadAllLines(filteredLocation + fileName);
            }
            else
            {
                return null;
            }
        }
    }
}
