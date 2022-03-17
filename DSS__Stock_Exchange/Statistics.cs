using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS__Stock_Exchange.AssessmentMethod;

namespace DSS__Stock_Exchange
{
    public class Statistics
    {
        double[][] TimeSeriesArray;
        public Statistics(double[][] TimeSeriesArray)
        {
            this.TimeSeriesArray = new double[TimeSeriesArray.Length][];
            for (int i = 0; i < this.TimeSeriesArray.Length; i++)
            {
                this.TimeSeriesArray[i] = new double[TimeSeriesArray[i].Length];
                for (int j = 0; j < this.TimeSeriesArray[i].Length; j++)
                {
                    this.TimeSeriesArray[i][j] = TimeSeriesArray[i][j];
                }
            }
        }

        public double[] MathAverages()
        {
            double[] rez = new double[TimeSeriesArray.Length];
            for (int i = 0; i < rez.Length; i++) rez[i] = TimeSeriesArray[i].Average();
            return rez;
        }

        public double[] Variances()
        {
            double[] rez = new double[TimeSeriesArray.Length];
            for (int i = 0; i < rez.Length; i++)
            {
                double aver = TimeSeriesArray[i].Average();
                rez[i] = 0;
                for (int j = 0; j < TimeSeriesArray[i].Length; j++)
                {
                    rez[i] += Math.Pow(TimeSeriesArray[i][j]-aver, 2);
                }
                rez[i] /= TimeSeriesArray[i].Length - 1;
            }
            return rez;
        }

        public double[] Skewnesses()
        {
            double[] rez = new double[TimeSeriesArray.Length];
            double[] disp = Variances();
            for (int i = 0; i < rez.Length; i++)
            {
                double aver = TimeSeriesArray[i].Average();
                rez[i] = 0;
                for (int j = 0; j < TimeSeriesArray[i].Length; j++)
                {
                    rez[i] += Math.Pow(TimeSeriesArray[i][j] - aver, 3);
                }
                rez[i] /= TimeSeriesArray[i].Length;
                rez[i] /= Math.Pow(Math.Sqrt(disp[i]), 3);
            }
            return rez;
        }

        public double[] Kurtosises()
        {
            double[] rez = new double[TimeSeriesArray.Length];
            double[] disp = Variances();
            for (int i = 0; i < rez.Length; i++)
            {
                double aver = TimeSeriesArray[i].Average();
                rez[i] = 0;
                for (int j = 0; j < TimeSeriesArray[i].Length; j++)
                {
                    rez[i] += Math.Pow(TimeSeriesArray[i][j] - aver, 4);
                }
                rez[i] /= TimeSeriesArray[i].Length;
                rez[i] /= Math.Pow(Math.Sqrt(disp[i]), 4);
                //rez[i] -= 3;
            }
            return rez;
        }

        public double[][] Kovariations()
        {
            for (int i = 0; i < TimeSeriesArray.Length - 1; i++)
            {
                if (TimeSeriesArray[i].Length != TimeSeriesArray[i + 1].Length) throw new Exception();
            }

            double[][] rez = new double[TimeSeriesArray.Length][];
            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = new double[TimeSeriesArray.Length];
                for (int j = 0; j < rez[i].Length; j++)
                {
                    double aver_i = TimeSeriesArray[i].Average(), aver_j = TimeSeriesArray[j].Average();
                    rez[i][j] = 0;
                    for (int k = 0; k < TimeSeriesArray[i].Length; k++)
                    {
                        rez[i][j] += (TimeSeriesArray[i][k] - aver_i) * (TimeSeriesArray[j][k] - aver_j);
                    }
                    rez[i][j] /= TimeSeriesArray[i].Length - 1;
                }
            }

            return rez;
        }

        public double[][] Korelations()
        {
            double[][] rez = Kovariations();
            double[] disp = Variances();
            for (int i = 0; i < rez.Length; i++)
            {
                for (int j = 0; j < rez[i].Length; j++)
                {
                    rez[i][j] /= Math.Sqrt(disp[i]) * Math.Sqrt(disp[j]);
                }
            }
            return rez;
        }

        public double[][] AСF(int lag)
        {
            double[][] rez=new double[TimeSeriesArray.Length][];
            double[] ser = MathAverages();
            double[] disp = Variances();

            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = new double[lag];
                for (int j = 0; j < rez[i].Length; j++)
                {
                    rez[i][j] = 0;
                    for (int k = j + 1; k < TimeSeriesArray[i].Length; k++)
                    {
                        rez[i][j] += (TimeSeriesArray[i][k] - ser[i]) * (TimeSeriesArray[i][k - (j + 1)] - ser[i]);
                    }
                    rez[i][j] /= disp[i];
                    rez[i][j] /= TimeSeriesArray[i].Length - 1;
                }
            }

            return rez;
        }

        public double[][] PAСF(int lag)
        {
            double[][] rez = new double[TimeSeriesArray.Length][];
            double[][] acf = AСF(lag);

            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = new double[lag];
                for (int j = 0; j < rez[i].Length; j++)
                {
                    Matrix matr = new Matrix(j+1);
                    for (int k = 0; k < j; k++)
                    {
                        for (int p = 0; p < j-k; p++)
                        {
                            matr[k, k + p + 1] = acf[i][p];
                            matr[k + p + 1, k] = acf[i][p];
                        }
                    }

                    Matrix vect = new Matrix(j+1,1);
                    for (int k = 0; k < j+1; k++)
                    {
                        vect[k, 0] = acf[i][k];
                    }
                    rez[i][j] = (matr.inverse()*vect)[j,0];
                }
            }

            return rez;
        }
    }
}
