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

            dynamic temp = null;

            for (row = 2; row <= range.Rows.Count; row++)
            {
                DataRow dr = dt.NewRow();

                temp = (range.Cells[row, 1] as Excel.Range).Value2;
                if (temp != null) // excel sloupec clenstvi
                {
                    string tempStr = Convert.ToString(temp);
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

                temp = (range.Cells[row, 2] as Excel.Range).Value2;
                if (temp != null) // excel sloupec zavod-licence-trening
                {
                    temp_pes.zavod_licence = Convert.ToString(temp);
                }
                else
                    temp_pes.zavod_licence = "";

                temp = (range.Cells[row, 3] as Excel.Range).Value2;
                if (temp != null) // excel sloupec jmeno psa
                {
                    temp_pes.jmeno = Convert.ToString(temp);
                }
                else
                    temp_pes.jmeno = "";

                temp = (range.Cells[row, 4] as Excel.Range).Value2;
                if (temp != null) // excel sloupec plemeno
                {
                    temp_pes.plemeno = Convert.ToString(temp);
                }
                else
                    temp_pes.plemeno = "";

                temp = (range.Cells[row, 5] as Excel.Range).Value2;
                if (temp != null) // excel sloupec poznamka ZAVODNI SKUPINA !!!!
                {
                    temp_pes.poznamka = Convert.ToString(temp);
                }
                else
                    temp_pes.poznamka = "";

                temp = (range.Cells[row, 6] as Excel.Range).Value2;
                if (temp != null) // excel sloupec pohlavi
                {
                    temp_pes.pohlavi = temp.ToLower();
                }
                else
                    temp_pes.pohlavi = "";

                //temp = (range.Cells[row, 8] as Excel.Range).Value2;
                //if (temp != null) // excel sloupec platba
                //{
                //    //int platba;
                //    temp_pes.platba = Convert.ToInt32(temp);
                //    //ERROR !!!!! debilní TryParse !!!!!
                //    /*if (int.TryParse(temp, out platba))
                //        temp_pes.platba = platba;
                //    else
                //        temp_pes.platba = 0;*/
                //}
                //else
                //    temp_pes.platba = 0;

                temp = (range.Cells[row, 11] as Excel.Range).Value2;
                if (temp != null) // excel sloupec licence
                {
                    temp_pes.licence = Convert.ToString(temp);
                }
                else
                    temp_pes.licence = "0";

                temp = (range.Cells[row, 14] as Excel.Range).Value;
                if (temp != null) // excel sloupec datum narozeni
                {
                    temp_pes.datum = DateTime.ParseExact(temp.ToShortDateString(), "d. M. yyyy", null);
                }
                else
                    temp_pes.datum = DateTime.ParseExact("1. 1. 1914", "d. M. yyyy", null);

                temp = (range.Cells[row, 15] as Excel.Range).Value2;
                if (temp != null) // excel sloupec jmeno
                {
                    temp_majitel.firstName = Convert.ToString(temp);
                }
                else
                    temp_majitel.firstName = "";

                temp = (range.Cells[row, 17] as Excel.Range).Value2;
                if (temp != null) // excel sloupec národnost
                {
                    temp_majitel.narodnost = Convert.ToString(temp);
                }
                else
                    temp_majitel.narodnost = "";

                temp = (range.Cells[row, 18] as Excel.Range).Value2;
                if (temp != null) // excel sloupec telefon
                {
                    temp_majitel.telefon = Convert.ToString(temp);
                }
                else
                    temp_majitel.telefon = "";

                temp = (range.Cells[row, 19] as Excel.Range).Value2;
                if (temp != null) // excel sloupec email
                {
                    temp_majitel.email = Convert.ToString(temp);
                }
                else
                    temp_majitel.email = "";

                temp_pes.doplatit = 0;

                temp_majitel.id = dataM.GetNewId(); // přiřzení ID majiteli
                temp_majitel.penize = temp_pes.platba + temp_pes.doplatit; // součet zaplatil a má zaplatit
                temp_majitel.potvrzeni = "--"; // implicitně nastaven jako nezaplaceno
                temp_majitel.pocet_psu = 1;

                temp_pes.id = dataP.GetNewId();
                temp_pes.majitel = temp_majitel.id;
                temp_pes.majitel_jmeno = temp_majitel.firstName;
                temp_pes.skupina = 0;
                temp_pes.start_beh1 = 0;
                temp_pes.start_beh1 = 0;
                temp_pes.diskval = "---";
                temp_pes.agility_A0 = 0;
                temp_pes.agility_A1 = 0;
                temp_pes.speed_A0 = 0;
                temp_pes.speed_A1 = 0;
                temp_pes.endurance_A0 = 0;
                temp_pes.endurance_A1 = 0;
                temp_pes.enthusiasm_A0 = 0;
                temp_pes.enthusiasm_A1 = 0;
                temp_pes.intelligence_A0 = 0;
                temp_pes.intelligence_A1 = 0;
                temp_pes.agility_B0 = 0;
                temp_pes.agility_B1 = 0;
                temp_pes.speed_B0 = 0;
                temp_pes.speed_B1 = 0;
                temp_pes.endurance_B0 = 0;
                temp_pes.endurance_B1 = 0;
                temp_pes.enthusiasm_B0 = 0;
                temp_pes.enthusiasm_B1 = 0;
                temp_pes.intelligence_B0 = 0;
                temp_pes.intelligence_B1 = 0;
                temp_pes.body1 = 0;
                temp_pes.body2 = 0;
                temp_pes.dvojice0 = 0;
                temp_pes.dvojice1 = 0;
                temp_pes.barva0 = "";
                temp_pes.barva1 = "";

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
                    temp_pes.majitel = tmp;
                }

                dataP.Add(new Pes(temp_pes));

            }
            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();

        }

        public void Write_to_excel(Psi dataP, string sourceFile, Int32 list, String type) // type "ROZPIS"/"ROZPIS_2"/...atd
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "ROZPIS")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "ROZPIS";
                    //TODO vyřešit názvy listů a zápis do správných listů (zobrazení názvů při výběru který list se má použít)!
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "ROZPIS" + Convert.ToString(list);
            }

            range = worksheet.UsedRange;

            Pes temp;
            //Pes temp2;
            Int32 pocet_psu = dataP.Length();
            String pomoc_cislo;

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();
            (range.Cells[1, 2] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();
            (range.Cells[1, 3] as Excel.Range).Value2 = "Dečka";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();
            (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();
            (range.Cells[1, 5] as Excel.Range).Value2 = "Jméno psa";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();

            List<int> dvojice = dataP.GetAllDvojice();
            int j = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByDvojice(x, "červená");
                if (temp == null)
                    continue;
                (range.Cells[j + 1, 1] as Excel.Range).Value2 = temp.dvojice0;
                (range.Cells[j + 1, 1] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 2] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[j + 1, 2] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 3] as Excel.Range).Value2 = temp.barva0;
                (range.Cells[j + 1, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[j + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[j + 1, 4] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 5] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[j + 1, 5] as Excel.Range).BorderAround2();

                ++j;

                temp = dataP.GetPesByDvojice(x, "bílá");
                if (temp == null)
                    continue;
                (range.Cells[j + 1, 1] as Excel.Range).Value2 = temp.dvojice0;
                (range.Cells[j + 1, 1] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 2] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[j + 1, 2] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 3] as Excel.Range).Value2 = temp.barva0;
                (range.Cells[j + 1, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[j + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[j + 1, 4] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 5] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[j + 1, 5] as Excel.Range).BorderAround2();

                ++j;
            }

            worksheet.Columns.AutoFit();
            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }


        /// <summary>
        /// Zapíše rozpis prvního kola do tabulky excel.
        /// </summary>
        public void write_to_excel(Psi dataP, string sourceFile,  Int32 list)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "ROZPIS")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "ROZPIS" + Convert.ToString(list);
                    //TODO vyřešit názvy listů a zápis do správných listů (zobrazení názvů při výběru který list se má použít)!
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "ROZPIS" + Convert.ToString(list);
            }

            range = worksheet.UsedRange;

            Pes temp;
            //Pes temp2;
            Int32 pocet_psu = dataP.Length();
            String pomoc_cislo;

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();
            (range.Cells[1, 2] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();
            (range.Cells[1, 3] as Excel.Range).Value2 = "Dečka";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();
            (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();
            (range.Cells[1, 5] as Excel.Range).Value2 = "Jméno psa";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();

            List<int> dvojice = dataP.GetAllDvojice();
            int j = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByDvojice(x, "červená");
                if (temp == null)
                    continue;
                (range.Cells[j + 1, 1] as Excel.Range).Value2 = temp.dvojice0;
                (range.Cells[j + 1, 1] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 2] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[j + 1, 2] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 3] as Excel.Range).Value2 = temp.barva0;
                (range.Cells[j + 1, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[j + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[j + 1, 4] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 5] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[j + 1, 5] as Excel.Range).BorderAround2();

                ++j;

                temp = dataP.GetPesByDvojice(x, "bílá");
                if (temp == null)
                    continue;
                (range.Cells[j + 1, 1] as Excel.Range).Value2 = temp.dvojice0;
                (range.Cells[j + 1, 1] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 2] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[j + 1, 2] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 3] as Excel.Range).Value2 = temp.barva0;
                (range.Cells[j + 1, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[j + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[j + 1, 4] as Excel.Range).BorderAround2();
                (range.Cells[j + 1, 5] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[j + 1, 5] as Excel.Range).BorderAround2();

                ++j;
            }

            worksheet.Columns.AutoFit();
            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }

        /// <summary>
        /// Zapíše rozpis druhého kola do tabulky excel.
        /// </summary>
        /// <param name="dataP"></param>
        /// <param name="sourceFile"></param>
        /// <param name="list"></param>
        public void write_to_excel_2(Psi dataP, string sourceFile, Int32 list)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "ROZPIS_2")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "ROZPIS_2";
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "ROZPIS_2";
            }

            range = worksheet.UsedRange;

            Pes temp;
            String pomoc_cislo;
            Int32 pocet_psu = dataP.Length();

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();
            (range.Cells[1, 2] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();
            (range.Cells[1, 3] as Excel.Range).Value2 = "Dečka";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();
            (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();
            (range.Cells[1, 5] as Excel.Range).Value2 = "Jméno psa";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();
            
            List<int> dvojice = dataP.GetAllDvojice_1();
            int i = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByDvojice_1(x, "červená");
                if (temp == null)
                    continue;
                (range.Cells[i + 1, 1] as Excel.Range).Value2 = temp.dvojice1;
                (range.Cells[i + 1, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + 1, 2] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + 1, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + 1, 3] as Excel.Range).Value2 = temp.barva1;
                (range.Cells[i + 1, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + 1, 4] as Excel.Range).BorderAround2();
                (range.Cells[i + 1, 5] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[i + 1, 5] as Excel.Range).BorderAround2();
                ++i;
                temp = dataP.GetPesByDvojice_1(x, "bílá");
                if (temp == null)
                    continue;
                (range.Cells[i + 1, 1] as Excel.Range).Value2 = temp.dvojice1;
                (range.Cells[i + 1, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + 1, 2] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + 1, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + 1, 3] as Excel.Range).Value2 = temp.barva1;
                (range.Cells[i + 1, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + 1, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + 1, 4] as Excel.Range).BorderAround2();
                (range.Cells[i + 1, 5] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[i + 1, 5] as Excel.Range).BorderAround2();
                ++i;

            }

            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }

        /// <summary>
        /// Zapíše do excelu tabulky pro rozhodčí.
        /// </summary>
        /// <param name="dataP"></param>
        /// <param name="sourceFile"></param>
        /// <param name="list"></param>
        public void write_to_excel_rozhodci(Psi dataP, string sourceFile, Int32 list)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "TABULKY")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "TABULKY";
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "TABULKY";
            }

            range = worksheet.UsedRange;

            Pes temp;
            String pomoc_cislo;
            Int32 pocet_psu = dataP.Length();
            Int32 n = 1;

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();

            (range.Cells[1, 2] as Excel.Range).Value2 = "Dečka";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();

            (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();

            (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();

            (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();

            (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
            (range.Cells[1, 6] as Excel.Range).BorderAround2();

            (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
            (range.Cells[1, 7] as Excel.Range).BorderAround2();

            (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
            (range.Cells[1, 8] as Excel.Range).BorderAround2();

            (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";
            (range.Cells[1, 9] as Excel.Range).BorderAround2();

            (range.Cells[1, 10] as Excel.Range).Value2 = "Součet";
            (range.Cells[1, 10] as Excel.Range).BorderAround2();
            
            List<int> dvojice = dataP.GetAllDvojice();
            int i = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByDvojice(x, "červená");
                if (temp == null)
                    continue;

                (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.dvojice0;
                (range.Cells[i + n, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.barva0;
                (range.Cells[i + n, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + n, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + n, 4] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 5] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 6] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 7] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 8] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 9] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 10] as Excel.Range).BorderAround2();
                ++i;

                temp = dataP.GetPesByDvojice(x, "bílá");
                if (temp == null)
                {
                    ++n;
                    continue;
                }

                (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.dvojice0;
                (range.Cells[i + n, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.barva0;
                (range.Cells[i + n, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + n, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + n, 4] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 5] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 6] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 7] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 8] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 9] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 10] as Excel.Range).BorderAround2();
                ++i;
                ++n;
            }

            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }

        /// <summary>
        /// Zapíše do excelu tabulky pro rozhodčí na druhý běh.
        /// </summary>
        /// <param name="dataP"></param>
        /// <param name="sourceFile"></param>
        /// <param name="list"></param>
        public void write_to_excel_rozhodci_2(Psi dataP, string sourceFile, Int32 list)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "TABULKY_2")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "TABULKY_2";
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "TABULKY_2";
            }

            range = worksheet.UsedRange;

            Pes temp;
            String pomoc_cislo;
            Int32 pocet_psu = dataP.Length();
            Int32 n = 1;

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.běhu";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();

            (range.Cells[1, 2] as Excel.Range).Value2 = "Dečka";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();

            (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();

            (range.Cells[1, 4] as Excel.Range).Value2 = "Číslo psa";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();

            (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();

            (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
            (range.Cells[1, 6] as Excel.Range).BorderAround2();

            (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
            (range.Cells[1, 7] as Excel.Range).BorderAround2();

            (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
            (range.Cells[1, 8] as Excel.Range).BorderAround2();

            (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";
            (range.Cells[1, 9] as Excel.Range).BorderAround2();

            (range.Cells[1, 10] as Excel.Range).Value2 = "Součet";
            (range.Cells[1, 10] as Excel.Range).BorderAround2();
            
            List<int> dvojice = dataP.GetAllDvojice_1();
            int i = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByDvojice_1(x, "červená");
                if (temp == null)
                    continue;

                (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.dvojice1;
                (range.Cells[i + n, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.barva1;
                (range.Cells[i + n, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + n, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + n, 4] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 5] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 6] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 7] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 8] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 9] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 10] as Excel.Range).BorderAround2();
                ++i;

                temp = dataP.GetPesByDvojice_1(x, "bílá");
                if (temp == null)
                {
                    ++n;
                    continue;
                }

                (range.Cells[i + n, 1] as Excel.Range).Value2 = temp.dvojice1;
                (range.Cells[i + n, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.barva1;
                (range.Cells[i + n, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + n, 3] as Excel.Range).BorderAround2();
                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + n, 4] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + n, 4] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 5] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 6] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 7] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 8] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 9] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 10] as Excel.Range).BorderAround2();
                ++i;
                ++n;
            }

            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }

        /// <summary>
        /// Zapíše do excelu výsledky prvního kola.
        /// </summary>
        /// <param name="dataP"></param>
        /// <param name="sourceFile"></param>
        /// <param name="list"></param>
        public void write_to_excel_vysledky(Psi dataP, string sourceFile, Int32 list)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "VYSLEDKY")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "VYSLEDKY";
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "VYSLEDKY";
            }

            range = worksheet.UsedRange;

            Pes temp;
            String pomoc_cislo;
            Int32 pocet_psu = dataP.Length();
            Int32 n = 1;

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.psa";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();

            (range.Cells[1, 2] as Excel.Range).Value2 = "Jméno psa";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();

            (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();

            (range.Cells[1, 4] as Excel.Range).Value2 = "Pohlaví";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();

            // první rozhodčí

            (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();

            (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
            (range.Cells[1, 6] as Excel.Range).BorderAround2();

            (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
            (range.Cells[1, 7] as Excel.Range).BorderAround2();

            (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
            (range.Cells[1, 8] as Excel.Range).BorderAround2();

            (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";
            (range.Cells[1, 9] as Excel.Range).BorderAround2();

            // druhý rozhodčí

            (range.Cells[1, 10] as Excel.Range).Value2 = "Obratnost";
            (range.Cells[1, 10] as Excel.Range).BorderAround2();

            (range.Cells[1, 11] as Excel.Range).Value2 = "Rychlost";
            (range.Cells[1, 11] as Excel.Range).BorderAround2();

            (range.Cells[1, 12] as Excel.Range).Value2 = "Vytrvalost";
            (range.Cells[1, 12] as Excel.Range).BorderAround2();

            (range.Cells[1, 13] as Excel.Range).Value2 = "Úsilí";
            (range.Cells[1, 13] as Excel.Range).BorderAround2();

            (range.Cells[1, 14] as Excel.Range).Value2 = "Inteligence";
            (range.Cells[1, 14] as Excel.Range).BorderAround2();

            (range.Cells[1, 15] as Excel.Range).Value2 = "Součet";
            (range.Cells[1, 15] as Excel.Range).BorderAround2();

            List<int> dvojice = dataP.GetAllStartList();
            int i = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByStartNo(x);
                if (temp == null)
                    continue;

                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + n, 1] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + n, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[i + n, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + n, 3] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 4] as Excel.Range).Value2 = temp.pohlavi;
                (range.Cells[i + n, 4] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 5] as Excel.Range).Value2 = temp.agility_A0;
                (range.Cells[i + n, 5] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 6] as Excel.Range).Value2 = temp.speed_A0;
                (range.Cells[i + n, 6] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 7] as Excel.Range).Value2 = temp.endurance_A0;
                (range.Cells[i + n, 7] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 8] as Excel.Range).Value2 = temp.enthusiasm_A0;
                (range.Cells[i + n, 8] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 9] as Excel.Range).Value2 = temp.intelligence_A0;
                (range.Cells[i + n, 9] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 10] as Excel.Range).Value2 = temp.agility_A1;
                (range.Cells[i + n, 10] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 11] as Excel.Range).Value2 = temp.speed_A1;
                (range.Cells[i + n, 11] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 12] as Excel.Range).Value2 = temp.endurance_A1;
                (range.Cells[i + n, 12] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 13] as Excel.Range).Value2 = temp.enthusiasm_A1;
                (range.Cells[i + n, 13] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 14] as Excel.Range).Value2 = temp.intelligence_A1;
                (range.Cells[i + n, 14] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 15] as Excel.Range).Value2 = temp.body1;
                (range.Cells[i + n, 15] as Excel.Range).BorderAround2();
                ++i;
            }

            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }

        /// <summary>
        /// Zapíše do excelu výsledky druhého kola (celkové výsledky).
        /// </summary>
        /// <param name="dataP"></param>
        /// <param name="sourceFile"></param>
        /// <param name="list"></param>
        public void write_to_excel_vysledky_2(Psi dataP, string sourceFile, Int32 list)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook;
            Excel.Worksheet worksheet;
            Excel.Range range;
            workbook = excelApp.Workbooks.Open(sourceFile);
            if (list <= workbook.Sheets.Count)
            {
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(list);

                if (worksheet.Name != "VYSLEDKY_2")
                {
                    workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                    worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                    worksheet.Name = "TABULKY_2";
                }
                else
                {
                    worksheet.Cells.Clear();
                }
            }
            else
            {
                // MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);

                workbook.Sheets.Add(Type.Missing, workbook.Sheets[workbook.Sheets.Count], Type.Missing, Type.Missing);
                worksheet = (Excel.Worksheet)workbook.Sheets.get_Item(workbook.Sheets.Count);
                worksheet.Name = "VYSLEDKY_2";
            }

            range = worksheet.UsedRange;

            Pes temp;
            String pomoc_cislo;
            Int32 pocet_psu = dataP.Length();
            Int32 n = 1;

            (range.Cells[1, 1] as Excel.Range).Value2 = "Č.psa";
            (range.Cells[1, 1] as Excel.Range).BorderAround2();

            (range.Cells[1, 2] as Excel.Range).Value2 = "Jméno psa";
            (range.Cells[1, 2] as Excel.Range).BorderAround2();

            (range.Cells[1, 3] as Excel.Range).Value2 = "Plemeno";
            (range.Cells[1, 3] as Excel.Range).BorderAround2();

            (range.Cells[1, 4] as Excel.Range).Value2 = "Pohlaví";
            (range.Cells[1, 4] as Excel.Range).BorderAround2();

            // první rozhodčí

            (range.Cells[1, 5] as Excel.Range).Value2 = "Obratnost";
            (range.Cells[1, 5] as Excel.Range).BorderAround2();

            (range.Cells[1, 6] as Excel.Range).Value2 = "Rychlost";
            (range.Cells[1, 6] as Excel.Range).BorderAround2();

            (range.Cells[1, 7] as Excel.Range).Value2 = "Vytrvalost";
            (range.Cells[1, 7] as Excel.Range).BorderAround2();

            (range.Cells[1, 8] as Excel.Range).Value2 = "Úsilí";
            (range.Cells[1, 8] as Excel.Range).BorderAround2();

            (range.Cells[1, 9] as Excel.Range).Value2 = "Inteligence";
            (range.Cells[1, 9] as Excel.Range).BorderAround2();

            // druhý rozhodčí

            (range.Cells[1, 10] as Excel.Range).Value2 = "Obratnost";
            (range.Cells[1, 10] as Excel.Range).BorderAround2();

            (range.Cells[1, 11] as Excel.Range).Value2 = "Rychlost";
            (range.Cells[1, 11] as Excel.Range).BorderAround2();

            (range.Cells[1, 12] as Excel.Range).Value2 = "Vytrvalost";
            (range.Cells[1, 12] as Excel.Range).BorderAround2();

            (range.Cells[1, 13] as Excel.Range).Value2 = "Úsilí";
            (range.Cells[1, 13] as Excel.Range).BorderAround2();

            (range.Cells[1, 14] as Excel.Range).Value2 = "Inteligence";
            (range.Cells[1, 14] as Excel.Range).BorderAround2();

            (range.Cells[1, 15] as Excel.Range).Value2 = "Součet";
            (range.Cells[1, 15] as Excel.Range).BorderAround2();

            (range.Cells[1, 16] as Excel.Range).Value2 = "Celkový součet";
            (range.Cells[1, 16] as Excel.Range).BorderAround2();

            (range.Cells[1, 17] as Excel.Range).Value2 = "Umístění ve skupině";
            (range.Cells[1, 17] as Excel.Range).BorderAround2();

            List<int> dvojice = dataP.GetAllStartList();
            int i = 1;
            foreach (int x in dvojice)
            {
                temp = dataP.GetPesByStartNo(x);
                if (temp == null)
                    continue;

                if (temp.zavod_licence == "Licence")
                {
                    pomoc_cislo = "L " + Convert.ToString(temp.start_beh1);
                }
                else
                {
                    pomoc_cislo = Convert.ToString(temp.start_beh1);
                }
                (range.Cells[i + n, 1] as Excel.Range).Value2 = pomoc_cislo;
                (range.Cells[i + n, 1] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 2] as Excel.Range).Value2 = temp.jmeno;
                (range.Cells[i + n, 2] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 3] as Excel.Range).Value2 = temp.plemeno;
                (range.Cells[i + n, 3] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 4] as Excel.Range).Value2 = temp.pohlavi;
                (range.Cells[i + n, 4] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 5] as Excel.Range).Value2 = temp.agility_B0;
                (range.Cells[i + n, 5] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 6] as Excel.Range).Value2 = temp.speed_B0;
                (range.Cells[i + n, 6] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 7] as Excel.Range).Value2 = temp.endurance_B0;
                (range.Cells[i + n, 7] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 8] as Excel.Range).Value2 = temp.enthusiasm_B0;
                (range.Cells[i + n, 8] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 9] as Excel.Range).Value2 = temp.intelligence_B0;
                (range.Cells[i + n, 9] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 10] as Excel.Range).Value2 = temp.agility_B1;
                (range.Cells[i + n, 10] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 11] as Excel.Range).Value2 = temp.speed_B1;
                (range.Cells[i + n, 11] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 12] as Excel.Range).Value2 = temp.endurance_B1;
                (range.Cells[i + n, 12] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 13] as Excel.Range).Value2 = temp.enthusiasm_B1;
                (range.Cells[i + n, 13] as Excel.Range).BorderAround2();
                (range.Cells[i + n, 14] as Excel.Range).Value2 = temp.intelligence_B1;
                (range.Cells[i + n, 14] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 15] as Excel.Range).Value2 = temp.body2;
                (range.Cells[i + n, 15] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 16] as Excel.Range).Value2 = temp.body1 + temp.body2;
                (range.Cells[i + n, 16] as Excel.Range).BorderAround2();

                (range.Cells[i + n, 17] as Excel.Range).Value2 = temp.skupina;
                (range.Cells[i + n, 17] as Excel.Range).BorderAround2();
                ++i;
            }

            workbook.Close(true, Missing.Value, Missing.Value);
            excelApp.Quit();
        }
    }
}
