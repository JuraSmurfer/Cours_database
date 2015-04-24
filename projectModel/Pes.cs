using System;
using System.Collections.Generic;
using System.Linq;

namespace projectModel
{
    /// <summary>
    /// Třída pes.
    /// #1  ID
    /// #2  JMENO
    /// #2  PLEMENO
    /// #3  POHLAVI
    /// #4  FCI
    /// #5  DATUM
    /// #6  LICENCE
    /// #7  MAJITEL
    /// #8  SKUPINA
    /// #9  START_BEH1
    /// #10 START_BEH2
    /// #11 ZAVOD_LICENCE
    /// #12 PLATBA
    /// #13 DOPLATIT
    /// #14 POZNAMKA
    /// #15 DISKVAL
    /// BODY 2 ROZHODCI 2 KOLA
    /// #16 AGILITY
    /// #17 SPEED
    /// #18 ENDURANCE
    /// #19 ENTHUSIASM
    /// #20 INTELLIGENCE
    /// #21-25 -//-
    /// #26 BODY1
    /// #27-31 -//-
    /// #32-36 -//-
    /// #37 BODY2
    /// </summary>
    public class Pes : Utils
    {
	    public Dictionary<string, string> GetDictionary { get { return _variables; } }
	    private Dictionary<string, string> _variables = new Dictionary<string, string>(); /// takto nebudou promenne zabirat tolik mista v programu
		private void _setVar(string name, string value) { _variables[name] = value; }
        public Pes()
        {
	        InitVar();
	        /**
            this.Id = 0;
            this.Jmeno = "";
            this.Plemeno = "";
            this.Pohlavi = "";
            this.Fci = "";
			this.Datum = GetDateTime("1. 1. 1993");
            this.Licence = "";
            this.Majitel = 0;
            this.MajitelJmeno = "";
            this.Skupina = 0;
            this.StartBeh1 = 0;
            this.StartBeh2 = 0;

            this.ZavodLicence = "";
            this.Platba = 0;
            this.Doplatit = 0;
            this.Poznamka = "";
            this.Diskval = "---";

            //body prvni kolo
            this.AgilityA0 = 0;
            this.SpeedA0 = 0;
            this.EnduranceA0 = 0;
            this.EnthusiasmA0 = 0;
            this.IntelligenceA0 = 0;
            this.AgilityA1 = 0;
            this.SpeedA1 = 0;
            this.EnduranceA1 = 0;
            this.EnthusiasmA1 = 0;
            this.IntelligenceA1 = 0;

            //body celkem kolo 1
            this.Body1 = 0;

            //body druhe kolo
            this.AgilityB0 = 0;
            this.SpeedB0 = 0;
            this.EnduranceB0 = 0;
            this.EnthusiasmB0 = 0;
            this.IntelligenceB0 = 0;
            this.AgilityB1 = 0;
            this.SpeedB1 = 0;
            this.EnduranceB1 = 0;
            this.EnthusiasmB1 = 0;
            this.IntelligenceB1 = 0;

            //body celkem kolo 2
            this.Body2 = 0;

            // číslo dvojice pro oba běhy
            this.Dvojice0 = 0;
            this.Dvojice1 = 0;
            // barva dresu pro oba běhy
            this.Barva0 = "";
            this.Barva1 = "";/**/
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
			/**
            this.Id =  GetInt(data[0]); /// Int32.Parse(data[]) pada, nevis 100% jestli tam bude cislo]
			this.Jmeno = data[1];
			this.Plemeno = data[2];
			this.Pohlavi = data[3];
			this.Fci = data[4];
			this.Datum = DateTime.ParseExact(data[5], "d. M. yyyy", null);
			this.Licence = data[6];
			this.Majitel = Int32.Parse(data[7]);
			this.MajitelJmeno = data[8];
			this.Skupina = Int32.Parse(data[9]);
			this.StartBeh1 = Int32.Parse(data[10]);
			this.StartBeh2 = Int32.Parse(data[11]);

			this.ZavodLicence = data[12];
			this.Platba = Int32.Parse(data[13]);
			this.Doplatit = Int32.Parse(data[14]);
			this.Poznamka = data[15];
			this.Diskval = data[16];

			//body prvni kolo, oba rozhodci (A0,A1)
			this.AgilityA0 = Int32.Parse(data[17]);
			this.AgilityA1 = Int32.Parse(data[22]);
			this.SpeedA0 = Int32.Parse(data[18]);
			this.SpeedA1 = Int32.Parse(data[23]);
			this.EnduranceA0 = Int32.Parse(data[19]);
			this.EnduranceA1 = Int32.Parse(data[24]);
			this.EnthusiasmA0 = Int32.Parse(data[20]);
			this.EnthusiasmA1 = Int32.Parse(data[25]);
			this.IntelligenceA0 = Int32.Parse(data[21]);
			this.IntelligenceA1 = Int32.Parse(data[26]);
			//body celkem kolo 1
			this.Body1 = Int32.Parse(data[27]);

			//body prvni kolo, oba rozhodci (B0,B1)
			this.AgilityB0 = Int32.Parse(data[28]);
			this.AgilityB1 = Int32.Parse(data[33]);
			this.SpeedB0 = Int32.Parse(data[29]);
			this.SpeedB1 = Int32.Parse(data[34]);
			this.EnduranceB0 = Int32.Parse(data[30]);
			this.EnduranceB1 = Int32.Parse(data[35]);
			this.EnthusiasmB0 = Int32.Parse(data[31]);
			this.EnthusiasmB1 = Int32.Parse(data[36]);
			this.IntelligenceB0 = Int32.Parse(data[32]);
			this.IntelligenceB1 = Int32.Parse(data[37]);
			//body celkem kolo 2
			this.Body2 = Int32.Parse(data[38]);

			this.Dvojice0 = Int32.Parse(data[39]);
			this.Dvojice1 = Int32.Parse(data[40]);

			this.Barva0 = data[41];
			this.Barva1 = data[42];/**/
        }

