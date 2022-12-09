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

        public int GetBrojGlasova()
        {
            return BrojGlasova;
        }
        
        private int dajBrojMandata()
        {
            int brojMandata = 0;
            for(int i = 0; i < clanovi.Count; i++)
            {
                if (clanovi[i].VratiBrojGlasova() >= 0.2 * GetBrojGlasova())
                {
                    brojMandata++;
                    clanoviSaMandatom.Add(clanovi[i]);
                }
            }
            return brojMandata;
        }

        // Funkcionalnost 3 uradila: Naida Pita
        public string prikaziRezultate(int ukupniBrojGlasova)
        {
            string ispis = "";
            ispis += "Stranka " + id.ToString() + "\n" + "Broj glasova: " + GetBrojGlasova().ToString() + "\n" + "Postotak glasova: ";
            ispis += Math.Round((Decimal)(GetBrojGlasova() / ukupniBrojGlasova * 100)).ToString() + "\n";
            ispis += "Broj članova sa mandatima: " + dajBrojMandata() + "\n";
            if (clanoviSaMandatom.Count != 0)
                ispis += "Članovi sa mandatom: \n";
            else
                ispis += "Nema članova sa mandatom.\n";
            for (int i = 0; i < clanoviSaMandatom.Count; i++)
            {

               ispis += clanoviSaMandatom[i].OsnovneInformacije() + ", broj glasova " + clanoviSaMandatom[i].VratiBrojGlasova().ToString();
               ispis += ", postotak glasova " + Math.Round((Decimal)(clanoviSaMandatom[i].VratiBrojGlasova() / GetBrojGlasova() * 100)).ToString() + ".";
               if (i != clanoviSaMandatom.Count - 1)
                    ispis += "\n";

            }
            return ispis;
        }

    }
}
