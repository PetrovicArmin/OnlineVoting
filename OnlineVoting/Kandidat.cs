using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    public class Kandidat : Osoba, Informacije
    {
        public Kandidat(string ime, string prezime, string adresa, String datumRodjenja, string brojLicneKarte, long maticniBroj) : base(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj)
        {
        }

        public String DetaljneInformacije()
        {
            String detaljneInformacije = "Detaljne informacije: \n"+"Adresa: " + this.adresa + "\nBroj lične karte:  " + this.brojLicneKarte + "\nMatični broj:  " + maticniBroj.ToString();
            return detaljneInformacije;
        }

        public String OsnovneInformacije()
        {
            String osnovneInformacije = "Osnovne informacije: \n"+ "Ime: " + this.ime + "\nPrezime: " + this.prezime + "\nDatum rođenja: " + this.datumRodjenja;
            return osnovneInformacije;
        }
    }
}
