using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallPrintD394Exporter
{
    class StatementEntry
    {
        public String Type;
        public string PartenerCUI;
        public string PartenerName;
        public int InvoiceCount;
        public int BaseValue;
        public int VATValue;

        public StatementEntry(string Type, string PartenerCUI, string PartenerName, int InvoiceCount, int BaseValue, int VATValue)
        {
            this.Type = Type;
            this.PartenerCUI = PartenerCUI;
            this.PartenerName = PartenerName;
            this.InvoiceCount = InvoiceCount;
            this.BaseValue = BaseValue;
            this.VATValue = VATValue;
        }
    }
}
