using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GallPrintD394Exporter
{
    class SumComponent
    {
        public int Base;
        public int VAT;

        internal int GetTotal()
        {
            return Base + VAT;
        }
    }
}
