using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineVoting
{
    public class Stranka
    {
        private int id { get; set; }
        private List<Kandidat> Clanovi { get; set; }
        private List<Kandidat> ClanoviSaMandatom { get; set; }
        private List<Kandidat> Rukovodstvo { get; set; }
        private int BrojGlasova { get; set; }

        public Stranka(List<Kandidat> clanovi, int id)
        {
            this.Clanovi = clanovi;
            this.id = id;
            this.ClanoviSaMandatom = new List<Kandidat>();
        }

        public void DodajClana(Kandidat noviClan)
        {
            Clanovi.Add(noviClan);
        }


        public void DodajClanoveURukovodstvo(List<Kandidat> dodatno)
        {
            Rukovodstvo ??= new List<Kandidat>();
            dodatno.ForEach(clan => {
                if (Clanovi.Contains(clan) && !Rukovodstvo.Contains(clan))
                    Rukovodstvo.Add(clan);
                else if (!Clanovi.Contains(clan))
                    throw new ArgumentException("Kandidat nije clan ove stranke!");
                else throw new ArgumentException("Kandidat je vec u rukovodstvu!");
            });
        }

        public void UkloniClanoveIzRukovodstva(List<Kandidat> visak)
        {
            Rukovodstvo ??= new List<Kandidat>();
            visak.ForEach(clan => {
                if (Clanovi.Contains(clan) && Rukovodstvo.Contains(clan))
                    Rukovodstvo.Remove(clan);
                else if (!Clanovi.Contains(clan))
                    throw new ArgumentException("Kandidat nije clan ove stranke!");
                else throw new ArgumentException("Kandidat nije u rukovodstvu!");
            });
        }

        public List<Kandidat> VratiClanove()
        {
            return Clanovi;
        }

        public int VratiIdStranke()
        {
            return id;
        }

        public void DodajGlas(List<string> kandidatiStranke)
        {
            BrojGlasova++;
            if (kandidatiStranke.Count == 0)
            {
                Clanovi[0].DodajGlas();
                return;
            }

            kandidatiStranke.ForEach(id =>
            {
                Clanovi.Find(clan => clan.dajJIK() == id).DodajGlas();
            });
        }

        public int GetBrojGlasova()
        {
            return BrojGlasova;
        }

        // dodala Naida Pita
        private int DajBrojMandata()
        {
            int brojMandata = 0;
            for (int i = 0; i < Clanovi.Count; i++)
            {
                if (Clanovi[i].VratiBrojGlasova() >= 0.2 * GetBrojGlasova())
                {
                    brojMandata++;
                }
            }
            return brojMandata;
        }

        // dodala Naida Pita
        public void NadjiMandatlije()
        {
            for (int i = 0; i < Clanovi.Count; i++)
            {
                if (Clanovi[i].VratiBrojGlasova() >= 0.2 * GetBrojGlasova())
                {
                    ClanoviSaMandatom.Add(Clanovi[i]);
                }
            }
        }


        // FUNKCIONALNOST 3 Naida Pita
        public string PrikaziRezultate(int ukupniBrojGlasova)
        {
            if(ukupniBrojGlasova < GetBrojGlasova())
            {
                throw new Exception("Broj glasova stranke je veći od broja ukupnih glasova!");
            }
            string ispis = "";
            ispis += "\nStranka " + id.ToString() + "\n" + "Broj glasova: " + GetBrojGlasova().ToString() + "\n" + "Postotak glasova: ";
            ispis += Math.Round((Decimal)((double)GetBrojGlasova() / ukupniBrojGlasova * 100),2).ToString() + "%\n";
            ispis += "Broj članova sa mandatima: " + DajBrojMandata() + "\n";
            if (DajBrojMandata() != 0)
            {
                ispis += "Članovi sa mandatom: \n";
                for (int i = 0; i < ClanoviSaMandatom.Count; i++)
                {
                    if (ClanoviSaMandatom.Count != 0)
                    {
                        ispis += i + 1 + ". " + ClanoviSaMandatom[i].OsnovneInformacije() + ", broj glasova " + ClanoviSaMandatom[i].VratiBrojGlasova().ToString();
                        ispis += ", postotak glasova " + Math.Round((Decimal)((double)ClanoviSaMandatom[i].VratiBrojGlasova() / GetBrojGlasova() * 100), 2).ToString() + "%.";
                        if (i != ClanoviSaMandatom.Count - 1)
                            ispis += "\n";
                    }
                }
            }
            else ispis += "Nema članova sa mandatom.";
            return ispis;
        }

        public string DajRezultateRukovodstva()
        {
            int brojGlasova = 0;
            string clanovi = "Kandidati:";
            Rukovodstvo ??= new List<Kandidat>();
            Rukovodstvo.ForEach(clan =>
            {
                Kandidat original = Clanovi.Find(k => k.Equals(clan));
                int osvojio = original.VratiBrojGlasova();
                brojGlasova += osvojio;
                clanovi += "\nIdentifikacioni broj: " + clan.dajJIK();
            });
            string ispis = "Ukupan broj glasova: " + brojGlasova.ToString() + "\n";
            ispis += clanovi;
            return ispis;
        }

        public void ResetujClanoveSaMandatom()
        {
            ClanoviSaMandatom.Clear();
        }

        public void PostaviBrojGlasova(int broj)
        {
            BrojGlasova = broj;
        }
    }
}