using System;
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
            
            /*VersenyBrigad asd = new VersenyBrigad();

            for (int i = 0; i < 10; i++)
            {
                asd.AddInSortedWay(new Versenyzo(RandomString(10)));
            }

            Console.WriteLine("1. brigád érték: " + asd.FirstElement.Key);
            
            VersenyBrigad asd2 = new VersenyBrigad();

            for (int i = 0; i < 10; i++)
            {
                asd2.AddInSortedWay(new Versenyzo(RandomString(10)));
            }

            Console.WriteLine("2. brigád érték: " + asd2.FirstElement.Key);
            
            VersenyBrigad asd3 = new VersenyBrigad();

            for (int i = 0; i < 10; i++)
            {
                asd3.AddInSortedWay(new Versenyzo(RandomString(10)));
            }

            Console.WriteLine("3. brigád érték: " + asd3.FirstElement.Key);
            
            VersenyzoKezelo.InitializeKezelo();
            VersenyzoKezelo handler = VersenyzoKezelo.Instance;
            
            handler.VersenyBrigadAdd(asd);
            handler.VersenyBrigadAdd(asd2);
            handler.VersenyBrigadAdd(asd3);
            
            handler.VersenyBrigadSort();
            
            handler.Print();*/
            if (!File.Exists("Brigadok.ini"))
            {
                throw new FileNotFoundException("Brigádok Ini file hiányzik!");
            }
            
            if (!File.Exists("Versenyek.ini"))
            {
                throw new FileNotFoundException("Versenyek Ini file hiányzik!");
            }
            
            VersenyzoKezelo.InitializeKezelo();
            VersenyzoKezelo handler = VersenyzoKezelo.Instance;
            
            IniParser versenyek = new IniParser("Versenyek.ini");
            RegularChainedList<Verseny> AvailableRaces = new RegularChainedList<Verseny>();
            try
            {
                foreach (var x in versenyek.EnumSection("Versenyek"))
                {
                    int time = 1;
                    int.TryParse(versenyek.GetSetting("Versenyek", x), out time);
                    AvailableRaces.Add(new Verseny(x, time));
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Hiba a versenyek file olvasásakor! " + ex);
            }
            
            
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
                            v.Versenyek.Add(new Verseny(z, 1));
                        }

                        //vteszt = brigad.FirstElement.ElementValue;
                        //bteszt = brigad;
                        //Console.WriteLine("V: " + v.UniqueID);
                    }

                    handler.VersenyBrigadAdd(brigad); // A brigádot hozzáadjuk a listához.

                    brigad.BrigadBeosztas(AvailableRaces);
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
            handler.VersenyBrigadSort();
            handler.Print();


            //Console.ReadKey();
        }
        
        internal static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                int random = Randomizer.Next(0, chars.Length);
                stringChars[i] = chars[random];
            }
            return new string(stringChars);
        }
    }
}