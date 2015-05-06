using System;
using System.Collections.Generic;
using System.Linq;

namespace projectModel
{
    /// <summary>
    /// Třída pes.
    /// </summary>
    public class Pes : Utils
    {
	    public Dictionary<string, string> GetDictionary { get { return _variables; } }
	    private Dictionary<string, string> _variables = new Dictionary<string, string>(); /// takto nebudou promenne zabirat tolik mista v programu
		private void _setVar(string name, string value) { _variables[name] = value; }
        public Pes()
        {
	        InitVar();
        }

	    public Pes(String[] data)
        {
			if (data.Count() < 43) /// kontroluj pocet prvku v poli !!
				throw new ArgumentException("Nedostatečný počet prvků pole");
			
			InitVar();
		    int i = 0;
		    foreach (var name in Enum.GetNames(typeof(EPes)))
		    {
			    _setVar(name, data[i]);
			    i++;
		    }
        }

        public Pes(Pes aPes)
		{
			InitVar(aPes.GetDictionary);
        }
		private void InitVar()
		{
			foreach (String name in Enum.GetNames(typeof(EPes)))
			{
				_variables.Add(name, "");
			}
			Diskval = "---";
		}
		private void InitVar(Dictionary<string, string> dictionary)
		{
			foreach (String key in dictionary.Keys)
			{
				_variables.Add(key, dictionary[key]);
			}
		}

		#region Get/Set method
		
		public int Id { get { return GetInt(_variables[GetString(EPes.Id)]); } set { _setVar(GetString(EPes.Id), GetString(value)); } }
		public String Jmeno { get { return _variables[GetString(EPes.Jmeno)]; } set { _setVar(GetString(EPes.Jmeno), GetString(value)); } }
		public String Plemeno { get { return _variables[GetString(EPes.Plemeno)]; } set { _setVar(GetString(EPes.Plemeno), GetString(value)); } }
		public String Pohlavi { get { return _variables[GetString(EPes.Pohlavi)]; } set { _setVar(GetString(EPes.Pohlavi), GetString(value)); } }
		public String Fci { get { return _variables[GetString(EPes.Fci)]; } set { _setVar(GetString(EPes.Fci), GetString(value)); } }
		public DateTime Datum { get { return GetDateTime(_variables[GetString(EPes.Datum)]); } set { _setVar(GetString(EPes.Datum), GetString(value)); } }
		public String Licence { get { return _variables[GetString(EPes.Licence)]; } set { _setVar(GetString(EPes.Licence), GetString(value)); } }
		public int Majitel { get { return GetInt(_variables[GetString(EPes.Majitel)]); } set { _setVar(GetString(EPes.Majitel), GetString(value)); } } // ID majitele psa
		public String MajitelJmeno { get { return _variables[GetString(EPes.Majitel_Jmeno)]; } set { _setVar(GetString(EPes.Majitel_Jmeno), GetString(value)); } } // jméno majitele psa
		public int Skupina { get { return GetInt(_variables[GetString(EPes.Skupina)]); } set { _setVar(GetString(EPes.Skupina), GetString(value)); } } // ID závodní skupiny
		public int StartBeh1 { get { return GetInt(_variables[GetString(EPes.Start_beh1)]); } set { _setVar(GetString(EPes.Start_beh1), GetString(value)); } } // pořadové číslo v prvním běhu
		public int StartBeh2 { get { return GetInt(_variables[GetString(EPes.Start_beh2)]); } set { _setVar(GetString(EPes.Start_beh2), GetString(value)); } } // pořadové číslo v druhém běhu
		public String ZavodLicence { get { return _variables[GetString(EPes.Zavod_licence)]; } set { _setVar(GetString(EPes.Zavod_licence), GetString(value)); } } // typ závodu (závod-licence-trening)
		public int Platba { get { return GetInt(_variables[GetString(EPes.Platba)]); } set { _setVar(GetString(EPes.Platba), GetString(value)); } } // zaplacená částka
		public int Doplatit { get { return GetInt(_variables[GetString(EPes.Doplatit)]); } set { _setVar(GetString(EPes.Doplatit), GetString(value)); } } // zbývá zaplatit
		public String Poznamka { get { return _variables[GetString(EPes.Poznamka)]; } set { _setVar(GetString(EPes.Poznamka), GetString(value)); } } // poznámka k platbě
		public String Diskval { get { return _variables[GetString(EPes.Diskval)]; } set { _setVar(GetString(EPes.Diskval), GetString(value)); } }

