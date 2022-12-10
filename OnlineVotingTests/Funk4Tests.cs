using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVotingTests
{
    [TestClass]
    public class Funk4Tests
        /*
        Funkcionalnost 4 razvijala: Dženana Terzić (18921)
        Za implementaciju funkcionalnosti su u klasi Stranka izvrsene sljedece izmjene:
            -> dodan atribut Rukovodstvo
            -> dodane metode:
                -> dodaj clanove u rukovodstvo
                -> ukloni clanove iz rukovodstva
                -> daj rezultate rukovodstva
        */
        
    {
        private List<Kandidat>? Kandidati;
        private Stranka? stranka;
        private Osoba? osoba=new Osoba("Neko", "Nekic", "Negdje", "21.02.2001", "1234567890", 210200112345);
        private Izbori izbori = Izbori.DajIzbore();

        [TestInitialize]
        public void InicijalizacijaStranke()
        {
            Kandidati = new List<Kandidat>
            {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "1234678765", 121299225234),
                new Kandidat("Haso", "Hasić", "Hendek bb", "12.12.1992", "1234678765", 121299225234),
                new Kandidat("Josip", "Josipović", "Adresa", "4.11.1989", "1231231231", 2214189271298)
            };
            stranka = new Stranka(Kandidati, 1);
            Izbori.stranke = new List<Stranka> { stranka };
            Izbori.kandidati = Kandidati;
            

        }

        #region Potpuno testiranje funkiconalnosti - izuzeci

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DodajURukovodstvo_NijeClan_BacanjeIzuzetka()
        {
            List<Kandidat> novi = new List<Kandidat> { 
                new Kandidat("Maja", "Majić", "Hendek 1", "31.12.1992", "3134678765", 121299225236) 
            };
            stranka.DodajClanoveURukovodstvo(novi);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DodajURukovodstvo_VecURukovodstvu_BacanjeIzuzetka()
        {
            List<Kandidat> Mujo2 = new List<Kandidat> {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "1234678765", 121299225234),
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "1234678765", 121299225234)
            };
            stranka.DodajClanoveURukovodstvo(Mujo2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UkloniIzRukovodstva_NijeClan_BacanjeIzuzetka()
        {
            List<Kandidat> novi = new List<Kandidat> {
                new Kandidat("Maja", "Majić", "Hendek 1", "31.12.1992", "3134678765", 121299225236)
            };
            stranka.UkloniClanoveIzRukovodstva(novi);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UkloniIzRukovodstva_NijeuRukovodstvu_BacanjeIzuzetka()
        {
            List<Kandidat> Mujo = new List<Kandidat> {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "1234678765", 121299225234)
            };
            stranka.UkloniClanoveIzRukovodstva(Mujo);
        }

        #endregion

        #region Potpuno testiranje funkcionalnosti - ispravni slucajevi


        [TestMethod]
        public void DodajURukovodstvo_Ispravno_1GlasZaMuju()
        {
            List<Kandidat> Mujo = new List<Kandidat> {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "1234678765", 121299225234)
            };
            stranka.DodajClanoveURukovodstvo(Mujo);

            //glasamo za muju
            Glas glasZaMuju = new(1, Mujo);
            izbori.ProcesirajGlas(osoba, glasZaMuju);
            
            string rezultati = stranka.DajRezultateRukovodstva();
            Assert.AreEqual("Ukupan broj glasova: 1\nKandidati:\nIdentifikacioni broj: " + Mujo[0].dajJIK(), rezultati);
        }


        [TestMethod]
        public void UkloniIzRukovodstva_Ispravno_0Glasova()
        {
            List<Kandidat> Mujo = new List<Kandidat> {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "1234678765", 121299225234)
            };
            stranka.DodajClanoveURukovodstvo(Mujo);

            //glasamo za muju
            Glas glasZaMuju = new(1, Mujo);
            izbori.ProcesirajGlas(osoba, glasZaMuju);

            stranka.UkloniClanoveIzRukovodstva(Mujo);

            string rezultati = stranka.DajRezultateRukovodstva();
            Assert.AreEqual("Ukupan broj glasova: 0\nKandidati:", rezultati);

        }

        [TestMethod]
        public void PraznoRukovodstvo_Ispravno_0Glasova0Kandidata()
        {
            string rezultati = stranka.DajRezultateRukovodstva();
            Assert.AreEqual("Ukupan broj glasova: 0\nKandidati:", rezultati);
        }

        #endregion


        #region Inline testiranje
        #endregion


        #region CSV testiranje
        #endregion

    }
}
