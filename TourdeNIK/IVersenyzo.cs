namespace TourdeNIK
{
    public interface IVersenyzo
    {
        string Nev { get; set; }
        string VersenyzoAzonosito { get; set; }
        int Fogyasztas(int ora);
        int Terheles(int ora);
        double TeherBiras();
        bool TerhelHetoMeg();
        RegularChainedList<Verseny> Versenyek { get; set; }
    }
}