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
    /// Логика взаимодействия для InputLagWindow.xaml
    /// </summary>
    public partial class InputLagWindow : Window
    {
        Statistics st;
        string[] names;

        public InputLagWindow(Statistics st, string[] names)
        {
            InitializeComponent();
            this.st = st;
            this.names = names;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[][] acf = st.AСF(int.Parse(Input.Text));
                double[][] pacf = st.PAСF(int.Parse(Input.Text));

                for (int i = 0; i < names.Length; i++)
                {
                    (new ShowAFsWindow(names[i], acf[i], pacf[i])).Show();
                }
            }
            catch
            {
                MessageBox.Show("Не вдається знайти АКФ (можливо ряд містить невизначені елементи)","Помилка");
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            if (!keys.Contains(e.Key)) e.Handled = true;
        }
    }
}