        public Pes(Pes aPes)
		{
			InitVar(aPes.GetDictionary);
			/*
			Id = aPes.Id;
			Jmeno = aPes.Jmeno;
			Plemeno = aPes.Plemeno;
			Pohlavi = aPes.Pohlavi;
			Fci = aPes.Fci;
			Datum = aPes.Datum;
			Licence = aPes.Licence;
			Majitel = aPes.Majitel;
			MajitelJmeno = aPes.MajitelJmeno;
			Skupina = aPes.Skupina;
			StartBeh1 = aPes.StartBeh1;
			StartBeh2 = aPes.StartBeh2;

			ZavodLicence = aPes.ZavodLicence;
			Platba = aPes.Platba;
			Doplatit = aPes.Doplatit;
			Poznamka = aPes.Poznamka;
			Diskval = aPes.Diskval;

			//body prvni kolo, prvni rozhodci
			AgilityA0 = aPes.AgilityA0;
			SpeedA0 = aPes.SpeedA0;
			EnduranceA0 = aPes.EnduranceA0;
			EnthusiasmA0 = aPes.EnthusiasmA0;
			IntelligenceA0 = aPes.IntelligenceA0;

			AgilityA1 = aPes.AgilityA1;
			SpeedA1 = aPes.SpeedA1;
			EnduranceA1 = aPes.EnduranceA1;
			EnthusiasmA1 = aPes.EnthusiasmA1;
			IntelligenceA1 = aPes.IntelligenceA1;

			Body1 = aPes.Body1;

			//body prvni kolo, druhy rozhodci
			AgilityB0 = aPes.AgilityB0;
			SpeedB0 = aPes.SpeedB0;
			EnduranceB0 = aPes.EnduranceB0;
			EnthusiasmB0 = aPes.EnthusiasmB0;
			IntelligenceB0 = aPes.IntelligenceB0;

			AgilityB1 = aPes.AgilityB1;
			SpeedB1 = aPes.SpeedB1;
			EnduranceB1 = aPes.EnduranceB1;
			EnthusiasmB1 = aPes.EnthusiasmB1;
			IntelligenceB1 = aPes.IntelligenceB1;

			Body2 = aPes.Body2;

			Dvojice0 = aPes.Dvojice0;
			Dvojice1 = aPes.Dvojice1;

			Barva0 = aPes.Barva0;
			Barva1 = aPes.Barva1;*/
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
		/**//**
		public Int32 Id { get; set; }
		public String Jmeno { get; set; }
		public String Plemeno { get; set; }
		public String Pohlavi { get; set; }
		public String Fci { get; set; }
		public DateTime Datum { get; set; }
		public String Licence { get; set; }
		public Int32 Majitel { get; set; } // ID majitele psa
		public String MajitelJmeno { get; set; } // jméno majitele psa
		public Int32 Skupina { get; set; } // ID závodní skupiny
		public Int32 StartBeh1 { get; set; } // pořadové číslo v prvním běhu
		public Int32 StartBeh2 { get; set; } // pořadové číslo v druhém běhu
		public String ZavodLicence { get; set; } // typ závodu (závod-licence-trening)
		public Int32 Platba { get; set; } // zaplacená částka
		public Int32 Doplatit { get; set; } // zbývá zaplatit
		public String Poznamka { get; set; } // poznámka k platbě
		public String Diskval { get; set; }

		public Int32 AgilityA0 { get; set; } // hodnocení z prvního běhu
		public Int32 SpeedA0 { get; set; }
		public Int32 EnduranceA0 { get; set; }
		public Int32 EnthusiasmA0 { get; set; }
		public Int32 IntelligenceA0 { get; set; }
		public Int32 AgilityA1 { get; set; } // hodnocení z prvního běhu
		public Int32 SpeedA1 { get; set; }
		public Int32 EnduranceA1 { get; set; }
		public Int32 EnthusiasmA1 { get; set; }
		public Int32 IntelligenceA1 { get; set; }

		public Int32 Body1 { get; set; } // celkove body z prvního běhu

		public Int32 AgilityB0 { get; set; } // hodnocení z druhého běhu
		public Int32 SpeedB0 { get; set; }
		public Int32 EnduranceB0 { get; set; }
		public Int32 EnthusiasmB0 { get; set; }
		public Int32 IntelligenceB0 { get; set; }
		public Int32 AgilityB1 { get; set; } // hodnocení z druhého běhu
		public Int32 SpeedB1 { get; set; }
		public Int32 EnduranceB1 { get; set; }
		public Int32 EnthusiasmB1 { get; set; }
		public Int32 IntelligenceB1 { get; set; }
		public Int32 Body2 { get; set; } // celkové body z druhého běhu
		public Int32 Dvojice0 { get; set; } // pořadové číslo dvojice v prvním kole
		public Int32 Dvojice1 { get; set; } // pořadové číslo dvojice v druhém kole
		public String Barva0 { get; set; } // barva dresu v prvním kole
		public String Barva1 { get; set; } // barva dresu v druhém kole
		/**/
		#endregion
	}
}
