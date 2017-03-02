using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X_Wing_Visual_Builder.Model
{
    class FileHandler
    {
        public void SaveFile(string fileName, string data)
        {
            File.WriteAllText(@"D:\Documents\Game Stuff\X-Wing\" + fileName, data);
        }
        public string[] LoadFile(string fileName)
        {
            return File.ReadAllLines(@"D:\Documents\Game Stuff\X-Wing\" + fileName);
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
