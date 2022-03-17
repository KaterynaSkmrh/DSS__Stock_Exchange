using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS__Stock_Exchange
{
    public class MyExcepion:Exception
    {
        public MyExcepion(string mes)
            : base(mes)
        {

        }
    }
}
