using System;

namespace TourdeNIK
{
    internal class Program
    {
        public static Random Randomizer;
        
        public static void Main(string[] args)
        {
            Randomizer = new Random();

            Console.ReadKey();
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