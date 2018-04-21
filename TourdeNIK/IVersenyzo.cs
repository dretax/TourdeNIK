namespace TourdeNIK
{
    public interface IVersenyzo
    {
        string Nev { get; set; }
        string VersenyzoAzonosito { get; set; }
        int Fogyasztas(int ora);
        int Terheles(int ora);
        int TeherBiras();
        bool TerhelHetoMeg();
        string Versenyek { get; set; } // todo
    }
}