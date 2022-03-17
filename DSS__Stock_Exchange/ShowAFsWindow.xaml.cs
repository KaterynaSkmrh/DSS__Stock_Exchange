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
using ZedGraph;

namespace DSS__Stock_Exchange
{
    /// <summary>
    /// Логика взаимодействия для ShowAFsWindow.xaml
    /// </summary>
    public partial class ShowAFsWindow : Window
    {
        double[] acf;
        double[] pacf;
        public ShowAFsWindow(string name,double[] acf,double[] pacf)
        {
            InitializeComponent();
            Title += name;
            this.acf = acf;
            this.pacf = pacf;
            
            {
                var c = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
                textBox_porig.Text = "0"+c+"2";
            }

            MenuItem_PACF_Click(null,null);

        }

        private void textBox_porig_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            var c = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;
            if ((!keys.Contains(e.Key)) && (e.Key != Key.OemComma)) e.Handled = true;
            if ((e.Key == Key.OemComma) && (textBox_porig.Text.Contains(c))) e.Handled = true;
        }        

        private void MenuItem_ACF_Click(object sender, RoutedEventArgs e)
        {
            ZedGraphControl ZGC = new ZedGraphControl();
            GraphPane pane = ZGC.GraphPane;
            pane.CurveList.Clear();
            pane.Title.Text = "Автокореляційна функція";
            BarItem curve = pane.AddBar("", null, acf, System.Drawing.Color.Red);
            pane.XAxis.Type = AxisType.Text;
            
            string[] names_x = new string[acf.Length];
            for (int i = 0; i < names_x.Length; i++)
            {
                names_x[i] = (i + 1).ToString();
            }
            pane.XAxis.Scale.TextLabels = names_x;
            
            pane.XAxis.Title.Text = "лаг";
            pane.YAxis.Title.Text = "АКФ";

            ZGC.AxisChange();
            ZGC.Invalidate();

            {
                LineObj line = new LineObj(pane.XAxis.Scale.Min, double.Parse(textBox_porig.Text), pane.XAxis.Scale.Max, double.Parse(textBox_porig.Text));
                line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                pane.GraphObjList.Add(line);
            }

            {
                LineObj line = new LineObj(pane.XAxis.Scale.Min, -double.Parse(textBox_porig.Text), pane.XAxis.Scale.Max, -double.Parse(textBox_porig.Text));
                line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                pane.GraphObjList.Add(line);
            }

            pane.YAxis.Scale.Min = Math.Min(-double.Parse(textBox_porig.Text), acf.Min()) - 0.1;
            pane.YAxis.Scale.Max = Math.Max(double.Parse(textBox_porig.Text), acf.Max()) + 0.1;

            host.Child = ZGC;

            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add(new System.Data.DataColumn("Лаг", typeof(int)));
                dt.Columns[0].AutoIncrement = true;
                dt.Columns[0].AutoIncrementSeed = 1;
                dt.Columns.Add(new System.Data.DataColumn("АКФ", typeof(double)));
                for (int i = 0; i < acf.Length; i++)
                {
                    string[] s = new string[2];
                    s[1] = acf[i].ToString();
                    System.Data.DataRow row = dt.NewRow();
                    row.ItemArray = s;
                    dt.Rows.Add(row);
                }

                DataGrid.ItemsSource = dt.DefaultView;
            }
        }

        private void MenuItem_PACF_Click(object sender, RoutedEventArgs e)
        {
            ZedGraphControl ZGC = new ZedGraphControl();
            GraphPane pane = ZGC.GraphPane;
            pane.CurveList.Clear();
            pane.Title.Text = "Часткова автокореляційна функція";
            BarItem curve = pane.AddBar("", null, pacf, System.Drawing.Color.Blue);
            pane.XAxis.Type = AxisType.Text;

            string[] names_x = new string[pacf.Length];
            for (int i = 0; i < names_x.Length; i++)
            {
                names_x[i] = (i + 1).ToString();
            }
            pane.XAxis.Scale.TextLabels = names_x;

            pane.XAxis.Title.Text = "лаг";
            pane.YAxis.Title.Text = "ЧАКФ";

            ZGC.AxisChange();
            ZGC.Invalidate();

            {
                LineObj line = new LineObj(pane.XAxis.Scale.Min, double.Parse(textBox_porig.Text), pane.XAxis.Scale.Max, double.Parse(textBox_porig.Text));
                line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                pane.GraphObjList.Add(line);
            }

            {
                LineObj line = new LineObj(pane.XAxis.Scale.Min, -double.Parse(textBox_porig.Text), pane.XAxis.Scale.Max, -double.Parse(textBox_porig.Text));
                line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                pane.GraphObjList.Add(line);
            }

            pane.YAxis.Scale.Min = Math.Min(-double.Parse(textBox_porig.Text),pacf.Min())-0.1;
            pane.YAxis.Scale.Max = Math.Max(double.Parse(textBox_porig.Text),pacf.Max())+0.1;

            host.Child = ZGC;

            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add(new System.Data.DataColumn("Лаг", typeof(int)));
                dt.Columns[0].AutoIncrement = true;
                dt.Columns[0].AutoIncrementSeed = 1;
                dt.Columns.Add(new System.Data.DataColumn("ЧАКФ", typeof(double)));
                for (int i = 0; i < pacf.Length; i++)
                {
                    string[] s = new string[2];
                    s[1] = pacf[i].ToString();
                    System.Data.DataRow row = dt.NewRow();
                    row.ItemArray = s;
                    dt.Rows.Add(row);
                }

                DataGrid.ItemsSource = dt.DefaultView;
            }
        }
    }
}
