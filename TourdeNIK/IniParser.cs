using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TourdeNIK
{
    internal class IniParser
    {
        /// <summary>
        /// Ini útvonala
        /// </summary>
        private string iniFilePath;
        
        /// <summary>
        /// Dictionary amely tárolja a section-t és key-t kulcsként illetve az értéket.
        /// </summary>
        private readonly Dictionary<SectionPair, string> keyPairs = new Dictionary<SectionPair, string>();
        
        /// <summary>
        /// Ini beolvasása szekcióktól, majd kulcsoktól és értékektől
        /// </summary>
        /// <param name="iniPath"></param>
        internal IniParser(string iniPath)
        {
            string str2 = null;
            this.iniFilePath = iniPath;

            if (!File.Exists(iniPath)) throw new FileNotFoundException("Nem található a file! " + iniPath);

            try
            {
                using (TextReader reader = new StreamReader(iniPath))
                {
                    for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
                    {
                        str = str.Trim();
                        if (string.IsNullOrEmpty(str)) continue;

                        if (str.StartsWith("[") && str.EndsWith("]"))
                            str2 = str.Substring(1, str.Length - 2); // [] között lévő section kivétele
                        else
                        {
                            SectionPair pair;


                            string[] strArray = str.Split(new char[] { '=' }, 2);
                            string str3 = null;
                            if (str2 == null)
                            {
                                str2 = "ROOT";
                            }
                            pair.Section = str2;
                            pair.Key = strArray[0];
                            
                            if (strArray.Length > 1) // Ha van értékünk is.
                            {
                                str3 = strArray[1];
                            }
                            this.keyPairs.Add(pair, str3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Hiba az ini olvasása közben! " + ex);
            }
        }
        
        /// <summary>
        /// Szekciók megszámlálása
        /// </summary>
        /// <returns></returns>
        internal int Count()
        {
            return this.Sections.Length;
        }

        /// <summary>
        /// Szekció kulcsainak lekérdezése
        /// </summary>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        internal string[] EnumSection(string sectionName)
        {
            List<string> list = new List<string>();
            foreach (SectionPair pair in this.keyPairs.Keys)
            {
                if (pair.Key.StartsWith(";"))
                    continue;

                if (pair.Section == sectionName)
                {
                    list.Add(pair.Key);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Szekciók
        /// </summary>
        internal string[] Sections
        {
            get
            {
                List<string> list = new List<string>();
                foreach (SectionPair pair in this.keyPairs.Keys)
                {
                    if (!list.Contains(pair.Section))
                    {
                        list.Add(pair.Section);
                    }
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// Adat lekérdezése
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        internal string GetSetting(string sectionName, string settingName)
        {
            SectionPair pair;
            pair.Section = sectionName;
            pair.Key = settingName;
            return (string)this.keyPairs[pair];
        }

        /// <summary>
        /// A tárolt hashtableben struktúrát alkalmazok az egyszerűbb tárolás érdekében
        /// </summary>
        private struct SectionPair
        {
            public string Section;
            public string Key;
        }
    }
}