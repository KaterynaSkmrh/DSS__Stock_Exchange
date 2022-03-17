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
    /// Логика взаимодействия для AddMovingAverageWindow.xaml
    /// </summary>
    public partial class AddMovingAverageWindow : Window
    {
        int index;
        MovingAverage.MovingAverageInterface ma;
        public AddMovingAverageWindow(string ma_type, int index, MovingAverage.MovingAverageInterface ma)
        {
            InitializeComponent();
            Title = ma_type;
            TextBox_NewName.Text += TimeSeriesClass.NameRow(index);
            this.index = index;
            this.ma = ma;
        }

        private void TextBox_Count_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            if (!keys.Contains(e.Key)) e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TimeSeriesClass.AddDataAr(ma.ma(TimeSeriesClass.ReturnRow(index),int.Parse(TextBox_Size_Window.Text)),TextBox_NewName.Text);
            this.Close();
        }
    }
}
