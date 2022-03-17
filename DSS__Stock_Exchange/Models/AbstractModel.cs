using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.Models
{
    public abstract class AbstractModel
    {
        protected double[] a, b;
        protected int[] indexes_a, indexes_b;
        /// <summary>
        /// q==-1 для AP модели 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        protected AbstractModel(int p=-1, int q=-1)
        {
            if (p >= 0)
            {
                indexes_a = new int[p + 1];
                for (int i = 0; i < indexes_a.Length; i++)
                {
                    indexes_a[i] = i;
                }
                a = new double[p + 1];
            } 
            if (q >= 0)
            {
                indexes_b = new int[q + 1];
                for (int i = 0; i < indexes_b.Length; i++)
                {
                    indexes_b[i] = i;
                }
                b = new double[q + 1];
            }
            if ((indexes_a == null) && (indexes_b == null)) throw new MyExcepion("Неприпустимі параметри моделі");
        }

        protected AbstractModel(int[] a = null, int[] b = null)
        {
            if (a != null)
            {
                indexes_a = new int[a.Length];
                this.a = new double[a.Length];
                for (int i = 0; i < indexes_a.Length; i++)
                {
                    indexes_a[i] = a[i];
                }
            }
            if (b != null)
            {
                indexes_b = new int[b.Length];
                this.b = new double[b.Length];
                for (int i = 0; i < indexes_b.Length; i++)
                {
                    indexes_b[i] = b[i];
                }
            }
            if ((indexes_a == null) && (indexes_b == null)) throw new MyExcepion("До моделі не включено жодної складової");
        }

        public int P
        {
            get
            {
                if (indexes_a == null) return -1;
                else return indexes_a[indexes_a.Length-1];
            }
        }

        public int Q
        {
            get
            {
                if (indexes_b == null) return -1;
                else return indexes_b[indexes_b.Length-1]; 
            }
        }

        public abstract int step
        {
            get;
        }

        abstract public double[][] X(double[] Y,double[] MA,int length);
        abstract public double[] Y(double[] Y, int length);
        abstract public void init(double[] teta);
        abstract public double[] forecast_dynamic(double[] Y, double[] MA,int from,int to);
        abstract public double[] forecast_static(double[] Y, double[] MA,int from,int to);
        abstract public string ToString(string y = "y", string ma = "ma");
    }
}
