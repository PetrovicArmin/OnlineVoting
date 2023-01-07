using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVotingTests
{
    
    [TestClass]
    public class Zadatak2Tests
    {
        public static List<Osoba> osobe = new List<Osoba>();
        public static FakeProvjera fakeProvjera = new FakeProvjera();

        #region Inicijalizacija podataka
        [ClassInitialize]
        public static void InicijalizacijaGlasaca(TestContext context)
        {
            osobe = new List<Osoba>
            {
                new Osoba("Faruk", "Šahat", "Negdje Sarajevu", "11.01.2001", "123E667", 1101001126211),
                new Osoba("Naida", "Pita", "Isto Sarajevo", "12.02.2001", "123J667", 1202001156234),
                new Osoba("Velid", "Imširović", "Aleja Bosne Srebrene bb", "31.08.2001", "123K667", 3108001123456),
                new Osoba("Dženana", "Terzić", "Zmaja od Bosne bb", "14.04.2001", "123M667", 1404001156234),
                new Osoba("Armin", "Petrović", "Aleja Bosne Srebrene bb", "15.05.2001", "123T667", 1505001156234),
                new Osoba("Mujo", "Mujić", "Hendek bb", "16.06.2001", "123T167", 1606001156234),
            };

            //prve tri osobe ćemo staviti da su glasale već:
            fakeProvjera.PopuniJIKove(new List<String> { osobe[0].dajJIK(), osobe[1].dajJIK(), osobe[2].dajJIK() });
        }

        #endregion

        #region Testovi vjerodostojnosti glasača pomoću zamjenskog objekta
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Vjerodostojnost_VecGlasao_BacanjeIzuzetka()
        {
            osobe[2].VjerodostojnostGlasaca(fakeProvjera);
        }

        [TestMethod]
        public void Vjerodostojnost_NijeGlasao_VracaTrue()
        {
            Assert.IsTrue(osobe[3].VjerodostojnostGlasaca(fakeProvjera));
        }

        #endregion
    }

    #region Klasa zamjenskog objekta
    //Zamjenski objekat u svrhu TDD-a: Fake, jer ima kompleksni atribut koji simulira bazu podataka onih koji su glasali
    public class FakeProvjera : IProvjera
    {
        List<String> JIKoviOnihKojiSuGlasali = new List<String>();

        public FakeProvjera()
        {
        }

        public void PopuniJIKove(List<string> jiks)
        {
            JIKoviOnihKojiSuGlasali = jiks;
        }

        public bool DaLiJeVecGlasao(string jik)
        {
            if (JIKoviOnihKojiSuGlasali.Contains(jik))
                return true;
            return false;
        }
    }

    #endregion
}