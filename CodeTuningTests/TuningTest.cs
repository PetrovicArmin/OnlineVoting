using OnlineVoting;

namespace CodeTuningTests
{
    [TestClass]
    public class TuningTest
    {
        [TestMethod]
        public void TestTuning()
        {


            int x = 0;

            for (int i = 0; i < 5500000; i++)
            {
                new Osoba("Ime", "Prezime", "Adresa 14", "29.12.2000", "999E999", 2912000144123);
                new Osoba("Ime", "Prezime", "Adresa 14", "29.12.2000", "999E999", 2912000144123);
            }

            int y = 0;

            Assert.IsTrue(true);
        }

    }
}