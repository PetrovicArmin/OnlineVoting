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

        [ClassInitialize]
        public void InicijalizacijaGlasaca()
        {
            osobe = new List<Osoba>
            {
                new Osoba("Faruk", "Šahat", "Negdje Sarajevu", "01.01.2001", "12345667", 0101001126211),
                new Osoba("Naida", "Pita", "Isto Sarajevo", "02.02.2001", "12345667", 0202001156234),
                new Osoba("Velid", "Imširović", "Aleja Bosne Srebrene bb", "31.08.2001", "12345667", 0303001156234),
                new Osoba("Dženana", "Terzić", "Zmaja od Bosne bb", "04.04.2001", "12345667", 0404001156234),
                new Osoba("Armin", "Petrović", "Aleja Bosne Srebrene bb", "05.05.2001", "12345667", 0505001156234),
                new Osoba("Mujo", "Mujić", "Hendek bb", "06.06.2001", "12345667", 0606001156234),
            };

            //prve tri osobe ćemo staviti da su glasale već:
            fakeProvjera.PopuniJIKove(new List<String> { osobe[0].dajJIK(), osobe[1].dajJIK(), osobe[2].dajJIK() });
        }

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
    }

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
}