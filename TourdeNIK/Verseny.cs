namespace TourdeNIK
{
    public class Verseny
    {
        private string _megnevezes;

        public Verseny(string type)
        {
            _megnevezes = type;
        }

        public string Megnevezes
        {
            get { return _megnevezes; }
        }
    }
}