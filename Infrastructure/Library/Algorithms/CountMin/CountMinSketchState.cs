using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Algorithms.CountMin
{
    public class CountMinSketchState
    {
        public Double Epsilon;
        public Double Delta;
        public UInt64 Count;
        public UInt64[][] Matrix;
    }
}
