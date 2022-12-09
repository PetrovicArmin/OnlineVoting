using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineVoting;
using System;

namespace OnlineVotingTests
{   
    // Uradila: Naida Pita
    [TestClass]
    public class Funk3Tests
    {
        static Izbori izbori;

        [ClassInitialize]
        public static void PocetnaInicijalizacija(TestContext context)
        {
            izbori = Izbori.DajIzbore();
            
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}