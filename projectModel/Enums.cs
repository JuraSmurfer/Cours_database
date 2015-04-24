using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectModel
{
	public enum EPes
	{
		Id = 0,
        Jmeno,
        Plemeno,
        Pohlavi,
        Fci,
		Datum,
        Licence,
        Majitel,
        Majitel_Jmeno,
        Skupina,
        Start_beh1,
        Start_beh2,

        Zavod_licence,
        Platba,
        Doplatit,
        Poznamka,
        Diskval,

        //body prvni kolo
        Agility_A0,
        Speed_A0,
        Endurance_A0,
        Enthusiasm_A0,
        Intelligence_A0,
        Agility_A1,
        Speed_A1,
        Endurance_A1,
        Enthusiasm_A1,
        Intelligence_A1,

        //body celkem kolo 1
        Body1,

        //body druhe kolo
        Agility_B0,
        speed_B0,
        Endurance_B0,
        Enthusiasm_B0,
        Intelligence_B0,
        Agility_B1,
        Speed_B1,
        Endurance_B1,
        Enthusiasm_B1,
        Intelligence_B1,

        //body celkem kolo 2
        Body2,

        // číslo dvojice pro oba běhy
        Dvojice0,
        Dvojice1,
        // barva dresu pro oba běhy
        Barva0,
        Barva1
	}
}
