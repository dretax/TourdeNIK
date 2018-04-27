using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;

namespace TourdeNIK
{
    internal class Program
    {
        public static Random Randomizer;
        
        public static event NoLongerParticipates NParticipates;
        public delegate void NoLongerParticipates(Versenyzo v);
        
        public static void Main(string[] args)
        {
            Randomizer = new Random();
            
            if (!File.Exists("Brigadok.ini"))
            {
                throw new FileNotFoundException("Brigádok Ini file hiányzik!");
            }
            
            if (!File.Exists("Versenyek.ini"))
            {
                throw new FileNotFoundException("Versenyek Ini file hiányzik!");
            }
            
            // Kezelő Statikus Inicializálója
            VersenyzoKezelo.InitializeKezelo();
            VersenyzoKezelo handler = VersenyzoKezelo.Instance;
            
            // Versenyek beolvasása
            IniParser versenyek = new IniParser("Versenyek.ini");
            List<Verseny> AvailableRacesNORML = new List<Verseny>();
            RegularChainedList<Verseny> AvailableRaces = new RegularChainedList<Verseny>();
            try
            {
                foreach (var x in versenyek.EnumSection("Versenyek"))
                {
                    AvailableRaces.Add(new Verseny(x));
                    
                    AvailableRacesNORML.Add(new Verseny(x));
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Hiba a versenyek file olvasásakor! " + ex);
            }
            
            
            // Versenyzők és brigádjaik/adataik beolvasása.
            IniParser ini = new IniParser("Brigadok.ini");
            try
            {
                //Versenyzo vteszt = null;
                //VersenyBrigad bteszt = null;
                foreach (var x in ini.Sections) // Végig megyünk az összes section-n [Section1]
                {
                    VersenyBrigad brigad = new VersenyBrigad(x); // Létrehozzuk a brigádot, de még nem adjuk hozzá a listához.
                    foreach (var y in ini.EnumSection(x))
                    {
                        var data = ini.GetSetting(x, y).Split('~');
                        Versenyzo v = new Versenyzo(y, data[1], data[0]); // Létrehozzuk a versenyzőt az adatokkal, majd hozzáadjuk a brigádhoz rendezett beilesztéssel
                        handler.VersenyzoHozzaAdd(brigad, v);
                        foreach (var z in data[2].Split(','))
                        {
                            foreach (var verseny in AvailableRacesNORML)
                            {
                                if (verseny.Megnevezes == z)
                                {
                                    v.Versenyek.Add(verseny);
                                }
                            }
                        }

                        //vteszt = brigad.FirstElement.ElementValue;
                        //bteszt = brigad;
                        //Console.WriteLine("V: " + v.UniqueID);
                    }

                    handler.VersenyBrigadAdd(brigad); // A brigádot hozzáadjuk a listához.

                    //Console.WriteLine(x + " : " + brigad.FirstElement.Key);
                }
                
                //handler.Print();
                //Console.WriteLine("print megvót");
                //handler.VersenyzoTorolFromBrigad(bteszt, vteszt);
                
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Hiba a brigádok file olvasásakor! " + ex);
            }
            //handler.Print();
            //handler.VersenyBrigadSort();
            //handler.Print();

            Help();
            while (true)
            {
                try
                {
                    string s = Console.ReadLine();
                    int i;
                    bool b = int.TryParse(s, out i);
                    while (!b)
                    {
                        Console.WriteLine("Hibás Opció! Listázom a lehetőségeket.");
                        Help();
                        s = Console.ReadLine();
                        b = int.TryParse(s, out i);
                    }

                    switch (i)
                    {
                        case 1:
                            handler.VersenyBrigadSort();
                            Console.WriteLine("Brigádok rendezve!");
                            break;
                        case 2:
                            Console.WriteLine("Listázás.");
                            var brigadelement = handler.FirstElement;
                            while (brigadelement != null)
                            {
                                Console.WriteLine("Brigád: " + brigadelement.ElementValue.Name + " -- első kulcsa: " + brigadelement.Key);
                                var versenyzoelement = brigadelement.ElementValue.FirstElement;
                                while (versenyzoelement != null)
                                {
                                    var versenyzo = versenyzoelement.ElementValue;
                                    Console.WriteLine(versenyzo.Nev + " " + versenyzo.VersenyzoAzonosito + " " + versenyzo.Lakhely);

                                    if (versenyzoelement.LastElement) break;
                                    versenyzoelement = versenyzoelement.NextElement;
                                }

                                Console.WriteLine();
                                brigadelement = brigadelement.NextElement;
                            }
                            break;
                        case 3:
                            Console.WriteLine("Megpróbálkozunk a versenyző versenyszámainak optimalizálásával.");
                            Console.WriteLine();

                            foreach (var x in AvailableRacesNORML)
                            {
                                Console.Write(x.Megnevezes + "\t");
                            }

                            Console.WriteLine();
                            try
                            {
                                handler.VersenyBrigadBeosztasKiir(AvailableRaces);
                            }
                            catch
                            {
                                //todo
                            }
                            Console.WriteLine();
                            break;
                    }
                    Console.WriteLine("Folytatáshoz nyomj meg egy billenytű gombot.");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    
                }
            }

            //Console.ReadKey();
        }
        
        private static void Help()
        {
            Console.WriteLine();
            Console.WriteLine("Válassz egy opciót (Üss be egy számot)");
            Console.WriteLine("1. Brigádok rendezése");
            Console.WriteLine("2. Összes versenyző kiíratása brigádonként sorrendben.");
            Console.WriteLine("3. Melyik versenyző melyik versenyen vett részt");
            Console.WriteLine("4. Versenyző törlése");
            Console.WriteLine("5. Brigád törlése");
        }
        
        internal static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                int random = Randomizer.Next(0, chars.Length);
                stringChars[i] = chars[random];
            }
            return new string(stringChars);
        }
    }
}