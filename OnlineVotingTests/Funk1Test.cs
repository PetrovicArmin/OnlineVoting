using CsvHelper;
using OnlineVoting;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;

namespace OnlineVotingTests
{
    [TestClass]
    public class Funk1Tests
    {
        //private Osoba? osoba = new Osoba("Neko", "Nekic", "Negdje", "21.02.2001", "1234567890", 210200112345);

        #region Testiranje validacije - izuzeci

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KreiranjeOsobe_PogresanFormatLicne_BacanjeIzuzetka()
        {
            Osoba osoba = new Osoba("Neko", "Nekic", "Negdje", "21.02.2001", "1234567890", 2102001123456);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KreiranjeOsobe_PraznoIme_BacanjeIzuzetka()
        {
            Osoba osoba = new Osoba("      ", "Nekic", "Negdje", "21.02.2001", "1234567890", 2102001123456);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KreiranjeOsobe_RodjendanUBuducnosti_BacanjeIzuzetka()
        {
            Osoba osoba = new Osoba("Velid", "Imširović", "Bugojno", "31.08.2029", "2214K2251", 3108029165423);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void KreiranjeOsobe_KratakMatični_BacanjeIzuzetka()
        {
            Osoba osoba = new Osoba("Velid", "Imširović", "Bugojno", "31.08.2009", "2214K2251", 31080091);
        }
        #endregion
        #region Inline testiranje - provjera izuzetaka
        static IEnumerable<object[]> Neispravni
        {
            get
            {
                return new[]
                {
                    new object[] {"      ", "Nekic", "Negdje", "21.02.2001", "1231K2244", 2102001123456, "Ime, prezime i adresa ne smiju biti prazni"},
                    new object[] {"Mujo", "Mujic", "", "21.02.2001", "1231K2244", 2102001123456, "Ime, prezime i adresa ne smiju biti prazni"},
                    new object[] {"Mujo", "    ", "Zmaja od Bosne", "21.02.2001", "1231K2244", 2102001123456, "Ime, prezime i adresa ne smiju biti prazni" },
                    new object[] {"Mujo", "Mujic", "Zmaja od Bosne", "21.02.2001", "11231K2244", 2102001123456, "Broj lične karte mora biti u formatu 9999A9999"},
                    new object[] {"Mujo", "Mujic", "Zmaja od Bosne", "21.02.2011", "1231K2244", 2102001123456, "Glasač mora biti punoljetan" },
                    new object[] {"Mujo", "Mujic", "Zmaja od Bosne", "21.02.2031", "1231K2244", 2102001123456, "Datum rođenja ne može biti u budučnosti" },
                    new object[] {"Mujo", "Mujic221", "Zmaja od Bosne", "21.02.2001", "1231K2244", 2102001123456, "Ime i prezime smiju sadržavati samo slova i crtice" },
                    new object[] {"A", "Hoc", "Zmaja od Bosne", "21.02.2001", "1231K2244", 2102001123456, "Ime mora biti između 2 i 40 karaktera" },
                    new object[] {"Asddeeeeeeekjlkivjaksdlejdkslvajedkalejvkaje", "Hoc", "Zmaja od Bosne", "21.02.2001", "1231K2244", 2102001123456, "Ime mora biti između 2 i 40 karaktera" },
                    new object[] {"Asddeeeeeeekjlk", "Ho", "Zmaja od Bosne", "21.02.2001", "1231K2244", 2102001123456, "Prezime mora biti između 3 i 50 karaktera" },
                };
            }
        }

        [TestMethod]
        [DynamicData("Neispravni")]
        public void KreirajOsobu_PrazniPodaci_Inline(string ime, string prezime, string adresa, string datum, string brojLicne, long jmbg, string izuzetak)
        {
            try
            {
                Osoba osoba = new Osoba(ime, prezime, adresa, datum, brojLicne, jmbg);
            } catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, izuzetak);
            }
        }
        #endregion
        #region CSV Testiranje
        static IEnumerable<object[]> OsobeCSV
        {
            get
            {
                return UčitajOsobeCSV();
            }
        }

        public static IEnumerable<object[]> UčitajOsobeCSV()
        {
            using (var reader = new StreamReader("Osobe.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], elements[1], elements[2], elements[3], elements[4], long.Parse(elements[5]), elements[6], elements[7] };
                }
            }
        }

        [TestMethod]
        [DynamicData("OsobeCSV")]
        public void Odjavi_Uclanjen_CSVTestiranje(string ime, string prezime, string adresa, string datumRodjenja, string brojLicneKarte, long maticniBroj, string rezultat, string jik)
        {
            try
            {
                Osoba osoba = new Osoba(ime, prezime, adresa, datumRodjenja, brojLicneKarte, maticniBroj);
                Assert.AreEqual(rezultat, "OK");
                // Da li je JIK generisan ispravno
                Assert.AreEqual(osoba.dajJIK(), jik);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(rezultat, "NOK");
            }
        }

        #endregion
        #region Tesitranje validacije
        [TestMethod]
        public void KreiranjeOsobe_PrezimeSaCrticama()
        {
            Osoba osoba = new Osoba("Mujo", "Mujić-Nekić", "Zmaja od Bosne", "11.10.1997", "2222J2222", 1110997243543);
            // Provjera da li je JIK dobar 
            Assert.AreEqual("MuMuZm112211", osoba.dajJIK());
        }

        public void KreiranjeOsobe_KratkoImeDugoPrezime()
        {
            Osoba osoba = new Osoba("Ed", "Mujićnekićgromdrvostablosjekirapticarugalica", "Negdje daleko bb", "11.11.1991", "8899J9988", 1111991253321);
        }
        #endregion

    }
}
