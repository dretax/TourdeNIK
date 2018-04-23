using System;

namespace TourdeNIK
{
    public class Versenyzo : IVersenyzo
    {
        private int _id;
        private double _folyadekigeny;
        private int _terheles;
        private int _maxteher;
        
        public Versenyzo(string nev, string nem, string lakhely)
        {
            Nev = nev;
            Nem = nem;
            Lakhely = lakhely;
            GenerateID();
            _folyadekigeny = Program.Randomizer.NextDouble() * (2.5 - 0.5) + 0.5; // Egy próba két érték közötti double randomra.
            _maxteher = Program.Randomizer.Next(10, 20); // Mennyi legyen a versenyző maximális óra szám teherbírása.
            Versenyek = new RegularChainedList<Verseny>();
            Program.NParticipates += NemVersenyzikTobbet;
        }

        public int UniqueID // Ez lesz a láncolt listában a kulcs
        {
            get { return _id; }
        }
        
        public RegularChainedList<Verseny> Versenyek { get; set; }
        public string Nev { get; set; }
        public string RovidNev { get; set; }
        public string VersenyzoAzonosito { get; set; }
        public string Lakhely { get; set; }
        public string Nem { get; set; }

        private void GenerateID()
        {
            _id = Program.Randomizer.Next(100, 1000);
            VersenyzoAzonosito = Nem + "-" + _id + Program.RandomString(2);
        }
        
        private void NemVersenyzikTobbet(Versenyzo v)
        {
            //todo:
        }
        
        public int Fogyasztas(int ora)
        {
            return (int) Math.Ceiling(_folyadekigeny * ora);
        }

        public void Terheles(int ora)
        {
            _terheles += ora;
        }

        public double TeherBiras()
        {
            return _terheles / _maxteher;
        }

        public bool TerhelHetoMeg()
        {
            return TeherBiras() < 0.95;
        }
    }
}