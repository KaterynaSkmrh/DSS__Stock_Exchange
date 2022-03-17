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
    /// Логика взаимодействия для ShowForecastWindow.xaml
    /// </summary>
    public partial class ShowForecastWindow : Window
    {
        public ShowForecastWindow(double[] x, double[] fr, string name,double mape,double mae,int counter)
        {
            InitializeComponent();

            {
                Title += name;
                TextBlock_MAPE.Text += mape + " %";
                TextBlock_MAE.Text += mae;
            }

            {
                ZedGraphControl ZGC = new ZedGraphControl();
                GraphPane pane = ZGC.GraphPane;
                pane.Title.Text = "Прогноз для ряду "+name;
                pane.XAxis.Title.Text = "";
                pane.YAxis.Title.Text = "";
                PointPairList list = new PointPairList();
                PointPairList list1 = new PointPairList();
                PointPairList list2 = new PointPairList();
                for (int j = 0; j < fr.Length; j++)
                {
                    list.Add(x[j], fr[j]);
                    if (j<counter)
                    {
                        list1.Add(x[j], fr[j] + mae);
                        list2.Add(x[j], fr[j] - mae);
                    }
                }
                pane.AddCurve("Прогноз", list, System.Drawing.Color.Blue, SymbolType.None);
                pane.AddCurve("Середні межі відхилення", list1, System.Drawing.Color.Red, SymbolType.None);
                pane.AddCurve(null, list2, System.Drawing.Color.Red, SymbolType.None);
                ZGC.AxisChange();
                ZGC.Invalidate();
                host.Child = ZGC;
            }
        }
    }
}
