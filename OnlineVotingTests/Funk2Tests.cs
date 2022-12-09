using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVotingTests
{
    //funkcionalnost obuhvata metode: (dakle dovoljno postići 100% pokrivenost navedenih metoda!)
    //Kandidat.DetaljneInformacije
    //Kandidat.DajTrenutnuStranku (u koju je kandidat učlanjen)
    //Kandidat.UclaniUStranku
    //Kandidat.OdjaviIzStranke
    [TestClass]
    public class Funk2Tests
    {
        public static Kandidat? kandidatUStranci;
        public static Kandidat? kandidatBezStranke;

        //mora biti prije svakog testa, jer u testovima modifikujemo ove objekte!
        [TestInitialize]
        public void InicijalizacijaKandidata()
        {
            kandidatUStranci = new Kandidat("kandidat1", "sa strankom", "adresa 1", "10/02/2002", "999A999", 1002002195843);
            kandidatUStranci.UclaniUStranku("stranka 1", DateTime.Now);
            kandidatUStranci.OdjaviIzStranke("stranka 1", DateTime.Now.AddDays(10));
            kandidatUStranci.UclaniUStranku("stranka 2", DateTime.Now.AddDays(15));
            kandidatUStranci.OdjaviIzStranke("stranka 2", DateTime.Now.AddDays(20));
            kandidatUStranci.UclaniUStranku("stranka 3", DateTime.Now.AddDays(22));

            kandidatBezStranke = new Kandidat("kandidat2", "bez stranke", "adresa 2", "10/02/2003", "982B133", 1243005195843);
            kandidatBezStranke.UclaniUStranku("stranka 1", DateTime.Now);
            kandidatBezStranke.OdjaviIzStranke("stranka 1", DateTime.Now.AddDays(4));
            kandidatBezStranke.UclaniUStranku("stranka 2", DateTime.Now.AddDays(6));
            kandidatBezStranke.OdjaviIzStranke("stranka 2", DateTime.Now.AddDays(8));
        }

        //inicijalizacija podataka za inline testiranje
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

        //testovi za UclaniUStranku
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

        //testovi za OdjaviIzStranke

    }
}
