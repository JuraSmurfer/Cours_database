using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
    public class Psi
    {
        /// <summary>
        /// Pole pro ulozeni objektu tridy Psi
        /// </summary>
        private ArrayList psi;

        /// <summary>
        /// Konstruktor tridy Psi
        /// </summary>
        public Psi()
        {
            this.psi = new ArrayList();
        }

        /// <summary>
        /// Metoda vraci nove nejvyssi ID, ktere se v DB jeste nevyskytuje.
        /// </summary>
        /// <returns></returns>
        public Int32 GetNewId()
        {
            Int32 highestID = 0;

            foreach(Pes pes in psi)
            {
                if (pes.id > highestID)
                    highestID = pes.id;
            }

            return ++highestID;
        }

        /// <summary>
        /// Vraci psa na zaklade vstupniho ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Pes GetPesById(int id)
        {
            foreach (Pes pes in psi)
            {
                if (pes.id == id)
                    return pes;
            }

            return null;
        }

        /// <summary>
        /// Vraci psa na zaklade startovniho cisla.
        /// </summary>
        /// <param name="startNo"></param>
        /// <returns></returns>
        public Pes GetPesByStartNo(int startNo)
        {
            foreach (Pes pes in psi)
            {
                if (pes.start_beh1 == startNo)
                    return pes;
            }
            return null;
        }

        /// <summary>
        /// Vraci psa na zaklade startovniho cisla v druhém kole.
        /// </summary>
        /// <param name="startNo"></param>
        /// <returns></returns>
        public Pes GetPesByStartNo_1(int startNo)
        {
            foreach (Pes pes in psi)
            {
                if (pes.start_beh2 == startNo)
                    return pes;
            }
            return null;
        }

        /// <summary>
        /// Vraci psa na zaklade cisla běhu a barvy v prvním kole.
        /// </summary>
        public Pes GetPesByDvojice(int dvoj, String barva)
        {
            foreach (Pes pes in psi)
            {
                if ((pes.dvojice0 == dvoj) && (pes.barva0 == barva))
                    return pes;
            }
            return null;
        }

        /// <summary>
        /// Vraci psa na zaklade cisla běhu a barvy v druhém kole.
        /// </summary>
        public Pes GetPesByDvojice_1(int dvoj, String barva)
        {
            foreach (Pes pes in psi)
            {
                if ((pes.dvojice1 == dvoj) && (pes.barva1 == barva))
                    return pes;
            }
            return null;
        }

        /// <summary>
        /// Vraci seznam psů se stejnou skupinou groupNo a pohlaví. Vstupní parametr pes_fena může obsahovat "pes", "fena" nebo NULL.
        /// </summary>
        /// <param name="groupNo"></param>
        /// <param name="pes_fena"></param>
        /// <returns></returns>
        public List<Pes> GetPesByGroup(int groupNo, string pes_fena)
        {
            List<Pes> apes = new List<Pes>();
            foreach (Pes pes in psi)
            {
                if ((Int32.Parse(pes.poznamka) == groupNo) && ((pes.pohlavi == pes_fena) || pes_fena == null))
                    apes.Add(pes);
            }
            return apes;
        }

        /// <summary>
        /// Vraci seznam psů se stejnou skupinou groupNo.
        /// </summary>
        /// <param name="groupNo"></param>
        /// <returns></returns>
        public List<Pes> GetPesByGroup_1(int groupNo)
        {
            List<Pes> apes = new List<Pes>();
            foreach (Pes pes in psi)
            {
                if ((Int32.Parse(pes.poznamka) == groupNo) && (pes.pohlavi == "pes"))
                    apes.Add(pes);
            }
            return apes;
        }

        /// <summary>
        /// Vraci seznam fen se stejnou skupinou groupNo.
        /// </summary>
        /// <param name="groupNo"></param>
        /// <returns></returns>
        public List<Pes> GetFenaByGroup_1(int groupNo)
        {
            List<Pes> apes = new List<Pes>();
            foreach (Pes pes in psi)
            {
                if ((Int32.Parse(pes.poznamka) == groupNo) && (pes.pohlavi == "fena"))
                    apes.Add(new Pes(pes));
            }
            return apes;
        }

        /// <summary>
        /// Vraci vsechny skupiny psu vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllGroups()
        {
            List<int> groups = new List<int>();
            Int32 temp_group = 0;

            foreach (Pes pes in psi)
            {
                temp_group = Int32.Parse(pes.poznamka);
                if (!groups.Contains(temp_group)) // pokud ještě neobsahuje čislo temp_group tak přidej
                    groups.Add(temp_group);
            }

            return groups;
        }

        /// <summary>
        /// Vraci vsechny ID psu vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public int[] GetAllIds()
        {
            int[] ids = new int[this.psi.Count];
            Int32 i = 0;

            foreach (Pes pes in psi)
            {
                ids[i] = pes.id;
                i++;
            }

            return ids;
        }

        /// <summary>
        /// Vraci vsechny dvojice psu vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllDvojice()
        {
            List<int> ids = new List<int>();

            foreach (Pes pes in psi)
            {
                if (!(ids.Contains(pes.dvojice0)))
                {
                    ids.Add(pes.dvojice0);
                }
            }
            ids.Sort();
            return ids;
        }

        /// <summary>
        /// Vraci vsechny dvojice psu vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllDvojice_1()
        {
            List<int> ids = new List<int>();

            foreach (Pes pes in psi)
            {
                if (!(ids.Contains(pes.dvojice1)))
                {
                    ids.Add(pes.dvojice1);
                }
            }
            ids.Sort();
            return ids;
        }

        /// <summary>
        /// Vraci vsechnyy Startovni cisla psu v DB
        /// </summary>
        /// <returns></returns>
        public int[] GetAllStartNos()
        {
            int[] ids = new int[this.psi.Count];
            Int32 i = 0;

            foreach (Pes pes in psi)
            {
                ids[i] = pes.start_beh1;
                i++;
            }

            return ids;
        }

        /// <summary>
        /// Vraci vsechny psy podle startovního čísla vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllStartList()
        {
            List<int> ids = new List<int>();

            foreach (Pes pes in psi)
            {
                if (!(ids.Contains(pes.start_beh1)))
                {
                    ids.Add(pes.start_beh1);
                }
            }
            ids.Sort();
            return ids;
        }

        /// <summary>
        /// Vraci vsechny psy podle startovního čísla pro druhý běh vyskytujici se v DB
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllStartList_1()
        {
            List<int> ids = new List<int>();

            foreach (Pes pes in psi)
            {
                if (!(ids.Contains(pes.start_beh2)))
                {
                    ids.Add(pes.start_beh2);
                }
            }
            ids.Sort();
            return ids;
        }

        /// <summary>
        /// Vrací počet psů v databázi
        /// </summary>
        public Int32 Length()
        {
            return psi.Count;
        }

        /// <summary>
        /// Prida zakaznika do databaze
        /// </summary>
        /// <param name="newPes"></param>
        public void Add(Pes newPes)
        {
            this.psi.Add(newPes);
        }

        /// <summary>
        /// Ziska data o psovi s danym ID, upravi je a zpet ulozi.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newData"></param>
        public void Edit(int id, Pes newData)
        {
            Pes oldData = this.GetPesById(id);

            oldData.jmeno = newData.jmeno;
            oldData.plemeno = newData.plemeno;
            oldData.pohlavi = newData.pohlavi;
            oldData.fci = newData.fci;
            oldData.datum = newData.datum;
            oldData.licence = newData.licence;
            oldData.majitel = newData.majitel;
            oldData.skupina = newData.skupina;
            oldData.start_beh1 = newData.start_beh1;
            oldData.start_beh2 = newData.start_beh2;
            oldData.zavod_licence = newData.zavod_licence;
            oldData.platba = newData.platba;
            oldData.doplatit = newData.doplatit;
            oldData.poznamka = newData.poznamka;
            oldData.diskval = newData.diskval;

            oldData.agility_A0 = newData.agility_A0;
            oldData.speed_A0 = newData.speed_A0;
            oldData.endurance_A0 = newData.endurance_A0;
            oldData.enthusiasm_A0 = newData.enthusiasm_A0;
            oldData.intelligence_A0 = newData.intelligence_A0;
            oldData.agility_A1 = newData.agility_A1;
            oldData.speed_A1 = newData.speed_A1;
            oldData.endurance_A1 = newData.endurance_A1;
            oldData.enthusiasm_A1 = newData.enthusiasm_A1;
            oldData.intelligence_A1 = newData.intelligence_A1;

            oldData.body1 = newData.body1;

            oldData.agility_B0 = newData.agility_B0;
            oldData.speed_B0 = newData.speed_B0;
            oldData.endurance_B0 = newData.endurance_B0;
            oldData.enthusiasm_B0 = newData.enthusiasm_B0;
            oldData.intelligence_B0 = newData.intelligence_B0;
            oldData.agility_B1 = newData.agility_B1;
            oldData.speed_B1 = newData.speed_B1;
            oldData.endurance_B1 = newData.endurance_B1;
            oldData.enthusiasm_B1 = newData.enthusiasm_B1;
            oldData.intelligence_B1 = newData.intelligence_B1;

            oldData.body2 = newData.body2;

            oldData.barva0 = newData.barva0;
            oldData.barva1 = newData.barva1;

            oldData.dvojice0 = newData.dvojice0;
            oldData.dvojice1 = newData.dvojice1;
        }

        /// <summary>
        /// Smaze psa dle daneho ID na vstupu.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            
            Int32 i = 0;

            foreach (Pes pes in psi)
            {
                if (pes.id == id)
                    break;
                i++;
            }

            this.psi.RemoveAt(i);
            
        }
    }
}
