using System;

namespace TourdeNIK
{
    public class Versenyzo : IVersenyzo
    {
        private int _id;
        

        public Versenyzo(string nev)
        {
            Nev = nev;
            GenerateID();
            Versenyek = new RegularChainedList<Verseny>();
            Program.NParticipates += NemVersenyzikTobbet;
        }

        private void NemVersenyzikTobbet(Versenyzo v)
        {
            //todo:
        }

        private void GenerateID()
        {
            _id = Program.Randomizer.Next(100, 1000);
            string s = "F";
            if (Program.Randomizer.Next(0, 2) == 1)
            {
                s = "N";
            }
            VersenyzoAzonosito = s + "-" + _id + Program.RandomString(2);
        }
        
        public int UniqueID // Ez lesz a láncolt listában a kulcs
        {
            get { return _id; }
        }
        
        public RegularChainedList<Verseny> Versenyek { get; set; }
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

        public double TeherBiras()
        {
            throw new System.NotImplementedException();
        }

        public bool TerhelHetoMeg()
        {
            throw new System.NotImplementedException();
        }
    }
}