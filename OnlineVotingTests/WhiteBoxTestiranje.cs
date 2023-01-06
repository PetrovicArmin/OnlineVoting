using CsvHelper;
using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVotingTests
{
    [TestClass]
    public class WhiteBoxTestiranje
    {

        //metoda validiraj se poziva u sklopu konstruktora klase Osoba, te se zbog toga testiraju razliciti slucajevi inicijalizacije.

        //FARUK ŠAHAT
        #region inicijalizacija podataka
        private readonly string ispravnoIme = "Ero";
        private readonly string ispravnoPrezime = "Remesedz";
        private readonly string ispravnaAdresa = "Sarajevo";
        private readonly string ispravanDatumRodjenja = "21.02.2001";
        private string ispravanBrojLicneKarte = "131K244";
        private readonly long ispravanMaticniBroj = 2102001123456;
        private Osoba? osoba = null;
        #endregion

        #region potpuni obuhvat odluka
        //ARMIN PETROVIĆ
        [TestMethod]
        public void IspravniPodaci_Put01()
        {
            osoba = new Osoba(ispravnoIme, ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
            Assert.IsNotNull(osoba);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_LosPrviDioMaticnogBroja_Put02()
        {
            new Osoba(ispravnoIme, ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, 2302001123456);
        }

        //VELID IMŠIROVIĆ
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_LosBrojLicneKarte_Put03()
        {
            new Osoba(ispravnoIme, ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte+"ne valja", ispravanMaticniBroj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_MaloljetnaOsoba_Put04()
        {
            string datumRodjenjaMaloljetneOsobe = DateTime.Now.AddYears(-6).ToString("dd.MM.yyyy");
            new Osoba(ispravnoIme, ispravnoPrezime, ispravnaAdresa, datumRodjenjaMaloljetneOsobe, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_DatumRodjenjaUBuducnosti_Put05()
        {
            string datumRodjenjaUBuducnosti = DateTime.Now.AddYears(6).ToString("dd.MM.yyyy");
            new Osoba(ispravnoIme, ispravnoPrezime, ispravnaAdresa, datumRodjenjaUBuducnosti, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        //NAIDA PITA
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_LosFormatPrezimena_Put06()
        {
            new Osoba(ispravnoIme, ispravnoPrezime+"1", ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_PrekratkoPrezime_Put07()
        {
            string prekratkoPrezime = "T";
            new Osoba(ispravnoIme, prekratkoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_prekratkoIme_Put08()
        {
            string prekratkoIme = "D";
            new Osoba(prekratkoIme, ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        //DŽENANA TERZIĆ
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_BlankAdresa_Put09()
        {
            new Osoba(ispravnoIme, ispravnoPrezime, "", ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        #endregion

        #region potpuni obuhvat uslova - dodatni testovi - slucajevi koji nisu pokriveni obuhvatom odluka
        //ARMIN PETROVIĆ
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_LosaDuzinaMaticnogBroja_Put02()
        {
            new Osoba(ispravnoIme, ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj*10);
        }

        //FARUK ŠAHAT
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_PredugoPrezime_Put07()
        {
            string predugoPrezime = "aaaaaaaaaa";
            for (int i = 1; i <= 3; i++)
                predugoPrezime += predugoPrezime;
            new Osoba(ispravnoIme, predugoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_predugoIme_Put08()
        {
            string predugoIme = "aaaaaaaaaa";
            for (int i = 1; i <= 3; i++)
                predugoIme += predugoIme;
            new Osoba(predugoIme, ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        //DŽENANA TERZIĆ
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_BlankPrezime_Put09()
        {
            new Osoba(ispravnoIme, "", ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NeispravniPodaci_BlankIme_Put09()
        {
            new Osoba("", ispravnoPrezime, ispravnaAdresa, ispravanDatumRodjenja, ispravanBrojLicneKarte, ispravanMaticniBroj);
        }
        #endregion
    }
}
