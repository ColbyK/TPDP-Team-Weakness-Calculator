using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace TPDP_Team_Weakness
{

    public struct Abil
    {
        public int typeIndex;
        public string name;
        public float modifier;
        public bool mark;
        public string markTag;
    }

    class ReadTypes
    {
        public List<List<float>> chart;
        public List<string> typeNames;
        public List<Color> typeColor;
        public List<Abil> abilities;

        public ReadTypes(string path)
        {
            chart = new List<List<float>>();
            typeNames = new List<string>();
            typeColor = new List<Color>();
            abilities = new List<Abil>();

            int typeCount = 0;

            XmlReader read = XmlReader.Create(path);
            read.ReadStartElement("Types");
            while (read.Read())
            {
                if (read.Name.Equals("Type"))
                {
                    typeNames.Add(read.GetAttribute("name"));
                    Color col = new Color();
                    col.R = Convert.ToByte(read.GetAttribute("r"));
                    col.G = Convert.ToByte(read.GetAttribute("g"));
                    col.B = Convert.ToByte(read.GetAttribute("b"));
                    //Console.WriteLine(col.R + "  " + col.G + "  " + col.B);
                    typeColor.Add(col);

                    while (!(read.NodeType == XmlNodeType.EndElement && read.Name.Equals("Type")))
                    {

                        if (read.Name.Equals("Abil") && read.NodeType == XmlNodeType.Element)
                        {
                            Abil cur = new Abil();
                            cur.name = read.GetAttribute("name");
                            cur.markTag = read.GetAttribute("tag");
                            if(cur.markTag == null)
                            {
                                cur.mark = false;
                            }
                            else
                            {
                                cur.mark = true;
                            }
                            cur.typeIndex = typeCount;
                            read.Read();
                            //Console.WriteLine(read.Value);
                            cur.modifier = (float)(Convert.ToDouble(read.Value));
                            abilities.Add(cur);
                        }

                        if (read.Name.Equals("DefList"))
                        {
                            List<float> row = new List<float>();
                            while (!(read.NodeType == XmlNodeType.EndElement && read.Name.Equals("DefList")))
                            {
                                if (read.Name.Equals("Defending") && read.NodeType == XmlNodeType.Element)
                                {
                                    read.Read();
                                    row.Add((float)(Convert.ToDouble(read.Value)));
                                }
                                read.Read();
                            }
                            chart.Add(row);
                        }
                        read.Read();
                    }
                    typeCount++;
                }
            }
        }

        public int getTypeIndexByName(string str)
        {
            for(int k = 0; k < typeNames.Count; k++)
            {
                if (typeNames[k].Equals(str))
                {
                    return k;
                }
            }
            return -1;
        }
    }
}
