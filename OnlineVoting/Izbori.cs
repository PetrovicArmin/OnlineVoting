using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    
    internal class Izbori
    {
        public static List<Stranka> stranke = new List<Stranka>();
        public static List<Kandidat> kandidati = new List<Kandidat>();
        static private Izbori singletonIzbori = null;
        static private string sifra = "VVS20222023";

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

            switch (tipGlasa)
            {
                case TipGlasa.NEVAZECI:
                    nevazecihGlasova++;
                    break;
                case TipGlasa.SAMO_STRANKA: //po automatizmu prvog člana stranke dodajemo
                    stranke.Find(s => s.VratiIdStranke() == glas.VratiIDStranke()).DodajGlas(new List<string>());
                    break;
                case TipGlasa.STRANKA_KANDIDATI:
                    stranke.Find(s => s.VratiIdStranke() == glas.VratiIDStranke()).DodajGlas(glas.VratiIDKandidata());
                    break;
                case TipGlasa.NEZAVISNI_KANDIDAT:
                    kandidati.Find(kandidat => kandidat.dajJIK() == glas.VratiIDKandidata()[0]).DodajGlas();
                    break;
            }

            return ukupnoGlasova;
        }

        //metoda dodana zbog potreba funkcionalnosti 5, Faruk
        public bool ProvjeraSifre(string unos)
        {
            if (unos != sifra) return false;
            return true;
        }

        //metoda dodana zbog potreba funkcionalnosti 5, Faruk
        public int PonistiGlas(Osoba osoba, Glas glas)
        {
            GlasackiProces glasackiProces = new GlasackiProces(osoba, glas);

            if (glasackiProces.IdentifikujGlasaca())
            {
                //nije nikad ni glasao
                throw new Exception("Gradjanin nije jos glasao");
            }

            ukupnoGlasova--;

            TipGlasa tipGlasa = glasackiProces.VerifikujGlas();

            switch (tipGlasa)
            {
                case TipGlasa.NEVAZECI:
                    nevazecihGlasova--;
                    break;
                case TipGlasa.SAMO_STRANKA: //po automatizmu prvog člana stranke dodajemo
                    stranke.Find(s => s.VratiIdStranke() == glas.VratiIDStranke()).OduzmiGlas(new List<string>());
                    break;
                case TipGlasa.STRANKA_KANDIDATI:
                    stranke.Find(s => s.VratiIdStranke() == glas.VratiIDStranke()).OduzmiGlas(glas.VratiIDKandidata());
                    break;
                case TipGlasa.NEZAVISNI_KANDIDAT:
                    kandidati.Find(kandidat => kandidat.dajJIK() == glas.VratiIDKandidata()[0]).OduzmiGlas();
                    break;
            }

            return ukupnoGlasova;
        }

        public string TrenutnoStanje()
        {
            string povrat = "Ukupno glasova: " + ukupnoGlasova + ", postotak nevažećih: " + ((nevazecihGlasova / ukupnoGlasova) * 100.0).ToString() + "% \n";

            int validnihGlasova = ukupnoGlasova - nevazecihGlasova;

            if (validnihGlasova == 0)
            {
                povrat += "Ne postoje validni glasovi, pa nije moguće obaviti rangiranje stranki i kandidata!\n";
                return povrat;
            }

            List<Stranka> strankeSaMandatom = new List<Stranka> { };
            List<Kandidat> nezavisniKandidatiSaMandatom = new List<Kandidat> { };

            stranke.ForEach(stranka =>
            {
                double postotak = (stranka.GetBrojGlasova() / (1.00 * validnihGlasova)) * 100.0;
                povrat += "Stranka sa ID-jem " + stranka.VratiIdStranke() + " je osvojila " + (postotak).ToString() + "% glasova! \n";

                if (postotak > 2.00)
                    strankeSaMandatom.Add(stranka);
            });

            kandidati.ForEach(kandidat =>
            {
                double postotak = (kandidat.VratiBrojGlasova() / (validnihGlasova * 1.00)) * 100.00;
                povrat += "Kandidat sa JIK-om " + kandidat.dajJIK() + " je osvojio " + (postotak).ToString() + "% glasova! \n";
                if (postotak > 2.00)
                    nezavisniKandidatiSaMandatom.Add(kandidat);
            });

            povrat += "Stranke i Kandidati koji su dobili mandat su: \n";

            strankeSaMandatom.ForEach(stranka =>
            {
                povrat += "ID stranke: " + stranka.VratiIdStranke() + ". Članovi stranke koji su dobili mandat su: \n";
                //ovdje nakon što velid implementira svoj dio!
            });

            nezavisniKandidatiSaMandatom.ForEach(nezavisni =>
            {
                povrat += "JIK nez. kandidata: " + nezavisni.dajJIK() + ". \n";
            });

            return povrat;
        }

        // dodala Naida Pita
        public int DajUkupneGlasove()
        {
            return ukupnoGlasova - nevazecihGlasova;
        }

    }
}
