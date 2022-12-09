using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    public class Kandidat : Osoba, Informacije
    {
        private String detaljneInformacije = "";
        private Dictionary<string, Tuple<DateTime, DateTime>> clanstvoUStrankama = new Dictionary<string, Tuple<DateTime, DateTime>>();
        private int BrojGlasova { get; set; }   
        public Kandidat(string ime, string prezime, string adresa, String datumRodjenja, string brojLicneKarte, long maticniBroj) : base(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj)
        {
        }

        //ažuriranje detaljnih informacija radi čuvanja stranki u kojima je bio kandidat: by Petrović Armin
        public String DetaljneInformacije()
        {

            detaljneInformacije = "Detaljne informacije: \n" + "Adresa: " + this.adresa + "\nBroj lične karte:  " + this.brojLicneKarte + "\nMatični broj:  " + maticniBroj.ToString() + "\n";
            //TODO: dodati iz dictionary-ja odgovarajuće rezultate!
            return detaljneInformacije;
        }

        public void UclaniUStranku(String nazivStranke, DateTime datumUclanjenja)
        {
            if (clanstvoUStrankama.ContainsKey(nazivStranke))
            {
                throw new ArgumentException("Kandidat je već prijavljen u stranku istog naziva!");
            } else
            {
                clanstvoUStrankama[nazivStranke] = new Tuple<DateTime, DateTime>(datumUclanjenja, DateTime.MinValue);
            }
        }

        public void OdjaviIzStranke(String nazivStranke, DateTime datumOdjave)
        {
            if (clanstvoUStrankama.ContainsKey(nazivStranke))
            {
                DateTime datumUclanjenja = clanstvoUStrankama[nazivStranke].Item1;
                clanstvoUStrankama[nazivStranke] = new Tuple<DateTime, DateTime>(datumUclanjenja, datumOdjave);
            } else
            {
                throw new ArgumentException("Kandidat nije učlanjen u navedenu stranku, da bismo ga iz iste odjavili!");
            }
        }

        public String OsnovneInformacije()
        {
            String osnovneInformacije = this.ime + " " + this.prezime;
            return osnovneInformacije;
        }

        public void DodajGlas()
        {
            BrojGlasova++;
        }

        public int VratiBrojGlasova()
        {
            return BrojGlasova;
        }
    }
}
