using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallPrintD394Exporter
{
    class Statement
    {
        public Dictionary<String, StatementEntry> Entries = new Dictionary<String, StatementEntry>();
        public StatementHeader Header;

        public void LoadDataUsing(DataAccess dataAccess)
        {
            if (dataAccess != null)
            {
                LoadHeaderInfo(dataAccess);
                LoadEntries(dataAccess);
                UpdateAllCounters();
            }
        }

        private void LoadHeaderInfo(DataAccess dataAccess)
        {
            StatementHeader header = dataAccess.ReadHeaderInto();
            Header = header;
        }

        private void LoadEntries(DataAccess dataAccess)
        {
            Entries.Clear();
            dataAccess.ReadEntries().ToList().ForEach(x => Entries[x.Key] = x.Value);
        }

        private void UpdateAllCounters()
        {
            if (Header.Summary == null)
            {
                Header.Summary = new StatementSummary();
            }
            foreach (KeyValuePair<String, StatementEntry> e in Entries)
            {
                switch (e.Value.Type)
                {
                    case "L":
                        Header.Summary.LType.Base += e.Value.BaseValue;
                        Header.Summary.LType.VAT += e.Value.VATValue;
                        Header.Summary.LTypeCount += e.Value.InvoiceCount;
                        break;
                    case "A":
                        Header.Summary.AType.Base += e.Value.BaseValue;
                        Header.Summary.AType.VAT += e.Value.VATValue;
                        Header.Summary.ATypeCount += e.Value.InvoiceCount;
                        break;
                    case "V":
                        Header.Summary.VType.Base += e.Value.BaseValue;
                        Header.Summary.VType.VAT += e.Value.VATValue;
                        Header.Summary.VTypeCount += e.Value.InvoiceCount;
                        break;
                    case "Vc":
                        Header.Summary.VcType.Base += e.Value.BaseValue;
                        Header.Summary.VcType.VAT += e.Value.VATValue;
                        Header.Summary.VTypeCount += e.Value.InvoiceCount;
                        break;
                    case "C":
                        Header.Summary.CType.Base += e.Value.BaseValue;
                        Header.Summary.CType.VAT += e.Value.VATValue;
                        Header.Summary.CTypeCount += e.Value.InvoiceCount;
                        break;
                    case "Cc":
                        Header.Summary.CcType.Base += e.Value.BaseValue;
                        Header.Summary.CcType.VAT += e.Value.VATValue;
                        Header.Summary.CTypeCount += e.Value.InvoiceCount;
                        break;
                    default:
                        break;
                }

            }
            Header.Summary.UniquePartenerCount = Entries.Select(x => x.Value.PartenerCUI).Distinct().Count();
        }

        public void SaveDataUsing(DataAccess dataAccess)
        {
            if (dataAccess != null)
            {
                UpdateAllCounters();
                dataAccess.Save(this);
            }
        }

        public void Append(Statement s)
        {
            if (s != null)
            {
                if (Header == null)
                {
                    Header = new StatementHeader();
                    Header.LoadInfoFrom(s.Header);
                }

                if (s.Header.Month != Header.Month || s.Header.Year != Header.Year)
                {
                    throw new ArgumentException("Statement date differs from base statement date.");
                }

                foreach (KeyValuePair<String, StatementEntry> e in s.Entries)
                {
                    StatementEntry entry;
                    if (Entries.TryGetValue(e.Key, out entry))
                    {
                        entry.InvoiceCount += e.Value.InvoiceCount;
                        entry.BaseValue += e.Value.BaseValue;
                        entry.VATValue += e.Value.VATValue;
                    }
                    else
                    {
                        Entries.Add(e.Key, e.Value);
                    }
                }
            }
        }
    }
}
