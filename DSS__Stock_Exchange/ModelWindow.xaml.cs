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
using DSS__Stock_Exchange.AssessmentMethod;
using DSS__Stock_Exchange.Models;

namespace DSS__Stock_Exchange
{
    /// <summary>
    /// Логика взаимодействия для ARCHmodelWindow.xaml
    /// </summary>
    public partial class ModelWindow : Window
    {
        ModelType mt;
        public ModelWindow(ModelType mt)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.mt = mt;
            switch (mt)
            {
                case ModelType.ARMA:
                    Title += "АРКС";
                    break;
                case ModelType.ARIMA:
                    Title += "АРІКС";
                    GroupBox_d.Visibility = System.Windows.Visibility.Visible;
                    break;
            }

            ComboBox_Chose_Series.ItemsSource = TimeSeriesClass.RowNames;            
            
            {
                string[] types_meth = { "МНК", "РМНК"};
                ComboBox_Method.ItemsSource = types_meth;
                ComboBox_Method.SelectedIndex = 0;
            }
        }

        private void TextBox_Count_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9 };
            if (!keys.Contains(e.Key)) e.Handled = true;
        }

        private void ComboBox_Chose_Series_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TextBox_Count.Text = TimeSeriesClass.ReturnRow(ComboBox_Chose_Series.SelectedIndex).Length.ToString();
                Button_Build.IsEnabled = true;
            }
            catch
            {

            }
        }

        private void CheckBox_MA_Checked(object sender, RoutedEventArgs e)
        {
            if (GroupBox_MA!=null) GroupBox_MA.IsEnabled = true;

            if (TextBox_Q_regresors!=null) TextBox_Q_regresors.Text = "0 1 2 3";
        }

        private void CheckBox_MA_Unchecked(object sender, RoutedEventArgs e)
        {
            if (GroupBox_MA != null) GroupBox_MA.IsEnabled = false;
            CheckBox_AR.IsChecked = true;

            if (TextBox_Q_regresors != null) TextBox_Q_regresors.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double[] Y = TimeSeriesClass.ReturnRow(ComboBox_Chose_Series.SelectedItem.ToString());
                AssessmentMethodInterface method;
                switch (ComboBox_Method.SelectedIndex)
                {
                    case 0:
                        method = new MLS();
                        break;
                    default:
                        method = new RMLS();
                        break;

                }
                double[] ma=null;
                AbstractModel model;
                if ((bool)CheckBox_details.IsChecked)
                {
                    int[] a=null,b=null;

                    if (!string.Equals(TextBox_P_regresors.Text,""))
                    {
                        string[] s_a = TextBox_P_regresors.Text.Split(' ');
                        a = new int[s_a.Length];
                        for (int i = 0; i < a.Length; i++)
                        {
                            a[i] = int.Parse(s_a[i]);
                        }
                    }
                    else
                    {
                        a = null;
                        MessageBox.Show("Не задано жодної складової авторегресії", "Попередження");
                    }
                    
                    if (!string.Equals(TextBox_Q_regresors.Text,""))
                    {
                        string[] s_b = TextBox_Q_regresors.Text.Split(' ');
                        b = new int[s_b.Length];
                        for (int i = 0; i < b.Length; i++)
                        {
                            b[i] = int.Parse(s_b[i]);
                        }
                    }
                    else
                    {
                        b = null;
                        MessageBox.Show("Не задано жодної складової ковзного середнього","Попередження");
                    }

                    if (b != null)
                    {
                        if ((bool)RadioButton_build_ma.IsChecked)
                        {                            
                            ma = new double[Y.Length];
                            White_Noise.Norm(ma, 0, 1);
                        }
                        else
                        {
                            ma = TimeSeriesClass.ReturnRow(ComboBox_TimeSeries_MA.SelectedItem.ToString());
                        }
                    }

                    switch (mt)
                    {
                        case ModelType.ARMA:
                            model = new ARMAmodel(a, b);
                            break;
                        default:
                            model = new ARIMAmodel(a, b, int.Parse(TextBox_d_param.Text));
                            break;
                    }
                    
                }
                else
                {
                    if ((bool)CheckBox_MA.IsChecked)
                    {
                        if ((bool)CheckBox_AR.IsChecked)
                        {
                            switch (mt)
                            {
                                case ModelType.ARMA:
                                    model = new ARMAmodel(int.Parse(TextBox_P_param.Text), int.Parse(TextBox_Q_param.Text));
                                    break;
                                default:
                                    model = new ARIMAmodel(int.Parse(TextBox_P_param.Text), int.Parse(TextBox_Q_param.Text), int.Parse(TextBox_d_param.Text));
                                    break;
                            }
                        }
                        else
                        {
                            switch (mt)
                            {
                                case ModelType.ARMA:
                                    model = new ARMAmodel(0, int.Parse(TextBox_Q_param.Text));
                                    break;
                                default:
                                    model = new ARIMAmodel(0, int.Parse(TextBox_Q_param.Text), int.Parse(TextBox_d_param.Text));
                                    break;
                            }
                        }
                        if ((bool)RadioButton_build_ma.IsChecked)
                        {
                            ma = new double[Y.Length];
                            White_Noise.Norm(ma, 0, 1);
                        }
                        else
                        {
                            ma = TimeSeriesClass.ReturnRow(ComboBox_TimeSeries_MA.SelectedItem.ToString());
                        }
                    }
                    else
                    {
                        switch (mt)
                        {
                            case ModelType.ARMA:
                                model = new ARMAmodel(int.Parse(TextBox_P_param.Text));
                                break;
                            default:
                                model = new ARIMAmodel(int.Parse(TextBox_P_param.Text), int.Parse(TextBox_d_param.Text));
                                break;
                        }
                        
                    }
                }
                
                double[] RS;
                double RSS, R2, Akaike,dw;
                model.init(method.teta(model.Y(Y, int.Parse(TextBox_Count.Text)), model.X(Y, ma, int.Parse(TextBox_Count.Text)), double.Parse(TextBox_Speed.Text), out RS, out RSS, out R2, out Akaike,out dw));
                                
                if ((bool)CheckBox_SaveResid.IsChecked)
                {
                    int step = Math.Max(model.P, model.Q);
                    double[] tmp = new double[RS.Length + step];
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (i < step) tmp[i] = double.NaN;
                        else tmp[i] = RS[i - step];
                    }
                    TimeSeriesClass.AddDataAr(tmp, "Похибки " + TimeSeriesClass.NameRow(ComboBox_Chose_Series.SelectedIndex));
                }

                (new ShowModelWindow(TimeSeriesClass.NameRow(ComboBox_Chose_Series.SelectedIndex),RSS,R2,Akaike,dw,model,Y,ma,mt)).Show();
            }
            catch (MyExcepion ex)
            {
                MessageBox.Show(ex.Message,"Помилка");
            }
            catch
            {
                MessageBox.Show("Некоректно задані параметри","Помилка");
            }
        }

        private void CheckBox_AR_Checked(object sender, RoutedEventArgs e)
        {
            if (GroupBox_AR != null) GroupBox_AR.IsEnabled = true;

            if (TextBox_P_regresors != null) TextBox_P_regresors.Text = "0 1 2 3";
        }

        private void CheckBox_AR_Unchecked(object sender, RoutedEventArgs e)
        {
            if (GroupBox_AR != null) GroupBox_AR.IsEnabled = false;
            CheckBox_MA.IsChecked = true;

            if (TextBox_P_regresors != null)  TextBox_P_regresors.Text = "";
        }

        private void ComboBox_Method_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_Method.SelectedIndex == 1)
            {
                if (Label_Speed != null) Label_Speed.Visibility = System.Windows.Visibility.Visible;
                if (TextBox_Speed != null) TextBox_Speed.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                if (Label_Speed != null) Label_Speed.Visibility = System.Windows.Visibility.Hidden;
                if (TextBox_Speed != null) TextBox_Speed.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void RadioButton_build_ma_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioButton_load_ma!=null) RadioButton_load_ma.IsChecked = false;
        }

        private void RadioButton_build_ma_Unchecked(object sender, RoutedEventArgs e)
        {
        }

        private void RadioButton_load_ma_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioButton_build_ma != null) RadioButton_build_ma.IsChecked = false;
            if (Grid_MA_load != null) 
            {
                ComboBox_TimeSeries_MA.ItemsSource = TimeSeriesClass.RowNames;
                Grid_MA_load.Visibility = System.Windows.Visibility.Visible; 
            }
        }

        private void RadioButton_load_ma_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Grid_MA_load != null) Grid_MA_load.Visibility = System.Windows.Visibility.Hidden;
        }

        private void TextBox_P_regresors_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.Space };
            if (!keys.Contains(e.Key)) e.Handled = true;            
        }

        private void CheckBox_details_Checked(object sender, RoutedEventArgs e)
        {
            if (Grid_simple_p != null) Grid_simple_p.Visibility = System.Windows.Visibility.Hidden;
            if (Grid_detail_p != null) Grid_detail_p.Visibility = System.Windows.Visibility.Visible;

            if (Grid_simple_q != null) Grid_simple_q.Visibility = System.Windows.Visibility.Hidden;
            if (Grid_detail_q != null) Grid_detail_q.Visibility = System.Windows.Visibility.Visible;
        }

        private void CheckBox_details_Unchecked(object sender, RoutedEventArgs e)
        {
            if (Grid_simple_p != null) Grid_simple_p.Visibility = System.Windows.Visibility.Visible;
            if (Grid_detail_p != null) Grid_detail_p.Visibility = System.Windows.Visibility.Hidden;

            if (Grid_simple_q != null) Grid_simple_q.Visibility = System.Windows.Visibility.Visible;
            if (Grid_detail_q != null) Grid_detail_q.Visibility = System.Windows.Visibility.Hidden;
        }

        private void TextBox_Q_regresors_KeyDown(object sender, KeyEventArgs e)
        {
            Key[] keys = { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.Space };
            if (!keys.Contains(e.Key)) e.Handled = true;    
        }

        private void TextBox_P_regresors_LostFocus(object sender, RoutedEventArgs e)
        {            
            string s=TextBox_P_regresors.Text;
            if (s.Length > 0)
            {
                while (s.Length > 0 && s[0] == ' ')
                {
                    s = s.Remove(0, 1);
                }
                for (int i = 1; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        if (i + 1 < s.Length) while (s[i + 1] == ' ')
                        {
                            s = s.Remove(i, 1);
                            if (i + 1 >= s.Length) break;
                        }
                    }
                }
                if (s.Length>0) if (s[s.Length - 1] == ' ') s = s.Remove(s.Length - 1,1);
                TextBox_P_regresors.Text = s;
            }
        }

        private void TextBox_Q_regresors_LostFocus(object sender, RoutedEventArgs e)
        {
            string s = TextBox_Q_regresors.Text;
            if (s.Length > 0)
            {
                while (s.Length > 0 && s[0] == ' ')
                {
                    s = s.Remove(0, 1);
                }
                for (int i = 1; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        if (i + 1 < s.Length) while (s[i + 1] == ' ')
                            {
                                s = s.Remove(i, 1);
                                if (i + 1 >= s.Length) break;
                            }
                    }
                }
                if (s.Length > 0) if (s[s.Length - 1] == ' ') s = s.Remove(s.Length - 1, 1);
                TextBox_Q_regresors.Text = s;
            }
        }
    }
}
