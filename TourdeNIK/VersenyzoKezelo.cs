using System;
using System.Collections.Generic;
using System.Dynamic;

namespace TourdeNIK
{
    public class VersenyzoKezelo
    {
        /// <summary>
        /// Ez az osztály kifejezetten csak VersenyBrigádokat tárol.
        /// Kulcs értéke a Brigádon belüli első elemnek az azonosítója.
        /// </summary>
        internal class ListElement
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
        private int _VersenyBrigadCount;

        /// <summary>
        /// Visszaadja a láncolt listában lévő első elemet.
        /// </summary>
        internal VersenyzoKezelo.ListElement FirstElement
        {
            get { return _FirstElement; }
        }
        
        /// <summary>
        /// Delegáltak.
        /// </summary>
        public static event VersenyBrigadDisbanded VDisbanded;
        public delegate void VersenyBrigadDisbanded(VersenyBrigad v);

        /// <summary>
        /// Konstruktor a feladat alapján nem elérhető, és belülről van létrehozva az instance.
        /// </summary>
        private VersenyzoKezelo()
        {
            VDisbanded += BrigadDisbanded;
        }
        

        /// <summary>
        /// Visszaadja az adott osztály instanceát ha az létezik.
        /// </summary>
        public static VersenyzoKezelo Instance
        {
            get;
            set;
        }

        /// <summary>
        /// Létrehoz egy instance-t ennek az osztálynak.
        /// </summary>
        public static void InitializeKezelo()
        {
            Instance = new VersenyzoKezelo();
        }

        private void BrigadDisbanded(VersenyBrigad v)
        {
            Console.WriteLine("Egy versenybrigád törlésre került! Neve: " + v.Name);
        }

        /// <summary>
        /// Hozzáadja a versenybrigádot a listához. NEM rendezett módon.
        /// </summary>
        /// <param name="element"></param>
        public void VersenyBrigadAdd(VersenyBrigad element)
        {
            // Létrehozunk egy új lista elementet
            VersenyzoKezelo.ListElement uj = new VersenyzoKezelo.ListElement(element);

            uj.NextElement = _FirstElement;
            
            _FirstElement = uj;
            _VersenyBrigadCount++;
        }
        
        /// <summary>
        /// Törli a specifikus versenybrigádot.
        /// </summary>
        /// <param name="element"></param>
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

                BrigadDisbanded(element);

