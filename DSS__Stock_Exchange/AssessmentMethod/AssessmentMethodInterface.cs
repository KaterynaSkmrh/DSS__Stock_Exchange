using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.AssessmentMethod
{
    interface AssessmentMethodInterface
    {
        double[] teta(double[] Y, double[][] X, double beta, out double[] RS, out double RSS, out double R2, out double Akaike,out double dw);
    }
}
