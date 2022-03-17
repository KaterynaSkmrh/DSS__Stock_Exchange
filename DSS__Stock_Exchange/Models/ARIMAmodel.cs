using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.Models
{
    class ARIMAmodel:AbstractModel
    {
        int d;
        public ARIMAmodel(int p,int d) : base(p)
        {
            if (d>0) this.d = d;
            else throw new MyExcepion("Неприпустимий параметр d");
        }

        public ARIMAmodel(int p,int q,int d)
            : base(p,q)
        {
            if (d > 0) this.d = d;
            else throw new MyExcepion("Неприпустимий параметр d");
            if (q < 0) throw new MyExcepion("Неприпустимий параметр q");
        }

        public ARIMAmodel(int[] a, int[] b,int d)
            : base(a, b)
        {
            if (d > 0) this.d = d;
            else throw new MyExcepion("Неприпустимий параметр d");
        }

        double[] diference(double[] y)
        {
            double[] rez = new double[y.Length];
            for (int i = rez.Length - 1; i > 0; i--)
            {
                rez[i] = y[i] - y[i - 1];
            }
            rez[0] = double.NaN;
            return rez;
        }

        double[] sumar(double y_0, double[] dif)
        {
            double[] rez = new double[dif.Length];
            rez[0] = y_0 + dif[0];
            for (int i = 1; i < rez.Length; i++)
            {
                rez[i] = rez[i - 1] + dif[i];
            }
            return rez;
        }

        public int D
        {
            get
            {
                return d;
            }
        }

        public override int step
        {
            get { return Math.Max(P, Q) + D; }
        } 

        override public double[][] X(double[] y, double[] MA,int length)
        {
            if (MA != null) if (y.Length != MA.Length) throw new Exception();
            if (length <= (Math.Max(P, Q)+D) || (length > y.Length)) throw new MyExcepion("Ряд надто короткий");
            {
                int step = (Math.Max(P, Q) + D);
                
                var Y = diference(y);
                for (int i = 1; i < d; i++)
                {
                    Y = diference(Y);
                }
                
                double[][] rez = new double[length-step][];
                for (int i = 0; i < rez.Length; i++)
                {
                    if (indexes_b != null && indexes_a != null) rez[i] = new double[indexes_a.Length + indexes_b.Length];
                    else if (indexes_a != null) rez[i] = new double[indexes_a.Length];
                    else rez[i] = new double[indexes_b.Length];

                    if (indexes_a != null)
                    {
                        if (indexes_a[0] == 0) rez[i][0] = 1;
                        else rez[i][0] = Y[step + i - indexes_a[0]];

                        for (int j = 1; j < indexes_a.Length; j++)
                        {
                            rez[i][j] = Y[step + i - indexes_a[j]];
                        }
                    }
                    if (indexes_b!=null) for (int j = 0; j < indexes_b.Length; j++)
                    {
                        if (indexes_a != null) rez[i][indexes_a.Length + j] = MA[step + i - indexes_b[j]];
                        else rez[i][j] = MA[step + i - indexes_b[j]];
                    }
                }
                return rez;
            }
        }

        override public double[] Y(double[] y, int length)
        {
            if (length <= (Math.Max(P, Q) + D) || (length > y.Length)) throw new MyExcepion("Ряд надто короткий");
            int step = (Math.Max(P, Q) + D);

            var Y = diference(y);
            for (int i = 1; i < d; i++)
            {
                Y = diference(Y);
            }

            double[] rez = new double[length-step];
            for (int i = 0; i < rez.Length; i++)
            {
                rez[i] = Y[step + i];
            }
            return rez;
        }

        override public void init(double[] teta)
        {
            if (indexes_b != null && indexes_a!=null) { if (teta.Length != (a.Length + b.Length)) throw new Exception(); }
            else if (indexes_a != null) { if (teta.Length != (a.Length)) throw new Exception(); }
            else { if (teta.Length != (b.Length)) throw new Exception(); }

                if (indexes_a != null) for (int i = 0; i < a.Length; i++)
                    {
                        a[i] = teta[i];
                    }
            
            if (indexes_b!=null) for (int i = 0; i < b.Length; i++)
            {
                if (indexes_a != null) b[i] = teta[a.Length + i];
                else b[i] = teta[i];
            }
        }

        override public double[] forecast_dynamic(double[] y, double[] MA, int from, int to)
        {
            if ((from < step) || (to < step) || (from > y.Length) || (from > to)) throw new MyExcepion("Неприпустимий діапазон прогнозування");
            else
            {
                if (MA != null) if (y.Length != MA.Length) throw new Exception();
                                
                double[] starts = new double[d];
                starts[0] = y[from - 1];
                double[] Y = diference(y);
                for (int i = 1; i < d; i++)
                {
                    starts[i] = Y[from - 1];
                    Y = diference(Y);
                }
                
                double[] rez = new double[to - from + 1];
                for (int i = 0; i < rez.Length; i++)
                {
                    if (indexes_a != null)
                    {
                        if (indexes_a[0] == 0) rez[i] = a[0];
                        else if (i - indexes_a[0] >= 0) rez[i] = a[0] * rez[i - indexes_a[0]];
                        else rez[i] = a[0] * Y[from + i - indexes_a[0]];
                        for (int j = 1; j < indexes_a.Length; j++)
                        {
                            if (i - indexes_a[j] >= 0) rez[i] += a[j] * rez[i - indexes_a[j]];
                            else rez[i] += a[j] * Y[from + i - indexes_a[j]];
                        }
                    }

                    if (indexes_b!=null) for (int j = 0; j < indexes_b.Length; j++)
                    {
                        rez[i] += b[j] * MA[from + i - indexes_b[j]];
                    }
                }

                for (int i = starts.Length - 1; i >= 0; i--)
                {
                    rez = sumar(starts[i], rez);
                }

                return rez;
            }
        }

        override public double[] forecast_static(double[] y, double[] MA, int from, int to)
        {
            if ((from < step) || (to < step) || (from > y.Length) || (from > to)) throw new MyExcepion("Неприпустимий діапазон прогнозування");
            else
            {
                if (MA!=null) if (y.Length != MA.Length) throw new Exception();

                double[] starts = new double[d];
                starts[0] = y[from - 1];
                double[] Y = diference(y);
                for (int i = 1; i < d; i++)
                {
                    starts[i] = Y[from - 1];
                    Y = diference(Y);
                }
                
                double[] rez = new double[to - from + 1];
                for (int i = 0; i < rez.Length; i++)
                {
                    if (indexes_a != null)
                    {
                        if (indexes_a[0] == 0) rez[i] = a[0];
                        else rez[i] = a[0] * Y[from + i - indexes_a[0]];
                        for (int j = 1; j < indexes_a.Length; j++)
                        {
                            rez[i] += a[j] * Y[from + i - indexes_a[j]];
                        }
                    }
                    if (indexes_b != null) for (int j = 0; j < indexes_b.Length; j++)
                        {
                            rez[i] += b[j] * MA[from + i - indexes_b[j]];
                        }
                }

                for (int i = starts.Length - 1; i >= 0; i--)
                {
                    rez = sumar(starts[i], rez);
                }

                return rez;
            }
        }

        override public string ToString(string y="y",string ma="eps")
        {
            string rez = "d^" + d + " " + y + "(k)=";

            if (indexes_a != null)
            {
                if (indexes_a[0] == 0) rez += a[0];
                else rez += a[0] + "*d^" + d + " " +y+"(k-" + indexes_a[0] + ")";
                for (int j = 1; j < indexes_a.Length; j++)
                {
                    rez += "+" + a[j] + "*d^" + d + " " +y+"(k-" + indexes_a[j] + ")";
                }
                if (indexes_b != null) rez += "+";
            }
            
            if (indexes_b!=null) for (int j = 0; j < indexes_b.Length; j++)
            {
                if (j > 0) rez += "+";
                if (indexes_b[j]!=0) rez += b[j] + "*"+ma+"(k-" + indexes_b[j] + ")";
                else rez += b[j] + "*"+ma+"(k)";
            }

            return rez;
        }
    }
}
