using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Resources;
using System.IO;

namespace X_Wing_Visual_Builder.Model
{
    static class Expansions
    {
        public static Dictionary<ExpansionType, Expansion> expansions = new Dictionary<ExpansionType, Expansion>();

        static Expansions()
        {
            using (TextFieldParser parser = new TextFieldParser(new StringReader(Properties.Resources.Expansions)))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                parser.HasFieldsEnclosedInQuotes = false;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    expansions.Add((ExpansionType)Int32.Parse(fields[0]), new Expansion((ExpansionType)Int32.Parse(fields[0]), fields[1]));
                }
            }
        }
    }
}
