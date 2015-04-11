using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Text.RegularExpressions;
using projectModel;

// TODO: algoritmus pro třídění psů
        // LUT - tabulka plemen a jejich rozdělení - hrubé roztřídění podle plemen a pohlaví?!?
        // určit rozdělení do dvojic a/nebo trojic (lichý/sudý počet závodníků!!! pravidla-závod/licence/trening?!?)
// TODO: rozdělit chrty a nechrty (volba na startu programu!?) parametr v hlavičce XML souboru !!!

namespace projectRun
{
    /// <summary>
    /// Globální konstanty
    /// </summary>
    static class Konstanty
    {   
        // filtr souborů XML
        public const string fileFilterXML = "Soubory XML (*.xml)|*.xml|Všechny soubory (*.*)|*.*";
        // filtr souborů EXCEL
        public const string fileFilterXLS = "Soubory EXCEL (*.xlsx; *.xlsm; *.xls)|*.xlsx; *.xlsm; *.xls|Všechny soubory (*.*)|*.*";
    }

    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _hodId;
        public int Hod_id { get { return _hodId; } set { _hodId = value; MessageBox.Show(value + ""); } }
        // databáze majitelů
        private Majitele database;
        // databáze psů
        private Psi database_P;
        // instance XML databáze
        private XML_Database XmlDatabase;
        // instance EXCEL databáze
        private EXCEL_Database xlsDB;
        // proměnná pro uchování výběru EXCEL souboru
        private String opened_file = null;

        // seznam ohodnocených psů
        private List<int> ListHodnocenychPsu = new List<int>();

        // příznak seřazení psů 0 neseřezeno, 1 seřazeno první běh, 2 seřazeno druhý běh
        int serazeni_psu = 1;
   
