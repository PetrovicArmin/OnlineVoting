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
        private List<Glas> glasovi = new List<Glas> { };
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

        //za potrebe funk 5 modifikovao Faruk
        public void DodajGlasaca(string jik, Glas glas)
        {
            glasaci.Add(jik);
            glasovi.Add(glas);
        }

        //za potrebe funk 5 dodao Faruk
        public Glas DajGlas(string jik)
        {
            int index = glasaci.FindIndex(a => a == jik);
            return glasovi.ElementAt(index);
        }

        //za potrebe funk 5 dodao Faruk
        public void UkloniGlasaca(string jik)
        {
            int index = glasaci.FindIndex(a => a == jik);
            glasaci.RemoveAt(index);
            glasovi.RemoveAt(index);
        }
    }
}
