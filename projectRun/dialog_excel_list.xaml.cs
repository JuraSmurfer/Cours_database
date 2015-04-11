using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace projectRun
{
    /// <summary>
    /// Interaction logic for dialog_excel_list.xaml
    /// </summary>
    public partial class dialog_excel_list : Window
    {
        public dialog_excel_list(string ktery_list, List<String> listy)
        {
            InitializeComponent();
            lblQuestion.Content = ktery_list;
            list_no.ItemsSource = listy;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            list_no.Focus();
        }

        public  Int32 Answer
        {
            get { return list_no.SelectedIndex; }
        }
    }
}
