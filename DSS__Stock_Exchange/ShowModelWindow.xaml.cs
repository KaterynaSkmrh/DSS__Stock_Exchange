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
    /// Логика взаимодействия для ShowModelWindow.xaml
    /// </summary>
    public partial class ShowModelWindow : Window
    {
        DSS__Stock_Exchange.Models.AbstractModel Model;
        double[] Y, MA;
        string name;
        System.Data.DataTable dataTable;
        public ShowModelWindow(string name,double rss,double r2,double akaike,double dw,DSS__Stock_Exchange.Models.AbstractModel Model,double[] Y,double[] MA,ModelType mt)
        {
            InitializeComponent();

            Title += name;
            this.name = name;
            TextBlock_RSS.Text = Math.Round(rss,7).ToString();
            TextBlock_R2.Text = Math.Round(r2,7).ToString();
            TextBlock_Akaike.Text = Math.Round(akaike,7).ToString();
            TextBlock_DW.Text = Math.Round(dw,7).ToString();

            switch (mt)
            {
                case ModelType.ARMA:
                    if (Model.Q >= 0 && Model.P > 0) TextBlock_Model.Text = "АРКС(" + Model.P + "," + Model.Q + ")";
                    else if (Model.P > 0) TextBlock_Model.Text = "АР(" + Model.P + ")";
                    else TextBlock_Model.Text = "КС(" + Model.Q + ")";
                    break;
                case ModelType.ARIMA:
                    if (Model.Q >= 0 && Model.P > 0) TextBlock_Model.Text = "АРІКС(" + Model.P + ","+ ((Models.ARIMAmodel)Model).D + ","+ Model.Q + ")";
                    else if (Model.P > 0) TextBlock_Model.Text = "АРІКС(" + Model.P + "," + ((Models.ARIMAmodel)Model).D + ",0)";
                    else TextBlock_Model.Text = "АРІКС(0," + ((Models.ARIMAmodel)Model).D + "," + Model.Q + ")";
                    break;
            }
            

            TextBlock_FormModel.Text = Model.ToString(y: name);
            this.Model = Model;
            this.Y = Y;
            this.MA = MA;

            TextBox_From.Text = (1 + Model.step).ToString();
            TextBox_To.Text = Y.Length.ToString();

            dataTable = new System.Data.DataTable();
            dataTable.Columns.Add(new System.Data.DataColumn(name, typeof(string)));
            dataTable.Columns.Add(new System.Data.DataColumn("Прогноз", typeof(string)));

            for (int i = 0; i < Y.Length; i++)
            {
                string[] s = new string[2];
                s[0] = Y[i].ToString();
                System.Data.DataRow row = dataTable.NewRow();
                row.ItemArray = s;
                dataTable.Rows.Add(row); 
            }

            DataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Radio_Static_Checked(object sender, RoutedEventArgs e)
        {
            if (Radio_DYnamic!=null) Radio_DYnamic.IsChecked = false;
        }

        private void Radio_DYnamic_Checked(object sender, RoutedEventArgs e)
        {
            if (Radio_Static!=null) Radio_Static.IsChecked = false;
        }

        private void TextBox_From_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            if (!keys.Contains(e.Key)) e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string[] s = new string[2];
                    s[0] = dataTable.Rows[i].ItemArray[0].ToString();
                    s[1] = "";
                    dataTable.Rows[i].ItemArray = s;
                }

                double[] rez;
                int from = int.Parse(TextBox_From.Text)-1,to=int.Parse(TextBox_To.Text)-1;
                if ((bool)Radio_Static.IsChecked)
                {
                    rez = Model.forecast_static(Y, MA, from, to);
                }
                else
                {
                    rez = Model.forecast_dynamic(Y, MA, from, to);
                }
                for (int i = 0; i < rez.Length; i++)
                {
                    if (from+i < dataTable.Rows.Count)
                    {
                        string[] s = new string[2];
                        s[0] = dataTable.Rows[from + i].ItemArray[0].ToString();
                        s[1] = rez[i].ToString();
                        dataTable.Rows[from + i].ItemArray = s;
                    }
                    else
                    {
                        string[] s = new string[2];
                        s[1] = rez[i].ToString();
                        System.Data.DataRow row = dataTable.NewRow();
                        row.ItemArray = s;
                        dataTable.Rows.Add(row); 
                    }
                }

                {
                    double[] x = new double[rez.Length];
                    double mape = double.NaN, mae = double.NaN;
                    bool nozero = true;
                    int counter=0;
                    for (int i = 0; i < x.Length; i++)
                    {
                        x[i] = from+i;

                        if (from + i < Y.Length)
                        {
                            counter++;


                            if (Y[from + i] != 0)
                            {
                                if (double.IsNaN(mape))
                                {
                                    mape = 100 * Math.Abs((Y[from + i] - rez[i]) / Y[from + i]);
                                }
                                else
                                {
                                    mape += 100 * Math.Abs((Y[from + i] - rez[i]) / Y[from + i]);
                                }
                            }
                            else nozero = false;
                            
                            
                            if (double.IsNaN(mae))
                            {
                                mae = Math.Abs(Y[from + i] - rez[i]);
                            }
                            else
                            {
                                mae += Math.Abs(Y[from + i] - rez[i]);
                            } 
                        }
                    }

                    if (!nozero) mape = double.NaN;

                    if (counter > 0)
                    {
                        mape /= counter;
                        mae /= counter;
                    }

                    (new ShowForecastWindow(x, rez, name, mape, mae,counter)).Show();
                }
            }
            catch (MyExcepion exc)
            {
                MessageBox.Show(exc.Message, "Помилка");
            }
            catch
            {
                MessageBox.Show("Некоректно задані параметри", "Помилка");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            double[] forecast=new double[dataTable.Rows.Count];
            for (int i = 0; i < forecast.Length; i++)
            {
                try
                {
                    forecast[i] = double.Parse(dataTable.Rows[i].ItemArray[1].ToString());
                }
                catch
                {
                    forecast[i] = double.NaN;
                }
            }

            TimeSeriesClass.AddDataAr(forecast,"Прогноз для "+name);
        }

        private void TextBlock_FormModel_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
