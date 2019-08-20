using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TPDP_Team_Weakness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReadTypes rt;
        List<string> types;
        List<string> abils;
        Puppet[] puppets;
        List<List<float>> typeResults;

        public MainWindow()
        {
            ReadXml();
            CreateComboboxContent();
            CreateContent();
            InitializeComponent();
            AssignComboboxContent();
        }
        public void ReadXml()
        {
            rt = new ReadTypes("TPDP_TypeEff_Data.xml");
        }
        public void CreateComboboxContent()
        {
            types = new List<string>();
            abils = new List<string>();

            types.Add("- None -");
            for(int k = 0; k < rt.typeNames.Count; k++)
            {
                types.Add(rt.typeNames[k]);
            }

            for(int k = 0; k < rt.abilities.Count; k++)
            {
                if (!abils.Contains(rt.abilities[k].name))
                {
                    abils.Add(rt.abilities[k].name);
                }
            }
        }
        public void CreateContent()
        {
            puppets = new Puppet[6];
            for(int k = 0; k < puppets.Length; k++)
            {
                puppets[k] = new Puppet(rt.chart.Count);
            }
            typeResults = new List<List<float>>();
            for(int k = 0; k < rt.chart.Count; k++)
            {
                typeResults.Add(new List<float>());
            }
        }
        public void AssignComboboxContent()
        {
            p1_Type1.ItemsSource = types;
            p2_Type1.ItemsSource = types;
            p3_Type1.ItemsSource = types;
            p4_Type1.ItemsSource = types;
            p5_Type1.ItemsSource = types;
            p6_Type1.ItemsSource = types;
            p1_Type2.ItemsSource = types;
            p2_Type2.ItemsSource = types;
            p3_Type2.ItemsSource = types;
            p4_Type2.ItemsSource = types;
            p5_Type2.ItemsSource = types;
            p6_Type2.ItemsSource = types;
            p1_Ability.ItemsSource = abils;
            p2_Ability.ItemsSource = abils;
            p3_Ability.ItemsSource = abils;
            p4_Ability.ItemsSource = abils;
            p5_Ability.ItemsSource = abils;
            p6_Ability.ItemsSource = abils;
            List<string> types_outList = new List<string>();
            for(int k = 1; k < types.Count; k++)
            {
                types_outList.Add(types[k]);
            }
            types_out.ItemsSource = types_outList;
        }
        public void clearResults()
        {
            for(int k = 0; k < typeResults.Count; k++)
            {
                typeResults[k].Clear();
            }
        }
        ///////////////////////////////////////////////
        public void UpdatePuppet(int puppetIndex, string typeName1, string typeName2, string abil)
        {
            clearResults();
            puppets[puppetIndex].type1 = rt.getTypeIndexByName(typeName1);
            puppets[puppetIndex].type2 = rt.getTypeIndexByName(typeName2);
            puppets[puppetIndex].abilName = abil;
            puppets[puppetIndex].calcWeakness(rt.chart, rt.abilities);
            CalcOutputWeaknesses();
        }

        public void CalcOutputWeaknesses()
        {
            for(int k = 0; k < typeResults.Count; k++)
            {
                for(int i = 0; i < puppets.Length; i++)
                {
                    typeResults[k].Add(puppets[i].weakness[k]);
                }
            }
        }

        public void UpdateResultsDisplay()
        {
            List<List<int>> dispContent = updateData();
            List<List<string>> convertCont = new List<List<string>>();

            for(int k = 0; k < dispContent.Count; k++)
            {
                convertCont.Add(new List<string>());
                for(int i = 0; i < dispContent[k].Count; i++)
                {
                    if(dispContent[k][i] == 0)
                    {
                        convertCont[k].Add("-");
                    }
                    else
                    {
                        convertCont[k].Add("" + dispContent[k][i]);
                    }
                }
            }
            updateNotes(convertCont);
            x0_eff_out.ItemsSource = convertCont[0];
            x0_25_eff_out.ItemsSource = convertCont[1];
            x0_5_eff_out.ItemsSource = convertCont[2];
            x2_eff_out.ItemsSource = convertCont[3];
            x4_eff_out.ItemsSource = convertCont[4];
        }

        public void updateNotes(List<List<string>> convertCont)
        {
            List<string> notes = new List<string>();

            for(int k = 0; k < puppets.Length; k++)
            {
                if (puppets[k].abilName != null && puppets[k].abilName != "- None -")
                {
                    for (int i = 0; i < rt.abilities.Count; i++)
                    {
                        if (rt.abilities[i].mark && rt.abilities[i].name.Equals(puppets[k].abilName))
                        {
                            if(puppets[k].weakness[rt.abilities[i].typeIndex] == 0)
                            {
                                convertCont[0][rt.abilities[i].typeIndex] += "*";
                            }
                            else if (puppets[k].weakness[rt.abilities[i].typeIndex] == 0.25)
                            {
                                convertCont[1][rt.abilities[i].typeIndex] += "*";
                            }
                            else if(puppets[k].weakness[rt.abilities[i].typeIndex] == 0.5)
                            {
                                convertCont[2][rt.abilities[i].typeIndex] += "*";
                            }
                            else if (puppets[k].weakness[rt.abilities[i].typeIndex] == 2)
                            {
                                convertCont[3][rt.abilities[i].typeIndex] += "*";
                            }
                            else if(puppets[k].weakness[rt.abilities[i].typeIndex] == 4)
                            {
                                convertCont[4][rt.abilities[i].typeIndex] += "*";
                            }
                            string note = "Puppet " + (int)(1+k) + ": " + rt.abilities[i].markTag;
                            notes.Add(note);
                        }
                    }
                } 
            }
            notes_out.ItemsSource = notes;
        }

        public List<List<int>> updateData()
        {
            List<int> x0 = new List<int>();
            List<int> x0_25 = new List<int>();
            List<int> x0_5 = new List<int>();
            List<int> x2 = new List<int>();
            List<int> x4 = new List<int>();

            for (int k = 0; k < typeResults.Count; k++)
            {
                x0.Add(0);
                x0_25.Add(0);
                x0_5.Add(0);
                x2.Add(0);
                x4.Add(0);
                for (int i = 0; i < typeResults[k].Count; i++)
                {
                    if (typeResults[k][i] == 0)
                    {
                        x0[k]++;
                    }
                    else if (typeResults[k][i] == 0.25)
                    {
                        x0_25[k]++;
                    }
                    else if (typeResults[k][i] == 0.5)
                    {
                        x0_5[k]++;
                    }
                    else if (typeResults[k][i] == 2)
                    {
                        x2[k]++;
                    }
                    else if (typeResults[k][i] == 4)
                    {
                        x4[k]++;
                    }
                    else
                    {
                        //TO BE DONE
                    }
                }
            }
            List<List<int>> ret = new List<List<int>>();
            ret.Add(x0);
            ret.Add(x0_25);
            ret.Add(x0_5);
            ret.Add(x2);
            ret.Add(x4);
            return ret;
        }
        ///////////////////////////////////////////////
        private void p1_Type1_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(0, (string)p1_Type1.SelectedItem, (string)p1_Type2.SelectedItem, (string)p1_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p1_Type2_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(0, (string)p1_Type1.SelectedItem, (string)p1_Type2.SelectedItem, (string)p1_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p1_Ability_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(0, (string)p1_Type1.SelectedItem, (string)p1_Type2.SelectedItem, (string)p1_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p2_Type1_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(1, (string)p2_Type1.SelectedItem, (string)p2_Type2.SelectedItem, (string)p2_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p2_Type2_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(1, (string)p2_Type1.SelectedItem, (string)p2_Type2.SelectedItem, (string)p2_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p2_Ability_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(1, (string)p2_Type1.SelectedItem, (string)p2_Type2.SelectedItem, (string)p2_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p3_Type1_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(2, (string)p3_Type1.SelectedItem, (string)p3_Type2.SelectedItem, (string)p3_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p3_Type2_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(2, (string)p3_Type1.SelectedItem, (string)p3_Type2.SelectedItem, (string)p3_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p3_Ability_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(2, (string)p3_Type1.SelectedItem, (string)p3_Type2.SelectedItem, (string)p3_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p4_Type1_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(3, (string)p4_Type1.SelectedItem, (string)p4_Type2.SelectedItem, (string)p4_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p4_Type2_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(3, (string)p4_Type1.SelectedItem, (string)p4_Type2.SelectedItem, (string)p4_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p4_Ability_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(3, (string)p4_Type1.SelectedItem, (string)p4_Type2.SelectedItem, (string)p4_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p5_Type1_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(4, (string)p5_Type1.SelectedItem, (string)p5_Type2.SelectedItem, (string)p5_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p5_Type2_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(4, (string)p5_Type1.SelectedItem, (string)p5_Type2.SelectedItem, (string)p5_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p5_Ability_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(4, (string)p5_Type1.SelectedItem, (string)p5_Type2.SelectedItem, (string)p5_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p6_Type1_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(5, (string)p6_Type1.SelectedItem, (string)p6_Type2.SelectedItem, (string)p6_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p6_Type2_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(5, (string)p6_Type1.SelectedItem, (string)p6_Type2.SelectedItem, (string)p6_Ability.SelectedItem);
            UpdateResultsDisplay();
        }

        private void p6_Ability_DropDownClosed(object sender, EventArgs e)
        {
            UpdatePuppet(5, (string)p6_Type1.SelectedItem, (string)p6_Type2.SelectedItem, (string)p6_Ability.SelectedItem);
            UpdateResultsDisplay();
        }
    }
}
