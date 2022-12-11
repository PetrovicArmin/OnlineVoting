using CsvHelper;
using OnlineVoting;
using System.Globalization;

namespace OnlineVotingTests
{
        [TestClass]
        public class Funk5Tests
        /*
        Funkcionalnost 5 razvio Faruk Sahat (18839)
        Implementirana funkcionalnost metodama koje su oznacene komentarima u klasama Populacija i Izbori, te konacni rezultat u mainu koji necemo testirati (Prema odgovorima na pitanja za zadacu 2 - Dio u mainu ne treba biti pokriven testovima)
        */

        {
        private List<Kandidat>? Kandidati;
        private Stranka? stranka;
        private Populacija pop;
        private Osoba? osoba1 = new Osoba("Neko", "Nekic", "Negdje", "21.02.2001", "123J123", 2102001123456);
        private Osoba? osoba2 = new Osoba("Drugi", "Neko", "Drugdje", "22.01.2001", "321J321", 2201001123456);
        private Izbori izbori = Izbori.DajIzbore();

        [TestInitialize]
        public void InicijalizacijaDostupnihGlasova()
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
            pop = Populacija.DajPopulaciju();
            List<String> glasaci = new List<string> { "glasacA", "glasacB" };   
            pop.setGlasaci(glasaci);
            List<Glas> glasovi = new List<Glas> { new Glas(1, new List<Kandidat> { Kandidati.ElementAt(1) }), new Glas(1, new List<Kandidat> { Kandidati.ElementAt(1) }) };
            pop.setGlasovi(glasovi);
        }

        #region Testiranje funkcionalnosti - metoda ProvjeriSifru

        [TestMethod]
        public void ProvjeriSifru_Netacno()
        {
            Assert.IsFalse(izbori.ProvjeraSifre(""));
            Assert.IsFalse(izbori.ProvjeraSifre("bilokakavstring"));
        }

        [TestMethod]
        public void ProvjeriSifru_Tacno()
        {
            Assert.IsTrue(izbori.ProvjeraSifre("VVS20222023"));
        }

        #endregion

        #region Testiranje funkcionalnosti - metoda PonistiGlas

        [TestMethod]
        public void PonistiGlas_Postoji()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void PonistiGlas_NePostoji()
        {
            izbori.PonistiGlas(osoba2, new Glas(1, new List<Kandidat> { Kandidati.ElementAt(1) }));
        }
    #endregion

    #region Testiranje funkcionalnosti - metoda DodajGlasaca
    [TestMethod]
    public void DodajGlasaca()
    {
        Assert.IsTrue(pop.getGlasaci().Count==2);
        Assert.IsTrue(pop.getGlasovi().Count==2);
        pop.DodajGlasaca("bilokakavJiK", new Glas(1, new List<Kandidat> { Kandidati.ElementAt(1) }));
        Assert.IsTrue(pop.getGlasaci().Count == 3);
        Assert.IsTrue(pop.getGlasovi().Count == 3);
            List<Glas> votes = pop.getGlasovi();
            List<string> voters = pop.getGlasaci();
            Assert.IsTrue(voters.ElementAt(2) == "bilokakavJiK");
            Assert.IsTrue(votes.ElementAt(2).VratiIDStranke() == 1);
        }

    #endregion

    #region Testiranje funkcionalnosti - metoda DajGlas
    [TestMethod]
    public void DajGlas_Ima()
    {
            Assert.AreEqual(pop.DajGlas("glasacA").VratiIDStranke(), 1);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void DajGlas_Nema()
    {
            pop.DajGlas("bilokakavglasac");
    }
    #endregion

    #region Testiranje funkcionalnosti - metoda UkloniGlasaca
    [TestMethod]
    public void UkloniGlasaca_Ima()
    {
            Assert.AreEqual(pop.getGlasaci().Count, 2);
            Assert.AreEqual(pop.getGlasovi().Count, 2);
            pop.UkloniGlasaca("glasacA");
            Assert.AreEqual(pop.getGlasaci().Count, 1);
            Assert.AreEqual(pop.getGlasovi().Count, 1);
        }

    [TestMethod]
    public void UkloniGlasaca_Nema()
    {
            Assert.AreEqual(pop.getGlasaci().Count, 2);
            Assert.AreEqual(pop.getGlasovi().Count, 2);
            pop.UkloniGlasaca("nepostojeci");
            Assert.AreEqual(pop.getGlasaci().Count, 2);
            Assert.AreEqual(pop.getGlasovi().Count, 2);

    }
        #endregion

        #region Inline testiranje
        static IEnumerable<object[]> Sifre_pogresne
        {
            get
            {
                return new[]
                {
                    new object[] {" "},
                    new object[] {"nebitan string"},
                    new object[] {"vvs20222023"},
                    new object[] {"Vvs20222023"},
                    new object[] {"VVS.20222023"}
                };
            }
        }

        [TestMethod]
        [DynamicData("Sifre_pogresne")]
        public void ProvjeraSifre_Pogresna(string pokusaj)
        {
            Assert.IsFalse(izbori.ProvjeraSifre(pokusaj));
        }


        #endregion

        #region CSV testiranje
        static IEnumerable<object[]> NepostojeciGlasaci
        {
            get
            {
                return LoadCSV();
            }
        }

        [TestMethod]
        [DynamicData("NepostojeciGlasaci")] //nemam neku veliku potrebu da testiram razlicite vrste podataka jer su mi specificni exceptioni, ali ovdje cu testirati system exception da pokazem znanje iz csva
        [ExpectedException(typeof(Exception))]
        public void Test(string ime, string prezime, string adresa, string rodjendan, string licna, long matbr, int stranka, int pozicija)
        {
            Osoba glasac = new Osoba(ime, prezime, adresa, rodjendan, licna, matbr);
            izbori.PonistiGlas(glasac, new Glas(stranka, new List<Kandidat> { Kandidati.ElementAt(pozicija) }));
        }
        
        static IEnumerable<object[]> LoadCSV()
        {
            using (var reader = new StreamReader("Funk5glasaci.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], elements[1],
                        elements[2], elements[3], elements[4], long.Parse(elements[5]), int.Parse(elements[6]), int.Parse(elements[7])};
                }
            }
        }

        #endregion

    }
}