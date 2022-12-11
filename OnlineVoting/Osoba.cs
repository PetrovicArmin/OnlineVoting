using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //zadatak 2 zadaća 5
        public bool VjerodostojnostGlasaca(IProvjera sigurnosnaProvjera)
        {
            if (sigurnosnaProvjera.DaLiJeVecGlasao(JIK))
                throw new Exception("Glasač je već izvršio glasanje!");
            return true;
        }
    }
}
