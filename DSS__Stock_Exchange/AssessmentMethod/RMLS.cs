using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.AssessmentMethod
{
    class RMLS:AssessmentMethodInterface
    {
        public double[] teta(double[] Y, double[][] X, double beta, out double[] RS, out double RSS, out double R2, out double Akaike, out double dw)
        {
            Matrix teta = new Matrix(X[0].Length, 1);
            Matrix P = new Matrix(X[0].Length, beta);

            for (int i = 0; i < Y.Length; i++)
            {
                Matrix x = new Matrix(X[i].Length, 1);
                for (int j = 0; j < X[i].Length; j++)
                {
                    x[j, 0] = X[i][j];
                }

                teta += (1 / (1 + (x.trans() * P * x)[0, 0])) * P * ((Y[i] - (teta.trans() * x)[0, 0]) * x);
                P += (-1 / (1 + (x.trans() * P * x)[0, 0])) * P * x * x.trans() * P;
            }

            Matrix xxx = new Matrix(X.Length, X[0].Length);
            {
                for (int i = 0; i < Y.Length; i++)
                {
                    for (int j = 0; j < X[i].Length; j++)
                    {
                        xxx[i, j] = X[i][j];
                    }

                }
            }
            Matrix ocen_Y = xxx * teta;

            RS = new double[Y.Length];
            RSS = 0;
            dw = 0;
            double TSS = 0;
            double med = Y.Average();
            for (int i = 0; i < Y.Length; i++)
            {
                RS[i] = Y[i] - ocen_Y[i, 0]; 
                RSS += Math.Pow(RS[i], 2);
                TSS += Math.Pow(Y[i] - med, 2);
                if (i > 0) dw += Math.Pow(RS[i] - RS[i - 1], 2);
            }
            R2 = RSS / TSS;
            if (R2 > 1) R2 = 1 / R2;
            R2 = 1 - R2;
            Akaike = Y.Length * Math.Log(RSS) + 2 * (X[0].Length + 1);

            double[] rez = new double[teta.Rows];
            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = teta[i, 0];
            }

            dw /= RSS;

            return rez;
        }
    }
}
