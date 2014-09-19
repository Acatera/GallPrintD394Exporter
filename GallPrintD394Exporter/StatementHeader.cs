using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallPrintD394Exporter
{
    class StatementHeader
    {
        public string Month;
        public string Year;
        public char Type;
        public Issuer Issuer;
        public Company Company;
        public StatementSummary Summary;

        internal void LoadInfoFrom(StatementHeader header)
        {
            if (header != null)
            {
                this.Month = header.Month;
                this.Year = header.Year;
                this.Type = header.Type;
                this.Issuer = header.Issuer.Clone();
                this.Company = header.Company.Clone();
                this.Summary = new StatementSummary();
            }
        }
    }
}
