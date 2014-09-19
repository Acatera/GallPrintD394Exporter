using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GallPrintD394Exporter
{
    class StatementSummary
    {
        public int UniquePartenerCount;
        public SumComponent LType = new SumComponent();
        public SumComponent AType = new SumComponent();
        public SumComponent VType = new SumComponent();
        public SumComponent VcType = new SumComponent();
        public SumComponent CType = new SumComponent();
        public SumComponent CcType = new SumComponent();
        public int LTypeCount;
        public int ATypeCount;
        public int VTypeCount;
        public int CTypeCount;

        public int GetCRCSum()
        {
            return UniquePartenerCount + LType.GetTotal() + AType.GetTotal() + VType.GetTotal() + VcType.GetTotal() + CType.GetTotal() + CcType.GetTotal();
        }
    }
}
