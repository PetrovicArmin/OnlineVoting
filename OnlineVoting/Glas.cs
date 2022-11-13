namespace OnlineVoting
{
    internal class Glas
    {
        private int idStranke { get; set; }
        private List<string> idKandidata { get; set; }
        private TipGlasa tipGlasa { get; set; }
        public Glas(int stranka, List<Kandidat> kandidati)
        {
            this.idStranke = stranka;
            this.idKandidata = kandidati.ConvertAll(
             new Converter<Kandidat, string>(k => k.dajJIK())); ;
        }

        public int VratiIDStranke()
        {
            return this.idStranke;
        }
        public List<string> VratiIDKandidata()
        {
            return this.idKandidata;
        }
        public void PostaviTipGlasa(TipGlasa tip)
        {
            tipGlasa = tip;
            return;
        }

    }
}