using System.Collections.Generic;

namespace OnlineVoting
{
    internal class Stranka
    {
        private List<Kandidat> clanovi { get; set; }
        public Stranka(List<Kandidat> clanovi)
        {
            this.clanovi = clanovi;
        }

        public void dodajClana(Kandidat noviClan)
        {
            clanovi.Add(noviClan);
        }
    }
}
