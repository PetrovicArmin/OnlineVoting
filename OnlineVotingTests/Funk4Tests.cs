using CsvHelper;
using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private Osoba? osoba=new Osoba("Neko", "Nekic", "Negdje", "21.02.2001", "123J123", 2102001123456);
        private Izbori izbori = Izbori.DajIzbore();

        [TestInitialize]
        public void InicijalizacijaStranke()
        {
            Kandidati = new List<Kandidat>
            {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "999K999", 1212992252342),
                    new Kandidat("Haso", "Hasić", "Hendek bb", "12.12.1992", "888M888", 1212992252341),
                    new Kandidat("Josip", "Josipović", "Adresa", "14.11.1989", "111E111", 1411989888888)
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
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "999K999", 1212992252342)
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
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "999K999", 1212992252342)
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
        static IEnumerable<object[]> Clanovi
        {
            get
            {
                return new[]
                {
                    new object[] {"Mujo", "Mujić", "Hendek bb", "12.12.1992", "999K999", 1212992252342 },
                    new object[] {"Haso", "Hasić", "Hendek bb", "12.12.1992", "888M888", 1212992252341},
                    new object[] {"Josip", "Josipović", "Adresa", "04.11.1989", "111E111", 0411989888888}
                };
            }
        }

        [TestMethod]
        [DynamicData("Clanovi")]
        public void TestIspisaRukovodstva_SadrziJikClana(string ime, string prezime, string adresa, string datumRodjenja, string brojLicneKarte, long maticniBroj)
        {
            Kandidat kandidat = new Kandidat(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj);
            stranka.DodajClanoveURukovodstvo(new List<Kandidat> { kandidat });
            string ispis = stranka.DajRezultateRukovodstva();
            StringAssert.Contains(ispis, kandidat.dajJIK());
        }

        
        #endregion


        #region CSV testiranje
        static IEnumerable<object[]> ClanoviCsv
        {
            get
            {
                return UcitajPodatkeCSV();
            }
        }

        [TestMethod]
        [DynamicData("ClanoviCsv")]
        public void TestUklanjanjaRukovodstva_NeSadrziJikClana(string ime, string prezime, string adresa, string datumRodjenja, string brojLicneKarte, long maticniBroj)
        {
            Kandidat kandidat = new Kandidat(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj);
            stranka.DodajClanoveURukovodstvo(new List<Kandidat> { kandidat });
            string ispis = stranka.DajRezultateRukovodstva();
            StringAssert.Contains(ispis, kandidat.dajJIK());
            stranka.UkloniClanoveIzRukovodstva(new List<Kandidat> { kandidat });
            ispis = stranka.DajRezultateRukovodstva();
            StringAssert.EndsWith(ispis, "Kandidati:");
        }

        static IEnumerable<object[]> UcitajPodatkeCSV()
        {
            using (var reader = new StreamReader("Funk4TestsData.csv")) 
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], elements[1],
                        elements[2], elements[3], elements[4], long.Parse(elements[5])};
                }
            }
        }

        #endregion

    }
}
