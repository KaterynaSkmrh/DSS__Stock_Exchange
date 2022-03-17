using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.AssessmentMethod
{
    class Matrix 
    {
        double[][] mat;//масив строк

        public Matrix(int m, int n)
        {
            mat = new double[m][];
            for (int i=0;i<mat.Length;i++)
            {
                mat[i] = new double[n];
            }
        }

        public Matrix(int n, double beta)
        {
            mat = new double[n][];
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = new double[n];
            }

            for (int i = 0; i < n; i++)
            {
                this[i, i] = beta;
            }
        }

        public Matrix(int n)
        {
            mat = new double[n][];
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = new double[n];
            }

            for (int i = 0; i < n; i++)
            {
                this[i, i] = 1;
            }
        }

        public double this[int i, int j]
        {
            get
            {
                return mat[i][j];
            }
            set
            {
                mat[i][j] = value;
            }
        }

        public static Matrix operator* (Matrix a,Matrix b)
        {
            if (a.mat[0].Length!=b.mat.Length) throw new Exception();
            else 
            {
                Matrix rez =new Matrix(a.mat.Length,b.mat[0].Length);

                for (int i=0;i<rez.mat.Length;i++)
                {
                    for (int j=0;j<rez.mat[i].Length;j++)
                    {
                        rez[i,j] = 0;
                        for (int k=0;k<b.mat.Length;k++)
                        {
                            rez[i,j] += a[i,k]*b[k,j];
                        }
                    }
                }

                return rez;
            }
        }

        public static Matrix operator *(double al, Matrix a)
        {
            Matrix rez = new Matrix(a.mat.Length, a.mat[0].Length);

            for (int i = 0; i < rez.mat.Length; i++)
            {
                for (int j = 0; j < rez.mat[i].Length; j++)
                {
                    rez[i, j] = al*a[i,j];                    
                }
            }

            return rez;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if ((a.mat.Length != b.mat.Length) || (a.mat[0].Length != b.mat[0].Length)) throw new Exception();
            else
            {
                Matrix rez = new Matrix(a.mat.Length, a.mat[0].Length);

                for (int i = 0; i < rez.mat.Length; i++)
                {
                    for (int j = 0; j < rez.mat[i].Length; j++)
                    {
                        rez[i, j] = a[i, j] + b[i, j];                        
                    }
                }

                return rez;
            }
        }

        Matrix minor(int stroka, int stolbec)
        {
            Matrix rez = new Matrix(this.mat.Length-1,this.mat[0].Length-1);

            for (int i = 0; i < this.mat.Length; i++)
            {
                for (int j = 0; j < this.mat[i].Length; j++)
                {
                    if ((i < stroka) && (j < stolbec)) rez[i, j] = this[i, j];
                    if ((i < stroka) && (j > stolbec)) rez[i, j-1] = this[i, j];
                    if ((i > stroka) && (j < stolbec)) rez[i-1, j] = this[i, j];
                    if ((i > stroka) && (j > stolbec)) rez[i-1, j-1] = this[i, j];
                }
            }

            return rez;
        }

        public double determinant()
        {
            if (this.mat.Length != this.mat[0].Length) throw new Exception();
            else
            {
                if (this.mat.Length < 2) return this[0, 0];
                else
                {
                    double rez = 0;
                    for (int i = 0; i < mat[0].Length; i++)
                    {
                        rez += Math.Pow(-1, i) * this[0, i] * this.minor(0, i).determinant();
                    }

                    return rez;
                }
            }
        }

        public Matrix trans()
        {
            Matrix rez = new Matrix(this.mat[0].Length,this.mat.Length);

            for (int i = 0; i < rez.mat.Length; i++)
            {
                for (int j = 0; j < rez.mat[i].Length; j++)
                {
                    rez[i, j] = this[j, i];
                }             
            }

            return rez;
        }

        public Matrix inverse()
        {
            if (this.mat.Length != this.mat[0].Length) throw new Exception();
            else
            {
                int n = mat.Length;

                double[,] A = new double[n, n];

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        A[i, j] = this[i, j];
                    }
                }

                int info;
                alglib.matinvreport rep;
                alglib.spdmatrixinverse(ref A, out info, out rep); // A^(-1)

                Matrix rez = new Matrix(n, n);

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        rez[i, j] = A[i,j];
                    }
                }
                
                return rez;
            }
        }

        public int Rows
        {
            get
            {
                return mat.Length;
            }
        }

        public int Columns
        {
            get
            {
                return mat[0].Length;
            }
        }        
    }
}
