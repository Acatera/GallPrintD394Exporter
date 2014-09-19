using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallPrintD394Exporter
{
    class StatementEntry
    {
        public String Type { get; set; }
        public string PartenerCUI { get; set; }
        public string PartenerName { get; set; }
        public int InvoiceCount { get; set; }
        public int BaseValue { get; set; }
        public int VATValue { get; set; }

        public StatementEntry(string Type, string PartenerCUI, string PartenerName, int InvoiceCount, int BaseValue, int VATValue)
        {
            this.Type = Type;
            this.PartenerCUI = PartenerCUI;
            this.PartenerName = PartenerName;
            this.InvoiceCount = InvoiceCount;
            this.BaseValue = BaseValue;
            this.VATValue = VATValue;
        }

        public void AppendStatementEntry(StatementEntry entry)
        {
            if (PartenerCUI == entry.PartenerName && Type == entry.Type)
            {
                this.InvoiceCount += entry.InvoiceCount;
                this.BaseValue += entry.BaseValue;
                this.VATValue += entry.VATValue;
            }
        }
    }
}
