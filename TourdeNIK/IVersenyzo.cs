namespace TourdeNIK
{
    internal interface IVersenyzo
    {
        string Nev { get; set; }
        string RovidNev { get; set; }
        string VersenyzoAzonosito { get; set; }
        string Lakhely { get; set; }
        string Nem { get; set; }
        int Fogyasztas(int ora);
        void Terheles(int ora);
        double TeherBiras();
        bool TerhelHetoMeg();
        RegularChainedList<Verseny> Versenyek { get; set; }
    }
}