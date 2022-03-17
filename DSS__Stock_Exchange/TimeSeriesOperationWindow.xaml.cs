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
    /// Логика взаимодействия для TimeSeriesOperationWindow.xaml
    /// </summary>
    public partial class TimeSeriesOperationWindow : Window
    {
        System.Data.DataTable dataTable;
        public TimeSeriesOperationWindow(System.Data.DataTable dataTable, string[] names_row)
        {
            InitializeComponent();
            this.dataTable = dataTable;
            ComboBox_Row1.ItemsSource = names_row;
            ComboBox_Row2.ItemsSource = names_row;
            ComboBox_ResultRow.ItemsSource = names_row;
        }

        private void TextBox_Osnova_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            var c = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            if ((!keys.Contains(e.Key)) && (e.Key != Key.OemComma)) e.Handled = true;
            if ((e.Key == Key.OemComma) && (TextBox_Osnova.Text.Contains(c))) e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp1 = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp1.Length; j++)
                {
                    try
                    {
                        tmp1[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString());
                    }
                    catch
                    {
                        tmp1[j] = double.NaN;
                    }
                    try
                    {
                        tmp2[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row2.SelectedIndex + 1].ToString());
                    }
                    catch
                    {
                        tmp2[j] = double.NaN;
                    }
                }

                double[] rez = new double[tmp1.Length];
                for (int j = 0; j < rez.Length; j++)
                {
                    rez[j] = tmp1[j] + tmp2[j];
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp1 = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp1.Length; j++)
                {
                    try
                    {
                        tmp1[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString());
                    }
                    catch
                    {
                        tmp1[j] = double.NaN;
                    }
                    try
                    {
                        tmp2[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row2.SelectedIndex + 1].ToString());
                    }
                    catch
                    {
                        tmp2[j] = double.NaN;
                    }
                }

                double[] rez = new double[tmp1.Length];
                for (int j = 0; j < rez.Length; j++)
                {
                    rez[j] = tmp1[j] - tmp2[j];
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] rez = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < rez.Length; j++)
                {
                    rez[j] = double.Parse(TextBox_Osnova.Text) * double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString());
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp1 = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp1.Length; j++)
                {
                    tmp1[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString());
                    tmp2[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row2.SelectedIndex + 1].ToString());
                }

                double[] rez = new double[tmp1.Length];
                for (int j = 0; j < rez.Length; j++)
                {
                    rez[j] = tmp1[j] * tmp2[j];
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] tmp1 = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < tmp1.Length; j++)
                {
                    tmp1[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString());
                    tmp2[j] = double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row2.SelectedIndex + 1].ToString());
                }

                double[] rez = new double[tmp1.Length];
                for (int j = 0; j < rez.Length; j++)
                {
                    rez[j] = tmp1[j] / tmp2[j];
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] rez = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < rez.Length; j++)
                {
                    rez[j] = 1 / double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString());
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {                
                double[] rez = new double[dataTable.Rows.Count], tmp2 = new double[dataTable.Rows.Count];
                for (int j = 0; j < rez.Length; j++)
                {
                    try
                    {
                        rez[j] = Math.Pow(double.Parse(dataTable.Rows[j].ItemArray[ComboBox_Row1.SelectedIndex + 1].ToString()), double.Parse(TextBox_Stepen.Text));
                    }
                    catch
                    {
                        rez[j] = double.NaN;
                    }
                }

                for (int j = 0; j < rez.Length; j++)
                {
                    string[] s = new string[dataTable.Columns.Count];
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (k != ComboBox_ResultRow.SelectedIndex + 1)
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
