using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Xml;
using System.Globalization;
using System.Threading;


namespace projectModel
{

    public class EXCEL_Database
    {
        public void read_excel_lists(string sourcefile, List<String> listy)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;

            workbook = excelApp.Workbooks.Open(sourcefile);

            for (int i = 1; i < workbook.Sheets.Count + 1; i++)
            {
                listy.Add(((Excel.Worksheet)workbook.Sheets.get_Item(i)).Name);
            }
            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }

        public void read_excel_table_temp(Majitele dataM, Psi dataP, string sourceFile, Int32 list)
        {

            Int32 tmp = 0;
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            string databasePath = sourceFile;
            workbook = excelApp.Workbooks.Open(databasePath);
            
            worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

            Majitel temp_majitel = new Majitel();
            Pes temp_pes = new Pes();

            Int32 row = 0;

            range = worksheet.UsedRange;
            DataTable dt = new DataTable();

            String temp = null;

            for (row = 2; row <= range.Rows.Count; row++)
            {
                DataRow dr = dt.NewRow();

                temp = (range.Cells[row, 1] as Excel.Range).Value2;
                if (temp != null) // excel sloupec clenstvi
                {
                    string tempStr = temp.ToString();
                    if (tempStr[0] == 'N')
                    {
                        temp_majitel.clen = "ne"; // neni clenem klubu
                    }
                    else
                    {
                        temp_majitel.clen = "ano"; // je clenem klubu
                    }
                }
                else
                {
                    temp_majitel.clen = "ano"; // policko by melo byt prazdne - mozna NULL ?!
                }

                //temp = (range.Cells[row, 2] as Excel.Range).Value2;
                temp_pes.ZavodLicence = SetTempFromExcel(range, row, 2); // excel sloupec zavod-licence-trening

                temp_pes.Jmeno = SetTempFromExcel(range, row, 3); // excel sloupec jmeno psa

                temp_pes.Plemeno = SetTempFromExcel(range, row, 4); // excel sloupec plemeno

                temp_pes.Poznamka = SetTempFromExcel(range, row, 5); // excel sloupec poznamka ZAVODNI SKUPINA !!!!

                temp_pes.Pohlavi = SetTempFromExcel(range, row, 6).ToLower(); // excel sloupec pohlavi

                temp_pes.Platba = SetTempIntFromExcel(range, row, 8); // excel sloupec platba

                temp_pes.Licence = SetTempFromExcel(range, row, 11); // excel sloupec licence
                
                temp = (range.Cells[row, 14] as Excel.Range).Value.ToShortDateString();
                if (temp != null) // excel sloupec datum narozeni
                {
                    temp_pes.Datum = DateTime.ParseExact(temp, Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern, Thread.CurrentThread.CurrentCulture);
                }
                else
                    temp_pes.Datum = DateTime.ParseExact(DateTime.MinValue.ToShortDateString(), Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern, Thread.CurrentThread.CurrentCulture);
                
                temp_majitel.firstName = SetTempFromExcel(range, row, 15); // excel sloupec jmeno

                temp_majitel.narodnost = SetTempFromExcel(range, row, 17); // excel sloupec národnost

                temp_majitel.telefon = SetTempFromExcel(range, row, 18); // excel sloupec telefon

                temp_majitel.email = SetTempFromExcel(range, row, 19); // excel sloupec email

                temp_pes.Doplatit = 0;

                temp_majitel.id = dataM.GetNewId(); // přiřzení ID majiteli
                temp_majitel.penize = temp_pes.Platba + temp_pes.Doplatit; // součet zaplatil a má zaplatit
                temp_majitel.potvrzeni = "--"; // implicitně nastaven jako nezaplaceno
                temp_majitel.pocet_psu = 1;

                temp_pes.Id = dataP.GetNewId();
                temp_pes.Majitel = temp_majitel.id;
                temp_pes.MajitelJmeno = temp_majitel.firstName;
                temp_pes.Skupina = 0;
                temp_pes.StartBeh1 = 0;
                temp_pes.StartBeh1 = 0;
                temp_pes.Diskval = "---";
                temp_pes.AgilityA0 = 0;
                temp_pes.AgilityA1 = 0;
                temp_pes.SpeedA0 = 0;
                temp_pes.SpeedA1 = 0;
                temp_pes.EnduranceA0 = 0;
                temp_pes.EnduranceA1 = 0;
                temp_pes.EnthusiasmA0 = 0;
                temp_pes.EnthusiasmA1 = 0;
                temp_pes.IntelligenceA0 = 0;
                temp_pes.IntelligenceA1 = 0;
                temp_pes.AgilityB0 = 0;
                temp_pes.AgilityB1 = 0;
                temp_pes.SpeedB0 = 0;
                temp_pes.SpeedB1 = 0;
                temp_pes.EnduranceB0 = 0;
                temp_pes.EnduranceB1 = 0;
                temp_pes.EnthusiasmB0 = 0;
                temp_pes.EnthusiasmB1 = 0;
                temp_pes.IntelligenceB0 = 0;
                temp_pes.IntelligenceB1 = 0;
                temp_pes.Body1 = 0;
                temp_pes.Body2 = 0;
                temp_pes.Dvojice0 = 0;
                temp_pes.Dvojice1 = 0;
                temp_pes.Barva0 = "";
                temp_pes.Barva1 = "";

                tmp = dataM.FindSame(temp_majitel.firstName, temp_majitel.lastName, temp_majitel.email);
                if (tmp < 0)
                {
                    dataM.Add(new Majitel(temp_majitel));
                }
                else
                {
                    Majitel editmajitel = dataM.GetMajitelById(tmp);
                    editmajitel.penize += temp_majitel.penize; // přičtení majiteli platbu za dalšího psa
                    editmajitel.pocet_psu++; 
                    dataM.Edit(tmp, editmajitel);
                    temp_pes.Majitel = tmp;
                }

                dataP.Add(new Pes(temp_pes));

            }
            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();

        }
        
