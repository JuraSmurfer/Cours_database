using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class Pes
    {
        public Pes()
        {
            this.id = 0;
            this.jmeno = "";
            this.plemeno = "";
            this.pohlavi = "";
            this.fci = "";
            this.datum = DateTime.ParseExact("1. 1. 1993", "d. M. yyyy", null);
            this.licence = "";
            this.majitel = 0;
            this.majitel_jmeno = "";
            this.skupina = 0;
            this.start_beh1 = 0;
            this.start_beh2 = 0;

            this.zavod_licence = "";
            this.platba = 0;
            this.doplatit = 0;
            this.poznamka = "";
            this.diskval = "---";

            //body prvni kolo
            this.agility_A0 = 0;
            this.speed_A0 = 0;
            this.endurance_A0 = 0;
            this.enthusiasm_A0 = 0;
            this.intelligence_A0 = 0;
            this.agility_A1 = 0;
            this.speed_A1 = 0;
            this.endurance_A1 = 0;
            this.enthusiasm_A1 = 0;
            this.intelligence_A1 = 0;

            //body celkem kolo 1
            this.body1 = 0;

            //body druhe kolo
            this.agility_B0 = 0;
            this.speed_B0 = 0;
            this.endurance_B0 = 0;
            this.enthusiasm_B0 = 0;
            this.intelligence_B0 = 0;
            this.agility_B1 = 0;
            this.speed_B1 = 0;
            this.endurance_B1 = 0;
            this.enthusiasm_B1 = 0;
            this.intelligence_B1 = 0;

            //body celkem kolo 2
            this.body2 = 0;

            // číslo dvojice pro oba běhy
            this.dvojice0 = 0;
            this.dvojice1 = 0;
            // barva dresu pro oba běhy
            this.barva0 = "";
            this.barva1 = "";
        }

        public Pes(String[] data)
        {
            this.id =  Int32.Parse(data[0]);
            this.jmeno = data[1];
            this.plemeno = data[2];
            this.pohlavi = data[3];
            this.fci = data[4];
            this.datum = DateTime.ParseExact(data[5], "d. M. yyyy", null);
            this.licence = data[6];
            this.majitel = Int32.Parse(data[7]);
            this.majitel_jmeno = data[8];
            this.skupina =  Int32.Parse(data[9]);
            this.start_beh1 =  Int32.Parse(data[10]);
            this.start_beh2 =  Int32.Parse(data[11]);

            this.zavod_licence = data[12];
            this.platba =  Int32.Parse(data[13]);
            this.doplatit =  Int32.Parse(data[14]);
            this.poznamka = data[15];
            this.diskval = data[16];

            //body prvni kolo, oba rozhodci (A0,A1)
            this.agility_A0 = Int32.Parse(data[17]);
            this.agility_A1 = Int32.Parse(data[22]);
            this.speed_A0 = Int32.Parse(data[18]);
            this.speed_A1 = Int32.Parse(data[23]);
            this.endurance_A0 = Int32.Parse(data[19]);
            this.endurance_A1 = Int32.Parse(data[24]);
            this.enthusiasm_A0 = Int32.Parse(data[20]);
            this.enthusiasm_A1 = Int32.Parse(data[25]);
            this.intelligence_A0 = Int32.Parse(data[21]);
            this.intelligence_A1 = Int32.Parse(data[26]);
            //body celkem kolo 1
            this.body1 =  Int32.Parse(data[27]);

            //body prvni kolo, oba rozhodci (B0,B1)
            this.agility_B0 = Int32.Parse(data[28]);
            this.agility_B1 = Int32.Parse(data[33]);
            this.speed_B0 = Int32.Parse(data[29]);
            this.speed_B1 = Int32.Parse(data[34]);
            this.endurance_B0 = Int32.Parse(data[30]);
            this.endurance_B1 = Int32.Parse(data[35]);
            this.enthusiasm_B0 = Int32.Parse(data[31]);
            this.enthusiasm_B1 = Int32.Parse(data[36]);
            this.intelligence_B0 = Int32.Parse(data[32]);
            this.intelligence_B1 = Int32.Parse(data[37]);
            //body celkem kolo 2
            this.body2 =  Int32.Parse(data[38]);

            this.dvojice0 = Int32.Parse(data[39]);
            this.dvojice1 = Int32.Parse(data[40]);

            this.barva0 = data[41];
            this.barva1 = data[42];
        }

        public Pes(Pes aPes)
        {
            this.id = aPes.id;
            this.jmeno = aPes.jmeno;
            this.plemeno = aPes.plemeno;
            this.pohlavi = aPes.pohlavi;
            this.fci = aPes.fci;
            this.datum = aPes.datum;
            this.licence = aPes.licence;
            this.majitel = aPes.majitel;
            this.majitel_jmeno = aPes.majitel_jmeno;
            this.skupina = aPes.skupina;
            this.start_beh1 = aPes.start_beh1;
            this.start_beh2 = aPes.start_beh2;

            this.zavod_licence = aPes.zavod_licence;
            this.platba = aPes.platba;
            this.doplatit = aPes.doplatit;
            this.poznamka = aPes.poznamka;
            this.diskval = aPes.diskval;

            //body prvni kolo, prvni rozhodci
            this.agility_A0 = aPes.agility_A0;
            this.speed_A0 = aPes.speed_A0;
            this.endurance_A0 = aPes.endurance_A0;
            this.enthusiasm_A0 = aPes.enthusiasm_A0;
            this.intelligence_A0 = aPes.intelligence_A0;

            this.agility_A1 = aPes.agility_A1;
            this.speed_A1 = aPes.speed_A1;
            this.endurance_A1 = aPes.endurance_A1;
            this.enthusiasm_A1 = aPes.enthusiasm_A1;
            this.intelligence_A1 = aPes.intelligence_A1;

            this.body1 = aPes.body1;

            //body prvni kolo, druhy rozhodci
            this.agility_B0 = aPes.agility_B0;
            this.speed_B0 = aPes.speed_B0;
            this.endurance_B0 = aPes.endurance_B0;
            this.enthusiasm_B0 = aPes.enthusiasm_B0;
            this.intelligence_B0 = aPes.intelligence_B0;

            this.agility_B1 = aPes.agility_B1;
            this.speed_B1 = aPes.speed_B1;
            this.endurance_B1 = aPes.endurance_B1;
            this.enthusiasm_B1 = aPes.enthusiasm_B1;
            this.intelligence_B1 = aPes.intelligence_B1;

            this.body2 = aPes.body2;

            this.dvojice0 = aPes.dvojice0;
            this.dvojice1 = aPes.dvojice1;

            this.barva0 = aPes.barva0;
            this.barva1 = aPes.barva1;
        }
        
        public Int32 id { get; set; }
        public String jmeno { get; set; }
        public String plemeno { get; set; }
        public String pohlavi { get; set; }
        public String fci { get; set; }
        public DateTime datum { get; set; }
        public String licence { get; set; }
        public Int32 majitel { get; set; } // ID majitele psa
        public String majitel_jmeno { get; set; } // jméno majitele psa
        public Int32 skupina { get; set; } // ID závodní skupiny
        public Int32 start_beh1 { get; set; } // pořadové číslo v prvním běhu
        public Int32 start_beh2 { get; set; } // pořadové číslo v druhém běhu
        public String zavod_licence { get; set; } // typ závodu (závod-licence-trening)
        public Int32 platba { get; set; } // zaplacená částka
        public Int32 doplatit { get; set; } // zbývá zaplatit
        public String poznamka { get; set; } // poznámka k platbě
        public String diskval { get; set; }

        public Int32 agility_A0 { get; set; } // hodnocení z prvního běhu
        public Int32 speed_A0 { get; set; }
        public Int32 endurance_A0 { get; set; }
        public Int32 enthusiasm_A0 { get; set; }
        public Int32 intelligence_A0 { get; set; }
        public Int32 agility_A1 { get; set; } // hodnocení z prvního běhu
        public Int32 speed_A1 { get; set; }
        public Int32 endurance_A1 { get; set; }
        public Int32 enthusiasm_A1 { get; set; }
        public Int32 intelligence_A1 { get; set; }
        
        public Int32 body1 { get; set; } // celkove body z prvního běhu

        public Int32 agility_B0 { get; set; } // hodnocení z druhého běhu
        public Int32 speed_B0 { get; set; }
        public Int32 endurance_B0 { get; set; }
        public Int32 enthusiasm_B0 { get; set; }
        public Int32 intelligence_B0 { get; set; }
        public Int32 agility_B1 { get; set; } // hodnocení z druhého běhu
        public Int32 speed_B1 { get; set; }
        public Int32 endurance_B1 { get; set; }
        public Int32 enthusiasm_B1 { get; set; }
        public Int32 intelligence_B1 { get; set; }
        public Int32 body2 { get; set; } // celkové body z druhého běhu
        public Int32 dvojice0 { get; set; } // pořadové číslo dvojice v prvním kole
        public Int32 dvojice1 { get; set; } // pořadové číslo dvojice v druhém kole
        public String barva0 { get; set; } // barva dresu v prvním kole
        public String barva1 { get; set; } // barva dresu v druhém kole
    }
}
