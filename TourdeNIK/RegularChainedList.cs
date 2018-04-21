﻿using System;

namespace TourdeNIK
{
    public class RegularChainedList<T>
    {
        internal class ListElement
        {
            public int Key;
            public T ElementValue;
            public ListElement NextElement;
          
            public ListElement(int key, T val)
            {
                this.ElementValue = val;
            }

            public bool LastElement
            {
                get;
                set;
            }
        }

        private ListElement _FirstElement;

        public void Add(int key, T element)
        {
            // Létrehozunk egy új lista elementet
            ListElement uj = new ListElement(key, element);
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
            
            // Az első elem az újonnan létrehozott lesz.
            _FirstElement = uj;
        }

        public bool Find(T element)
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null && !aktualis.ElementValue.Equals(element) && !aktualis.LastElement)
            {
                aktualis = aktualis.NextElement;
            }
            return aktualis != null;
        }
        
        public void Print()
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null)
            {
                Console.WriteLine("Adat: " + aktualis.ElementValue);
                if (aktualis.LastElement) break;
                aktualis = aktualis.NextElement;
            }
        }

        public void Remove(T element)
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
            }
        }
    }
}