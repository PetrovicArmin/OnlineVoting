using System;
using System.Collections.Generic;

namespace OnlineVoting
{
    public class Stranka
    {
        private int id { get; set; }
        private List<Kandidat> clanovi { get; set; }
        private List<Kandidat> clanoviSaMandatom { get; set; }
        private int BrojGlasova { get; set; }

        public Stranka(List<Kandidat> clanovi, int id)
        {
            this.clanovi = clanovi;
            this.id = id;
            this.clanoviSaMandatom = new List<Kandidat>();
        }

        public void dodajClana(Kandidat noviClan)
        {
            clanovi.Add(noviClan);
        }

        public List<Kandidat> vratiClanove()
        {
            return clanovi;
        }

        public int vratiIdStranke()
        {
            return id;
        }

        public void DodajGlas(List<string> kandidatiStranke)
        {
            BrojGlasova++;
            if (kandidatiStranke.Count == 0)
            {
                clanovi[0].DodajGlas();
                return;
            }

            kandidatiStranke.ForEach(id =>
            {
                clanovi.Find(clan => clan.dajJIK() == id).DodajGlas();
            });
        }

        // dodala Naida Pita
        public int GetBrojGlasova()
        {
            return BrojGlasova;
        }

        // dodala Naida Pita
        public void postaviBrojGlasova(int broj)
        {
            BrojGlasova = broj;
        }
        
        // uradila Naida Pita
        private int dajBrojMandata()
        {
            int brojMandata = 0;
            for(int i = 0; i < clanovi.Count; i++)
            {
                if (clanovi[i].VratiBrojGlasova() >= 0.2 * GetBrojGlasova())
                {
                    brojMandata++;
                }
            }
            return brojMandata;
        }

        // dodala Naida Pita
        public void nadjiMandatlije()
        {
            for (int i = 0; i < clanovi.Count; i++)
            {
                if (clanovi[i].VratiBrojGlasova() >= 0.2 * GetBrojGlasova())
                {
                    clanoviSaMandatom.Add(clanovi[i]);
                }
            }
        }

        // Funkcionalnost 3 uradila: Naida Pita
        public string prikaziRezultate(int ukupniBrojGlasova)
        {
            if(GetBrojGlasova()>ukupniBrojGlasova)
            {
                throw new Exception("Broj glasova stranke je veći od ukupnih glasova na izborima!");
            }
            string ispis = "";
            ispis += "\nStranka " + id.ToString() + "\n" + "Broj glasova: " + GetBrojGlasova().ToString() + "\n" + "Postotak glasova: ";
            ispis += Math.Round((Decimal)((double)GetBrojGlasova() / (double)ukupniBrojGlasova * 100.0),2).ToString() + "%\n";
            ispis += "Broj članova sa mandatima: " + dajBrojMandata() + "\n";
            if (dajBrojMandata() != 0)
            {
                ispis += "Članovi sa mandatom: \n";
                for (int i = 0; i < clanoviSaMandatom.Count; i++)
                {
                    ispis += i+1 + ". " + clanoviSaMandatom[i].OsnovneInformacije() + ", broj glasova " + clanoviSaMandatom[i].VratiBrojGlasova().ToString();
                    ispis += ", postotak glasova " + Math.Round((Decimal)((double)clanoviSaMandatom[i].VratiBrojGlasova() / (double)GetBrojGlasova() * 100), 2).ToString() + "%.";
                    if (i != clanoviSaMandatom.Count - 1)
                        ispis += "\n";
                }
            }
            else
                ispis += "Nema članova sa mandatom.\n";
            return ispis;
        }

        // dodala Naida Pita
        public void resetujClanoveSaMandatom()
        {
            clanoviSaMandatom.Clear();
        }


    }
}
