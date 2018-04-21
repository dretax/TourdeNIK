using System;

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
            
            VersenyBrigad asd = new VersenyBrigad();

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