		public int AgilityA0 { get { return GetInt(_variables[GetString(EPes.Agility_A0)]); } set { _setVar(GetString(EPes.Agility_A0), GetString(value)); } } // hodnocení z prvního běhu
		public int SpeedA0 { get { return GetInt(_variables[GetString(EPes.Speed_A0)]); } set { _setVar(GetString(EPes.Speed_A0), GetString(value)); } }
		public int EnduranceA0 { get { return GetInt(_variables[GetString(EPes.Endurance_A0)]); } set { _setVar(GetString(EPes.Endurance_A0), GetString(value)); } }
		public int EnthusiasmA0 { get { return GetInt(_variables[GetString(EPes.Enthusiasm_A0)]); } set { _setVar(GetString(EPes.Enthusiasm_A0), GetString(value)); } }
		public int IntelligenceA0 { get { return GetInt(_variables[GetString(EPes.Intelligence_A0)]); } set { _setVar(GetString(EPes.Intelligence_A0), GetString(value)); } }
		public int AgilityA1 { get { return GetInt(_variables[GetString(EPes.Agility_A1)]); } set { _setVar(GetString(EPes.Agility_A1), GetString(value)); } } // hodnocení z prvního běhu
		public int SpeedA1 { get { return GetInt(_variables[GetString(EPes.Speed_A1)]); } set { _setVar(GetString(EPes.Speed_A1), GetString(value)); } }
		public int EnduranceA1 { get { return GetInt(_variables[GetString(EPes.Endurance_A1)]); } set { _setVar(GetString(EPes.Endurance_A1), GetString(value)); } }
		public int EnthusiasmA1 { get { return GetInt(_variables[GetString(EPes.Enthusiasm_A1)]); } set { _setVar(GetString(EPes.Enthusiasm_A1), GetString(value)); } }
		public int IntelligenceA1 { get { return GetInt(_variables[GetString(EPes.Intelligence_A1)]); } set { _setVar(GetString(EPes.Intelligence_A1), GetString(value)); } }

		public int Body1 { get { return GetInt(_variables[GetString(EPes.Body1)]); } set { _setVar(GetString(EPes.Body1), GetString(value)); } } // celkove body z prvního běhu

		public int AgilityB0 { get { return GetInt(_variables[GetString(EPes.Agility_B0)]); } set { _setVar(GetString(EPes.Agility_B0), GetString(value)); } } // hodnocení z druhého běhu
		public int SpeedB0 { get { return GetInt(_variables[GetString(EPes.speed_B0)]); } set { _setVar(GetString(EPes.speed_B0), GetString(value)); } }
		public int EnduranceB0 { get { return GetInt(_variables[GetString(EPes.Endurance_B0)]); } set { _setVar(GetString(EPes.Endurance_B0), GetString(value)); } }
		public int EnthusiasmB0 { get { return GetInt(_variables[GetString(EPes.Enthusiasm_B0)]); } set { _setVar(GetString(EPes.Enthusiasm_B0), GetString(value)); } }
		public int IntelligenceB0 { get { return GetInt(_variables[GetString(EPes.Intelligence_B0)]); } set { _setVar(GetString(EPes.Intelligence_B0), GetString(value)); } }
		public int AgilityB1 { get { return GetInt(_variables[GetString(EPes.Agility_B1)]); } set { _setVar(GetString(EPes.Agility_B1), GetString(value)); } } // hodnocení z druhého běhu
		public int SpeedB1 { get { return GetInt(_variables[GetString(EPes.Speed_B1)]); } set { _setVar(GetString(EPes.Speed_B1), GetString(value)); } }
		public int EnduranceB1 { get { return GetInt(_variables[GetString(EPes.Endurance_B1)]); } set { _setVar(GetString(EPes.Endurance_B1), GetString(value)); } }
		public int EnthusiasmB1 { get { return GetInt(_variables[GetString(EPes.Enthusiasm_B1)]); } set { _setVar(GetString(EPes.Enthusiasm_B1), GetString(value)); } }
		public int IntelligenceB1 { get { return GetInt(_variables[GetString(EPes.Intelligence_B1)]); } set { _setVar(GetString(EPes.Intelligence_B1), GetString(value)); } }
		public int Body2 { get { return GetInt(_variables[GetString(EPes.Body2)]); } set { _setVar(GetString(EPes.Body2), GetString(value)); } } // celkové body z druhého běhu
		public int Dvojice0 { get { return GetInt(_variables[GetString(EPes.Dvojice0)]); } set { _setVar(GetString(EPes.Dvojice0), GetString(value)); } } // pořadové číslo dvojice v prvním kole
		public int Dvojice1 { get { return GetInt(_variables[GetString(EPes.Dvojice1)]); } set { _setVar(GetString(EPes.Dvojice1), GetString(value)); } } // pořadové číslo dvojice v druhém kole
		public String Barva0 { get { return _variables[GetString(EPes.Barva0)]; } set { _setVar(GetString(EPes.Barva0), GetString(value)); } } // barva dresu v prvním kole
		public String Barva1 { get { return _variables[GetString(EPes.Barva1)]; } set { _setVar(GetString(EPes.Barva1), GetString(value)); } } // barva dresu v druhém kole
		
		#endregion
	}
}
