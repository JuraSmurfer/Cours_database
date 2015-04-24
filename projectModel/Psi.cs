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

			foreach (Pes pes in psi)
			{
				if (pes.Id > highestID)
					highestID = pes.Id;
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
				if (pes.Id == id)
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
				if (pes.StartBeh1 == startNo)
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
				if (pes.StartBeh2 == startNo)
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
				if ((pes.Dvojice0 == dvoj) && (pes.Barva0 == barva))
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
				if ((pes.Dvojice1 == dvoj) && (pes.Barva1 == barva))
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
				if ((Int32.Parse(pes.Poznamka) == groupNo) && ((pes.Pohlavi == pes_fena) || pes_fena == null))
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
				if ((Int32.Parse(pes.Poznamka) == groupNo) && (pes.Pohlavi == "pes"))
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
				if ((Int32.Parse(pes.Poznamka) == groupNo) && (pes.Pohlavi == "fena"))
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
				temp_group = Int32.Parse(pes.Poznamka);
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
				ids[i] = pes.Id;
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
				if (!(ids.Contains(pes.Dvojice0)))
				{
					ids.Add(pes.Dvojice0);
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
				if (!(ids.Contains(pes.Dvojice1)))
				{
					ids.Add(pes.Dvojice1);
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
				ids[i] = pes.StartBeh1;
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
				if (!(ids.Contains(pes.StartBeh1)))
				{
					ids.Add(pes.StartBeh1);
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
				if (!(ids.Contains(pes.StartBeh2)))
				{
					ids.Add(pes.StartBeh2);
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

			oldData.Jmeno = newData.Jmeno;
			oldData.Plemeno = newData.Plemeno;
			oldData.Pohlavi = newData.Pohlavi;
			oldData.Fci = newData.Fci;
			oldData.Datum = newData.Datum;
			oldData.Licence = newData.Licence;
			oldData.Majitel = newData.Majitel;
			oldData.Skupina = newData.Skupina;
			oldData.StartBeh1 = newData.StartBeh1;
			oldData.StartBeh2 = newData.StartBeh2;
			oldData.ZavodLicence = newData.ZavodLicence;
			oldData.Platba = newData.Platba;
			oldData.Doplatit = newData.Doplatit;
			oldData.Poznamka = newData.Poznamka;
			oldData.Diskval = newData.Diskval;

			oldData.AgilityA0 = newData.AgilityA0;
			oldData.SpeedA0 = newData.SpeedA0;
			oldData.EnduranceA0 = newData.EnduranceA0;
			oldData.EnthusiasmA0 = newData.EnthusiasmA0;
			oldData.IntelligenceA0 = newData.IntelligenceA0;
			oldData.AgilityA1 = newData.AgilityA1;
			oldData.SpeedA1 = newData.SpeedA1;
			oldData.EnduranceA1 = newData.EnduranceA1;
			oldData.EnthusiasmA1 = newData.EnthusiasmA1;
			oldData.IntelligenceA1 = newData.IntelligenceA1;

			oldData.Body1 = newData.Body1;

			oldData.AgilityB0 = newData.AgilityB0;
			oldData.SpeedB0 = newData.SpeedB0;
			oldData.EnduranceB0 = newData.EnduranceB0;
			oldData.EnthusiasmB0 = newData.EnthusiasmB0;
			oldData.IntelligenceB0 = newData.IntelligenceB0;
			oldData.AgilityB1 = newData.AgilityB1;
			oldData.SpeedB1 = newData.SpeedB1;
			oldData.EnduranceB1 = newData.EnduranceB1;
			oldData.EnthusiasmB1 = newData.EnthusiasmB1;
			oldData.IntelligenceB1 = newData.IntelligenceB1;

			oldData.Body2 = newData.Body2;

			oldData.Barva0 = newData.Barva0;
			oldData.Barva1 = newData.Barva1;

			oldData.Dvojice0 = newData.Dvojice0;
			oldData.Dvojice1 = newData.Dvojice1;
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
				if (pes.Id == id)
					break;
				i++;
			}

			this.psi.RemoveAt(i);

		}
	}
}
