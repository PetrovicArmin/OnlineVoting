using System.Collections.Generic;

namespace OnlineVoting
{
    internal class Stranka
    {
        private int id { get; set; }
        private List<Kandidat> clanovi { get; set; }
        private int BrojGlasova { get; set; }

<<<<<<< HEAD
        public int GetBrojGlasova()
        {
            return BrojGlasova;
        }

        public void SetBrojGlasova(int brojGlasova)
        {
            BrojGlasova = brojGlasova;
        }

        public Stranka(List<Kandidat> clanovi)
=======
        public Stranka(List<Kandidat> clanovi, int id)
>>>>>>> fda4d402cd6fcf299ea439524d9a2ca61a54aba4
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
<<<<<<< HEAD
=======

        public int GetBrojGlasova()
        {
            return BrojGlasova;
        }
>>>>>>> fda4d402cd6fcf299ea439524d9a2ca61a54aba4
    }
}
