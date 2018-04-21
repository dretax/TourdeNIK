using System;

namespace TourdeNIK
{
    public class Versenyzo : IVersenyzo
    {
        private int _id;

        public Versenyzo(string nev)
        {
            Nev = nev;
            _id = Program.Randomizer.Next(100, 1000);
            VersenyzoAzonosito = Program.RandomString(1) + "-" + _id + Program.RandomString(2);
        }
        
        public int VersenyzoAzonositoSzam // Ez lesz a láncolt listában a kulcs
        {
            get { return _id; }
        }
        
        public string Versenyek { get; set; }
        public string Nev { get; set; }
        public string VersenyzoAzonosito { get; set; }
        
        public int Fogyasztas(int ora)
        {
            throw new System.NotImplementedException();
        }

        public int Terheles(int ora)
        {
            throw new System.NotImplementedException();
        }

        public int TeherBiras()
        {
            throw new System.NotImplementedException();
        }

        public bool TerhelHetoMeg()
        {
            throw new System.NotImplementedException();
        }
    }
}