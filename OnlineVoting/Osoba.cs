using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineVoting
{
    public abstract class Osoba
    {
        private String imeIPrezime { get; set; }
        private String adresa { get; set; }
        private DateTime datumRodjenja { get; set; }
        private String brojLicneKarte { get; set; }
        private long maticniBroj { get; set; }
        private String JIK { get; set; }

    }
}
