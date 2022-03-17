using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange
{
    class White_Noise
    {
        static public void Norm(double[] massive, double mu, double sigma)
        {
            double dSumm = 0, dRandValue = 0;
            Random ran = new Random();
            for (int n = 0; n < massive.Length; n++)
            {
                dSumm = 0;
                for (int i = 0; i < 100; i++)
                {
                    double R = ran.NextDouble();
                    dSumm = dSumm + R;
                }
                dRandValue = mu + sigma * (Math.Sqrt(12) * (dSumm - 50) / 10);
                massive[n] = dRandValue;
            }
        }
    }
}
