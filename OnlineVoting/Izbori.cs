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

        //koliko je gradjana izaslo na izbore, ovo nije ukupna populacija
        private int ukupnoGlasova = 0;
        private int nevazecihGlasova = 0;


        private Izbori()
        {
        }
        

        static public Izbori DajIzbore()
        {
            if (singletonIzbori == null)
                singletonIzbori = new Izbori();
            return singletonIzbori;
        }

        public int ProcesirajGlas(Osoba osoba, Glas glas)
        {
            GlasackiProces glasackiProces = new GlasackiProces(osoba, glas);
            
            if (!glasackiProces.IdentifikujGlasaca())
            {
                //već je glasao
                throw new Exception("Već ste glasali!");
            }

            ukupnoGlasova++;

            TipGlasa tipGlasa = glasackiProces.VerifikujGlas();

            switch(tipGlasa)
            {
                case TipGlasa.NEVAZECI:
                    nevazecihGlasova++;
                    break;
                case TipGlasa.SAMO_STRANKA: //po automatizmu prvog člana stranke dodajemo
                    stranke.Find(s => s.vratiIdStranke() == glas.VratiIDStranke()).DodajGlas(new List<string>());
                    break;
                case TipGlasa.STRANKA_KANDIDATI:
                    stranke.Find(s => s.vratiIdStranke() == glas.VratiIDStranke()).DodajGlas(glas.VratiIDKandidata());
                    break;
                case TipGlasa.NEZAVISNI_KANDIDAT:
                    kandidati.Find(kandidat => kandidat.dajJIK() == glas.VratiIDKandidata()[0]).DodajGlas();
                    break;
            }


            return ukupnoGlasova;
        }

        public string TrenutnoStanje()
        {
            string povrat = "Ukupno glasova: " + ukupnoGlasova + ", postotak nevažećih: " + ((nevazecihGlasova / ukupnoGlasova) * 100.0).ToString() + "% \n";

            int validnihGlasova = ukupnoGlasova - nevazecihGlasova;

            List<Stranka> strankeSaMandatom = new List<Stranka> { };
            List<Kandidat> nezavisniKandidatiSaMandatom = new List<Kandidat> { };

            stranke.ForEach(stranka =>
            {
                double postotak = (stranka.GetBrojGlasova() / (1.00*validnihGlasova)) * 100.0;
                povrat += "Stranka sa ID-jem " + stranka.vratiIdStranke() + " je osvojila " + (postotak).ToString() + "% glasova! \n";

                if (postotak > 2.00) 
                    strankeSaMandatom.Add(stranka);
            });

            kandidati.ForEach(kandidat =>
            {
                double postotak = (kandidat.VratiBrojGlasova() / (validnihGlasova*1.00)) * 100.00;
                povrat += "Kandidat sa JIK-om " + kandidat.dajJIK() + " je osvojio " + (postotak).ToString() + "% glasova! \n";
                if (postotak > 2.00)
                    nezavisniKandidatiSaMandatom.Add(kandidat);
            });

            povrat += "Stranke i Kandidati koji su dobili mandat su: \n";

            strankeSaMandatom.ForEach(stranka =>
            {
                povrat += "ID stranke: " + stranka.vratiIdStranke() + ". Članovi stranke koji su dobili mandat su: \n";
                //ovdje nakon što velid implementira svoj dio!
            });

            nezavisniKandidatiSaMandatom.ForEach(nezavisni =>
            {
                povrat += "JIK nez. kandidata: " + nezavisni.dajJIK() + ". \n";
            });

            return povrat;
        }
    }
}
