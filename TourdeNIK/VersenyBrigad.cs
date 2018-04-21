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
        public class ListElement
        {
            public int Key;
            public Versenyzo ElementValue;
            public ListElement NextElement;
          
            public ListElement(TourdeNIK.Versenyzo val)
            {
                this.ElementValue = val;
                this.Key = val.VersenyzoAzonositoSzam;
            }

            public bool LastElement
            {
                get;
                set;
            }
        }

        private ListElement _FirstElement;
        private int _ListCount;

        public ListElement FirstElement
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

        public ListElement Find(Versenyzo element)
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null && !aktualis.ElementValue.Equals(element) && !aktualis.LastElement)
            {
                aktualis = aktualis.NextElement;
            }
            return aktualis;
        }

        public ListElement Find(int azonosito)
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null && aktualis.Key != azonosito && !aktualis.LastElement)
            {
                aktualis = aktualis.NextElement;
            }
            return aktualis != null ? aktualis : null;
        }
        
        public ListElement Find(string azonosito)
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
        
        /// <summary>
        /// Megkeressük a legkissebb elemet, és azt tesszük meg az első elemnek (fej), és ezt N elemszámig nézzük.
        /// Lényegében egy kiválasztásos rendezés az egész.
        /// </summary>
        /*public void SortLinkedList()
        {
            if (Count <= 1) // Egy, vagy 0 elemet azért ne akarjunk már rendezni.
            {
                return;
            }
            ListElement head = _FirstElement;
            ListElement _current = head;
            ListElement _previous = _current;
            ListElement _min = _current;
            ListElement _minPrevious = _min;
            ListElement _sortedListHead = null;
            ListElement _sortedListTail = _sortedListHead;
            for (int i = 0; i < Count; i++)
            {
                _current = head;
                _min = _current;
                _minPrevious = _min;
                //Megkeressük a legkisebb elemet.
                while (_current != null)
                {
                    if (_current.Key < _min.Key)
                    {
                        _min = _current;
                        _minPrevious = _previous;
                    }

                    // Ha az utolsó elemet értük el, állítsuk le a ciklust, hogy ne menjünk végtelenbe
                    // illetve, hogy azt az elemet is a megfelelő helyre tegyük.
                    if (_current.LastElement)
                    {
                        _current.NextElement = null;
                        _current.LastElement = false;
                        break;
                    }
                    _previous = _current;
                    _current = _current.NextElement;
                }
                // Töröljük a minimális elemet.
                if (_min == head)
                {
                    head = head.NextElement;
                }
                else if (_min.NextElement == null) //Ha esetlegesen a listánk vége nem lenne összekötve, akkor azt így offolnánk ki.
                {
                    Console.WriteLine("ab");
                    _minPrevious.NextElement = null;
                }
                else
                {
                    _minPrevious.NextElement = _minPrevious.NextElement.NextElement;
                }
                
                if (_sortedListHead != null)
                {
                    _sortedListTail.NextElement = _min;
                    _sortedListTail = _sortedListTail.NextElement;
                    if (i == Count - 1)
                    {
                        //Console.WriteLine("Utolsó elemünk: " + _sortedListTail.Key);
                        _sortedListTail.LastElement = true;
                    }
                }
                else
                {
                    _sortedListHead = _min;
                    //Console.WriteLine("Első, és egyben a legkisebb elem: " + _min.Key);
                    _sortedListTail = _sortedListHead;
                }
            }

            _FirstElement = _sortedListHead;
            if (_sortedListTail != null)
            {
                //Console.WriteLine("Utolsó elemünk: " + _sortedListTail.Key);
                _sortedListTail.NextElement = _FirstElement;
                //Console.WriteLine("Első elemünk: " + _sortedListTail.NextElement.Key);
            }
        }*/
    }
}