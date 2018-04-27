using System;

namespace TourdeNIK
{
    /// <summary>
    /// Egy versenyzőt reprezentáló osztály.
    /// </summary>
    public class Versenyzo : IVersenyzo
    {
        private int _id;
        private double _folyadekigeny;
        private double _terheles;
        
        public Versenyzo(string nev, string nem, string lakhely)
        {
            Nev = nev;
            Nem = nem;
            Lakhely = lakhely;
            GenerateID();
            _folyadekigeny = Program.Randomizer.NextDouble() * (2.5 - 1) + 1; // Egy próba két érték közötti double randomra.
            Versenyek = new RegularChainedList<Verseny>();
            Program.NParticipates += NemVersenyzikTobbet;
        }

        /// <summary>
        /// Egyszerűvé tettem, hogy bármikor kilehessen olvasni csak a számot az azonosítóból.
        /// </summary>
        public int UniqueID
        {
            get { return _id; }
        }
        
        public RegularChainedList<Verseny> Versenyek { get; set; }
        public string Nev { get; set; }
        public string VersenyzoAzonosito { get; set; }
        public string Lakhely { get; set; }
        public string Nem { get; set; }

        /// <summary>
        /// Legenerál egy random azonosítót.
        /// </summary>
        private void GenerateID()
        {
            _id = Program.Randomizer.Next(100, 1000);
            VersenyzoAzonosito = Nem + "-" + _id + Program.RandomString(2);
        }
        
        /// <summary>
        /// Ezt a delegált fogja meghívni amennyiben törlésre kerül a versenyző.
        /// </summary>
        /// <param name="v"></param>
        private void NemVersenyzikTobbet(Versenyzo v)
        {
            if (v == this)
            {
                Console.WriteLine(v.Nev + " " + v.VersenyzoAzonosito + " nem versenyzik többet!");
            }
        }
        
        /// <summary>
        /// Megadja a versenyző folyadék szükségletét óra alapján.
        /// </summary>
        /// <param name="ora"></param>
        /// <returns></returns>
        public int Fogyasztas(int ora)
        {
            return (int) Math.Ceiling(_folyadekigeny * ora);
        }

        /// <summary>
        /// Leterheli a versenyzőt X órával, amelyet %-ban értünk.
        /// </summary>
        /// <param name="ora"></param>
        public void Terheles(int ora)
        {
            // ez mondjuk 5 óra esetén, 0.05-s terhelés lenne! tehát 30 óra versenyenként, az 0.3-as terhelés.
            _terheles += (double) ora / 100;
        }

        /// <summary>
        /// Visszaadja a versenyző teherbírását.
        /// </summary>
        /// <returns></returns>
        public double TeherBiras()
        {
            return _terheles;
        }

        /// <summary>
        /// Megadja, hogy a versenyző terhelhető-e.
        /// </summary>
        /// <returns></returns>
        public bool TerhelHetoMeg()
        {
            return TeherBiras() < 0.95;
        }
    }
}