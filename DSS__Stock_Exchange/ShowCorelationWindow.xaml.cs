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
    /// Логика взаимодействия для ShowCorelationWindow.xaml
    /// </summary>
    public partial class ShowCorelationWindow : Window
    {
        System.Data.DataTable Kov, Kor;
        public ShowCorelationWindow(double[][] kov,double[][] kor,string[] names)
        {
            InitializeComponent();

            Kov=new System.Data.DataTable();
            Kor=new System.Data.DataTable();

            for (int i = 0; i < kov[0].Length; i++)
            {
                this.Kov.Columns.Add(new System.Data.DataColumn(names[i], typeof(double)));
                this.Kor.Columns.Add(new System.Data.DataColumn(names[i], typeof(double)));
            }
            for (int i = 0; i < kov.Length; i++)
            {
                {
                    string[] s = new string[kov[i].Length];
                    for (int j = 0; j < s.Length; j++)
                    {
                        s[j] = kov[i][j].ToString();
                    }
                    System.Data.DataRow row = this.Kov.NewRow();
                    row.ItemArray = s;
                    this.Kov.Rows.Add(row);
                }

                {
                    string[] s = new string[kor[i].Length];
                    for (int j = 0; j < s.Length; j++)
                    {
                        s[j] = kor[i][j].ToString();
                    }
                    System.Data.DataRow row = this.Kor.NewRow();
                    row.ItemArray = s;
                    this.Kor.Rows.Add(row);
                }

                Variations.ItemsSource = Kov.DefaultView;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Variations.ItemsSource = Kov.DefaultView;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Variations.ItemsSource = Kor.DefaultView;
        }
    }
}
