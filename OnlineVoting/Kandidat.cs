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

        //by Petrović Armin
        public String DetaljneInformacije()
        {
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
        public DateTime DajNajkasnijuOdjavu()
        {
            DateTime najkasnijaOdjava = DateTime.MinValue;
            for (int i = 0; i < clanstvoUStrankama.Count; i++)
            {
                if (clanstvoUStrankama.ElementAt(i).Value.Item2 > najkasnijaOdjava)
                    najkasnijaOdjava = clanstvoUStrankama.ElementAt(i).Value.Item2;
            }

            return najkasnijaOdjava;
        }

        //by: Petrović Armin 
        public Tuple<string, DateTime, DateTime> DajTrenutnuStranku()
        {
            for (int i = 0; i < clanstvoUStrankama.Count; i++)
            {
                var trenutniPodaci = clanstvoUStrankama.ElementAt(i);
                if (trenutniPodaci.Value.Item2 == DateTime.MinValue)
                    return new Tuple<string, DateTime, DateTime>(trenutniPodaci.Key, trenutniPodaci.Value.Item1, trenutniPodaci.Value.Item2);
            }

            return null;
        }

        //by: Petrović Armin
        public void UclaniUStranku(String nazivStranke, DateTime datumUclanjenja)
        {
            if (DajTrenutnuStranku() != null)
                throw new ArgumentException("Kandidat je već učlanjen u neku stranku!");

            if (datumUclanjenja < DajNajkasnijuOdjavu())
                throw new ArgumentException("Datum učlanjenja mora biti kasniji od najkasnijeg datuma odjave!");

            clanstvoUStrankama[nazivStranke] = new Tuple<DateTime, DateTime>(datumUclanjenja, DateTime.MinValue);
        }

        //by: Petrović Armin
        public void OdjaviIzStranke(String nazivStranke, DateTime datumOdjave)
        {
            Tuple<string, DateTime, DateTime> informacijeOTrenutnojStranki = DajTrenutnuStranku();

            if (informacijeOTrenutnojStranki == null)
                throw new ArgumentException("Kandidat nije učlanjen niti u jednu stranku!");

            if (informacijeOTrenutnojStranki.Item1 != nazivStranke)
                throw new ArgumentException("Kandidat je trenutno učlanjen u neku drugu stranku!");

            DateTime datumUclanjenja = informacijeOTrenutnojStranki.Item2;

            if (datumUclanjenja > datumOdjave)
                throw new ArgumentException("Kandidat ne može prije napraviti odjavu nego prijavu!");

            clanstvoUStrankama[nazivStranke] = new Tuple<DateTime, DateTime>(datumUclanjenja, datumOdjave);
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

        //za potrebe funkcionalnosti 5 dodao Faruk S 
        public void OduzmiGlas() 
        { 
            BrojGlasova--;
        }

        public int VratiBrojGlasova()
        {
            return BrojGlasova;
        }

        // dodala Naida Pita
        public void PostaviBrojGlasova(int broj)
        {
            BrojGlasova = broj;
        }
    }
}
