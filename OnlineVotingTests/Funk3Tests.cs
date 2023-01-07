using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace OnlineVotingTests
{   
    // Uradila: Naida Pita, 18849
    // Za ovu funkcionalnost dodane su:
    // metoda dajBrojMandata u klasi Stranka koja ujedno i stavlja u listu clanove sa mandatom
    // metoda u klasi Izbori koja vraca broj ukupnih validnih glasova na izborima
    // metoda prikaziRezultate koja se primarno odnosi na trazenu funkcionalnost broj 3 u klasi Stranka, za ispis trazenih informacija
    // metode postavljanja broja glasova u cilju lakseg testiranjas (i konkretno ove funkcionalnosti)
    // metoda resetiranja clanova zbog TestInitialize
    // metoda nadjiMandatlije u Stranka koja nalazi ljude sa mandatima da bi se ispisali

    [TestClass]
    public class Funk3Tests
    {
        static Stranka strankaA;
        static List<Kandidat> kandidatiA;


        [ClassInitialize]
        public static void PocetnaInicijalizacijaStalnihPodataka(TestContext context)
        {
            kandidatiA = new List<Kandidat> {
                new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "121K123", 1212992541234),
                new Kandidat("Haso", "Hasić", "Hendek bb", "12.12.1992", "456M224", 1212992541234),
                new Kandidat("Josip", "Josipović", "Adresa", "11.11.1989", "777T777", 1111989541231)
            };
            strankaA = new Stranka(kandidatiA, 1);
        }


        [TestInitialize]
        public void InicijalizacijaIzbora()
        {
            strankaA.ResetujClanoveSaMandatom();
        }

        #region Inline testiranje bez mandata
        
        static IEnumerable<object[]> BrojeviGlasovaBezMandata
        {
            get
            {
                return new[]
                {
                    new object[] {1500,500, 20, 5, 30},
                    new object[] {150, 86, 4, 5, 2}
                };
            }
        }


        [TestMethod]
        [DynamicData("BrojeviGlasovaBezMandata")]
        public void prikaziRezultate_NemaClanovaSaMandatom_IspisNemaMandata(int ukupnoGlasova, int glasoviStranke, int glasoviPrviKandidat, int glasoviDrugiKandidat, int glasoviTreciKandidat)
        {
            strankaA.PostaviBrojGlasova(glasoviStranke);
            kandidatiA[0].PostaviBrojGlasova(glasoviPrviKandidat);
            kandidatiA[1].PostaviBrojGlasova(glasoviDrugiKandidat);
            kandidatiA[2].PostaviBrojGlasova(glasoviTreciKandidat);
            strankaA.NadjiMandatlije();
            Assert.IsTrue(strankaA.PrikaziRezultate(ukupnoGlasova).Contains("Nema članova sa mandatom."));
        }
        #endregion

        #region Inline testiranje jedan mandat

        static IEnumerable<object[]> BrojeviGlasovaJedanMandat
        {
            get
            {
                return new[]
                {
                    new object[] {150,100,50,0,0},
                    new object[] {174,100,0,52,0},
                    new object[] {1200,100,0,0,52}
                };
            }
        }

        [TestMethod]
        [DynamicData("BrojeviGlasovaJedanMandat")]
        public void prikaziRezultate_JedanMandatlija_IspisBrojMandataJedan(int ukupnoGlasova, int glasoviStranke, int glasoviPrviKandidat, int glasoviDrugiKandidat, int glasoviTreciKandidat)
        {
            strankaA.PostaviBrojGlasova(glasoviStranke);
            kandidatiA[0].PostaviBrojGlasova(glasoviPrviKandidat);
            kandidatiA[1].PostaviBrojGlasova(glasoviDrugiKandidat);
            kandidatiA[2].PostaviBrojGlasova(glasoviTreciKandidat);
            strankaA.NadjiMandatlije();       
            Assert.IsTrue(strankaA.PrikaziRezultate(ukupnoGlasova).Contains("Broj članova sa mandatima: 1"));
        }


        #endregion

        #region Testiranje svi clanovi imaju mandat + da li je cijeli ispis tacan

        [TestMethod]
        public void prikaziRezultate_TriMandatlije_IspisBrojMandataTri()
        {
            strankaA.PostaviBrojGlasova(100);
            kandidatiA[0].PostaviBrojGlasova(50);
            kandidatiA[1].PostaviBrojGlasova(25);
            kandidatiA[2].PostaviBrojGlasova(25);
            strankaA.NadjiMandatlije();
            string exp = "\nStranka 1\nBroj glasova: " + 100 + "\nPostotak glasova: " + (66.67).ToString() + "%\n";
            exp += "Broj članova sa mandatima: 3\nČlanovi sa mandatom: \n1. Mujo Mujić, broj glasova " + 50 + ", postotak glasova 50%.\n";
            exp += "2. Haso Hasić, broj glasova " + 25 + ", postotak glasova 25%.\n";
            exp += "3. Josip Josipović, broj glasova " + 25 + ", postotak glasova 25%.";
            Assert.AreEqual(exp, strankaA.PrikaziRezultate(150));
        }
        #endregion

        #region Testiranje bacanje izuzetka ukoliko su glasovi stranke veći od ukupnih
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void prikaziRezultate_GlasoviStrankeVeciOdUkupnih_Izuzetak()
        {
            strankaA.PostaviBrojGlasova(151);
            kandidatiA[0].PostaviBrojGlasova(50);
            kandidatiA[1].PostaviBrojGlasova(25);
            kandidatiA[2].PostaviBrojGlasova(25);
            strankaA.NadjiMandatlije();
            strankaA.PrikaziRezultate(150);
        }

        #endregion

        #region CSV testiranje da li sadrži konkretnog člana sa mandatom
        public static IEnumerable<object[]> UčitajPodatkeCSV()
        {
            using (var reader = new StreamReader("GlasoviZaFunk3.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { int.Parse(elements[0]), int.Parse(elements[1]), int.Parse(elements[2]), int.Parse(elements[3]), int.Parse(elements[4]) };
                }
            }
        }
        static IEnumerable<object[]> GlasoviZaFunk3
        {
            get
            {
                return UčitajPodatkeCSV();
            }
        }

        [TestMethod]
        [DynamicData("GlasoviZaFunk3")]
        public void prikaziRezultate_CSVPodaci_Ispis2Mandata(int ukupnoGlasova, int glasoviStranke, int glasoviPrviKandidat, int glasoviDrugiKandidat, int glasoviTreciKandidat)
        {
            strankaA.PostaviBrojGlasova(glasoviStranke);
            kandidatiA[0].PostaviBrojGlasova(glasoviPrviKandidat);
            kandidatiA[1].PostaviBrojGlasova(glasoviDrugiKandidat);
            kandidatiA[2].PostaviBrojGlasova(glasoviTreciKandidat);
            strankaA.NadjiMandatlije();
            Assert.IsTrue(strankaA.PrikaziRezultate(ukupnoGlasova).Contains("1. Mujo Mujić"));
            Assert.IsTrue(strankaA.PrikaziRezultate(ukupnoGlasova).Contains("2. Josip Josipović"));
        }
        #endregion
    }
}