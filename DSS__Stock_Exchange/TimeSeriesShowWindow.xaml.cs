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
using System.Data;

namespace DSS__Stock_Exchange
{
    /// <summary>
    /// Логика взаимодействия для TimeSeriesShowWindow.xaml
    /// </summary>    
    public partial class TimeSeriesShowWindow : Window
    {
        DataTable dataTable;
        int[] indexes;
        public TimeSeriesShowWindow(int[] indexes)
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Номер", typeof(int)));
            dataTable.Columns[0].AutoIncrement = true;
            dataTable.Columns[0].AutoIncrementSeed = 1;
            int maxLength = 0;
            for (int i = 0; i < indexes.Length; i++)
            {
                dataTable.Columns.Add(new DataColumn(TimeSeriesClass.NameRow(indexes[i]), typeof(double)));
                if (maxLength < TimeSeriesClass.LenghtRow(indexes[i])) maxLength = TimeSeriesClass.LenghtRow(indexes[i]);
            }

            for (int i = 0; i < maxLength; i++)
            {
                string[] tmp = new string[indexes.Length + 1];
                for (int j = 0; j < indexes.Length; j++)
                {
                    if (i < TimeSeriesClass.LenghtRow(indexes[j]))
                    {
                        tmp[j + 1] = TimeSeriesClass.Element(indexes[j], i).ToString();
                    }
                    else
                    {
                        tmp[j + 1] = double.NaN.ToString();
                    }
                }
                DataRow row = dataTable.NewRow();
                row.ItemArray = tmp;
                dataTable.Rows.Add(row);
            }

            TimeSeries.DataContext = dataTable.DefaultView;
            this.indexes = indexes;
        }

        private void Button_Click_Save_TimeSeries(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < indexes.Length; i++)
                {
                    double[] tmp = new double[dataTable.Rows.Count];
                    for (int j = 0; j < dataTable.Rows.Count; j++)
                    {
                        tmp[j] = double.Parse(dataTable.Rows[j].ItemArray[i+1].ToString());
                    }
                    TimeSeriesClass.ReplaceDataAr(indexes[i], tmp);
                    TimeSeriesClass.EditNameRow(indexes[i],TimeSeries.Columns[i+1].Header.ToString());
                }

            }
            catch
            {
                MessageBox.Show("Некоректні дані"," Помилка");
            }
        }

        private void Button_Click_Rename_TimeSeries(object sender, RoutedEventArgs e)
        {
            (new RenameSeriesWindow(TimeSeries.Columns)).Show();
        }

        private void Button_Click_Manipulate_TimeSeries(object sender, RoutedEventArgs e)
        {
            string[] names_row = new string[TimeSeries.Columns.Count - 1];
            for (int i = 0; i < names_row.Length; i++)
            {
                names_row[i] = TimeSeries.Columns[i + 1].Header.ToString();
            }
            (new TimeSeriesManipulateWindow(dataTable, names_row)).Show();
        }

        private void Button_Click_Show_TimeSeries(object sender, RoutedEventArgs e)
        {
            double[] x = new double[dataTable.Rows.Count];
            double[][] ys = new double[TimeSeries.Columns.Count - 1][];
            string[] names = new string[TimeSeries.Columns.Count - 1];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = double.Parse(dataTable.Rows[i].ItemArray[0].ToString());
            }
            for (int i = 0; i < names.Length; i++)
            {
                names[i] = TimeSeries.Columns[i+1].Header.ToString();
            }
            for (int i = 0; i < ys.Length; i++)
            {
                ys[i] = new double[dataTable.Rows.Count];
                for (int j = 0; j < ys[i].Length; j++)
                {
                    ys[i][j] = double.Parse(dataTable.Rows[j].ItemArray[i+1].ToString());
                }
            }

            (new ShowGraphicTimeSeriesWindow(x, ys, names)).Show();
        }

        private void Button_Click_Operations_TimeSeries(object sender, RoutedEventArgs e)
        {
            string[] names_row = new string[TimeSeries.Columns.Count - 1];
            for (int i = 0; i < names_row.Length; i++)
            {
                names_row[i] = TimeSeries.Columns[i + 1].Header.ToString();
            }
            (new TimeSeriesOperationWindow(dataTable, names_row)).Show();
        }
    }
}
