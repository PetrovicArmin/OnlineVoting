using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    internal class GlasackiProces
    {
        private Osoba osoba { get; set; }   
        private Glas glas { get; set; }

        public GlasackiProces(Osoba osoba, Glas glas)
        {
            this.osoba = osoba;
            this.glas = glas;
            TipizirajIVratiGlas();

        }

        public bool IdentifikujGlasaca()   
        {
            List<string> glasaci = Populacija.DajPopulaciju().getGlasaci();
            if (glasaci.Contains(osoba.dajJIK()))   //ako je vec glasao, pada na ID fazi
                return false;
            return true;
        }

        public TipGlasa VerifikujGlas()
        {
            return TipizirajIVratiGlas().VratiTipGlasa();
        }

        public Glas TipizirajIVratiGlas()
        {
            int idStranke = glas.VratiIDStranke();
            List<string> odabraniKandidati = glas.VratiIDKandidata(); //ID u smislu IDeve, mnozina.
            List<Stranka> stranke = Izbori.stranke;
            List<Kandidat> nezavisni = Izbori.kandidati;
            Stranka odabranaStranka = stranke.Find(s => s.vratiIdStranke() == idStranke);
            if (idStranke == 0) //uzmimo da je to ako nije odabrao stranku
            {
                if (odabraniKandidati!=null && odabraniKandidati.Count == 1 && 
                    nezavisni.Where(k => k.dajJIK() == odabraniKandidati[0]).Any()
                    )
                {
                    glas.PostaviTipGlasa(TipGlasa.NEZAVISNI_KANDIDAT);
                }
                else glas.PostaviTipGlasa(TipGlasa.NEVAZECI);
            }
            else
            {
                if (odabraniKandidati == null || odabraniKandidati.Count() == 0)
                    glas.PostaviTipGlasa(TipGlasa.SAMO_STRANKA);
                else
                {   //da li su svi odabrani clanovi iz odabrane stranke
                    List<string> kandidatiOdabraneStranke = odabranaStranka.vratiClanove().ConvertAll(
                            new Converter<Kandidat, string>(k => k.dajJIK()));
                    if (ContainsAllItems(kandidatiOdabraneStranke, odabraniKandidati))
                        glas.PostaviTipGlasa(TipGlasa.STRANKA_KANDIDATI);
                    else
                        glas.PostaviTipGlasa(TipGlasa.NEVAZECI);
                }
            }
            return glas;
        }


        private bool ContainsAllItems(List<string> a, List<string> b)
        {
            return !b.Except(a).Any();
        }
    }
}
