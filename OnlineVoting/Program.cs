using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineVoting
{
    internal class Program
    {
        // Ove informacije nemaju veze s vezom

        private static List<Osoba> osobe = new List<Osoba>
        {
            new Osoba("Faruk", "Šahat", "Negdje Sarajevu", "21.01.2001", "333M333", 2101001126211),
            new Osoba("Naida", "Pita", "Isto Sarajevo", "12.02.2001", "341T312", 1202001156234),
            new Osoba("Velid", "Imširović", "Aleja Bosne Srebrene bb", "31.08.2001", "111T111", 3108001154123),
            new Osoba("Dženana", "Terzić", "Zmaja od Bosne bb", "14.04.2001", "222J222", 1404001156234),
            new Osoba("Armin", "Petrović", "Aleja Bosne Srebrene bb", "25.05.2001", "871K261", 2505001156234),
            new Osoba("Mujo", "Mujić", "Hendek bb", "16.06.2001", "123E123", 1606001156234),
        }; // Nek ima nekih random osoba, samo da se ima

        private static List<Kandidat> kandidatiA = new List<Kandidat>
        {
            new Kandidat("Mujo", "Mujić", "Hendek bb", "12.12.1992", "678M612", 1212992222645),
            new Kandidat("Haso", "Hasić", "Hendek bb", "12.12.1992", "323T451", 1212992252341),
            new Kandidat("Josip", "Josipović", "Adresa", "24.11.1989", "654T545", 2411989541235)
        };

        private static List<Kandidat> kandidatiB = new List<Kandidat>
        {
            new Kandidat("Stipo", "Stipić", "Hendek bb", "12.12.1992", "332T331", 1212992010001),
            new Kandidat("Suljo", "Suljić", "Hendek bb", "12.12.1992", "112T112", 1212992010211),
            new Kandidat("Suada", "Suadić", "Adresa", "14.11.1989", "115J551", 1411989578945)
        };

        private static List<Kandidat> nezavisni = new List<Kandidat>
        {
            new Kandidat("Džo", "Bajden", "Amerika", "11.11.1959", "889K882", 1111959541232),
            new Kandidat("Sin", "Džin-Pin", "Kina", "10.10.1234", "112K336", 1010234541235),
            new Kandidat("Pedro", "Sančez", "Španija", "25.06.1961", "889T998", 2506961452324)
        };

        private static Stranka strankaA = new Stranka(kandidatiA, 1);
        private static Stranka strankaB = new Stranka(kandidatiB, 2);
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Izbori izbori = Izbori.DajIzbore();
            Izbori.stranke = new List<Stranka> { strankaA, strankaB };
            Izbori.kandidati = new List<Kandidat>(kandidatiA.Concat(kandidatiB).Concat(nezavisni));
            Populacija pop = Populacija.DajPopulaciju();
            int opcija;
            do
            {
                Console.WriteLine("0. Izlaz (Šalim se -1, al nek bude greška jer sam i ja greškom)");
                Console.WriteLine("1. Unos glasača");
                Console.WriteLine("2. Prikaz glasača");
                Console.WriteLine("3. Prikaz stranki i kandidata");
                Console.WriteLine("4. Glasaj");
                Console.WriteLine("5. Ispis trenutnih stanja");
                Console.Write("Unesite opciju: ");
                opcija = Int32.Parse(Console.ReadLine());
                switch (opcija)
                {
                    case 1:
                        osobe.Add(unesiGlasaca());
                        break;
                    case 2:
                        Console.WriteLine("Glasači");
                        osobe.ForEach(delegate (Osoba o)
                        {
                            Console.WriteLine(o.dajJIK());
                        });
                        break;
                    case 3:
                        Console.WriteLine("Opcija 3");
                        break;
                    case 4:
                        Console.WriteLine("JIK: ");
                        string jik = Console.ReadLine();

                        Console.WriteLine("1. Nezavisni");
                        Console.WriteLine("2. Stranka i(ili) kandidati");
                        int nacin = Int32.Parse(Console.ReadLine());
                        Glas g;
                        if (nacin == 1)
                        {
                            nezavisni.ForEach(delegate (Kandidat k)
                            {
                                Console.WriteLine(k.OsnovneInformacije());
                            });

                            int izborKandidata = Int32.Parse(Console.ReadLine());
                            g = new Glas(0, new List<Kandidat> { nezavisni.ElementAt(izborKandidata) });
                        }
                        else
                        {
                            ispisStranaka();
                            Console.Write("Odaberite stranku: ");
                            int s = Int32.Parse(Console.ReadLine());
                            var stranka = Izbori.stranke.Where(str => str.VratiIdStranke() == s).Single();

                            Console.Write("Odaberite kandidate razdvojene (,): ");
                            string odabrani = Console.ReadLine();
                            var kandidati = odabrani.Split(',')?.Select(Int32.Parse)?.ToList();
                            //Console.WriteLine("Broj: " + kandidati.Count() + " - " + stranka.vratiClanove().Count());
                            List<Kandidat> sviKandidati = stranka.VratiClanove();
                            List<Kandidat> listaKandidata = new List<Kandidat>();
                            for (int i = 0; i < kandidati.Count(); i++)
                            {
                                listaKandidata.Add(stranka.VratiClanove()[kandidati[i]]);
                            }
                            g = new Glas(stranka.VratiIdStranke(), listaKandidata);
                        }
                        try
                        {
                            izbori.ProcesirajGlas(prekoJik(jik), g);
                        }catch (Exception e)
                        {
                            Console.WriteLine("Greška: " +  e.Message);
                        }
                        var glasaci = pop.getGlasaci();
                        glasaci.Add(jik);
                        pop.setGlasaci(glasaci);
                        break;
                    case 5:
                        Console.WriteLine(izbori.TrenutnoStanje());
                        break;
                }
            } while (opcija != -1);
        }

        private static Osoba unesiGlasaca()
        {
            string ime, prezime, dob, adresa, blk;
            long maticniBroj;
            Console.Write("Ime: ");
            ime = Console.ReadLine();
            Console.Write("Prezime: ");
            prezime = Console.ReadLine();
            Console.Write("Adresa: ");
            adresa = Console.ReadLine();
            Console.Write("Datum rođenja: ");
            dob = Console.ReadLine();
            Console.Write("Broj lične karte: ");
            blk = Console.ReadLine();
            Console.Write("Matični broj: ");
            maticniBroj = long.Parse(Console.ReadLine());
            return new Osoba(ime, prezime, adresa, dob, blk, maticniBroj);
        }

        private static Osoba prekoJik(string jik)
        {
            return osobe.Where(o => o.dajJIK() == jik).Single();
        }

        private static void ispisStranaka()
        {
            Izbori.stranke.ForEach(stranka =>
            {
                Console.WriteLine("ID: " + stranka.VratiIdStranke());
                stranka.VratiClanove().ForEach(clan => Console.WriteLine("---" + clan.OsnovneInformacije()));
            });
        }
    }

}