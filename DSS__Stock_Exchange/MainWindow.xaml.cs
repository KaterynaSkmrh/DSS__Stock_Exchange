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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DSS__Stock_Exchange
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TimeSeries.DataContext = TimeSeriesClass.dataTable.DefaultView;
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuItem_Click_Load_TimeSeries(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
                ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (ofd.ShowDialog() == true)
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(ofd.FileName);
                    List<double> series = new List<double>();
                    string s;
                    while ((s = reader.ReadLine()) != null)
                    {
                        series.Add(double.Parse(s));
                    }
                    reader.Close();
                    (new InputSeriesNameWindow(series.ToArray())).Show();
                }
            }
            catch
            {
                MessageBox.Show("Не вдалося відкрити файл","Помилка");
            }
        }

        private void MenuItem_Click_Add_Empty_TimeSeries(object sender, RoutedEventArgs e)
        {
            (new InputSeriesSizeWindow()).Show();
        }

        private void MenuItem_Click_Copy_TimeSeries(object sender, RoutedEventArgs e)
        {
            var SelectedItems = TimeSeries.SelectedItems;
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                (new InputSeriesNameWindow(TimeSeriesClass.ReturnRow(TimeSeries.Items.IndexOf(SelectedItems[i])))).Show();
            }
        }

        private void MenuItem_Click_Delete_TimeSeries(object sender, RoutedEventArgs e)
        {
            int SelectedIndex = TimeSeries.SelectedIndex;
            while (SelectedIndex >= 0)
            {
                TimeSeriesClass.DelDataAr(SelectedIndex);
                SelectedIndex = TimeSeries.SelectedIndex;
            }
        }

        private void MenuItem_Click_Show_TimeSeries(object sender, RoutedEventArgs e)
        {
            var SelectedItems = TimeSeries.SelectedItems;
            int[] SelectedIndexes = new int[SelectedItems.Count];
            for (int i = 0; i < SelectedIndexes.Length; i++)
            {
                SelectedIndexes[i]=TimeSeries.Items.IndexOf(SelectedItems[i]);
            }
            if (SelectedIndexes.Length>0) (new TimeSeriesShowWindow(SelectedIndexes)).Show();
        }

        private void MenuItem_Click_Save_TimeSeries(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(ofd.FileName);

                var SelectedItems = TimeSeries.SelectedItems;
                for (int i = 0; i < SelectedItems.Count; i++)
                {
                    double[] serie = TimeSeriesClass.ReturnRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
                    writer.WriteLine(TimeSeriesClass.NameRow(TimeSeries.Items.IndexOf(SelectedItems[i])));
                    for (int j = 0; j < serie.Length; j++)
                    {
                        writer.WriteLine(serie[j]);
                    }
                    writer.WriteLine();
                }

                writer.Close();
            }
        }

        private void MenuItem_Click_Show_Statistic(object sender, RoutedEventArgs e)
        {
            var SelectedItems = TimeSeries.SelectedItems;
            double[][] ryadi= new double[SelectedItems.Count][];
            string[] names=new string[SelectedItems.Count];
            for (int i = 0; i < ryadi.Length; i++)
            {
                ryadi[i]=TimeSeriesClass.ReturnRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
                names[i]=TimeSeriesClass.NameRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
            }
            Statistics st = new Statistics(ryadi);
            double[] aver = st.MathAverages();
            double[] disp = st.Variances();
            double[] asym = st.Skewnesses();
            double[] eks = st.Kurtosises();

            for (int i = 0; i < names.Length; i++)
            {
                (new ShowStatisticWindow(aver[i], disp[i], asym[i], eks[i], names[i])).Show();
            }
        }

        private void MenuItem_Click_Show_Correlation(object sender, RoutedEventArgs e)
        {
            try
            {
                var SelectedItems = TimeSeries.SelectedItems;
                double[][] ryadi = new double[SelectedItems.Count][];
                string[] names = new string[SelectedItems.Count];
                for (int i = 0; i < ryadi.Length; i++)
                {
                    ryadi[i] = TimeSeriesClass.ReturnRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
                    names[i] = TimeSeriesClass.NameRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
                }
                Statistics st = new Statistics(ryadi);
                (new ShowCorelationWindow(st.Kovariations(), st.Korelations(),names)).Show();
            }
            catch
            {
                MessageBox.Show("Ряди мають різну розмірність", "Помилка");
            }
        }

        private void MenuItem_Click_Build_Model(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            if (string.Equals(menuitem.Header,"АРКС")) (new ModelWindow(ModelType.ARMA)).Show();
            else if (string.Equals(menuitem.Header, "АРІКС")) (new ModelWindow(ModelType.ARIMA)).Show();
        }

        private void MenuItem_Click_Show_CorrelationFunctions(object sender, RoutedEventArgs e)
        {
            var SelectedItems = TimeSeries.SelectedItems;
            double[][] ryadi = new double[SelectedItems.Count][];
            string[] names = new string[SelectedItems.Count];
            for (int i = 0; i < ryadi.Length; i++)
            {
                ryadi[i] = TimeSeriesClass.ReturnRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
                names[i] = TimeSeriesClass.NameRow(TimeSeries.Items.IndexOf(SelectedItems[i]));
            }
            Statistics st = new Statistics(ryadi);

            if (SelectedItems.Count>0) (new InputLagWindow(st, names)).Show();
        }

        private void MenuItem_Click_Build_MovingAverage(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            MovingAverage.MovingAverageInterface ma;
            if (string.Equals(menuitem.Header, "Побудувати просте ковзнє середнє"))
            {
                ma = new MovingAverage.SimpleMovingAverage();
            }
            else if (string.Equals(menuitem.Header, "Побудувати експоненційне ковзнє середнє (з акцентом на остані виміри)"))
            {
                ma = new MovingAverage.ExponentialMovingAverageDecrease();
            }else 
            {
                ma = new MovingAverage.ExponentialMovingAverageIncrease();
            }

            var SelectedItems = TimeSeries.SelectedItems;
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                (new AddMovingAverageWindow(menuitem.Header.ToString(),TimeSeries.Items.IndexOf(SelectedItems[i]),ma)).Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }                
    }
}
