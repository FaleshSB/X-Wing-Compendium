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
            filteredLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\X-Wing Compendium";
            if(!Directory.Exists(filteredLocation)) Directory.CreateDirectory(filteredLocation);
        }

        public static void SaveFile(string fileName, string data)
        {
            File.WriteAllText(Path.Combine(filteredLocation, fileName), data);
        }
        public static string[] LoadFile(string fileName)
        {
            if (File.Exists(Path.Combine(filteredLocation, fileName)))
            {
                return File.ReadAllLines(Path.Combine(filteredLocation, fileName));
            }
            else
            {
                return null;
            }
        }
    }
}
