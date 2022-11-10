using System.Collections.Generic;

namespace OnlineVoting
{
    internal class Stranka
    {
        private List<string> clanovi { get; set; }
        public Stranka(List<string> clanovi)
        {
            this.clanovi = clanovi;
        }

        public void dodajClana(string clan)
        {
            clanovi.Add(clan);
        }
    }
}
