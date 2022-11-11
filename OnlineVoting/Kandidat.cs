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

        public string DetaljneInformacije()
        {
            String detaljneInformacije = "";
            return detaljneInformacije;
        }

        public string OsnovneInformacije()
        {
            String osnovneInformacije = "";
            return osnovneInformacije;
        }
    }
}
