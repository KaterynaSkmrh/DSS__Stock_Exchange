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
    /// Логика взаимодействия для ShowStatisticWindow.xaml
    /// </summary>
    public partial class ShowStatisticWindow : Window
    {
        public ShowStatisticWindow(double average,double variance, double skewness, double kutosis, string ryad)
        {
            InitializeComponent();

            TextBlock_Average.Text = average.ToString();
            TextBlock_Variancz.Text = variance.ToString();
            TextBlock_Skewness.Text = skewness.ToString();
            TextBlock_Kutosis.Text = kutosis.ToString();
            Title += ryad;
        }
    }
}
