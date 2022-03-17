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
    /// Логика взаимодействия для InputSeriesNameWindow.xaml
    /// </summary>
    public partial class InputSeriesNameWindow : Window
    {
        double[] series;
        int size = -1;
        public InputSeriesNameWindow(double[] series)
        {
            InitializeComponent();
            this.series = series;
        }

        public InputSeriesNameWindow(int size)
        {
            InitializeComponent();
            this.series = null;
            this.size = size;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (series != null) TimeSeriesClass.AddDataAr(series, Input.Text);
            else TimeSeriesClass.AddDataAr(size, Input.Text);
            this.Close();
        }
    }
}
