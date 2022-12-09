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
            //budući da se same informacije unutar clanstva mogu mijenjati, to se ovaj string mora generirati iznova i iznova!
            detaljneInformacije = "Detaljne informacije: \n" + "Adresa: " + this.adresa + "\nBroj lične karte:  " + this.brojLicneKarte + "\nMatični broj:  " + maticniBroj.ToString() + ".\n";
            detaljneInformacije += "Kandidat je bio: \n";
            for (int i = 0; i < clanstvoUStrankama.Count; i++)
            {
                var element = clanstvoUStrankama.ElementAt(i);
                String imeStranke = element.Key;
                DateTime datumPrijave = element.Value.Item1;
                DateTime datumOdjave = element.Value.Item2;
                String datumOdjavePlaceholder = datumOdjave.ToString();
                if (datumOdjave == DateTime.MinValue)
                    datumOdjavePlaceholder = "DAN DANAS";
                detaljneInformacije += "Član stranke '" + imeStranke + "' od '" + datumPrijave.ToString() + "' do '" + datumOdjavePlaceholder + "'\n"; 
            }
            return detaljneInformacije;
        }

        //by: Petrović Armin
        public void UclaniUStranku(String nazivStranke, DateTime datumUclanjenja)
        {
            if (clanstvoUStrankama.ContainsKey(nazivStranke))
            {
                throw new ArgumentException("Kandidat je već prijavljen u stranku istog naziva!");
            }
            
            for (int i = 0; i < clanstvoUStrankama.Count; i++)
            {
                if (clanstvoUStrankama.ElementAt(i).Value.Item2 == DateTime.MinValue)
                    throw new DataException("Kandidat je već učlanjen u neku stranku!");
            }

            clanstvoUStrankama[nazivStranke] = new Tuple<DateTime, DateTime>(datumUclanjenja, DateTime.MinValue);
        }

        //by: Petrović Armin
        public void OdjaviIzStranke(String nazivStranke, DateTime datumOdjave)
        {
            if (clanstvoUStrankama.ContainsKey(nazivStranke))
            {
                DateTime datumUclanjenja = clanstvoUStrankama[nazivStranke].Item1;
                if (datumOdjave < datumUclanjenja)
                    throw new DataException("Kandidat ne može da se isčlani prije nego što se učlanio u stranku!");
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
