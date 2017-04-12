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
        public static void SaveFile(string fileName, string data)
        {
            File.WriteAllText(@"D:\Documents\Game Stuff\X-Wing\" + fileName, data);
        }
        public static string[] LoadFile(string fileName)
        {
            if(File.Exists(@"D:\Documents\Game Stuff\X-Wing\" + fileName))
            {
                return File.ReadAllLines(@"D:\Documents\Game Stuff\X-Wing\" + fileName);
            }
            else
            {
                return null;
            }
            /*
            string line;
            using (StreamReader streamReader = new StreamReader(@"D:\Documents\Game Stuff\X-Wing\" + fileName))
            {
                line = streamReader.ReadLine();
            }
            return line;*/
        }
    }
}
