using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace TourdeNIK
{
    /// <summary>
    /// Egy láncolt listát reprezentál de kifejezetten csak a Versenyzo osztályra.
    /// 
    /// </summary>
    public class VersenyBrigad
    {
        internal class ListElement
        {
            public int Key;
            public Versenyzo ElementValue;
            public ListElement NextElement;
          
            public ListElement(TourdeNIK.Versenyzo val)
            {
                this.ElementValue = val;
                this.Key = val.UniqueID;
            }

            public bool LastElement
            {
                get;
                set;
            }
        }

        private ListElement _FirstElement;
        private int _ListCount;

        internal ListElement FirstElement
        {
            get { return _FirstElement; }
        }

        public int Count
        {
            get { return _ListCount; }
        }

        public void AddInSortedWay(Versenyzo element)
        {
            ListElement uj = new ListElement(element);

            ListElement p = _FirstElement;
            
            // Ha a mostani elem az első a listában, akkor ez lesz az "utolsó" elem is egyben.
            if (p == null)
            {
                uj.LastElement = true;
            }

            ListElement e = null;
            while (p != null && p.Key < uj.Key)
            {
                e = p;
                // Ha az jelenlegi elemünk kisebb mint a behelyezett elemünk, akkor már nem az lesz az utolsó elem.
                if (e.LastElement)
                {
                    e.LastElement = false;
                    uj.LastElement = true;
                    break;
                }
                //Console.WriteLine("E értéke: " + e.Key);
                //Console.WriteLine("element értéke: " + element.VersenyzoAzonositoSzam);
                p = p.NextElement;
                /*if (p != null)
                {
                    Console.WriteLine("P értéke: " + p.Key);
                }*/
            }

            if (e == null)
            {
                uj.NextElement = _FirstElement;
                _FirstElement = uj;
            }
            else
            {
                uj.NextElement = p;

                e.NextElement = uj;
            }

            if (uj.LastElement) // Ha az újonnan behelyezett érték az utolsó elem akkor adjuk meg neki az első elemet mint következő érték.
            {
                //Console.WriteLine("Last element értéke: " + uj.Key);
                uj.NextElement = _FirstElement;
                //Console.WriteLine("F: " + uj.NextElement.Key);
            }
        }

        /*public void Add(TourdeNIK.Versenyzo element)
        {
            // Létrehozunk egy új lista elementet
            ListElement uj = new ListElement(element);
            //Console.WriteLine("Hozzáadott érték: " + uj.ElementValue);
            
            // Ha a mostani elem az első a listában, akkor ez lesz az "utolsó" elem is egyben.
            if (_FirstElement == null)
            {
                //Console.WriteLine("Első elem nulla, úgyhogy ez az utolsó is egyben: " + uj.ElementValue);
                //Console.WriteLine();
                uj.LastElement = true;
            }
            else
            {
                // Fussunk végig a láncolt listánkon, vegyük a mostani első elemünket.
                ListElement aktualis = _FirstElement;
                while (aktualis != null && !aktualis.LastElement)
                {
                    //Console.WriteLine("Mostani: " + aktualis.ElementValue);
                    aktualis = aktualis.NextElement; // Addig keresgélünk még nem az utolsó elemnél vagyunk.
                    //Console.WriteLine("Következő: " + aktualis.ElementValue);
                    //Console.WriteLine();
                }

                if (aktualis != null)
                {
                    //Console.WriteLine("Utolsó elem: " + aktualis.ElementValue);
                    aktualis.NextElement = uj; // utolsó elemből elérhető a legelső,
                    //Console.WriteLine("Elérhető első elem: " + aktualis.NextElement.ElementValue);
                }
            }
            
            // Az újonnan létrehozott elemnek megadjuk az előtte lévő elemet, de ez null ha az első element tesszük a listába
            uj.NextElement = _FirstElement;
            //Console.WriteLine(uj.LastElement + " e");
            
            // Az első elem az újonnan létrehozott lesz.
            _FirstElement = uj;
            _ListCount++;
        }*/

        internal ListElement Find(Versenyzo element)
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null && !aktualis.ElementValue.Equals(element) && !aktualis.LastElement)
            {
                aktualis = aktualis.NextElement;
            }
            return aktualis;
        }

        internal ListElement Find(int azonosito)
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null && aktualis.Key != azonosito && !aktualis.LastElement)
            {
                aktualis = aktualis.NextElement;
            }
            return aktualis != null ? aktualis : null;
        }
        
        internal ListElement Find(string azonosito)
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null && aktualis.ElementValue.VersenyzoAzonosito != azonosito && !aktualis.LastElement)
            {
                aktualis = aktualis.NextElement;
            }
            return aktualis != null ? aktualis : null;
        }
        
        public void Print()
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null)
            {
                Console.WriteLine("Adat: " + aktualis.ElementValue + " Key: " + aktualis.Key + " Last: " + aktualis.LastElement);
                if (aktualis.LastElement) break;
                aktualis = aktualis.NextElement;
            }
        }

        public void Remove(Versenyzo element)
        {
            ListElement currentElement = _FirstElement; // Vesszük az első elemet
            ListElement lastElement = null;
            
            // Addig megyünk amíg a jelenlegi elemünk nem null, és az érték nem egyezik a megadottal, illetve nem az utolsó elemnél tartunk.
            while (currentElement != null && !currentElement.ElementValue.Equals(element) && !currentElement.LastElement)
            {
                lastElement = currentElement; // Az "utolsó" elemünk lesz a jelenlegi elem. Ez akar lenni a keresett elem előtti elem.
                currentElement = currentElement.NextElement; // Vesszük a következő elemet
            }
            if (currentElement != null)
            {
                if (lastElement != null) // Ha van utolsó értékünk
                {
                    //Console.WriteLine("Megvan az elem: " + currentElement.ElementValue);
                    //Console.WriteLine("Az előtte lévő elem értéke: " + lastElement.ElementValue);
                    // Ha az utolsó elemet töröltük tegyük meg az előtte lévőt utolsónak.
                    if (currentElement.LastElement)
                    {
                        lastElement.LastElement = true;
                    }
                    lastElement.NextElement = currentElement.NextElement; // Ha az utolsó elemet töröltük akkor a következő az első lesz, ha nem akkor pedig a törölt elem utáni.
                    //Console.WriteLine("last element nem null, és az utáni értéke: " + lastElement.NextElement.ElementValue);
                }
                else
                {
                    _FirstElement = currentElement.NextElement; // Ez null lesz ha a listánk abszolút üres lesz.
                    /*if (_FirstElement == null)
                    {
                        Console.WriteLine("Null, üres");
                    }
                    else
                    {
                        Console.WriteLine("First element: " + _FirstElement.ElementValue + " kövi: " +
                                          currentElement.NextElement.ElementValue);
                    }*/
                }
                _ListCount--;
            }
        }
    }
}