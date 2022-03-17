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
    /// Логика взаимодействия для TimeSeriesManipulateWindow.xaml
    /// </summary>
    public partial class TimeSeriesManipulateWindow : Window
    {
        System.Data.DataTable dataTable;
        public TimeSeriesManipulateWindow(System.Data.DataTable dataTable, string[] names_row)
        {
            InitializeComponent();
            this.dataTable = dataTable;
            ComboBox_RowName.ItemsSource = names_row;
            ComboBox_RowName.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_RowName.SelectedIndex+1].ToString());
                }

                for (int j = 0; j < tmp.Length; j++)
                {
                    if (tmp[j] > 0) tmp[j] = Math.Log(tmp[j]);
                    else throw new Exception();
                }

                for (int j = 0; j < tmp.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_RowName.SelectedIndex+1)
                        {
                            s[k] = dataTable.Rows[j].ItemArray[k].ToString();
                        }
                        else
                        {
                            s[k] = tmp[j].ToString();
                        }
                    }
                    dataTable.Rows[j].ItemArray = s;
                }
            }
            catch
            {
                MessageBox.Show("Недопустиме перетворення даних", "Помилка");
            }        
        }

        private void Osnova_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            var c = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            if ((!keys.Contains(e.Key)) && (e.Key != Key.OemComma)) e.Handled = true;
            if ((e.Key == Key.OemComma) && (Osnova.Text.Contains(c))) e.Handled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_RowName.SelectedIndex+1].ToString());
                }

                for (int j = 0; j < tmp.Length; j++)
                {
                    if (tmp[j] > 0) tmp[j] = Math.Log(tmp[j], double.Parse(Osnova.Text));
                    else throw new Exception();
                }

                for (int j = 0; j < tmp.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_RowName.SelectedIndex + 1)
                        {
                            s[k] = dataTable.Rows[j].ItemArray[k].ToString();
                        }
                        else
                        {
                            s[k] = tmp[j].ToString();
                        }
                    }
                    dataTable.Rows[j].ItemArray = s;
                }
            }
            catch
            {
                MessageBox.Show("Недопустиме перетворення даних", "Помилка");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_RowName.SelectedIndex+1].ToString());
                }

                double max = tmp.Max();
                double min = tmp.Min();
                if (max == min) throw new Exception();
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = (tmp[j] - min) / (max - min);
                }

                for (int j = 0; j < tmp.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_RowName.SelectedIndex + 1)
                        {
                            s[k] = dataTable.Rows[j].ItemArray[k].ToString();
                        }
                        else
                        {
                            s[k] = tmp[j].ToString();
                        }
                    }
                    dataTable.Rows[j].ItemArray = s;
                }
            }
            catch
            {
                MessageBox.Show("Недопустиме перетворення даних", "Помилка");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_RowName.SelectedIndex+1].ToString());
                }

                double abs_max = Math.Max(Math.Abs(tmp.Max()), Math.Abs(tmp.Min()));
                if (abs_max == 0) throw new Exception();
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] /= abs_max;
                }

                for (int j = 0; j < tmp.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_RowName.SelectedIndex + 1)
                        {
                            s[k] = dataTable.Rows[j].ItemArray[k].ToString();
                        }
                        else
                        {
                            s[k] = tmp[j].ToString();
                        }
                    }
                    dataTable.Rows[j].ItemArray = s;
                }
            }
            catch
            {
                MessageBox.Show("Недопустиме перетворення даних", "Помилка");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp.Length; j++)
                {
                    tmp[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_RowName.SelectedIndex + 1].ToString());
                }

                double[] rez = new double[tmp.Length];
                for (int j = rez.Length-1; j >0; j--)
                {
                    rez[j] = tmp[j] - tmp[j-1];
                }
                rez[0] = double.NaN;

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_RowName.SelectedIndex + 1)
                        {
                            s[k] = dataTable.Rows[j].ItemArray[k].ToString();
                        }
                        else
                        {
                            s[k] = rez[j].ToString();
                        }
                    }
                    dataTable.Rows[j].ItemArray = s;
                }
            }
            catch
            {
                MessageBox.Show("Недопустиме перетворення даних", "Помилка");
            }
        }
    }
}
