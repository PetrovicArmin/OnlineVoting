using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    public class Osoba
    {
        private String ime { get; set; }
        private String prezime { get; set; }
        private String adresa { get; set; }
        private DateTime datumRodjenja { get; set; }
        private String brojLicneKarte { get; set; }
        private long maticniBroj { get; set; }
        private String JIK { get; }

        public Osoba (string ime, string prezime, string adresa, DateTime datumRodjenja, string brojLicneKarte, long maticniBroj)
        {
            this.ime = ime;
            this.prezime = prezime;
            this.adresa = adresa;
            this.datumRodjenja = datumRodjenja;
            this.brojLicneKarte = brojLicneKarte;
            this.maticniBroj = maticniBroj;
            this.JIK = generisiJIK(this.ime, this.prezime, this.adresa, this.datumRodjenja,this.brojLicneKarte,this.maticniBroj);
        }

        private String generisiJIK(String ime, String prezime, String adresa, DateTime datumRodjenja, String brojLicneKarte, long maticniBroj)
        {
            String jik = "";
            jik = ime.Substring(0,2) + prezime.Substring(0,2) + adresa.Substring(0,2) + datumRodjenja.ToString("dd.MM.yyyy").Substring(0,2) + 
                brojLicneKarte.Substring(0,2) + maticniBroj.ToString().Substring(0,2);
            return jik;
        }


    }
}
