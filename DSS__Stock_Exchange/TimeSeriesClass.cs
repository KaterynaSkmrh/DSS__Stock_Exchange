using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Data;

namespace DSS__Stock_Exchange
{
    static class TimeSeriesClass
    {
        private static List<double[]> dataArrays;
        public static DataTable dataTable { get; set; }

        static TimeSeriesClass()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Номер ряду", typeof(int)));
            dataTable.Columns[0].AutoIncrement = true;
            dataTable.Columns[0].AutoIncrementSeed = 1;
            dataTable.Columns.Add(new DataColumn("Назва ряду", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Довжина ряду", typeof(string)));

            dataArrays = new List<double[]>();
        }

        public static void AddDataAr(double[] newDataAr, string nameNewAr)
        {
            while (IndexRow(nameNewAr) != -1)
            {
                nameNewAr += "_1";
            } 
            double[] tmp = new double[newDataAr.Length];
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = newDataAr[i];
            }
            dataArrays.Add(tmp);

            string[] s = new string[3];
            s[1] = nameNewAr;
            s[2] = tmp.Length.ToString();
            DataRow row = dataTable.NewRow(); 
            row.ItemArray = s;
            dataTable.Rows.Add(row);           
        }

        public static void AddDataAr(int sizeNewDataAr, string nameNewAr)
        {
            while (IndexRow(nameNewAr) != -1)
            {
                nameNewAr += "_1";
            } 
            double[] tmp = new double[sizeNewDataAr];
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = double.NaN;
            }
            dataArrays.Add(tmp);

            string[] s = new string[3];
            s[1] = nameNewAr;
            s[2] = tmp.Length.ToString();
            DataRow row = dataTable.NewRow();
            row.ItemArray = s;
            dataTable.Rows.Add(row);  
        }

        public static void AddElements(int numberRow, int count)
        {
            if (numberRow < dataArrays.Count)
            {
                double[] tmp = new double[dataArrays[numberRow].Length + count];
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (i < dataArrays[numberRow].Length) tmp[i] = dataArrays[numberRow][i];
                    else tmp[i] = double.NaN;
                }
                dataArrays[numberRow] = tmp;

                dataTable.Rows[numberRow].ItemArray[2] = tmp.Length.ToString();
            }
        }

        public static void AddElements(string nameRow, int count)
        {
            int numberRow = IndexRow(nameRow);
            if (numberRow >= 0)
            {
                AddElements(numberRow, count);
            }
        }

        public static void ReplaceDataAr(int numberRow,double[] newRow)
        {
            if (numberRow < dataArrays.Count)
            {
                double[] tmp = new double[newRow.Length];
                for (int i = 0; i < tmp.Length; i++) tmp[i] = newRow[i];
                dataArrays[numberRow] = tmp;
                string[] s=new string[3];
                s[0] = dataTable.Rows[numberRow].ItemArray[0].ToString();
                s[1] = dataTable.Rows[numberRow].ItemArray[1].ToString();
                s[2] = tmp.Length.ToString();
                dataTable.Rows[numberRow].ItemArray = s;
            }
        }

        public static void DelDataAr(int numberRow)
        {
            if (numberRow < dataArrays.Count)
            {
                dataArrays.RemoveAt(numberRow);
                dataTable.Rows.RemoveAt(numberRow);
            }
        }

        public static void DelDataAr(string nameRow)
        {
            DelDataAr(IndexRow(nameRow));
        }

        public static void DelElement(int numberRow, int numberEl)
        {
            if (numberRow<dataArrays.Count)
                if (numberEl < dataArrays[numberRow].Length)
                {
                    double[] tmp = new double[dataArrays[numberRow].Length - 1];
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (i < numberEl)
                        {
                            tmp[i] = dataArrays[numberRow][i];
                        }
                        if (i > numberEl)
                        {
                            tmp[i] = dataArrays[numberRow][i+1];
                        }
                    }
                    dataArrays[numberRow] = tmp;

                    dataTable.Rows[numberRow].ItemArray[2] = tmp.Length.ToString();
                }
        }

        public static void DelElement(string nameRow, int numberEl)
        {
            int numberRow = IndexRow(nameRow);
            if (numberRow >= 0)
            {
                DelElement(numberRow, numberEl);
            }
        }

        public static void EditElement(int numberRow, int numberEl, double newValue)
        {
            if (numberRow<dataArrays.Count)
                if (numberEl < dataArrays[numberRow].Length)
                {
                    dataArrays[numberRow][numberEl] = newValue;
                }
        }

        public static void EditElement(string nameRow, int numberEl, double newValue)
        {
            EditElement(IndexRow(nameRow), numberEl, newValue);
        }

        public static void EditNameRow(int indexRow, string newNameRow)
        {
            if (indexRow<dataTable.Rows.Count){
                string[] s = new string[3];
                s[0] = dataTable.Rows[indexRow].ItemArray[0].ToString();
                s[1] = newNameRow;
                s[2] = dataTable.Rows[indexRow].ItemArray[2].ToString();
                dataTable.Rows[indexRow].ItemArray = s;
            }
        }

        public static void EditNameRow(string nameRow, string newNameRow)
        {
            EditNameRow(IndexRow(nameRow), newNameRow);
        }

        public static double Element(int numberRow, int numberEl)
        {
            if (numberRow<dataArrays.Count)
                if (numberEl < dataArrays[numberRow].Length)
                {
                    return dataArrays[numberRow][numberEl];
                }
            return double.NaN;
        }

        public static double Element(string nameRow, int numberEl)
        {
            return Element(IndexRow(nameRow), numberEl);
        }

        public static int IndexRow(string nameRow)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (string.Equals(dataTable.Rows[i].ItemArray[1], nameRow))
                {
                    return i;
                }
            }
            return -1;
        }

        public static int LenghtRow(int indexRow)
        {
            if (indexRow < dataArrays.Count)
            {
                return dataArrays[indexRow].Length;
            }
            else return -1;
        }

        public static int LenghtRow(string nameRow)
        {
            return LenghtRow(IndexRow(nameRow));
        }

        public static string NameRow(int indexRow)
        {
            if (indexRow < dataArrays.Count)
            {
                return dataTable.Rows[indexRow].ItemArray[1].ToString();
            }
            else return null;
        }

        public static double[] ReturnRow(int indexRow)
        {
            if (indexRow < dataArrays.Count)
            {
                double[] tmp = new double[dataArrays[indexRow].Length];
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = dataArrays[indexRow][i];
                }
                return tmp;
            }
            else return null;
        }

        public static double[] ReturnRow(string nameRow)
        {
            return ReturnRow(IndexRow(nameRow));
        }

        public static int CountAllRows
        {
            get
            {
                return (int)dataTable.Rows[dataTable.Rows.Count-1].ItemArray[0];
            }
        }

        public static int CountRows
        {
            get
            {
                return dataTable.Rows.Count;
            }
        }

        public static string[] RowNames
        {
            get
            {
                string[] rez = new string[dataTable.Rows.Count];

                for (int i = 0; i < rez.Length; i++)
                {
                    rez[i] = dataTable.Rows[i].ItemArray[1].ToString();
                }

                return rez;
            }
        }
    }
}
