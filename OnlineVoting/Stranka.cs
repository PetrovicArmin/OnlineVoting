using System.Collections.Generic;

namespace OnlineVoting
{
    internal class Stranka
    {
        private int id { get; set; }
        private List<Kandidat> clanovi { get; set; }
        private int BrojGlasova { get; set; }

        public int GetBrojGlasova()
        {
            return BrojGlasova;
        }

        public void SetBrojGlasova(int brojGlasova)
        {
            BrojGlasova = brojGlasova;
        }

        public Stranka(List<Kandidat> clanovi)
        {
            this.clanovi = clanovi;
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
    }
}
