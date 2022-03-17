using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.MovingAverage
{
    class SimpleMovingAverage:MovingAverageInterface
    {
        public double[] ma(double[] x, int size)
        {
            double[] rez = new double[x.Length];
            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = 0;
                for (int j = 0; j < size; j++)
                {
                    if (i - j >= 0) rez[i] += x[i - j];
                    else break;
                }
                 rez[i] /= size;
            }
            return rez;
        } 
    }
}
