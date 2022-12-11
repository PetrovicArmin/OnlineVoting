using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineVoting
{
    public class Osoba
    {
        protected String ime { get; set; }
        protected String prezime { get; set; }
        protected String adresa { get; set; }
        protected String datumRodjenja { get; set; }
        protected String brojLicneKarte { get; set; }
        protected long maticniBroj { get; set; }
        protected String JIK { get; set; }

        public Osoba (string ime, string prezime, string adresa, String datumRodjenja, string brojLicneKarte, long maticniBroj)
        {
            validiraj(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj);
            this.ime = ime;
            this.prezime = prezime;
            this.adresa = adresa;
            this.datumRodjenja = datumRodjenja;
            this.brojLicneKarte = brojLicneKarte;
            this.maticniBroj = maticniBroj;
            JIK = generisiJIK(this.ime, this.prezime, this.adresa, this.datumRodjenja,this.brojLicneKarte,this.maticniBroj);
        }

        private String generisiJIK(String ime, String prezime, String adresa, String datumRodjenja, String brojLicneKarte, long maticniBroj)
        {
            String jik = "";
            jik = ime.Substring(0,2) + prezime.Substring(0,2) + adresa.Substring(0,2) + datumRodjenja.Substring(0,2) + 
                brojLicneKarte.Substring(0,2) + maticniBroj.ToString().Substring(0,2);
            return jik;
        }

        public String dajJIK()  // samo JIK-u mogu sa vana pristupiti oni koji verifikuju glasove (treca recenica zahtjeva u zadaci)
        {
            return this.JIK;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return false;
            Osoba o = obj as Osoba;
            if (o == null) return false;
            return this.dajJIK() == o.dajJIK();
        }

        // Velid

        private bool validiraj(string ime, string prezime, string adresa, String datumRodjenja, string brojLicneKarte, long maticniBroj)
        {
            string prviDioMaticnog = datumRodjenja.Substring(0, 2) + datumRodjenja.Substring(3, 2) + datumRodjenja.Substring(7, 3);
            string pattern = @"^([A-Z\u0100-\u017Fa-z\-]+)$";
            bool samoSlovaICrtice = Regex.IsMatch(ime + prezime, pattern);
            if (ime.Trim(' ') == "" || prezime.Trim(' ') == "" || adresa.Trim(' ') == "")
                throw new ArgumentException("Ime, prezime i adresa ne smiju biti prazni");
            if (!(ime.Count() >= 2 && ime.Count() <= 40))
                throw new ArgumentException("Ime mora biti između 2 i 40 karaktera");
            if (!(prezime.Count() >= 3 && prezime.Count() <= 50))
                throw new ArgumentException("Prezime mora biti između 3 i 50 karaktera");
            if (!samoSlovaICrtice)
                throw new ArgumentException("Ime i prezime smiju sadržavati samo slova i crtice");
            DateTime dob = DateTime.ParseExact(datumRodjenja, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            if (dob > DateTime.Now)
                throw new ArgumentException("Datum rođenja ne može biti u budučnosti");
            if (dob.AddYears(18) > DateTime.Now)
                throw new ArgumentException("Glasač mora biti punoljetan");
            if (!Regex.IsMatch(brojLicneKarte, @"^\d{3}[EJKMT]\d{3}$"))
                throw new ArgumentException("Broj lične karte mora biti u formatu 999A999");
            if (maticniBroj.ToString().Substring(0, 7) != prviDioMaticnog || maticniBroj.ToString().Count() != 13)
                throw new ArgumentException("Matični broj nije validan");
            return true;
        }

        private void ValidirajJIK(string ime, string prezime, string adresa, string datumRodjenja, string brojLicneKarte, long maticniBroj, string JIK)
        {
            if (generisiJIK(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj) != JIK)
                throw new ArgumentException("Neispravan JIK")
        }

        //zadatak 2 zadaća 5
        public bool VjerodostojnostGlasaca(IProvjera sigurnosnaProvjera)
        {
            if (sigurnosnaProvjera.DaLiJeVecGlasao(JIK))
                throw new Exception("Glasač je već izvršio glasanje!");

            return true;
        }
    }
}
