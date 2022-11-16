using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    internal class Populacija
    {
        private List<string> glasaci = new List<string> { };
        static private Populacija populacija = null;

        private Populacija(List<string> glasaci)
        {
            this.glasaci = glasaci;
        }

        private Populacija() { }

        static public Populacija DajPopulaciju()
        {
            if (populacija == null)
                populacija = new Populacija();
            return populacija;
        }


        public void setGlasaci(List<string> glasacii)
        {
            glasaci = glasacii;
        }

        public List<string> getGlasaci()
        {
            return glasaci;
        }
    }
}
