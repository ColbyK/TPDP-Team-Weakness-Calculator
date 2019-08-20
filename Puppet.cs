using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPDP_Team_Weakness
{
    class Puppet
    {
        public int type1;
        public int type2;
        public float[] weakness;
        public string abilName;

        public Puppet(int size, string abilName = null, int t1 = -1, int t2 = -1)
        {
            weakness = new float[size];
            type1 = t1;
            type2 = t2;
            this.abilName = abilName;
            assignInitWeakness();
        }
        public void calcWeakness(List<List<float>> chart, List<Abil> abils)
        {
            assignInitWeakness();
            for (int k = 0; k < chart.Count; k++)
            {
                if(type1 != -1)
                {
                    weakness[k] *= chart[k][type1];
                }
                if(type2 != -1 && type2 != type1)
                {
                    weakness[k] *= chart[k][type2];
                }
            }
            for(int k = 0; k < abils.Count; k++)
            {
                if (abils[k].name.Equals(abilName))
                {
                    weakness[abils[k].typeIndex] *= abils[k].modifier;
                }
            }
            string affTwist = "Affinity Twist";
            if(affTwist.Equals(abilName))
            {
                for(int k = 0; k < weakness.Length; k++)
                {
                    if(weakness[k] == 0)
                    {
                        weakness[k] = 2;
                    }
                    else
                    {
                        weakness[k] = 1 / weakness[k];
                    }
                }
            }
        }
        public void assignInitWeakness()
        {
            for (int k = 0; k < weakness.Length; k++)
            {
                weakness[k] = 1;
            }
        }
    }
}