                _VersenyBrigadCount--;
            }
        }
        
        /// <summary>
        /// Törli a specifikus versenybrigádot.
        /// </summary>
        /// <param name="element"></param>
        public bool VersenyBrigadDelete(string brigadname)
        {
            VersenyzoKezelo.ListElement currentElement = _FirstElement; // Vesszük az első elemet
            VersenyzoKezelo.ListElement lastElement = null;
            
            // Addig megyünk amíg a jelenlegi elemünk nem null, és az érték nem egyezik a megadottal.
            while (currentElement != null && currentElement.ElementValue.Name.ToLower() != brigadname.ToLower())
            {
                lastElement = currentElement; // Az "utolsó" elemünk lesz a jelenlegi elem. Ez akar lenni a keresett elem előtti elem.
                currentElement = currentElement.NextElement; // Vesszük a következő elemet
            }
            if (currentElement != null)
            {
                BrigadDisbanded(currentElement.ElementValue);
                if (lastElement != null) // Ha van utolsó értékünk
                {
                    lastElement.NextElement = currentElement.NextElement; 
                }
                else
                {
                    _FirstElement = currentElement.NextElement; // Ez null lesz ha a listánk abszolút üres lesz.
                }

                _VersenyBrigadCount--;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Csak is a delegate miatt lett létrehozva a feladat szerint.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal Versenyzo AlertRemoval(string id)
        {
            VersenyzoKezelo.ListElement currentElement = _FirstElement; // Vesszük az első elemet
            
            // Addig megyünk amíg a jelenlegi elemünk nem null, és az érték nem egyezik a megadottal.
            while (currentElement != null)
            {
                VersenyBrigad brigad = currentElement.ElementValue;
                VersenyBrigad.ListElement element = brigad.FirstElement;
                while (element != null)
                {
                    Versenyzo v2 = element.ElementValue;
                    if (v2.VersenyzoAzonosito == id)
                    {
                        return v2;
                    }
                    if (element.LastElement) break;
                    element = element.NextElement;
                }
                
                currentElement = currentElement.NextElement; // Vesszük a következő elemet
            }
            return null;
        }

        /// <summary>
        /// Hozzáadja a versenyzőt a specifikus brigádhoz.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="v"></param>
        public void VersenyzoHozzaAdd(VersenyBrigad b, Versenyzo v)
        {
            b.AddInSortedWay(v);
        }

        /// <summary>
        /// Törli a versenyzőt a specifikus brigádból.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="v"></param>
        public void VersenyzoTorolFromBrigad(VersenyBrigad b, Versenyzo v)
        {
            b.Remove(v);
            // Ha véletlenül az első elem kerülne törlésre frissítsük az kulcs értéket a legkisebbre.
            // Így megtartjuk, hogy a brigádok mindíg a legkisebb azonosítószámmal vannak feltüntetve.
            if (b.FirstElement != null)
            {
                this._FirstElement.Key = b.FirstElement.Key;
            }
        }

        /// <summary>
        /// Ezt a methodot úgy írtam meg, hogy az összes brigádot végig nézi egy adott versenyzőért, és ha megtalálja törli.
        /// Nem feltétlenül szükséges a használata, csak előre megcsináltam ha esetleg kellene.
        /// </summary>
        /// <param name="v"></param>
        public void VersenyzoTorol(Versenyzo v)
        {
            VersenyzoKezelo.ListElement currentElement = _FirstElement; // Vesszük az első elemet
            
            // Addig megyünk amíg a jelenlegi elemünk nem null, és az érték nem egyezik a megadottal.
            while (currentElement != null)
            {
                VersenyBrigad brigad = currentElement.ElementValue;
                VersenyBrigad.ListElement element = brigad.FirstElement;
                while (element != null)
                {
                    Versenyzo v2 = element.ElementValue;
                    if (v2.Equals(v))
                    {
                        brigad.Remove(v2);
                        if (brigad.FirstElement == null)
                        {
                            VersenyBrigadDelete(brigad);
                        }
                        else
                        {
                            // Ha véletlenül az első elem kerülne törlésre frissítsük az kulcs értéket a legkisebbre.
                            // Így megtartjuk, hogy a brigádok mindíg a legkisebb azonosítószámmal vannak feltüntetve.
                            if (brigad.FirstElement != null)
                            {
                                this._FirstElement.Key = brigad.FirstElement.Key;
                            }
                        }
                        break;
                    }
                    if (element.LastElement) break;
                    element = element.NextElement;
                }
                
                currentElement = currentElement.NextElement; // Vesszük a következő elemet
            }
        }
        
        /// <summary>
        /// Ezt a methodot úgy írtam meg, hogy az összes brigádot végig nézi egy adott versenyzőért, és ha megtalálja törli.
        /// Nem feltétlenül szükséges a használata, csak előre megcsináltam ha esetleg kellene.
        /// </summary>
        /// <param name="v"></param>
        public bool VersenyzoTorol(string id)
        {
            VersenyzoKezelo.ListElement currentElement = _FirstElement; // Vesszük az első elemet
            
            // Addig megyünk amíg a jelenlegi elemünk nem null, és az érték nem egyezik a megadottal.
            while (currentElement != null)
            {
                VersenyBrigad brigad = currentElement.ElementValue;
                VersenyBrigad.ListElement element = brigad.FirstElement;
                while (element != null)
                {
                    Versenyzo v2 = element.ElementValue;
                    if (v2.VersenyzoAzonosito == id)
                    {
                        brigad.Remove(v2);
                        if (brigad.FirstElement == null)
                        {
                            VersenyBrigadDelete(brigad);
                        }
                        else
                        {
                            // Ha véletlenül az első elem kerülne törlésre frissítsük az kulcs értéket a legkisebbre.
                            // Így megtartjuk, hogy a brigádok mindíg a legkisebb azonosítószámmal vannak feltüntetve.
                            if (brigad.FirstElement != null)
                            {
                                this._FirstElement.Key = brigad.FirstElement.Key;
                            }
                        }

                        return true;
                    }
                    if (element.LastElement) break;
                    element = element.NextElement;
                }
                
                currentElement = currentElement.NextElement; // Vesszük a következő elemet
            }

            return false;
        }
        
        /// <summary>
        /// Kiírja a listában lévő kulcsértékeket és azok értékeit.
        /// A kulcs a brigádban lévő versenyző UniqueID-je lesz (Az elsőé)
        /// </summary>
        public void Print()
        {
            ListElement aktualis = _FirstElement;
            while (aktualis != null)
            {
                Console.WriteLine("Adat: " + aktualis.ElementValue + " Key: " + aktualis.Key);
                aktualis = aktualis.NextElement;
            }
        }

        /// <summary>
        /// A brigádban lévő versenyzőknek hasonló mennyiségű versenyt oszt ki a leterheltségük alapján.
        /// </summary>
        /// <param name="b"></param>
        public void VersenyBrigadBeosztasKiir(RegularChainedList<Verseny> versenyek)
        {
            //verseny.Reverse();
            var brigadelement = _FirstElement;
            //Console.WriteLine("brigadelement " + brigadelement);
            while (brigadelement != null)
            {
                string[] val = brigadelement.ElementValue.BrigadBeosztas(versenyek);
                //Console.WriteLine("HAHÓ");
                //Console.WriteLine(string.Join(" ", val));
                for (int i = 0; i < val.Length; i++)
                {
                    Console.Write(val[i] + "\t");
                }

                Console.WriteLine();
                brigadelement = brigadelement.NextElement;
            }
        }
        
        /// <summary>
        /// Meghívásra képes rendezni a láncolt listát amennyiben több mint 1 elem van benne.
        /// </summary>
        public void VersenyBrigadSort()
        {
            if (_VersenyBrigadCount <= 1)
            {
                return;
            }
            // Fej és a jelenlegi elem az elejénél ugyanaz lesz, ahogy az előző elem is.
            VersenyzoKezelo.ListElement head = _FirstElement;
            VersenyzoKezelo.ListElement _current = _FirstElement;
            VersenyzoKezelo.ListElement _previous = _current;
            // A "legkisebb" elemmel is ugyanez a helyzet, és vesszük mellé az előző legkisebbet.
            VersenyzoKezelo.ListElement _min = _current;
            VersenyzoKezelo.ListElement _minPrevious = _min;
            
            // A Rendezett listánk első elemje még nem létezik, és a vége sem.
            VersenyzoKezelo.ListElement _sortedListHead = null;
            VersenyzoKezelo.ListElement _sortedListTail = _sortedListHead;
            // N darabszámig megyünk.
            for (int i = 0; i < _VersenyBrigadCount; i++)
            {
                _current = head;
                _min = _current;
                _minPrevious = _min;
                
                // Amíg a jelenlegi elemünk nem null
                while (_current != null)
                {
                    // Ha a jelenlegi kulcsa kisebb mint a minimumé
                    // Ez elősször hamis lesz, hisz mind a kettő az első elem lesz.
                    if (_current.Key < _min.Key)
                    {
                        _min = _current;
                        _minPrevious = _previous;
                    }
                    // Előző elemet megadjuk a jelenlegivel, majd továbbmegyünk (Végig), ha véletlenül a következő elemünk kisebb
                    // mint a jelenlegi ^, akkor új minimális elemünk lesz.
                    _previous = _current;
                    _current = _current.NextElement;
                }

                // Ha a jelenlegi fejünk megegyezik a minimális elemmel, akkor a "fej" a következő elem lesz.
                if (_min == head)
                {
                    //Console.WriteLine("A fej egyenlő, " + head.Key);
                    head = head.NextElement;
                }
                else if (_min.NextElement == null) // Ez akkor következne be, ha 1nél kevesebb elemünk lenne, ezt itt hagyom demonstrációnak.
                {
                    //Console.WriteLine("lolle, null");
                    _minPrevious.NextElement = null;
                }
                else
                {
                    // Minden más egyébben az előző minimális értékű elem következő elemjének megadjuk az az utáni elemet.
                    _minPrevious.NextElement = _minPrevious.NextElement.NextElement;
                }

                if (_sortedListHead != null)
                {
                    _sortedListTail.NextElement = _min;
                    _sortedListTail = _sortedListTail.NextElement;
                }
                else
                {
                    //Console.WriteLine("lolle: " + _min.Key);
                    _sortedListHead = _min;
                    _sortedListTail = _sortedListHead;
                }
            }

            // Az első elem az új "fej" lesz.
            _FirstElement = _sortedListHead;
        }
    }
}