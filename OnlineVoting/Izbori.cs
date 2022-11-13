using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    internal class Izbori
    {
        public static List<Stranka> stranke = new List<Stranka>();
        public static List<Kandidat> kandidati = new List<Kandidat>();
        static private Izbori singletonIzbori = null;

        private Izbori()
        {
        }

        static public Izbori dajIzbore()
        {
            if (singletonIzbori == null)
                singletonIzbori = new Izbori();
            return singletonIzbori;
        }
    }
}
