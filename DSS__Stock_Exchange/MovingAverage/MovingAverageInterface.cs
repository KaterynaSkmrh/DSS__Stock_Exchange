using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange.MovingAverage
{
    public interface MovingAverageInterface
    {
        double[] ma(double[] x,int size);
    }
}
