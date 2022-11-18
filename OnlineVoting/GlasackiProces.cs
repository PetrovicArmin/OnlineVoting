using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    internal class GlasackiProces
    {
        private Osoba Osoba { get; set; }   
        private Glas Glas { get; set; }

        public GlasackiProces(Osoba osoba, Glas glas)
        {
            this.Osoba = osoba;
            this.Glas = glas;
            TipizirajIVratiGlas();

        }

        public bool IdentifikujGlasaca()   
        {
            List<string> glasaci = Populacija.DajPopulaciju().getGlasaci();
            if (glasaci.Contains(Osoba.dajJIK()))   //ako je vec glasao, pada na ID fazi
                return false;
            return true;
        }

        public TipGlasa VerifikujGlas()
        {
            return TipizirajIVratiGlas().VratiTipGlasa();
        }

        public Glas TipizirajIVratiGlas()
        {
            int idStranke = Glas.VratiIDStranke();
            List<string> odabraniKandidati = Glas.VratiIDKandidata(); //ID u smislu IDeve, mnozina.
            List<Stranka> stranke = Izbori.stranke;
            List<Kandidat> nezavisni = Izbori.kandidati;
            Stranka odabranaStranka = stranke.Find(s => s.vratiIdStranke() == idStranke);
            if (idStranke == 0) //uzmimo da je to ako nije odabrao stranku
            {
                if (odabraniKandidati!=null && odabraniKandidati.Count == 1 && 
                    nezavisni.Where(k => k.dajJIK() == odabraniKandidati[0]).Any()
                    )
                {
                    Glas.PostaviTipGlasa(TipGlasa.NEZAVISNI_KANDIDAT);
                }
                else Glas.PostaviTipGlasa(TipGlasa.NEVAZECI);
            }
            else
            {
                if (odabraniKandidati == null || odabraniKandidati.Count() == 0)
                    Glas.PostaviTipGlasa(TipGlasa.SAMO_STRANKA);
                else
                {   //da li su svi odabrani clanovi iz odabrane stranke
                    List<string> kandidatiOdabraneStranke = odabranaStranka.vratiClanove().ConvertAll(
                            new Converter<Kandidat, string>(k => k.dajJIK()));
                    if (ContainsAllItems(kandidatiOdabraneStranke, odabraniKandidati))
                        Glas.PostaviTipGlasa(TipGlasa.STRANKA_KANDIDATI);
                    else
                        Glas.PostaviTipGlasa(TipGlasa.NEVAZECI);
                }
            }
            return Glas;
        }


        private bool ContainsAllItems(List<string> a, List<string> b)
        {
            return !b.Except(a).Any();
        }
    }
}
