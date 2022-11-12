using System;

namespace OnlineVoting
{
    internal class Glas
    {
        private Integer idStranke { get; set; }
        private List<Integer> idKandidata { get; set; }
        private TipGlasa tipGlasa { get; set; }
        public Glas(Integer stranka, List<Kandidat> kandidati)
        {
            this.idStranke = stranka;
            this.idKandidata = kandidati;
        }

        public Integer VratiIDStranke()
        {
            return this.idStranke;
        }
        public List<Kandidat> VratiIDKandidata()
        {
            return this.idKandidata;
        }
        public void PostaviTipGlasa(TipGlasa tip)
        {
            tipGlasa= tip;
            return;
        }
        
    }
}