        private String SetTempFromExcel(Excel.Range range, Int32 row, Int32 col)
        {
            return range.Cells[row, col].Value2 != null ? range.Cells[row, col].Value2.ToString() : String.Empty;
        }

        private Int32 SetTempIntFromExcel(Excel.Range range, Int32 row, Int32 col)
        {
            Int32 outint = 0;
            if (range.Cells[row, col].Value2 != null)
                Int32.TryParse(range.Cells[row, col].Value2.ToString(), out outint);
            return range.Cells[row, col].Value2 != null ? outint : 0;
        }

        public void Write_to_excel(Psi dataP, string sourceFile, Int32 list, String type) // type "ROZPIS"/"ROZPIS_2"/...atd
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            Pes temp;
            String pomoc_cislo;
            List<int> dvojice = new List<int>();
            Int32 i = 0;
            Int32 n = 0;

            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != type)
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = type + Convert.ToString(list);
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = type + Convert.ToString(list);
            }

            range = worksheet.UsedRange;

            switch (type)
            {
                case "ROZPIS":
                    (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
                    (range.Cells[1, 2] as Excel.Range).Value2 = "Plemeno";
                    (range.Cells[1, 3] as Excel.Range).Value2 = "Dečka";
                    (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
                    (range.Cells[1, 5] as Excel.Range).Value2 = "Jméno psa";

                    dvojice = dataP.GetAllDvojice();
                    i = 1;
                    foreach (int x in dvojice)
                    {
                        temp = dataP.GetPesByDvojice(x, "červená");
                        if (temp == null)
                            continue;
                        (range.Cells[i + 1, 1] as Excel.Range).Value2 = temp.Dvojice0;
                        (range.Cells[i + 1, 2] as Excel.Range).Value2 = temp.Plemeno;
                        (range.Cells[i + 1, 3] as Excel.Range).Value2 = temp.Barva0;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                        (range.Cells[i + 1, 5] as Excel.Range).Value2 = temp.Jmeno;

                        ++i;

                        temp = dataP.GetPesByDvojice(x, "bílá");
                        if (temp == null)
                            continue;
                        (range.Cells[i + 1, 1] as Excel.Range).Value2 = temp.Dvojice0;
                        (range.Cells[i + 1, 2] as Excel.Range).Value2 = temp.Plemeno;
                        (range.Cells[i + 1, 3] as Excel.Range).Value2 = temp.Barva0;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                        (range.Cells[i + 1, 5] as Excel.Range).Value2 = temp.Jmeno;

                        ++i;
                    }
                    for (int z = 1; z < i + 1; z++)
                        for (int y = 1; y < 6; y++)
                            (range.Cells[z, y] as Excel.Range).BorderAround2();

                    worksheet.Columns.AutoFit();
                    break;

                case "ROZPIS_2":
                    (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
                    (range.Cells[1, 2] as Excel.Range).Value2 = "Plemeno";
                    (range.Cells[1, 3] as Excel.Range).Value2 = "Dečka";
                    (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
                    (range.Cells[1, 5] as Excel.Range).Value2 = "Jméno psa";
            
                    dvojice = dataP.GetAllDvojice_1();
                    i = 1;
                    foreach (int x in dvojice)
                    {
                        temp = dataP.GetPesByDvojice_1(x, "červená");
                        if (temp == null)
                            continue;
                        (range.Cells[i + 1, 1] as Excel.Range).Value2 = temp.Dvojice1;
                        (range.Cells[i + 1, 2] as Excel.Range).Value2 = temp.Plemeno;
                        (range.Cells[i + 1, 3] as Excel.Range).Value2 = temp.Barva1;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                        (range.Cells[i + 1, 5] as Excel.Range).Value2 = temp.Jmeno;
                        ++i;
                        temp = dataP.GetPesByDvojice_1(x, "bílá");
                        if (temp == null)
                            continue;
                        (range.Cells[i + 1, 1] as Excel.Range).Value2 = temp.Dvojice1;
                        (range.Cells[i + 1, 2] as Excel.Range).Value2 = temp.Plemeno;
                        (range.Cells[i + 1, 3] as Excel.Range).Value2 = temp.Barva1;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                        (range.Cells[i + 1, 5] as Excel.Range).Value2 = temp.Jmeno;
                        ++i;
                        for (int z = 1; z < i + 1; z++)
                            for (int y = 1; y < 6; y++)
                                (range.Cells[z, y] as Excel.Range).BorderAround2();

                        worksheet.Columns.AutoFit();
                    }
                    break;
                case "TABULKY":
                    n = 1;

                    (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
                    (range.Cells[1, 2] as Excel.Range).Value2 = "Dečka";
                    (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
                    (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
                    (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
                    (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
                    (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
                    (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
                    (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";
                    (range.Cells[1, 10] as Excel.Range).Value2 = "Součet";

                    for (int y = 1; y < 11; y++)
                        (range.Cells[1, y] as Excel.Range).BorderAround2();

                    dvojice = dataP.GetAllDvojice();
                    i = 1;
                    foreach (int x in dvojice)
                    {
                        temp = dataP.GetPesByDvojice(x, "červená");
                        if (temp == null)
                            continue;

                        (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.Dvojice0;
                        (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.Barva0;
                        (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.Plemeno;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;

                        for (int y = 1; y < 11; y++)
                            (range.Cells[i + n, y] as Excel.Range).BorderAround2();

                        ++i;

                        temp = dataP.GetPesByDvojice(x, "bílá");
                        if (temp == null)
                        {
                            ++n;
                            continue;
                        }

                        (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.Dvojice0;
                        (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.Barva0;
                        (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.Plemeno;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;

                        for (int y = 1; y < 11; y++)
                            (range.Cells[i + n, y] as Excel.Range).BorderAround2();

                        ++i;
                        ++n;
                    }
                    break;
                case "TABULKY_2":
                    n = 1;

                    (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
                    (range.Cells[1, 2] as Excel.Range).Value2 = "Dečka";
                    (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
                    (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
                    (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
                    (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
                    (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
                    (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
                    (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";
                    (range.Cells[1, 10] as Excel.Range).Value2 = "Součet";

                    for (int y = 1; y < 11; y++)
                        (range.Cells[1, y] as Excel.Range).BorderAround2();

                    dvojice = dataP.GetAllDvojice_1();
                    i = 1;
                    foreach (int x in dvojice)
                    {
                        temp = dataP.GetPesByDvojice_1(x, "červená");
                        if (temp == null)
                            continue;

                        (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.Dvojice1;
                        (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.Barva1;
                        (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.Plemeno;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;

                        for (int y = 1; y < 11; y++)
                            (range.Cells[i + n, y] as Excel.Range).BorderAround2();

                        ++i;

                        temp = dataP.GetPesByDvojice_1(x, "bílá");
                        if (temp == null)
                        {
                            ++n;
                            continue;
                        }

                        (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.Dvojice1;
                        (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.Barva1;
                        (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.Plemeno;
                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;

                        for (int y = 1; y < 11; y++)
                            (range.Cells[i + n, y] as Excel.Range).BorderAround2();

                        ++i;
                        ++n;
                    }
                    break;
                case "VYSLEDKY":
                    n = 1;

                    (range.Cells[1, 1] as Excel.Range).Value2 = "Č.psa";
                    (range.Cells[1, 2] as Excel.Range).Value2 = "Jméno psa";
                    (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
                    (range.Cells[1, 4] as Excel.Range).Value2 = "Pohlaví";

                    // první rozhodčí

                    (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
                    (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
                    (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
                    (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
                    (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";

                    // druhý rozhodčí

                    (range.Cells[1, 10] as Excel.Range).Value2 = "Obratnost";
                    (range.Cells[1, 11] as Excel.Range).Value2 = "Rychlost";
                    (range.Cells[1, 12] as Excel.Range).Value2 = "Vytrvalost";
                    (range.Cells[1, 13] as Excel.Range).Value2 = "Úsilí";
                    (range.Cells[1, 14] as Excel.Range).Value2 = "Inteligence";
                    (range.Cells[1, 15] as Excel.Range).Value2 = "Součet";

                    for (int y = 1; y < 16; y++)
                        (range.Cells[1, y] as Excel.Range).BorderAround2();

                    dvojice = dataP.GetAllStartList();
                    i = 1;
                    foreach (int x in dvojice)
                    {
                        temp = dataP.GetPesByStartNo(x);
                        if (temp == null)
                            continue;

                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + n, 1] as Excel.Range).Value2 = pomoc_cislo;
                        (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.Jmeno;
                        (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.Plemeno;
                        (range.Cells[i + n, 4] as Excel.Range).Value2 = temp.Pohlavi;

                        (range.Cells[i + n, 5] as Excel.Range).Value2 = temp.AgilityA0;
                        (range.Cells[i + n, 6] as Excel.Range).Value2 = temp.SpeedA0;
                        (range.Cells[i + n, 7] as Excel.Range).Value2 = temp.EnduranceA0;
                        (range.Cells[i + n, 8] as Excel.Range).Value2 = temp.EnthusiasmA0;
                        (range.Cells[i + n, 9] as Excel.Range).Value2 = temp.IntelligenceA0;

                        (range.Cells[i + n, 10] as Excel.Range).Value2 = temp.AgilityA1;
                        (range.Cells[i + n, 11] as Excel.Range).Value2 = temp.SpeedA1;
                        (range.Cells[i + n, 12] as Excel.Range).Value2 = temp.EnduranceA1;
                        (range.Cells[i + n, 13] as Excel.Range).Value2 = temp.EnthusiasmA1;
                        (range.Cells[i + n, 14] as Excel.Range).Value2 = temp.IntelligenceA1;

                        (range.Cells[i + n, 15] as Excel.Range).Value2 = temp.Body1;

                        for (int y = 1; y < 16; y++)
                            (range.Cells[i + n, y] as Excel.Range).BorderAround2();

                        ++i;
                    }
                    break;
                case "VYSLEDKY_2":
                    n = 1;

                    (range.Cells[1, 1] as Excel.Range).Value2 = "Č.psa";
                    (range.Cells[1, 2] as Excel.Range).Value2 = "Jméno psa";
                    (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
                    (range.Cells[1, 4] as Excel.Range).Value2 = "Pohlaví";

                    // první rozhodčí

                    (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
                    (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
                    (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
                    (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
                    (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";

                    // druhý rozhodčí

                    (range.Cells[1, 10] as Excel.Range).Value2 = "Obratnost";
                    (range.Cells[1, 11] as Excel.Range).Value2 = "Rychlost";
                    (range.Cells[1, 12] as Excel.Range).Value2 = "Vytrvalost";
                    (range.Cells[1, 13] as Excel.Range).Value2 = "Úsilí";
                    (range.Cells[1, 14] as Excel.Range).Value2 = "Inteligence";
                    (range.Cells[1, 15] as Excel.Range).Value2 = "Součet";
                    (range.Cells[1, 16] as Excel.Range).Value2 = "Celkový součet";
                    (range.Cells[1, 17] as Excel.Range).Value2 = "Umístění ve skupině";

                    for (int y = 1; y < 18; y++)
                        (range.Cells[1, y] as Excel.Range).BorderAround2();

                    dvojice = dataP.GetAllStartList();
                    i = 1;
                    foreach (int x in dvojice)
                    {
                        temp = dataP.GetPesByStartNo(x);
                        if (temp == null)
                            continue;

                        if (temp.ZavodLicence == "Licence")
                        {
                            pomoc_cislo = "L " + Convert.ToString(temp.StartBeh1);
                        }
                        else
                        {
                            pomoc_cislo = Convert.ToString(temp.StartBeh1);
                        }
                        (range.Cells[i + n, 1] as Excel.Range).Value2 = pomoc_cislo;
                        (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.Jmeno;
                        (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.Plemeno;
                        (range.Cells[i + n, 4] as Excel.Range).Value2 = temp.Pohlavi;

                        (range.Cells[i + n, 5] as Excel.Range).Value2 = temp.AgilityB0;
                        (range.Cells[i + n, 6] as Excel.Range).Value2 = temp.SpeedB0;
                        (range.Cells[i + n, 7] as Excel.Range).Value2 = temp.EnduranceB0;
                        (range.Cells[i + n, 8] as Excel.Range).Value2 = temp.EnthusiasmB0;
                        (range.Cells[i + n, 9] as Excel.Range).Value2 = temp.IntelligenceB0;

                        (range.Cells[i + n, 10] as Excel.Range).Value2 = temp.AgilityB1;
                        (range.Cells[i + n, 11] as Excel.Range).Value2 = temp.SpeedB1;
                        (range.Cells[i + n, 12] as Excel.Range).Value2 = temp.EnduranceB1;
                        (range.Cells[i + n, 13] as Excel.Range).Value2 = temp.EnthusiasmB1;
                        (range.Cells[i + n, 14] as Excel.Range).Value2 = temp.IntelligenceB1;

                        (range.Cells[i + n, 15] as Excel.Range).Value2 = temp.Body2;

                        (range.Cells[i + n, 16] as Excel.Range).Value2 = temp.Body1 + temp.Body2;

                        (range.Cells[i + n, 17] as Excel.Range).Value2 = temp.Skupina;

                        for (int y = 1; y < 18; y++)
                            (range.Cells[i + n, y] as Excel.Range).BorderAround2();

                        ++i;
                    }
                    break;
                default: break;
            }

            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }
    }
}
