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
    /// Логика взаимодействия для DrawTimeSeriesWindow.xaml
    /// </summary>
    public partial class ShowGraphicTimeSeriesWindow : Window
    {
        public ShowGraphicTimeSeriesWindow(double[] x, double[][] ys, string[] names)
        {
            InitializeComponent();

            try
            {
                System.Drawing.Color[] colors = {System.Drawing.Color.Green,System.Drawing.Color.Red,System.Drawing.Color.Yellow,System.Drawing.Color.Blue,System.Drawing.Color.Black };
                ZedGraphControl ZGC = new ZedGraphControl();
                GraphPane pane = ZGC.GraphPane;
                pane.Title.Text = "Часові ряди";
                pane.XAxis.Title.Text = "";
                pane.YAxis.Title.Text = "";
                for (int i = 0; i < ys.Length; i++)
                {
                    PointPairList list = new PointPairList();
                    for (int j = 0; j < ys[i].Length; j++)
                    {
                        list.Add(x[j], ys[i][j]);
                    }
                    pane.AddCurve(names[i], list, colors[i%colors.Length], SymbolType.None);                    
                }

                ZGC.AxisChange();
                ZGC.Invalidate();
                host.Child = ZGC;
                
            } 
            catch
            {
                MessageBox.Show("Не вдалося побудувати графік","Помилка");
            }

        }
    }
}
