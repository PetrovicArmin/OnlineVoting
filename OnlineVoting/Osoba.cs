using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("CodeTuningTests")]
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
            if (ime.Trim(' ') == "" || prezime.Trim(' ') == "" || adresa.Trim(' ') == "")
                throw new ArgumentException("Ime, prezime i adresa ne smiju biti prazni");
            if (ime.Length is not (>= 2 and <= 40))
                throw new ArgumentException("Ime mora biti između 2 i 40 karaktera");
            if (prezime.Length is not (>= 3 and <= 50))
                throw new ArgumentException("Prezime mora biti između 3 i 50 karaktera");
            if (!Regex.IsMatch(ime + prezime, @"^([A-Z\u0100-\u017Fa-z\-]+)$"))
                throw new ArgumentException("Ime i prezime smiju sadržavati samo slova i crtice");
            DateTime dob = DateTime.ParseExact(datumRodjenja, "dd.MM.yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            DateTime sada = DateTime.Now;
            if (dob > sada)
                throw new ArgumentException("Datum rođenja ne može biti u budućnosti");
            if (dob.AddYears(18) > sada)
                throw new ArgumentException("Glasač mora biti punoljetan");
            if (!Regex.IsMatch(brojLicneKarte, @"^\d{3}[EJKMT]\d{3}$"))
                throw new ArgumentException("Broj lične karte mora biti u formatu 999A999");
            string maticni = maticniBroj.ToString();
            string prviDioMaticnog = datumRodjenja[..2] + datumRodjenja.Substring(3, 2) + datumRodjenja.Substring(7, 3);
            if (maticni[..7] != prviDioMaticnog || maticni.Length != 13)
                throw new ArgumentException("Matični broj nije validan");
            return true;
        }

        public bool ValidirajJIK(string JIK)
        {
            if (generisiJIK(this.ime, this.prezime, this.adresa, this.datumRodjenja, this.brojLicneKarte, this.maticniBroj) != JIK)
                throw new ArgumentException("Neispravan JIK");
            return true;
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
