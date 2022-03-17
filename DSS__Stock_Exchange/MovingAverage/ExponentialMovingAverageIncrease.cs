using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.MovingAverage
{
    class ExponentialMovingAverageIncrease:MovingAverageInterface
    {
        public double[] ma(double[] x, int size)
        {
            double alp = (2.0 / (size + 1));
            double[] w = new double[size];
            for (int i = 0; i < w.Length; i++)
            {
                w[i] = Math.Pow(1 - alp, size - i);
            }

            double sum = w.Sum();
            double[] rez = new double[x.Length];
            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = 0;
                for (int j = 0; j < w.Length; j++)
                {
                    if (i - j >= 0) rez[i] += w[j] * x[i - j];
                    else break;
                }
                rez[i] /= sum;
            }
            return rez;
        }
    }
}
