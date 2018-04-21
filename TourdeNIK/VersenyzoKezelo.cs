using System;
using System.Dynamic;

namespace TourdeNIK
{
    public class VersenyzoKezelo
    {
        /// <summary>
        /// Ez az osztály kifejezetten csak VersenyBrigádokat tárol.
        /// Kulcs értéke a Brigádon belüli első elemnek az azonosítója.
        /// </summary>
        private class ListElement
        {
            public int Key;
            public VersenyBrigad ElementValue;
            public ListElement NextElement;
          
            public ListElement(VersenyBrigad val)
            {
                this.ElementValue = val;
                if (val.FirstElement != null)
                {
                    this.Key = val.FirstElement.Key;
                }
            }
        }
        
        private VersenyzoKezelo.ListElement _FirstElement;
        private int VersenyBrigadCount;

        public static VersenyzoKezelo Instance
        {
            get;
            set;
        }

        public static void InitializeKezelo()
        {
            Instance = new VersenyzoKezelo();
        }

        public void VersenyBrigadAdd(VersenyBrigad element)
        {
            // Létrehozunk egy új lista elementet
            VersenyzoKezelo.ListElement uj = new VersenyzoKezelo.ListElement(element);

            uj.NextElement = _FirstElement;
            
            _FirstElement = uj;
            VersenyBrigadCount++;
        }
        
        public void VersenyBrigadDelete(VersenyBrigad element)
        {
            VersenyzoKezelo.ListElement currentElement = _FirstElement; // Vesszük az első elemet
            VersenyzoKezelo.ListElement lastElement = null;
            
            // Addig megyünk amíg a jelenlegi elemünk nem null, és az érték nem egyezik a megadottal.
            while (currentElement != null && !currentElement.ElementValue.Equals(element))
            {
                lastElement = currentElement; // Az "utolsó" elemünk lesz a jelenlegi elem. Ez akar lenni a keresett elem előtti elem.
                currentElement = currentElement.NextElement; // Vesszük a következő elemet
            }
            if (currentElement != null)
            {
                if (lastElement != null) // Ha van utolsó értékünk
                {
                    lastElement.NextElement = currentElement.NextElement; 
                }
                else
                {
                    _FirstElement = currentElement.NextElement; // Ez null lesz ha a listánk abszolút üres lesz.
                }

                VersenyBrigadCount--;
            }
        }

        public void VersenyzoHozzaAdd(VersenyBrigad b, Versenyzo v)
        {
            
        }
        
        public void Print()
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null)
            {
                Console.WriteLine("Adat: " + aktualis.ElementValue + " Key: " + aktualis.Key);
                aktualis = aktualis.NextElement;
            }
        }
        
        public void VersenyBrigadSort()
        {
            if (VersenyBrigadCount <= 1)
            {
                return;
            }
            VersenyzoKezelo.ListElement head = _FirstElement;
            VersenyzoKezelo.ListElement _current = _FirstElement;
            VersenyzoKezelo.ListElement _previous = _current;
            VersenyzoKezelo.ListElement _min = _current;
            VersenyzoKezelo.ListElement _minPrevious = _min;
            VersenyzoKezelo.ListElement _sortedListHead = null;
            VersenyzoKezelo.ListElement _sortedListTail = _sortedListHead;
            for (int i = 0; i < VersenyBrigadCount; i++)
            {
                _current = head;
                _min = _current;
                _minPrevious = _min;
                //Find min Node
                while (_current != null)
                {
                    if (_current.Key < _min.Key)
                    {
                        _min = _current;
                        _minPrevious = _previous;
                    }
                    _previous = _current;
                    _current = _current.NextElement;
                }
                // Remove min Node 
                if (_min == head)
                {
                    head = head.NextElement;
                }
                else if (_min.NextElement == null) //if tail is min node
                {
                    _minPrevious.NextElement = null;
                }
                else
                {
                    _minPrevious.NextElement = _minPrevious.NextElement.NextElement;
                }
                //Attach min Node to the new sorted linked list
                if (_sortedListHead != null)
                {
                    _sortedListTail.NextElement = _min;
                    _sortedListTail = _sortedListTail.NextElement;
                }
                else
                {
                    _sortedListHead = _min;
                    _sortedListTail = _sortedListHead;
                }
            }

            _FirstElement = _sortedListHead;
        }
    }
}