        /// <summary>
        /// Inicializace hlavního okna
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);

            // zobrazení hlavní databáze a skrytí ostatních objektů
            show_gridA();
            hide_gridB();
            hide_gridC();
        }

        /// <summary>
        /// Zeptat se na uložení před ukončením programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, EventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Přejete si před ukončením uložit databázi?", "Konec", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                const string strFilter = "Soubory XML (*.xml)|" + "*.xml|Všechny soubory (*.*)|*.*";
                dlg.Filter = strFilter;

                if (XmlDatabase != null)
                {
                    if ((bool)dlg.ShowDialog(this))
                        if (XmlDatabase.save_XML(this.database, this.database_P, dlg.FileName) != true)
                            MessageBox.Show("Chyba při ukládání databáze.");
                }
                else
                    MessageBox.Show("Není vytvořena žádná databáze.");
            }
        }

        /// <summary>
        /// Funkce tlačítka pro načtení databáze z XML souboru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.database = new Majitele();
            this.database_P = new Psi();
            this.XmlDatabase = new XML_Database();

            // inicializace dialogového okna pro otevření souboru
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // aplikace globálního filtru pro XML soubory
            dlg.Filter = Konstanty.fileFilterXML;

            // pokud se podařilo vybrat soubor, načti databázi
            if ((bool)dlg.ShowDialog(this))
            {
                if (this.XmlDatabase.load_XML(this.database, this.database_P, dlg.FileName))
                    inicializace_ohodnocenych_psu_xml();
                else
                    // pokud se nepodařilo načíst databázi nahlaš chybu
                    MessageBox.Show("Nepodařilo se načíst databázový soubor!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // obnovení zobrazených dat
            ReloadData();
        }

        /// <summary>
        /// Funkce pro načtení databáze (majitelů i psů) z EXCEL tabulky.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadExcel_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // inicializace dialogového okna pro otevření souboru
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                // aplikace globálního filtru pro EXCEL soubory
                dlg.Filter = Konstanty.fileFilterXLS;


                if ((bool)dlg.ShowDialog(this))
                {
                    // uložení otevřeného souboru
                    opened_file = dlg.FileName;

                    this.xlsDB = new EXCEL_Database();

                    List<String> lists = new List<String>();
                    xlsDB.read_excel_lists(opened_file, lists);
                    //zobrazí dialogové okno pro vybrání listu tabulky Excel
                    projectRun.dialog_excel_list list_dialog = new projectRun.dialog_excel_list("Který list tabulky si přeješ načíst?", lists);
                    list_dialog.ShowDialog();

                    if (list_dialog.DialogResult == true)
                    {
                        Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                        // zobrazit vyčkávací kurzor (operace načítání z Excelu je zdlouhavá)
                        using (new WaitCursor())
                        {
                            this.database = new Majitele();
                            this.database_P = new Psi();
                            this.XmlDatabase = new XML_Database();
                            // načtení Excel dat do databáze
                            xlsDB.read_excel_table_temp(database, database_P, opened_file, cislo_listu);
                        }
                    }
                    // obnovení zobrazených dat
                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Zapsání rozpisu do Excelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_to_Excel(object sender, RoutedEventArgs e) // TODO: odladit ukládání do excelu, vyřešit vložení na A4
        {
            try
            {
                // pokud databáze existuje
                if (database_P != null)
                {
                    // inicializace dialogového okna pro otevření souboru
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    // aplikace globálního filtru pro Excel soubory
                    dlg.Filter = Konstanty.fileFilterXLS;
                    // pokud je načteno
                    if ((bool)dlg.ShowDialog(this))
                    {
                        // instance Excel databáze
                        this.xlsDB = new EXCEL_Database();
                        // ulož otevřený soubor do paměti
                        opened_file = dlg.FileName;

                        List<String> lists = new List<String>();
                        xlsDB.read_excel_lists(opened_file, lists);

                        //zobrazí dialogové okno pro vybrání listu tabulky Excel
                        projectRun.dialog_excel_addlist list_dialog = new projectRun.dialog_excel_addlist("Do kterého listu si přeješ zapsat data?", lists);
                        list_dialog.ShowDialog();
                        if (list_dialog.DialogResult == true)
                        {
                            Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                            // vyčkávací kurzor (zdlouhavá operace)
                            using (new WaitCursor())
                            {
                                // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                //xlsDB.AddData2Excel(database_P, opened_file, 1);
                                xlsDB.Write_to_excel(database_P, opened_file, cislo_listu,"ROZPIS");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO řešit výjimky !!!
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zapsání rozpisu druhého běhu do Excelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_to_Excel_2(object sender, RoutedEventArgs e) // TODO: odladit ukládání do excelu, vyřešit vložení na A4
        {
            try
            {
                // instance Excel databáze
                this.xlsDB = new EXCEL_Database();

                // pokud databáze existuje
                if (database_P != null)
                {
                    // pokud nebyl otevřen ještě žádný Excel, vytvoř dialogové okno a načti soubor
                    if (opened_file == null)
                    {
                        // inicializace dialogového okna pro otevření souboru
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        // aplikace globálního filtru pro Excel soubory
                        dlg.Filter = Konstanty.fileFilterXLS;
                        // pokud je načteno
                        if ((bool)dlg.ShowDialog(this))
                        {
                            // ulož otevřený soubor do paměti
                            opened_file = dlg.FileName;

                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor (zdlouhavá operace)
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    //xlsDB.AddData2Excel(database_P, opened_file, 1);
                                    xlsDB.write_to_excel_2(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                    }
                    else
                    {   // opened_file existuje, již byl soubor načten
                        MessageBoxResult res = MessageBox.Show("Tabulka Excel je otevřena.\n\n ANO - data budou uložena do stávájící tabulky\nNE - data budou uložena do jiné existující tabulky", "Uložit/Uložit jako?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor, zdlouhavá operace
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    xlsDB.write_to_excel_2(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                        else
                        {
                            // inicializace dialogového okna pro otevření souboru
                            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                            // aplikace globálního filtru pro Excel soubory
                            dlg.Filter = Konstanty.fileFilterXLS;
                            // pokud je načteno
                            if ((bool)dlg.ShowDialog(this))
                            {
                                //zobrazí dialogové okno pro vybrání listu tabulky Excel
                                projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                                list_dialog.ShowDialog();
                                if (list_dialog.DialogResult == true)
                                {
                                    Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                    // vyčkávací kurzor, zdlouhavá operace
                                    using (new WaitCursor())
                                    {
                                        // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                        xlsDB.write_to_excel_2(database_P, dlg.FileName, cislo_listu);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO řešit výjimky !!!
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zapsání tabulek pro rozhodčí do Excelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_tab_to_Excel(object sender, RoutedEventArgs e)
        {
            try
            {
                // instance Excel databáze
                this.xlsDB = new EXCEL_Database();

                // pokud databáze existuje
                if (database_P != null)
                {
                    // pokud nebyl otevřen ještě žádný Excel, vytvoř dialogové okno a načti soubor
                    if (opened_file == null)
                    {
                        // inicializace dialogového okna pro otevření souboru
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        // aplikace globálního filtru pro Excel soubory
                        dlg.Filter = Konstanty.fileFilterXLS;
                        // pokud je načteno
                        if ((bool)dlg.ShowDialog(this))
                        {
                            // ulož otevřený soubor do paměti
                            opened_file = dlg.FileName;

                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor (zdlouhavá operace)
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    //xlsDB.AddData2Excel(database_P, opened_file, 1);
                                    xlsDB.write_to_excel_rozhodci(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                    }
                    else
                    {   // opened_file existuje, již byl soubor načten
                        MessageBoxResult res = MessageBox.Show("Tabulka Excel je otevřena.\n\n ANO - data budou uložena do stávájící tabulky\nNE - data budou uložena do jiné existující tabulky", "Uložit/Uložit jako?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor, zdlouhavá operace
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    xlsDB.write_to_excel_rozhodci(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                        else
                        {
                            // inicializace dialogového okna pro otevření souboru
                            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                            // aplikace globálního filtru pro Excel soubory
                            dlg.Filter = Konstanty.fileFilterXLS;
                            // pokud je načteno
                            if ((bool)dlg.ShowDialog(this))
                            {
                                //zobrazí dialogové okno pro vybrání listu tabulky Excel
                                projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                                list_dialog.ShowDialog();
                                if (list_dialog.DialogResult == true)
                                {
                                    Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                    // vyčkávací kurzor, zdlouhavá operace
                                    using (new WaitCursor())
                                    {
                                        // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                        xlsDB.write_to_excel_rozhodci(database_P, dlg.FileName, cislo_listu);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO řešit výjimky !!!
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zapsání tabulek pro rozhodčí do Excelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_tab_to_Excel_2(object sender, RoutedEventArgs e)
        {
            try
            {
                // instance Excel databáze
                this.xlsDB = new EXCEL_Database();

                // pokud databáze existuje
                if (database_P != null)
                {
                    // pokud nebyl otevřen ještě žádný Excel, vytvoř dialogové okno a načti soubor
                    if (opened_file == null)
                    {
                        // inicializace dialogového okna pro otevření souboru
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        // aplikace globálního filtru pro Excel soubory
                        dlg.Filter = Konstanty.fileFilterXLS;
                        // pokud je načteno
                        if ((bool)dlg.ShowDialog(this))
                        {
                            // ulož otevřený soubor do paměti
                            opened_file = dlg.FileName;

                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor (zdlouhavá operace)
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    //xlsDB.AddData2Excel(database_P, opened_file, 1);
                                    xlsDB.write_to_excel_rozhodci_2(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                    }
                    else
                    {   // opened_file existuje, již byl soubor načten
                        MessageBoxResult res = MessageBox.Show("Tabulka Excel je otevřena.\n\n ANO - data budou uložena do stávájící tabulky\nNE - data budou uložena do jiné existující tabulky", "Uložit/Uložit jako?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor, zdlouhavá operace
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    xlsDB.write_to_excel_rozhodci_2(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                        else
                        {
                            // inicializace dialogového okna pro otevření souboru
                            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                            // aplikace globálního filtru pro Excel soubory
                            dlg.Filter = Konstanty.fileFilterXLS;
                            // pokud je načteno
                            if ((bool)dlg.ShowDialog(this))
                            {
                                //zobrazí dialogové okno pro vybrání listu tabulky Excel
                                projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                                list_dialog.ShowDialog();
                                if (list_dialog.DialogResult == true)
                                {
                                    Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                    // vyčkávací kurzor, zdlouhavá operace
                                    using (new WaitCursor())
                                    {
                                        // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                        xlsDB.write_to_excel_rozhodci_2(database_P, dlg.FileName, cislo_listu);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO řešit výjimky !!!
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zapsání výsledků prvního kola do Excelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_to_Excel_vysledky(object sender, RoutedEventArgs e) // TODO: odladit ukládání do excelu, vyřešit vložení na A4
        {
            try
            {
                // instance Excel databáze
                this.xlsDB = new EXCEL_Database();

                // pokud databáze existuje
                if (database_P != null)
                {
                    // pokud nebyl otevřen ještě žádný Excel, vytvoř dialogové okno a načti soubor
                    if (opened_file == null)
                    {
                        // inicializace dialogového okna pro otevření souboru
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        // aplikace globálního filtru pro Excel soubory
                        dlg.Filter = Konstanty.fileFilterXLS;
                        // pokud je načteno
                        if ((bool)dlg.ShowDialog(this))
                        {
                            // ulož otevřený soubor do paměti
                            opened_file = dlg.FileName;

                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor (zdlouhavá operace)
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    //xlsDB.AddData2Excel(database_P, opened_file, 1);
                                    xlsDB.write_to_excel_vysledky(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                    }
                    else
                    {   // opened_file existuje, již byl soubor načten
                        MessageBoxResult res = MessageBox.Show("Tabulka Excel je otevřena.\n\n ANO - data budou uložena do stávájící tabulky\nNE - data budou uložena do jiné existující tabulky", "Uložit/Uložit jako?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor, zdlouhavá operace
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    xlsDB.write_to_excel_vysledky(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                        else
                        {
                            // inicializace dialogového okna pro otevření souboru
                            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                            // aplikace globálního filtru pro Excel soubory
                            dlg.Filter = Konstanty.fileFilterXLS;
                            // pokud je načteno
                            if ((bool)dlg.ShowDialog(this))
                            {
                                //zobrazí dialogové okno pro vybrání listu tabulky Excel
                                projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                                list_dialog.ShowDialog();
                                if (list_dialog.DialogResult == true)
                                {
                                    Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                    // vyčkávací kurzor, zdlouhavá operace
                                    using (new WaitCursor())
                                    {
                                        // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                        xlsDB.write_to_excel_vysledky(database_P, dlg.FileName, cislo_listu);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO řešit výjimky !!!
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Zapsání výsledků druhého kola do Excelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_to_Excel_vysledky_2(object sender, RoutedEventArgs e) // TODO: odladit ukládání do excelu, vyřešit vložení na A4
        {
            try
            {
                // instance Excel databáze
                this.xlsDB = new EXCEL_Database();

                // pokud databáze existuje
                if (database_P != null)
                {
                    // pokud nebyl otevřen ještě žádný Excel, vytvoř dialogové okno a načti soubor
                    if (opened_file == null)
                    {
                        // inicializace dialogového okna pro otevření souboru
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        // aplikace globálního filtru pro Excel soubory
                        dlg.Filter = Konstanty.fileFilterXLS;
                        // pokud je načteno
                        if ((bool)dlg.ShowDialog(this))
                        {
                            // ulož otevřený soubor do paměti
                            opened_file = dlg.FileName;

                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor (zdlouhavá operace)
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    //xlsDB.AddData2Excel(database_P, opened_file, 1);
                                    xlsDB.write_to_excel_vysledky_2(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                    }
                    else
                    {   // opened_file existuje, již byl soubor načten
                        MessageBoxResult res = MessageBox.Show("Tabulka Excel je otevřena.\n\n ANO - data budou uložena do stávájící tabulky\nNE - data budou uložena do jiné existující tabulky", "Uložit/Uložit jako?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            //zobrazí dialogové okno pro vybrání listu tabulky Excel
                            projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                            list_dialog.ShowDialog();
                            if (list_dialog.DialogResult == true)
                            {
                                Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                // vyčkávací kurzor, zdlouhavá operace
                                using (new WaitCursor())
                                {
                                    // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                    xlsDB.write_to_excel_vysledky_2(database_P, opened_file, cislo_listu);
                                }
                            }
                        }
                        else
                        {
                            // inicializace dialogového okna pro otevření souboru
                            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                            // aplikace globálního filtru pro Excel soubory
                            dlg.Filter = Konstanty.fileFilterXLS;
                            // pokud je načteno
                            if ((bool)dlg.ShowDialog(this))
                            {
                                //zobrazí dialogové okno pro vybrání listu tabulky Excel
                                projectRun.Window1 list_dialog = new projectRun.Window1("Do kterého listu si přeješ zapsat data?", new List<String>());
                                list_dialog.ShowDialog();
                                if (list_dialog.DialogResult == true)
                                {
                                    Int32 cislo_listu = list_dialog.list_no.SelectedIndex + 1;

                                    // vyčkávací kurzor, zdlouhavá operace
                                    using (new WaitCursor())
                                    {
                                        // zapiš data do excelu, na pozici opened_file, do zvoleného listu
                                        xlsDB.write_to_excel_vysledky_2(database_P, dlg.FileName, cislo_listu);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // TODO řešit výjimky !!!
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Funkce pro vytvoření nové prázdné databáze.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadNewDatabase(object sender, RoutedEventArgs e)
        {
            this.database = new Majitele();
            this.database_P = new Psi();
            this.XmlDatabase = new XML_Database();
            opened_file = null;
            ReloadData();
        }

        /// <summary>
        /// Načte data majitelů z databáze a zobrazí v tabulce.
        /// </summary>
        private void ReloadMajitelData()
        {
            //číslo sloupce podle kterého je datagrid seřazen (aby bylo seřazení zachováno i po obnovení dat)
            int sorted_column = WhatSortedDG(DGV);
            ListSortDirection sorted_type = new ListSortDirection();
            //pokud je datagrid seřazen, zjistit v jakém smylsu je seřazen (sestupně/vzestupně)
            if (sorted_column != -1)
                sorted_type = HowSortedDG(DGV, sorted_column);

            var items = new List<Majitel>();

            //pole ID všech majitelů v databázi
            int[] majitelId = this.database.GetAllIds();
            
            for (int i = 0; i < majitelId.Length; i++)
            {
                //přidá do Listu items všechny majitele (obnoví tak staré data)
                items.Add(this.database.GetMajitelById(majitelId[i]));
            }
            var grid = DGV;
            grid.ItemsSource = items;

            //znovu seřadí podle uložených parametrů
            if (sorted_column != -1)
                SortDataGrid(DGV, sorted_column, sorted_type);
        }

        /// <summary>
        /// Načte data psů z databáze a zobrazí v tabulce.
        /// </summary>
        private void ReloadPesData()
        {
            //číslo sloupce podle kterého je datagrid seřazen (aby bylo seřazení zachováno i po obnovení dat)
            int sorted_column = WhatSortedDG(DGV_P);
            ListSortDirection sorted_type = new ListSortDirection();
            //pokud je datagrid seřazen, zjistit v jakém smylsu je seřazen (sestupně/vzestupně)
            if (sorted_column != -1)
                sorted_type = HowSortedDG(DGV_P, sorted_column);

            var items = new List<Pes>();

            //pole ID všech psů v databázi
            int[] pesId = this.database_P.GetAllIds();

            for (int i = 0; i < pesId.Length; i++)
            {
                //přidá do Listu items všechny psy (obnoví tak staré data)
                items.Add(this.database_P.GetPesById(pesId[i]));
            }
            var grid = DGV_P;
            grid.ItemsSource = items;

            //znovu seřadí podle uložených parametrů
            if (sorted_column != -1)
                SortDataGrid(DGV_P, sorted_column, sorted_type);
        }

        /// <summary>
        /// Načte data a zobrazí v tabulce přehled psů.
        /// </summary>
        private void skupinGridPesData()
        {
            //číslo sloupce podle kterého je datagrid seřazen (aby bylo seřazení zachováno i po obnovení dat)
            int sorted_column = WhatSortedDG(skupinGrid);
            ListSortDirection sorted_type = new ListSortDirection();
            //pokud je datagrid seřazen, zjistit v jakém smylsu je seřazen (sestupně/vzestupně)
            if (sorted_column != -1)
                sorted_type = HowSortedDG(skupinGrid, sorted_column);

            var items = new List<Pes>();

            //pole ID všech psů v databázi
            int[] pesId = this.database_P.GetAllIds();

            for (int i = 0; i < pesId.Length; i++)
            {
                //přidá do Listu items všechny psy (obnoví tak staré data)
                items.Add(this.database_P.GetPesById(pesId[i]));
            }
            
            var grid = skupinGrid;
            grid.ItemsSource = items;

            //znovu seřadí podle uložených parametrů
            if (sorted_column != -1)
                SortDataGrid(skupinGrid, sorted_column, sorted_type);
        }

        /// <summary>
        /// Obnoví data v tabulkách, stavový řádek a zobrazí hlavní tabulku gridA.
        /// </summary>
        private void ReloadData()
        {
            this.ReloadMajitelData();
            this.ReloadPesData();
            this.stav();
            show_gridA();
            hide_gridB();
            hide_gridC();
        }

        /// <summary>
        /// Uloží databazi do XML souboru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_OnClick(object sender, RoutedEventArgs e)
        {
            // inicializace dialogového okna pro ukládání souboru
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            // aplikace globálního filtru pro XML soubory
            dlg.Filter = Konstanty.fileFilterXML;

            if (XmlDatabase != null)
                if ((bool)dlg.ShowDialog(this))
                    if (this.XmlDatabase.save_XML(this.database, this.database_P, dlg.FileName) != true)
                        MessageBox.Show("Chyba při ukládání");
        }

        /// <summary>
        /// Zobrazí formulář pro přidání nového majitele.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newMajite_click(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
                this.divNewMajitel.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Zobrazí formulář pro přidání nového psa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newPes_OnClick(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
                this.divNewPes.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Zobrazí formulář pro přidání nového psa pro zvoleného majitele.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void add_pes_majitele(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                Int32 selectedMajitelIndex = DGV.SelectedIndex;
                if (selectedMajitelIndex != -1)
                {
                    this.divNewPes.Visibility = System.Windows.Visibility.Visible;
                    int[] majitelId = this.database.GetAllIds();
                    this.boxMajitel.Text = Convert.ToString(majitelId[selectedMajitelIndex]);
                }
                else
                    MessageBox.Show("Není vybrán žádný majitel.\nMajitele vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            
            }
        }

        /// <summary>
        /// Zobrazí formulář pro úpravu majitele a vyplní data podle zvoleného majitele.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editMajitel_click(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (DGV.SelectedIndex != -1)
                {
                    this.divNewMajitel.Visibility = System.Windows.Visibility.Visible;

                    this.boxId.Text = Convert.ToString(((Majitel)DGV.SelectedItem).id);
                    this.boxFirstName.Text = ((Majitel)DGV.SelectedItem).firstName;
                    this.boxLastName.Text = ((Majitel)DGV.SelectedItem).lastName;
                    if (((Majitel)DGV.SelectedItem).clen == "ano")
                        this.clenCB.IsChecked = true;
                    this.platbaTB.Text = Convert.ToString(((Majitel)DGV.SelectedItem).penize);
                    if (((Majitel)DGV.SelectedItem).potvrzeni == "OK")
                        this.platba_OK_CB.IsChecked = true;
                    this.telefonBox.Text = ((Majitel)DGV.SelectedItem).telefon;
                    this.emailBox.Text = ((Majitel)DGV.SelectedItem).email;
                    this.boxNarod.Text = ((Majitel)DGV.SelectedItem).narodnost;
                    this.psiTB.Text = Convert.ToString(((Majitel)DGV.SelectedItem).pocet_psu);
                }
                else
                    MessageBox.Show("Není vybrán žádný majitel.\nMajitele vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Zobrazí formulář pro úpravu psa a vyplní data podle zvoleného psa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editPes_OnClick(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (DGV_P.SelectedIndex != -1)
                {
                    this.hlavniGrid(sender, e);

                    this.divNewPes.Visibility = System.Windows.Visibility.Visible;

                    this.boxId_P.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).id);

                    // rychlo oprava...
                    this.boxstart_P.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).start_beh1);
                    this.boxstart_P.Focus();
                    this.boxstart_P.SelectAll();
                    this.boxBeh.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).dvojice0);

                    this.boxFirstName_P.Text = ((Pes)DGV_P.SelectedItem).jmeno;
                    this.comboPlemeno.Text = ((Pes)DGV_P.SelectedItem).plemeno;
                    this.fciCombo.Text = ((Pes)DGV_P.SelectedItem).fci;
                    this.boxLicence.Text = ((Pes)DGV_P.SelectedItem).licence;
                    this.pohlaviCB.Text = ((Pes)DGV_P.SelectedItem).pohlavi;
                    this.datumPicker.SelectedDate = ((Pes)DGV_P.SelectedItem).datum;
                    this.boxMajitel.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).majitel);
                    if (((Pes)DGV_P.SelectedItem).zavod_licence == "Závod")
                        this.zavodCB.SelectedIndex = 0;
                    else if (((Pes)DGV_P.SelectedItem).zavod_licence == "Licence")
                        this.zavodCB.SelectedIndex = 1;
                    else
                        this.zavodCB.SelectedIndex = 2;

                    this.boxPlat.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).platba);
                    this.boxDoplat.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).doplatit);
                    this.boxPozn.Text = ((Pes)DGV_P.SelectedItem).poznamka;
                }
                else
                    MessageBox.Show("Není vybrán žádný pes.\nPsa vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Zobrazí formulář pro úpravu psa v přehledu psů a vyplní data podle zvoleného psa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editPesprehled(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (skupinGrid.SelectedIndex != -1)
                {
                    this.edit_skup(sender, e);

                    this.diveditprehled.Visibility = System.Windows.Visibility.Visible;

                    this.boxbeh1.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).dvojice0);
                    this.boxstart1.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).start_beh1);
                    this.decka1CB.Text = ((Pes)skupinGrid.SelectedItem).barva0;

                    this.boxbeh2.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).dvojice1);
                    this.boxstart2.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).start_beh2);
                    this.decka2CB.Text = ((Pes)skupinGrid.SelectedItem).barva1;

                    //this.hlavniGrid(sender, e);

                    //this.divNewPes.Visibility = System.Windows.Visibility.Visible;

                    //this.boxId_P.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).id);

                    //// rychlo oprava...
                    //this.boxstart_P.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).start_beh1);
                    //this.boxstart_P.Focus();
                    //this.boxBeh.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).dvojice0);

                    //this.boxFirstName_P.Text = ((Pes)DGV_P.SelectedItem).jmeno;
                    //this.comboPlemeno.Text = ((Pes)DGV_P.SelectedItem).plemeno;
                    //this.fciCombo.Text = ((Pes)DGV_P.SelectedItem).fci;
                    //this.boxLicence.Text = ((Pes)DGV_P.SelectedItem).licence;
                    //this.pohlaviCB.Text = ((Pes)DGV_P.SelectedItem).pohlavi;
                    //this.datumPicker.SelectedDate = ((Pes)DGV_P.SelectedItem).datum;
                    //this.boxMajitel.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).majitel);
                    //if (((Pes)DGV_P.SelectedItem).zavod_licence == "Závod")
                    //    this.zavodCB.SelectedIndex = 0;
                    //else if (((Pes)DGV_P.SelectedItem).zavod_licence == "Licence")
                    //    this.zavodCB.SelectedIndex = 1;
                    //else
                    //    this.zavodCB.SelectedIndex = 2;

                    //this.boxPlat.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).platba);
                    //this.boxDoplat.Text = Convert.ToString(((Pes)DGV_P.SelectedItem).doplatit);
                    //this.boxPozn.Text = ((Pes)DGV_P.SelectedItem).poznamka;
                }
                else
                    MessageBox.Show("Není vybrán žádný pes.\nPsa vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Vybranému psovi přiřadí startovní číslo, aniž by se musela celá databáze generovat znovu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zaradit_psa(object sender, RoutedEventArgs e)
        { /// TODO odladit a vychytat chyby !!!
            if (XmlDatabase != null)
            {
                if (DGV_P.SelectedIndex != -1)
                {
                    if (((Pes)DGV_P.SelectedItem).start_beh1 == 0)
                    {
                        Pes editpes_c = database_P.GetPesById(((Pes)DGV_P.SelectedItem).id);
                        int temp_start_c = 0;
                        int temp_skup_c = 0;
                        int new_dvojice = 0;
                        String new_barva = "červená";
                        List<Pes> skupina_psu = database_P.GetPesByGroup(Int32.Parse(((Pes)DGV_P.SelectedItem).poznamka), null);
                        foreach(Pes temp in skupina_psu)
                        {
                            if (temp.start_beh1 > temp_start_c)
                            {
                                temp_start_c = temp.start_beh1;
                                temp_skup_c = temp.dvojice0;
                            }
                        }
                        foreach(Pes temp in skupina_psu)
                        {
                            if ((temp.dvojice0 == temp_skup_c) && (temp.start_beh1 != temp_start_c))
                            {
                                new_dvojice = temp_skup_c + 1;
                                break;
                            }
                        }

                        if (new_dvojice == 0)
                        {
                            new_dvojice = temp_skup_c;
                        }

                        if (new_dvojice == temp_skup_c)
                        {
                            // poslední dvojice má jen jednoho člena, nový se přídá, zbylí psi se posunou pouze o startovní číslo
                            editpes_c.dvojice0 = new_dvojice;
                            for (int x = 0; x < database_P.Length(); x++)
                            {
                                Pes editpes = database_P.GetPesByStartNo(x);
                                if (editpes == null)
                                    continue;
                                if (editpes.dvojice0 > new_dvojice)
                                {
                                    editpes.start_beh1 += 1;
                                    new_barva = "bílá";
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            editpes_c.start_beh1 = temp_start_c + 1;
                            editpes_c.barva0 = new_barva;
                        }
                        else
                        {
                            // poslední dvojice je plná, nový pes tak vytvoří novou dvojici, zbylí psi se posunou o startovní číslo i o číslo dvojice
                            editpes_c.dvojice0 = new_dvojice;
                            for (int x = 0; x < database_P.Length(); x++)
                            {
                                Pes editpes = database_P.GetPesByStartNo(x);
                                if (editpes == null)
                                    continue;
                                if (editpes.dvojice0 > new_dvojice)
                                {
                                    editpes.start_beh1 += 1;
                                    editpes.dvojice0 += 1;
                                    new_barva = "červená";
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            editpes_c.start_beh1 = temp_start_c + 1;
                            editpes_c.barva0 = new_barva;
                        }
                        ReloadPesData();
                    }
                    else
                        MessageBox.Show("Je vybrán pes, který má přiřazené startovní číslo.\nZařadit do závodu lze jen pes bez startovního čísla (Číslo psa -> 0).", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                    MessageBox.Show("Není vybrán žádný pes.\nPsa vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Prohodí závodní dvojice dvěma psům, označeným v datagridu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prohodit_dvojice_beh1(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (skupinGrid.SelectedIndex != -1)
                {
                    if (skupinGrid.SelectedItems.Count == 2)
                    {
                        int temp_dvojice = 0;
                        int temp_start = 0;
                        String temp_barva = "";
                        Pes tmppes1 = (Pes)(skupinGrid.SelectedItems[0]);
                        Pes tmppes2 = (Pes)(skupinGrid.SelectedItems[1]);

                        temp_dvojice = tmppes1.dvojice0;
                        tmppes1.dvojice0 = tmppes2.dvojice0;
                        tmppes2.dvojice0 = temp_dvojice;

                        temp_start = tmppes1.start_beh1;
                        tmppes1.start_beh1 = tmppes2.start_beh1;
                        tmppes2.start_beh1 = temp_start;

                        temp_barva = tmppes1.barva0;
                        tmppes1.barva0 = tmppes2.barva0;
                        tmppes2.barva0 = temp_barva;

                        database_P.Edit(tmppes1.id, tmppes1);
                        database_P.Edit(tmppes2.id, tmppes2);
                    }
                    else
                    {
                        // je označen špatný počet psů (musí být 2)
                        MessageBox.Show("Je vybrán nesprávný počet psů.\nVyberte prosím 2 psy které chcete prohodit.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    // není označen žádný pes
                    MessageBox.Show("Není vybrán žádný pes.\nProsím vyberte 2 psy které chcete prohodit.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                //TODO není vytvořena databáze
            }
            this.skupinGridPesData();
        }

        /// <summary>
        /// Prohodí závodní dvojice dvěma psům, označeným v datagridu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prohodit_dvojice_beh2(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (skupinGrid.SelectedIndex != -1)
                {
                    if (skupinGrid.SelectedItems.Count == 2)
                    {
                        int temp_dvojice = 0;
                        int temp_start = 0;
                        String temp_barva = "";

                        Pes tmppes1 = (Pes)(skupinGrid.SelectedItems[0]);
                        Pes tmppes2 = (Pes)(skupinGrid.SelectedItems[1]);

                        temp_dvojice = tmppes1.dvojice1;
                        tmppes1.dvojice1 = tmppes2.dvojice1;
                        tmppes2.dvojice1 = temp_dvojice;

                        temp_start = tmppes1.start_beh2;
                        tmppes1.start_beh2 = tmppes2.start_beh2;
                        tmppes2.start_beh2 = temp_start;

                        temp_barva = tmppes1.barva1;
                        tmppes1.barva1 = tmppes2.barva1;
                        tmppes2.barva1 = temp_barva;

                        database_P.Edit(tmppes1.id, tmppes1);
                        database_P.Edit(tmppes2.id, tmppes2);
                    }
                    else
                    {
                        // je označen špatný počet psů (musí být 2)
                        MessageBox.Show("Je vybrán nesprávný počet psů.\nVyberte prosím 2 psy které chcete prohodit.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    // není označen žádný pes
                    MessageBox.Show("Není vybrán žádný pes.\nProsím vyberte 2 psy které chcete prohodit.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                //TODO není vytvořena databáze
            }
            this.skupinGridPesData();
        }

        /// <summary>
        /// Přidělení startovních čísel náhodně v rámci skupiny pro první běh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serad_psy(object sender, RoutedEventArgs e)
        {
            List<Pes> temppes = new List<Pes>();
            List<int> cisla = new List<int>();
            List<int> skupiny = database_P.GetAllGroups();
            Int32 j = 0;
            Int32 max = 0;
            Int32 pocet_psu = 0;
            Int32 min = 1;
            foreach (int i in skupiny)
            {
                min += pocet_psu;
                temppes = database_P.GetPesByGroup(i,null);

                pocet_psu = temppes.Count;
                max = min + pocet_psu - 1;

                // cyklus pro nalezení diskvalifikovaných psů
                for (j = 0; j < pocet_psu; j++)
                {
                    if (temppes[j].diskval != "---")
                    {
                        --max;
                        temppes[j].start_beh1 = 999;
                        temppes[j].barva0 = "";
                        temppes[j].dvojice0 = 999;
                    }
                }

                cisla = Generator_cisel(min, max);

                int index_cisel = 0;
                //cyklus pro přidělení psů
                for (j = 0; j < pocet_psu; j++)
                {
                    if (temppes[j].diskval == "---")
                    {
                        temppes[j].start_beh1 = cisla[index_cisel++];
                    }
                }
                temppes.Clear();
                cisla.Clear();

            }

            int diskvalifikovani = 0;
            for (int i = 1; i < (database_P.Length() + 1); i++)
            {
                Pes temp1 = database_P.GetPesByStartNo(i);
                while (temp1 == null)
                {
                    temp1 = database_P.GetPesByStartNo(++i);
                    ++diskvalifikovani;
                }
                temp1.start_beh1 = i - diskvalifikovani;
            }

            int pocitadlo = 1;
            for (int i = 1; i < database_P.Length() + 1 - diskvalifikovani; i += 2)
            {
                Pes tmp1 = database_P.GetPesByStartNo(i);
                Pes tmp2 = database_P.GetPesByStartNo(i + 1);

                tmp1.dvojice0 = pocitadlo;
                tmp1.barva0 = "červená";

                if (tmp2 != null)
                {
                    if (tmp1.poznamka == tmp2.poznamka)
                    {
                        tmp2.dvojice0 = pocitadlo++;
                        tmp2.barva0 = "bílá";
                    }
                    else
                    {
                        ++pocitadlo;
                        --i;
                    }
                }
                else
                    break;
            }

            this.ReloadPesData();
            serazeni_psu = 1;
        }

        /// <summary>
        /// Přidělení startovních čísel podle bodů v prvním kole v rámci skupiny pro druhý běh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serad_psy_2(object sender, RoutedEventArgs e)
        {
            List<Pes> temppes = new List<Pes>();
            List<int> skupiny = database_P.GetAllGroups();

            Int32 j = 0;
            Int32 k = 0;

            Int32 temp = 0;

            Int32 pocet_psu = 0;
            Int32 min = 1;

            if ((serazeni_psu == 1) || (serazeni_psu == 2))
            {
                foreach (int i in skupiny)
                {
                    min += pocet_psu;
                    temppes = database_P.GetPesByGroup(i, null);
                    pocet_psu = temppes.Count;

                    for (j = 0; j < pocet_psu; j++)
                    {
                        if (temppes[j].diskval != "---") // pokud je pes diskvalifikován přeskočit jej při řazení
                        {
                            temppes[j].start_beh2 = 999; // psovi se přiřadí startovní číslo 999, bude tedy na konci/vyřaze ze závodu
                            temppes[j].dvojice1 = 999;
                            temppes[j].barva1 = "";
                        }
                        else
                        {
                            temp = 1;
                            for (k = 0; k < pocet_psu; k++)
                            {
                                if ((j == k) || (temppes[k].diskval != "---"))
                                    continue;

                                if (temppes[j].body1 < temppes[k].body1)
                                    temp++;
                                else if (temppes[j].body1 == temppes[k].body1)
                                {
                                    if ((temppes[j].agility_A0 + temppes[j].agility_A1) < (temppes[k].agility_A0 + temppes[k].agility_A1))
                                        temp++;
                                    else if ((temppes[j].agility_A0 + temppes[j].agility_A1) == (temppes[k].agility_A0 + temppes[k].agility_A1))
                                    {
                                        if ((temppes[j].speed_A0 + temppes[j].speed_A1) < (temppes[k].speed_A0 + temppes[k].speed_A1))
                                            temp++;
                                        else if ((temppes[j].speed_A0 + temppes[j].speed_A1) == (temppes[k].speed_A0 + temppes[k].speed_A1))
                                        {
                                            if ((temppes[j].endurance_A0 + temppes[j].endurance_A1) < (temppes[k].endurance_A0 + temppes[k].endurance_A1))
                                                temp++;
                                            else if ((temppes[j].endurance_A0 + temppes[j].endurance_A1) == (temppes[k].endurance_A0 + temppes[k].endurance_A1))
                                            {
                                                if ((temppes[j].enthusiasm_A0 + temppes[j].enthusiasm_A1) < (temppes[k].enthusiasm_A0 + temppes[k].enthusiasm_A1))
                                                    temp++;
                                                else if ((temppes[j].enthusiasm_A0 + temppes[j].enthusiasm_A1) == (temppes[k].enthusiasm_A0 + temppes[k].enthusiasm_A1))
                                                {
                                                    if ((temppes[j].intelligence_A0 + temppes[j].intelligence_A1) < (temppes[k].intelligence_A0 + temppes[k].intelligence_A1))
                                                        temp++;
                                                  //  else 
                                                  //      temp++; // pokud mají absolutně stejný výsledek, ignoruj,mohla by následovat chyba při vyhodnocování, např pšichni psi nulové body...
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            temppes[j].start_beh2 = temp + min - 1;
                        }
                        database_P.Edit(temppes[j].id, new Pes(temppes[j]));
                    }
                    temppes.Clear();
                }

                int diskvalifikovani = 0;
                for (int i = 1; i < (database_P.Length() + 1); i++)
                {
                    Pes temp1 = database_P.GetPesByStartNo_1(i);
                    while (temp1 == null)
                    {
                        temp1 = database_P.GetPesByStartNo_1(++i);
                        ++diskvalifikovani;
                        if (i > database_P.Length())
                        {
                            MessageBox.Show("Chyba seřazení psů!\n\nZkontrolujte prosím hodnocení psů.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            break;
                        }
                    }

                    if (temp1 != null)
                        temp1.start_beh2 = i - diskvalifikovani;
                }

                int pocitadlo = 1;
                for (int i = 1; i < database_P.Length() + 1 - diskvalifikovani; i += 2)
                {
                    Pes tmp1 = database_P.GetPesByStartNo_1(i);
                    Pes tmp2 = database_P.GetPesByStartNo_1(i + 1);

                    tmp1.dvojice1 = pocitadlo;
                    tmp1.barva1 = "červená";

                    if (tmp2 != null)
                    {
                        if (tmp1.poznamka == tmp2.poznamka)
                        {
                            tmp2.dvojice1 = pocitadlo++;
                            tmp2.barva1 = "bílá";
                        }
                        else
                        {
                            ++pocitadlo;
                            --i;
                        }
                    }
                    else
                        break;
                }
                zamichat_dvojice();
                this.ReloadPesData();
                serazeni_psu = 2;
            }
            else
            {
                MessageBox.Show("Psi nejsou seřazeni k prvnímu běhu!\n\nZkontrolujte prosím jejich seřazení.", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Testovací funkce na náhodné promíchání dvojic v druhém kole.
        /// </summary>
        private void zamichat_dvojice()
        {
            List<Pes> temppes = new List<Pes>();
            List<int> skupiny = database_P.GetAllGroups();
            int pocet_psu = 0;

            int x = 0;
            int y = 0;
            int j = 0;
            int k = 0;

            int IDA1 = 0;
            int IDA2 = 0;
            int IDB1 = 0;
            int IDB2 = 0;

            int pocitadlo1 = 0;
            int pocitadlo2 = 0;

            int pomocInt = 0;

            Pes temp1 = new Pes();
            Pes temp2 = new Pes();

            foreach (int i in skupiny)
            {
                temppes = database_P.GetPesByGroup(i, null);
                    pocet_psu = temppes.Count;

                // pokud je míň jak 2 dvojice ve skupině, může se pokračovat do další skupiny
                if (pocet_psu < 4)
                    continue;

                for (j = 0; j < pocet_psu; j++)
                {
                    /*
                     *najít psy ve dvojici a uložit jejich ID do proměnných IDA1 a IDA2
                     *IDA1 - pes s více body
                     *IDA2 - pes s méně body
                     *najít následující dvojici a uložit ID psů do IDB1 a IDB2 (stejně jako IDA)
                     *s pravděpodobností 50% prohodit tyto dvojice (start_beh2 a dvojice1)
                     *v dalším kroku vybírat dvojici podle psa na druhém místě
                     *pokud se nenajde pes do dvojice (lichý počet psů) přeskočit tento cyklus
                     */
                    IDA1 = temppes[j].id;
                    IDA2 = 0;

                    for (x = 0; x < pocet_psu; x++)
                    {
                        if ((temppes[x].dvojice1 == temppes[j].dvojice1) && (x != j))
                        {
                            IDA2 = temppes[x].id;
                            break;
                        }
                    }
                    if (IDA2 == 0)
                    {
                        // nebyl nalezen pes do dvojice (lichý počet psů)
                        continue;
                    }

                    if (temppes[j].start_beh2 > temppes[x].start_beh2)
                    {
                        // pokud běží IDA2 jako první prohodit aby byl jako IDA1
                        pomocInt = IDA1;
                        IDA1 = IDA2;
                        IDA2 = pomocInt;
                    }

                    IDB1 = 0;
                    IDB2 = 0;

                    for (k = 0; k < pocet_psu; k++)
                    {
                        if (IDB1 == 0)
                        {
                            if (temppes[k].dvojice1 == (temppes[j].dvojice1 + 1))
                            {
                                IDB1 = temppes[k].id;
                                y = k;
                            }
                        }
                        else
                        {
                            if (temppes[k].dvojice1 == (temppes[j].dvojice1 + 1))
                            {
                                IDB2 = temppes[k].id;
                                break;
                            }
                        }
                    }

                    if ((IDB1 == 0) || (IDB2 == 0))
                    {
                        continue;
                    }

                    if (temppes[y].start_beh2 > temppes[k].start_beh2)
                    {
                        // pokud běží IDB2 jako první prohodit aby byl jako IDB1
                        pomocInt = IDB1;
                        IDB1 = IDB2;
                        IDB2 = pomocInt;
                    }
                    pocitadlo1++;

                    Random gen = new Random();
                    int prob = gen.Next(100);
                    if (prob < 50)
                    {
                        Pes A1 = database_P.GetPesById(IDA1);
                        Pes B1 = database_P.GetPesById(IDB1);

                        pomocInt = A1.start_beh2;
                        A1.start_beh2 = B1.start_beh2;
                        B1.start_beh2 = pomocInt;

                        pomocInt = A1.dvojice1;
                        A1.dvojice1 = B1.dvojice1;
                        B1.dvojice1 = pomocInt;

                        Pes A2 = database_P.GetPesById(IDA2);
                        Pes B2 = database_P.GetPesById(IDB2);

                        pomocInt = A2.start_beh2;
                        A2.start_beh2 = B2.start_beh2;
                        B2.start_beh2 = pomocInt;

                        pomocInt = A2.dvojice1;
                        A2.dvojice1 = B2.dvojice1;
                        B2.dvojice1 = pomocInt;

                        pocitadlo2++;
                        /*
                        temp1 = database_P.GetPesById(IDA1);
                        database_P.Edit(IDA1, database_P.GetPesById(IDB1));
                        database_P.Edit(IDB1, temp1);
                        temp1 = database_P.GetPesById(IDA2);
                        database_P.Edit(IDA2, database_P.GetPesById(IDB2));
                        database_P.Edit(IDB2, temp1);*/
                    }

                }
            }
        }

        /// <summary>
        /// Vyhodnocení výsledků a rozdělení titulů, medailí atd... =)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serad_psy_finish(object sender, RoutedEventArgs e)
        {
            List<Pes> temppes = new List<Pes>();
            List<int> skupiny = database_P.GetAllGroups();

            Int32 j = 0;
            Int32 k = 0;
            
            Int32 temp = 0;

            Int32 pocet_psu = 0;
            Int32 pocet_fen = 0;
            Int32 min = 1;

            if ((serazeni_psu == 2) || (serazeni_psu == 1))
            {
                foreach (int i in skupiny)
                {
                    min += pocet_psu + pocet_fen;
                    temppes = database_P.GetPesByGroup(i, null);
                    pocet_psu = temppes.Count;

                    for (j = 0; j < pocet_psu; j++)
                    {
                        if (temppes[j].diskval != "---") // pokud je diskvalifikován bude vyřazen z bodování
                        {
                            temppes[j].skupina = 999; // umístění na pozici 999, vyřazení ze závodu
                        }
                        else
                        {
                            temp = 1;
                            for (k = 0; k < pocet_psu; k++)
                            {
                                if ((j == k) || (temppes[k].diskval != "---"))
                                    continue;

                                if ((temppes[j].body1 + temppes[j].body2) < (temppes[k].body1 + temppes[k].body2))
                                    temp++;
                                else if ((temppes[j].body1 + temppes[j].body2) == (temppes[k].body1 + temppes[k].body2))
                                {
                                    if (temppes[j].body2 < temppes[k].body2)
                                        temp++;
                                    else if (temppes[j].body2 == temppes[k].body2)
                                    {
                                        if ((temppes[j].agility_B0 + temppes[j].agility_B1) < (temppes[k].agility_B0 + temppes[k].agility_B1))
                                            temp++;
                                        else if ((temppes[j].agility_B0 + temppes[j].agility_B1) == (temppes[k].agility_B0 + temppes[k].agility_B1))
                                        {
                                            if ((temppes[j].speed_B0 + temppes[j].speed_B1) < (temppes[k].speed_B0 + temppes[k].speed_B1))
                                                temp++;
                                            else if ((temppes[j].speed_B0 + temppes[j].speed_B1) == (temppes[k].speed_B0 + temppes[k].speed_B1))
                                            {
                                                if ((temppes[j].endurance_B0 + temppes[j].endurance_B1) < (temppes[k].endurance_B0 + temppes[k].endurance_B1))
                                                    temp++;
                                                else if ((temppes[j].endurance_B0 + temppes[j].endurance_B1) == (temppes[k].endurance_B0 + temppes[k].endurance_B1))
                                                {
                                                    if ((temppes[j].enthusiasm_B0 + temppes[j].enthusiasm_B1) < (temppes[k].enthusiasm_B0 + temppes[k].enthusiasm_B1))
                                                        temp++;
                                                    else if ((temppes[j].enthusiasm_B0 + temppes[j].enthusiasm_B1) == (temppes[k].enthusiasm_B0 + temppes[k].enthusiasm_B1))
                                                    {
                                                        if ((temppes[j].intelligence_B0 + temppes[j].intelligence_B1) < (temppes[k].intelligence_B0 + temppes[k].intelligence_B1))
                                                            temp++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            temppes[j].skupina = temp; // TODO: přidat do třídy pes parametr umístění v cíli a titul !
                        }
                        database_P.Edit(temppes[j].id, new Pes(temppes[j]));
                    }
                    temppes.Clear();
                }
                //this.ReloadPesData();
                this.skupinGridPesData();
            }
            else
            {
                MessageBox.Show("Psi nejsou seřazeni k druhému běhu!\n\nZkontrolujte prosím jejich seřazení.", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Generator nahodnych cisel pro serazeni psu.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private List<int> Generator_cisel(int min,  Int32 max)
        {
            List<int> cisla = new List<int>();
            Random rand = new Random();
            while(cisla.Count < (max - min + 1))
            {
                 Int32 cislo = rand.Next(min, max+1);
                if (!cisla.Contains(cislo))
                {
                    cisla.Add(cislo);
                }
            }

            return cisla;
        }


        /// <summary>
        /// Smaže zvoleného majitele.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteMajitel_OnClick(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                 Int32 selectedMajitelIndex = DGV.SelectedIndex;
                if (selectedMajitelIndex != -1)
                {
                    MessageBoxResult result = MessageBox.Show("Přejete si vymazat majitele " + ((Majitel)DGV.SelectedItem).firstName + " " + ((Majitel)DGV.SelectedItem).lastName + "?", "Pozor!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.database.Delete(((Majitel)DGV.SelectedItem).id);
                        this.ReloadMajitelData();
                        this.stav();
                    }
                }
                else
                    MessageBox.Show("Není vybrán žádný majitel.\nMajitele vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Smaže zvoleného psa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deletePes_OnClick(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                 Int32 selectedPesIndex = DGV_P.SelectedIndex;
                if(selectedPesIndex != -1)
                {
                    MessageBoxResult result = MessageBox.Show("Přejete si vymazat psa " + ((Pes)DGV_P.SelectedItem).jmeno + "?", "Pozor!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.database_P.Delete(((Pes)DGV_P.SelectedItem).id);
                        this.ReloadPesData();
                        this.stav();
                    }
                }
                else
                    MessageBox.Show("Není vybrán žádný pes.\nPsa vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Vyprazdni pole ve formulari pro pridani/upravu majitele.
        /// </summary>
        private void NewMajitelClearFields()
        {
            boxId.Text = "";
            boxFirstName.Text = "";
            boxLastName.Text = "";
            clenCB.IsChecked = false;
            platbaTB.Text = "0";
            platba_OK_CB.IsChecked = false;
            telefonBox.Text = "";
            emailBox.Text = "";
        }

        /// <summary>
        /// Vyprazdni pole ve formulari pro pridani/upravu psa.
        /// </summary>
        private void NewPesClearFields()
        {
            boxId_P.Text = "";
            boxFirstName_P.Text = "";
            //boxPlemeno.Text = "";
            comboPlemeno.Text = "";
            fciCombo.Text = "";
            boxLicence.Text = "";
            boxMajitel.Text = "";
            datumPicker.Text = "";
            boxPlat.Text = "0";
            boxDoplat.Text = "0";
            boxPozn.Text = "";
        }

        /// <summary>
        /// Vyprázdnění polí pro upravu psa v přehledu psů.
        /// </summary>
        private void EditPesClearFields()
        {
            boxstart1.Text = "";
            boxstart2.Text = "";
            boxbeh1.Text = "";
            boxbeh2.Text = "";
            decka1CB.Text = "Červená";
            decka2CB.Text = "Červená";
        }

        /// <summary>
        /// Overuje jestli jsou vsechna povinna pole vyplnena.
        /// </summary>
        /// <returns></returns>
        private bool NewMajitelIsFilled()
        {
            if ((boxFirstName.Text != "")/* && (boxLastName.Text != "") */&& (platbaTB.Text != ""))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Overuje jestli jsou vsechna povinna pole vyplnena.
        /// </summary>
        /// <returns></returns>
        private  Int32 NewPesIsFilled()
        {
            if ((boxFirstName_P.Text != "") && /*(boxPlemeno.Text != "")*/(comboPlemeno.Text != "")/* && (fciCombo.Text != "")*/ && (boxMajitel.Text != "") && (datumPicker.SelectedDate != null))
                if (boxLicence.Text == "")
                    return -1;
                else
                    return 1;
            else
                return 0;
        }

        /// <summary>
        /// Overuje jestli jsou vsechna povinna pole vyplnena.
        /// </summary>
        /// <returns> 1 pole jsou vyplnena a cislo je relevantni, -1 pole jsou vyplnena, ale cislo je irelevantni, 0 pole nejsou vyplnena </returns>
        private  Int32 hodnoceniIsFilled()
        {
            if ((hod_id.Text != "") && (hod_agil_A.Text != "") && (hod_agil_B.Text != "") && (hod_speed_A.Text != "") && (hod_speed_B.Text != "") && (hod_endur_A.Text != "") && (hod_endur_B.Text != "") && (hod_enthu_A.Text != "") && (hod_enthu_B.Text != "") && (hod_intel_A.Text != "") && (hod_intel_B.Text != ""))
            {
                if (IdValidation( Int32.Parse(hod_id.Text)))
                    return 1;
                else
                    return -1;
            }
            else
                return 0;
        }

        /// <summary>
        /// Funkce ověřuje jestl je zadáno platné ID psa.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private bool IdValidation(Int32 ID)
        {
            int[] cisla = database_P.GetAllStartNos();

            foreach (int No in cisla)
            {
                if (No == ID)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Prida/upravi majitele podle parametru ve formulari.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_OnClick(object sender, RoutedEventArgs e)
        {
            String[] majitelArrayData = new String[10];

            if (this.NewMajitelIsFilled() != true)
                MessageBox.Show("Všechna pole musí být vyplněna!", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            else
            {
                majitelArrayData[1] = boxFirstName.Text;
                majitelArrayData[2] = boxLastName.Text;
                if (this.clenCB.IsChecked == true)
                    majitelArrayData[3] = "ano";
                else
                    majitelArrayData[3] = "ne";
                majitelArrayData[4] = platbaTB.Text;
                if (this.platba_OK_CB.IsChecked == true)
                    majitelArrayData[5] = "OK";
                else
                    majitelArrayData[5] = "--";
                majitelArrayData[6] = telefonBox.Text;
                majitelArrayData[7] = emailBox.Text;
                majitelArrayData[8] = boxNarod.Text;
                majitelArrayData[9] = psiTB.Text;

                if (boxId.Text == "")
                {
                    majitelArrayData[0] = Convert.ToString(this.database.GetNewId());
                    database.Add(new Majitel(majitelArrayData));
                }
                else
                {
                    majitelArrayData[0] = Convert.ToString(boxId.Text);
                    database.Edit(Convert.ToInt32(boxId.Text), new Majitel(majitelArrayData));
                }
                this.divNewMajitel.Visibility = System.Windows.Visibility.Collapsed;
                this.NewMajitelClearFields();
                ReloadMajitelData();
                stav();
            }
        }

        /// <summary>
        /// Prida/upravi psa podle parametru ve formulari.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSave_P_OnClick(object sender, RoutedEventArgs e)
        {
            Int32 error = this.NewPesIsFilled();
            if (error == 0)
                MessageBox.Show("Všechna pole musí být vyplněna!", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            else
            {
                MessageBoxResult pokracovat = MessageBoxResult.OK;
                if (error == -1)
                    pokracovat = MessageBox.Show("Není zadaná licence.\nPřejete si pokračovat?", "Pes bez licence", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (pokracovat == MessageBoxResult.Yes)
                {
                    boxLicence.Text = "";
                    pokracovat = MessageBoxResult.OK;
                }
                if (pokracovat == MessageBoxResult.OK)
                {
                    if (boxId_P.Text == "")
                    {
                        Pes temppes = new Pes();

                        temppes.id = this.database_P.GetNewId();

                        // rychlo oprava...
                        temppes.start_beh1 = Int32.Parse(boxstart_P.Text);
                        temppes.dvojice0 = Int32.Parse(boxBeh.Text);

                        temppes.jmeno = boxFirstName_P.Text;
                        temppes.plemeno = comboPlemeno.Text;
                        temppes.pohlavi = pohlaviCB.Text;
                        temppes.fci = fciCombo.Text;
                        temppes.datum = datumPicker.SelectedDate.Value.Date;
                        temppes.licence = boxLicence.Text;
                        temppes.majitel =  Int32.Parse(boxMajitel.Text);

                        temppes.zavod_licence = zavodCB.Text;

                        temppes.platba =  Int32.Parse(boxPlat.Text);//platba
                        temppes.doplatit =  Int32.Parse(boxDoplat.Text);//doplatit
                        temppes.poznamka = boxPozn.Text;//poznamka

                        database_P.Add(new Pes(temppes)); //TODO opravit konstruktory a práva !!!

                        Majitel tempmaj = database.GetMajitelById(temppes.majitel);
                        tempmaj.pocet_psu++;
                        tempmaj.penize += (temppes.platba + temppes.doplatit);

                        ReloadMajitelData();
                    }
                    else
                    {
                        Pes temppes = database_P.GetPesById( Int32.Parse(boxId_P.Text));

                        temppes.id = Int32.Parse(boxId_P.Text);

                        // rychlo oprava...
                        temppes.start_beh1 = Int32.Parse(boxstart_P.Text);
                        temppes.dvojice0 = Int32.Parse(boxBeh.Text);

                        temppes.jmeno = boxFirstName_P.Text;
                        temppes.plemeno = comboPlemeno.Text;
                        temppes.pohlavi = pohlaviCB.Text;
                        temppes.fci = fciCombo.Text;
                        temppes.datum = datumPicker.SelectedDate.Value.Date;
                        temppes.licence = boxLicence.Text;
                        temppes.majitel = Int32.Parse(boxMajitel.Text);

                        temppes.zavod_licence = zavodCB.Text;

                        temppes.platba = Int32.Parse(boxPlat.Text);//platba
                        temppes.doplatit = Int32.Parse(boxDoplat.Text);//doplatit
                        temppes.poznamka = boxPozn.Text;//poznamka

                        database_P.Edit(temppes.id, temppes);
                    }
                }
                this.divNewPes.Visibility = System.Windows.Visibility.Collapsed;
                this.NewPesClearFields();
                ReloadPesData();
                stav();
            }    
        }

        /// <summary>
        /// Prida/upravi psa podle parametru ve formulari.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveprehled(object sender, RoutedEventArgs e)
        {
          /*  Int32 error = this.NewPesIsFilled();
            if (error == 0)
                MessageBox.Show("Všechna pole musí být vyplněna!", "Pozor!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            else
            {*/
                Pes temppes = database_P.GetPesById(((Pes)skupinGrid.SelectedItem).id);

                temppes.start_beh1 = Int32.Parse(boxstart1.Text);
                temppes.dvojice0 = Int32.Parse(boxbeh1.Text);
                temppes.barva0 = decka1CB.Text;

                temppes.start_beh2 = Int32.Parse(boxstart2.Text);
                temppes.dvojice1 = Int32.Parse(boxbeh2.Text);
                temppes.barva1 = decka2CB.Text;

                this.diveditprehled.Visibility = System.Windows.Visibility.Collapsed;
                this.EditPesClearFields();
                skupinGridPesData();
          /*  }*/
        }
        
        /// <summary>
        /// Ukonceni formulare pro upravu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.divNewMajitel.Visibility = System.Windows.Visibility.Collapsed;
            this.NewMajitelClearFields();
        }

        /// <summary>
        /// Ukonceni formulare pro upravu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_P_OnClick(object sender, RoutedEventArgs e)
        {
            this.divNewPes.Visibility = System.Windows.Visibility.Collapsed;
            this.NewPesClearFields();
        }

        /// <summary>
        /// Ukonceni formulare pro upravu psa v přehledu psů.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCanceleditprehled(object sender, RoutedEventArgs e)
        {
            this.diveditprehled.Visibility = System.Windows.Visibility.Collapsed;
            this.EditPesClearFields();
        }

        /// <summary>
        /// Ukonceni aplikace s ulozenim dat do XML souboru.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exit_OnClick(object sender, RoutedEventArgs e)
        {
                this.Close();
        }

        /// <summary>
        /// Zobrazi okno s informacemi o programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void show_about(object sender, RoutedEventArgs e)
        {
            if (DGV.Visibility == System.Windows.Visibility.Visible)
            {
                hide_gridA();
                DGV.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (skupinGrid.Visibility == System.Windows.Visibility.Visible)
            {
                hide_gridB();
                skupinGrid.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (hodnoceniGrid.Visibility == System.Windows.Visibility.Visible)
            {
                hide_gridC();
                hodnoceniGrid.Visibility = System.Windows.Visibility.Hidden;
            }
       //     mainWindow.Background = new SolidColorBrush(Color.FromRgb(20, 20, 200));
            this.help_about.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Schova okno s informacemi o programu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_hide_about(object sender, RoutedEventArgs e)
        {
            if (DGV.Visibility == System.Windows.Visibility.Hidden)
            {
                show_gridA();
            }
            else if (skupinGrid.Visibility == System.Windows.Visibility.Hidden)
            {
                show_gridB();
            }
            else if (hodnoceniGrid.Visibility == System.Windows.Visibility.Hidden)
            {
                show_gridC();
            }
        //    mainWindow.Background = new SolidColorBrush(Color.FromRgb(238, 228, 193));
            this.help_about.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Vypocita stav pro stavovy radek (pocet majitelu a pocet psu).
        /// </summary>
        private void stav()
        {
            if (this.XmlDatabase != null)
            {
                String output = "Počet majitelů v databázi: ";
                int[] pole = this.database.GetAllIds();
                 Int32 pocet = pole.Length;
                output = output + pocet.ToString() + ", počet psů v databázi: ";
                pole = this.database_P.GetAllIds();
                pocet = pole.Length;
                output = output + pocet.ToString() + ".";
                this.statusBlock.Text = output;
            }
        }

        /// <summary>
        /// Vypise do stavoveho radku pocet ohodnocenych psu.
        /// </summary>
        private void stavHodnoceni()
        {
            if (this.XmlDatabase != null)
            {
                String output = "Počet ohodnocených psů: " + ListHodnocenychPsu.Count().ToString() + ", počet zbývajících psů: " + (database_P.Length() - ListHodnocenychPsu.Count()).ToString() + "."; 
               
                this.statusHodnoceniBlock.Text = output;
            }
        }

        /// <summary>
        /// Zobrazi statisiky majitelu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stat_majitele(object sender, RoutedEventArgs e)
        {
            System.Windows.GridLength x = new System.Windows.GridLength(200);
      //      this.row2.Height = x;
      //      this.row2.MinHeight = 200;
            this.stat_m.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// Zobrazi statistiky psu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stat_psi(object sender, RoutedEventArgs e)
        {
            System.Windows.GridLength x = new System.Windows.GridLength(200);
      //      this.row1.Height = x;
      //      this.row1.MinHeight = 200;
            this.stat_p.Visibility = System.Windows.Visibility.Visible;
        }

        private void hide_statp(object sender, RoutedEventArgs e)
        {
            this.stat_p.Visibility = System.Windows.Visibility.Collapsed;
       //     this.row1.Height = System.Windows.GridLength.Auto;
            this.row1.MinHeight = 50;
        }

        private void hide_statm(object sender, RoutedEventArgs e)
        {
            this.stat_m.Visibility = System.Windows.Visibility.Collapsed;
      //      this.row2.Height = System.Windows.GridLength.Auto;
            this.row2.MinHeight = 50;
        }

        private void edit_skup(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                hide_gridA();
                show_gridB();
                hide_gridC();

                skupinGridPesData();
            }
        }

        private void hodnoceni(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                hide_gridA();
                hide_gridB();
                show_gridC();

                clear_hodnoceni();
            }
        }

        private void show_gridA()
        {
            this.DGV.Visibility = System.Windows.Visibility.Visible;
            this.DGV_label.Visibility = System.Windows.Visibility.Visible;
            this.DGV_P.Visibility = System.Windows.Visibility.Visible;
            this.DGV_P_label.Visibility = System.Windows.Visibility.Visible;
            this.splitter.Visibility = System.Windows.Visibility.Visible;
        }
        private void hide_gridA()
        {
            this.DGV.Visibility = System.Windows.Visibility.Collapsed;
            this.DGV_label.Visibility = System.Windows.Visibility.Collapsed;
            this.DGV_P.Visibility = System.Windows.Visibility.Collapsed;
            this.DGV_P_label.Visibility = System.Windows.Visibility.Collapsed;
            this.splitter.Visibility = System.Windows.Visibility.Collapsed;
            this.divNewMajitel.Visibility = System.Windows.Visibility.Collapsed;
            this.divNewPes.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void show_gridB()
        {
            this.skupinGrid.Visibility = System.Windows.Visibility.Visible;
            this.skupGrid_label.Visibility = System.Windows.Visibility.Visible;
        }

        private void hide_gridB()
        {
            this.skupinGrid.Visibility = System.Windows.Visibility.Collapsed;
            this.skupGrid_label.Visibility = System.Windows.Visibility.Collapsed;
            this.diveditprehled.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void show_gridC()
        {
            this.hodnoceniGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void hide_gridC()
        {
            this.hodnoceniGrid.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void hlavniGrid(object sender, RoutedEventArgs e)
        {
            show_gridA();
            hide_gridB();
            hide_gridC();
        }

        private void uloz_hodnoceni(object sender, RoutedEventArgs e)
        {
            if (hodnoceniIsFilled() == 1)
            {
                //ulozPriznakHodnoceni(Convert.ToInt32(hod_id.Text));
                zapis_body(sender, e);
                hod_id.Focus();
            }
            else if (hodnoceniIsFilled() == 0)
                MessageBox.Show("Musí být vyplněna všechna pole!", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Warning);
            else if (hodnoceniIsFilled() == -1)
                MessageBox.Show("Zadané číslo psa neexistuje, zadejte prosím správné číslo!", "Nesprávné číslo psa!", MessageBoxButton.OK, MessageBoxImage.Stop);
            stavHodnoceni();
        }

        /// <summary>
        /// Přidá ID do seznamu hodnocených psů pokud tam už není.
        /// </summary>
        /// <param name="ID"></param>
        private void ulozPriznakHodnoceni(int ID)
        {
            if (!ListHodnocenychPsu.Contains(ID))
                ListHodnocenychPsu.Add(ID);
        } //TODO: pošéfovat diskvalifikované psy!!!
        
        /// <summary>
        /// Inicializuje globální proměnnou ListHodnocenychPsu při načtení databáze z XML souboru.
        /// </summary>
        private void inicializace_ohodnocenych_psu_xml()
        {
            ListHodnocenychPsu.Clear(); // vyprázdnění listu z předchozích operací
            int[] IDs = database_P.GetAllIds(); // seznam ID psů v nové databázi
            foreach(int index in IDs)
            {
                if (database_P.GetPesById(IDs[index-1]).body1 != 0) // pokud nejsou nulové body byl pes již ohodnocen a jeho index je zapsán do listu ohodnocených psů
                    ListHodnocenychPsu.Add(IDs[index-1]);
            }
            stavHodnoceni();
        }

        /// <summary>
        /// Zvolenému psovi zapíše body.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zapis_body(object sender, RoutedEventArgs e)
        {
          //  Pes pes = this.database_P.GetPesById(Convert.ToInt32(hod_id.Text));

            Pes pes = this.database_P.GetPesByStartNo(Convert.ToInt32(hod_id.Text));

            if (hod_kolo.SelectedIndex == 0)
            {   // hodnocení obou rozhodčích
                pes.agility_A0 = Convert.ToInt32(hod_agil_A.Text);
                pes.agility_A1 = Convert.ToInt32(hod_agil_B.Text);
                pes.speed_A0 = Convert.ToInt32(hod_speed_A.Text);
                pes.speed_A1 = Convert.ToInt32(hod_speed_B.Text);
                pes.endurance_A0 = Convert.ToInt32(hod_endur_A.Text);
                pes.endurance_A1 = Convert.ToInt32(hod_endur_B.Text);
                pes.enthusiasm_A0 = Convert.ToInt32(hod_enthu_A.Text);
                pes.enthusiasm_A1 = Convert.ToInt32(hod_enthu_B.Text);
                pes.intelligence_A0 = Convert.ToInt32(hod_intel_A.Text);
                pes.intelligence_A1 = Convert.ToInt32(hod_intel_B.Text);

                pes.body1 = Convert.ToInt32(hod_sum_TOTAL.Text);
            }
            else
            {   // hodnocení obou rozhodčích
                pes.agility_B0 = Convert.ToInt32(hod_agil_A.Text);
                pes.agility_B1 = Convert.ToInt32(hod_agil_B.Text);
                pes.speed_B0 = Convert.ToInt32(hod_speed_A.Text);
                pes.speed_B1 = Convert.ToInt32(hod_speed_B.Text);
                pes.endurance_B0 = Convert.ToInt32(hod_endur_A.Text);
                pes.endurance_B1 = Convert.ToInt32(hod_endur_B.Text);
                pes.enthusiasm_B0 = Convert.ToInt32(hod_enthu_A.Text);
                pes.enthusiasm_B1 = Convert.ToInt32(hod_enthu_B.Text);
                pes.intelligence_B0 = Convert.ToInt32(hod_intel_A.Text);
                pes.intelligence_B1 = Convert.ToInt32(hod_intel_B.Text);

                pes.body2 = Convert.ToInt32(hod_sum_TOTAL.Text);
            }

            pes.diskval = hod_dis.Text;

            this.database_P.Edit(pes.id, new Pes(pes));
            ReloadPesData();
            clear_hodnoceni();
        }

        private void sumsum(object sender, RoutedEventArgs e)
        {
             Int32 bodyA = 0;
             Int32 bodyB = 0;
            if (this.hod_agil_A.Text != "")
                bodyA += Convert.ToInt32(this.hod_agil_A.Text);
            if (this.hod_speed_A.Text != "")
                bodyA += Convert.ToInt32(this.hod_speed_A.Text);
            if (this.hod_endur_A.Text != "")
                bodyA += Convert.ToInt32(this.hod_endur_A.Text);
            if (this.hod_enthu_A.Text != "")
                bodyA += Convert.ToInt32(this.hod_enthu_A.Text);
            if (this.hod_intel_A.Text != "")
                bodyA += Convert.ToInt32(this.hod_intel_A.Text);


            if (this.hod_agil_B.Text != "")
                bodyB += Convert.ToInt32(this.hod_agil_B.Text);
            if (this.hod_speed_B.Text != "")
                bodyB += Convert.ToInt32(this.hod_speed_B.Text);
            if (this.hod_endur_B.Text != "")
                bodyB += Convert.ToInt32(this.hod_endur_B.Text);
            if (this.hod_enthu_B.Text != "")
                bodyB += Convert.ToInt32(this.hod_enthu_B.Text);
            if (this.hod_intel_B.Text != "")
                bodyB += Convert.ToInt32(this.hod_intel_B.Text);

            this.hod_sum_A.Text = Convert.ToString(bodyA);
            this.hod_sum_B.Text = Convert.ToString(bodyB);

            this.hod_sum_TOTAL.Text = Convert.ToString(bodyA + bodyB);
        }

        private void clear_hodnoceni()
        {
            this.hod_id.Text = "";
            this.hod_agil_A.Text = "";
            this.hod_speed_A.Text = "";
            this.hod_endur_A.Text = "";
            this.hod_enthu_A.Text = "";
            this.hod_intel_A.Text = "";
            this.hod_agil_B.Text = "";
            this.hod_speed_B.Text = "";
            this.hod_endur_B.Text = "";
            this.hod_enthu_B.Text = "";
            this.hod_intel_B.Text = "";
            this.hod_sum_A.Text = "";
            this.hod_sum_B.Text = "";
            this.hod_sum_TOTAL.Text = "";

            this.hod_dis.SelectedIndex = 0;
        }

        private void HodnoceniVybranehoPsa(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (skupinGrid.SelectedIndex != -1)
                {
                    hide_gridA();
                    hide_gridB();
                    show_gridC();

                    hod_kolo.SelectedIndex = 0;

                    this.hod_id.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).start_beh1);

                    this.hod_agil_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).agility_A0);
                    this.hod_speed_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).speed_A0);
                    this.hod_endur_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).endurance_A0);
                    this.hod_enthu_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).enthusiasm_A0);
                    this.hod_intel_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).intelligence_A0);

                    this.hod_agil_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).agility_A1);
                    this.hod_speed_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).speed_A1);
                    this.hod_endur_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).endurance_A1);
                    this.hod_enthu_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).enthusiasm_A1);
                    this.hod_intel_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).intelligence_A1);

                    this.hod_dis.Text = ((Pes)skupinGrid.SelectedItem).diskval;
                }
                else
                    MessageBox.Show("Není vybrán žádný pes.\nPsa vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

        private void HodnoceniVybranehoPsa2(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (skupinGrid.SelectedIndex != -1)
                {
                    hide_gridA();
                    hide_gridB();
                    show_gridC();

                    hod_kolo.SelectedIndex = 1;
                    
                    this.hod_id.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).start_beh1);

                    this.hod_agil_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).agility_B0);
                    this.hod_speed_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).speed_B0);
                    this.hod_endur_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).endurance_B0);
                    this.hod_enthu_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).enthusiasm_B0);
                    this.hod_intel_A.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).intelligence_B0);

                    this.hod_agil_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).agility_B1);
                    this.hod_speed_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).speed_B1);
                    this.hod_endur_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).endurance_B1);
                    this.hod_enthu_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).enthusiasm_B1);
                    this.hod_intel_B.Text = Convert.ToString(((Pes)skupinGrid.SelectedItem).intelligence_B1);

                    this.hod_dis.Text = ((Pes)skupinGrid.SelectedItem).diskval;
                }
                else
                    MessageBox.Show("Není vybrán žádný pes.\nPsa vyberete kliknutím na položku v seznamu.", "Chyba!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

        private void Refresh_bodovani(object sender, RoutedEventArgs e)
        {
            if (XmlDatabase != null)
            {
                if (hod_id.Text != "") // provést jen pokud není pole prázdné
                {
                    Pes temppes = database_P.GetPesByStartNo( Int32.Parse(hod_id.Text));
                    if (temppes != null) // pokud je zadané číslo mimo rozsah seznamu // TODO: vybírat pouze výběrového boxu místo textového
                    {
                        if (hod_kolo.SelectedIndex == 0)
                        {
                            this.hod_agil_A.Text = Convert.ToString(temppes.agility_A0);
                            this.hod_speed_A.Text = Convert.ToString(temppes.speed_A0);
                            this.hod_endur_A.Text = Convert.ToString(temppes.endurance_A0);
                            this.hod_enthu_A.Text = Convert.ToString(temppes.enthusiasm_A0);
                            this.hod_intel_A.Text = Convert.ToString(temppes.intelligence_A0);

                            this.hod_agil_B.Text = Convert.ToString(temppes.agility_A1);
                            this.hod_speed_B.Text = Convert.ToString(temppes.speed_A1);
                            this.hod_endur_B.Text = Convert.ToString(temppes.endurance_A1);
                            this.hod_enthu_B.Text = Convert.ToString(temppes.enthusiasm_A1);
                            this.hod_intel_B.Text = Convert.ToString(temppes.intelligence_A1);
                        }
                        else
                        {
                            this.hod_agil_A.Text = Convert.ToString(temppes.agility_B0);
                            this.hod_speed_A.Text = Convert.ToString(temppes.speed_B0);
                            this.hod_endur_A.Text = Convert.ToString(temppes.endurance_B0);
                            this.hod_enthu_A.Text = Convert.ToString(temppes.enthusiasm_B0);
                            this.hod_intel_A.Text = Convert.ToString(temppes.intelligence_B0);

                            this.hod_agil_B.Text = Convert.ToString(temppes.agility_B1);
                            this.hod_speed_B.Text = Convert.ToString(temppes.speed_B1);
                            this.hod_endur_B.Text = Convert.ToString(temppes.endurance_B1);
                            this.hod_enthu_B.Text = Convert.ToString(temppes.enthusiasm_B1);
                            this.hod_intel_B.Text = Convert.ToString(temppes.intelligence_B1);
                        }

                        this.hod_dis.Text = temppes.diskval;
                    }
                    else
                    {
                        this.hod_agil_A.Text = "";
                        this.hod_speed_A.Text = "";
                        this.hod_endur_A.Text = "";
                        this.hod_enthu_A.Text = "";
                        this.hod_intel_A.Text = "";

                        this.hod_agil_B.Text = "";
                        this.hod_speed_B.Text = "";
                        this.hod_endur_B.Text = "";
                        this.hod_enthu_B.Text = "";
                        this.hod_intel_B.Text = "";

                        this.hod_dis.Text = "---";
                    }
                }
                else
                {
                    this.hod_agil_A.Text = "";
                    this.hod_speed_A.Text = "";
                    this.hod_endur_A.Text = "";
                    this.hod_enthu_A.Text = "";
                    this.hod_intel_A.Text = "";

                    this.hod_agil_B.Text = "";
                    this.hod_speed_B.Text = "";
                    this.hod_endur_B.Text = "";
                    this.hod_enthu_B.Text = "";
                    this.hod_intel_B.Text = "";

                    this.hod_dis.Text = "---";
                }
            }
        }

        /// <summary>
        /// Omezení textového pole pouze na čísla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// Při změně bodování (první nebo druhý běh) se mění barva pozadí.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bodovani_changed(object sender, RoutedEventArgs e)
        {
            if (hod_kolo.SelectedIndex == 0)
                hodnoceni_rect.Fill = new SolidColorBrush(Color.FromRgb(120, 160, 200));
            else if (hod_kolo.SelectedIndex == 1)
                hodnoceni_rect.Fill = new SolidColorBrush(Color.FromRgb(200, 120, 120));
            Refresh_bodovani(sender, e);
        }

        /// <summary>
        /// Testovaci funkce na nahodne generovani dat ze zavodu "hodnoceni rozhodcich".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomNumbers_TEST(object sender, RoutedEventArgs e)
        {
            int[] vsichni = database_P.GetAllStartNos();
            Random rnd = new Random();
                Int32 total = 0;

            foreach (int pesId in vsichni)
            {
                Pes pes = new Pes(database_P.GetPesByStartNo(pesId));

                if (pes.zavod_licence == "Závod")
                {
                    total += pes.agility_A0 = rnd.Next(13, 20);
                    total += pes.speed_A0 = rnd.Next(13, 20);
                    total += pes.endurance_A0 = rnd.Next(13, 20);
                    total += pes.enthusiasm_A0 = rnd.Next(13, 20);
                    total += pes.intelligence_A0 = rnd.Next(13, 20);

                    total += pes.agility_A1 = rnd.Next(13, 20);
                    total += pes.speed_A1 = rnd.Next(13, 20);
                    total += pes.endurance_A1 = rnd.Next(13, 20);
                    total += pes.enthusiasm_A1 = rnd.Next(13, 20);
                    total += pes.intelligence_A1 = rnd.Next(13, 20);
                    pes.body1 = total;
                    total = 0;
                }
                else
                {
                    pes.agility_A0 = 0;
                    pes.speed_A0 = 0;
                    pes.endurance_A0 = 0;
                    pes.enthusiasm_A0 = 0;
                    pes.intelligence_A0 = 0;

                    pes.agility_A1 = 0;
                    pes.speed_A1 = 0;
                    pes.endurance_A1 = 0;
                    pes.enthusiasm_A1 = 0;
                    pes.intelligence_A1 = 0;
                }

                database_P.Edit(pes.id, new Pes(pes));
            }

            skupinGridPesData();
        }
    

        /// <summary>
        /// Testovaci funkce na nahodne generovani dat z druheho kola "hodnoceni rozhodcich".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RandomNumbers_TEST_2(object sender, RoutedEventArgs e)
        {
            int[] vsichni = database_P.GetAllStartNos();
            Random rnd = new Random();
            Int32 total = 0;

            foreach (int pesId in vsichni)
            {
                Pes pes = database_P.GetPesByStartNo(pesId);

                if (pes.zavod_licence == "Závod")
                {
                    total += pes.agility_B0 = rnd.Next(13, 20);
                    total += pes.speed_B0 = rnd.Next(13, 20);
                    total += pes.endurance_B0 = rnd.Next(13, 20);
                    total += pes.enthusiasm_B0 = rnd.Next(13, 20);
                    total += pes.intelligence_B0 = rnd.Next(13, 20);

                    total += pes.agility_B1 = rnd.Next(13, 20);
                    total += pes.speed_B1 = rnd.Next(13, 20);
                    total += pes.endurance_B1 = rnd.Next(13, 20);
                    total += pes.enthusiasm_B1 = rnd.Next(13, 20);
                    total += pes.intelligence_B1 = rnd.Next(13, 20);
                    pes.body2 = total;
                    total = 0;
                }
                else
                {
                    pes.agility_B0 = 0;
                    pes.speed_B0 = 0;
                    pes.endurance_B0 = 0;
                    pes.enthusiasm_B0 = 0;
                    pes.intelligence_B0 = 0;

                    pes.agility_B1 = 0;
                    pes.speed_B1 = 0;
                    pes.endurance_B1 = 0;
                    pes.enthusiasm_B1 = 0;
                    pes.intelligence_B1 = 0;
                }

                database_P.Edit(pes.id, new Pes(pes));
            }

            skupinGridPesData();
        }


        /// <summary>
        /// Seřadí datagrid podle zvoleného sloupce.
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="columnIndex"></param>
        /// <param name="sortDirection"></param>
        public static void SortDataGrid(DataGrid dataGrid, int columnIndex = 0, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            var column = dataGrid.Columns[columnIndex];

            // Clear current sort descriptions
            dataGrid.Items.SortDescriptions.Clear();

            // Add the new sort description
            dataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, sortDirection));

            // Apply sort
            foreach (var col in dataGrid.Columns)
            {
                col.SortDirection = null;
            }
            column.SortDirection = sortDirection;

            // Refresh items to display sort
            dataGrid.Items.Refresh();
        }

        /// <summary>
        /// Zjistí podle kterého sloupce je datagrid seřazen.
        /// </summary>
        /// <param name="dataGrid"></param>
        public static int WhatSortedDG(DataGrid dataGrid)
        {
            int n = dataGrid.Columns.Count;
            for (int columnIndex = 0; columnIndex < n; columnIndex++)
            {
                if (dataGrid.Columns[columnIndex].SortDirection != null)
                {
                    return columnIndex;
                }
            }
            return -1;
        }

        /// <summary>
        /// Zjisti jak je daný sloupec seřazen.
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static ListSortDirection HowSortedDG(DataGrid dataGrid, int columnIndex)
        {
            ListSortDirection serazeni = dataGrid.Columns[columnIndex].SortDirection.Value;

            return serazeni;
        }

        private void comboPlemeno_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            foreach (String str in Resource1.String1.Split('\n'))
            {
                comboBox.Items.Add(str.Remove(str.Length - 1));
            }
        }

        private void fciCombo_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            foreach (String str in Resource1.String2.Split('\n'))
            {
                comboBox.Items.Add(str.Remove(str.Length - 1));
            }
        }

    }

    public class WaitCursor : IDisposable
    {
        private Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }


        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}