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

namespace DSS__Stock_Exchange
{
    /// <summary>
    /// Логика взаимодействия для RenameSeriesWindow.xaml
    /// </summary>
    public partial class RenameSeriesWindow : Window
    {
        System.Collections.ObjectModel.ObservableCollection<System.Windows.Controls.DataGridColumn> columns;
        public RenameSeriesWindow(System.Collections.ObjectModel.ObservableCollection<System.Windows.Controls.DataGridColumn> columns)
        {
            InitializeComponent();
            this.columns = columns;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < columns.Count; i++)
            {
                if (string.Equals(OldNAme.Text, columns[i].Header))
                {
                    columns[i].Header = NewName.Text;
                    this.Close();
                    break;                    
                }
                else if (i == columns.Count - 1) MessageBox.Show("Такого стовпця не існує", "Попередження");
            }
            
        }
    }
}
