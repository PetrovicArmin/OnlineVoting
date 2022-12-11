using OnlineVoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVotingTests
{
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
