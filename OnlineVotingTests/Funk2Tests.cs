using CsvHelper;
using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineVotingTests
{
    //funkcionalnost obuhvata metode: (dakle dovoljno postići 100% pokrivenost navedenih metoda!)
    //Kandidat.DetaljneInformacije
    //Kandidat.DajTrenutnuStranku (u koju je kandidat učlanjen)
    //Kandidat.UclaniUStranku
    //Kandidat.OdjaviIzStranke
    //Kandidat.DajNajkasnijuOdjavu
    //klasu implementirao: Petrović Armin

    [TestClass]
    public class Funk2Tests
    {
        public static Kandidat? kandidatUStranci;
        public static Kandidat? kandidatBezStranke;
        public static DateTime najkasnijaOdjavaPrijavljenog = DateTime.Now.AddDays(20);
        public static DateTime najkasnijaOdjavaOdjavljenog = DateTime.Now.AddDays(8);

        #region inicijaizacijska metoda
        [TestInitialize]
        public void InicijalizacijaKandidata()
        {

            kandidatUStranci = new Kandidat("kandidatj", "sa-strankom", "adresa 1", "10.02.2002", "999J999", 1002002195843);
            kandidatUStranci.UclaniUStranku("stranka 1", DateTime.Now);
            kandidatUStranci.OdjaviIzStranke("stranka 1", DateTime.Now.AddDays(10));
            kandidatUStranci.UclaniUStranku("stranka 2", DateTime.Now.AddDays(15));
            kandidatUStranci.OdjaviIzStranke("stranka 2", najkasnijaOdjavaPrijavljenog);
            kandidatUStranci.UclaniUStranku("stranka 3", DateTime.Now.AddDays(22));

            kandidatBezStranke = new Kandidat("kandidatd", "bez-stranke", "adresa 2", "10.02.2003", "982K133", 1002003195843);
            kandidatBezStranke.UclaniUStranku("stranka 1", DateTime.Now);
            kandidatBezStranke.OdjaviIzStranke("stranka 1", DateTime.Now.AddDays(4));
            kandidatBezStranke.UclaniUStranku("stranka 2", DateTime.Now.AddDays(6));
            kandidatBezStranke.OdjaviIzStranke("stranka 2", najkasnijaOdjavaOdjavljenog);
        }
        #endregion

        #region inicijalizacija podataka za inline i csv testiranje
        static IEnumerable<object[]> ArgumentiZaUclanjenje
        {
            get
            {
                return new[]
                {
                    new object[] {"nova stranka", DateTime.Now.AddDays(-20), "Datum učlanjenja mora biti kasniji od najkasnijeg datuma odjave!"},
                    new object[] {"nova stranka", DateTime.Now.AddDays(50), "Nema izuzetka"}
                };
            }
        }

        static IEnumerable<object[]> ArgumentiOdjavaCSV
        {
            get
            {
                return UčitajPodatkeCSV();
            }
        }

        public static IEnumerable<object[]> UčitajPodatkeCSV()
        {
            using (var reader = new StreamReader("ArgumentiZaOdjavu.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], int.Parse(elements[1]), elements[2]};
                }
            }
        }
        #endregion

        #region testovi za UclaniUStranku
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Uclani_VecUclanjen_BacanjeIzuzetka()
        {
            kandidatUStranci?.UclaniUStranku("neka tamo stranka", DateTime.Now.AddMonths(3));
        }

        [TestMethod]
        [DynamicData("ArgumentiZaUclanjenje")]
        public void Uclani_NijeUclanjen_InlineTestiranje(String novaStranka, DateTime datumUclanjenja, String tekstIzuzetka)
        {
            try
            {
                kandidatBezStranke?.UclaniUStranku(novaStranka, datumUclanjenja);

                Assert.AreEqual(tekstIzuzetka, "Nema izuzetka");
                StringAssert.Contains(kandidatBezStranke?.DetaljneInformacije(), novaStranka);
                StringAssert.Contains(kandidatBezStranke?.DetaljneInformacije(), datumUclanjenja.ToString());
            } catch(ArgumentException argEx)
            {
                Assert.AreEqual(tekstIzuzetka, argEx.Message);
            }
        }
        #endregion

        #region Testovi za OdjaviIzStranke
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Odjavi_NijeUclanjen_BacanjeIzuzetka()
        {
            kandidatBezStranke?.OdjaviIzStranke("neka stranka", DateTime.Now.AddDays(400));
        }

        [TestMethod]
        [DynamicData("ArgumentiOdjavaCSV")]
        public void Odjavi_Uclanjen_CSVTestiranje(String imeStranke, int offsetUDanima, String tekstIzuzetka)
        {
            DateTime datum = DateTime.Now.AddDays(offsetUDanima);
            try
            {
                kandidatUStranci?.OdjaviIzStranke(imeStranke, datum);
                Assert.AreEqual(tekstIzuzetka, "Nema izuzetka");
                StringAssert.Contains(kandidatUStranci?.DetaljneInformacije(), datum.ToString());
            } catch(ArgumentException argEx)
            {
                StringAssert.Matches(argEx.Message, new Regex(tekstIzuzetka));
            }
        }

        #endregion

        #region testovi metode DetaljneInformacije
        [TestMethod]
        public void DetaljneInformacije_Uclanjen_SadrziDanDanas()
        {
            StringAssert.Contains(kandidatUStranci?.DetaljneInformacije(), "DAN DANAS");
        }

        [TestMethod]
        public void DetaljneInformacije_BezStranke_NemaDanDanas()
        {
            StringAssert.DoesNotMatch(kandidatBezStranke?.DetaljneInformacije(), new Regex("(?:^|\\W)DAN DANAS(?:$|\\W)"));
        }

        #endregion

        #region testovi metode DajNajkasnijuOdjavu
        [TestMethod]
        public void NajkasnijaOdjava_Uclanjen_VracaOdjavu()
        {
            Assert.AreEqual(kandidatUStranci.DajNajkasnijuOdjavu(), najkasnijaOdjavaPrijavljenog);
        }

        [TestMethod]
        public void NajkasnijaOdjava_Odjavljen_VracaOdjavu()
        {
            Assert.AreEqual(kandidatBezStranke.DajNajkasnijuOdjavu(), najkasnijaOdjavaOdjavljenog);
        }

        [TestMethod]
        public void NajkasnijaOdjava_NemaOdjava_VracaMinValue()
        {
            Assert.AreEqual(new Kandidat("kandidatj", "sa-strankom", "adresa 1", "10.02.2002", "999J999", 1002002195843).DajNajkasnijuOdjavu(), DateTime.MinValue);
        }
        #endregion

        #region testovi metode DajTrenutnuStranku
        [TestMethod]
        public void TrenutnaStranka_KandidatSaStrankom_VracaStranku()
        {
            Assert.AreEqual("stranka 3", kandidatUStranci?.DajTrenutnuStranku().Item1);
        }

        [TestMethod]
        public void TrenutnaStranka_KandidatBezStranke_VracaNull()
        {
            Assert.IsNull(kandidatBezStranke?.DajTrenutnuStranku());
        }

        #endregion
    }
}
