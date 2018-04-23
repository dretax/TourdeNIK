namespace TourdeNIK
{
    public class Verseny
    {
        private string _megnevezes;
        private int _ora;

        public Verseny(string type, int ora)
        {
            _megnevezes = type;
            _ora = ora;
        }

        public string Megnevezes
        {
            get { return _megnevezes; }
        }

        public int IdoTartam
        {
            get { return _ora; }
        }
    }
}