using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GallPrintD394Exporter
{
    class DataAccess
    {
        private string filename;
        private XmlDocument doc;
        XmlNamespaceManager nsmgr;

        public DataAccess(String filename)
        {
            if (filename.Trim() == "")
            {
                throw new InvalidOperator();
            }

            this.filename = filename;
            if (File.Exists(filename)) {
                PrepareXML();
            }
        }

        private void PrepareXML()
        {
            doc = new XmlDocument();
            doc.Load(filename);
            nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ds", "mfp:anaf:dgti:d394:declaratie:v2");
        }

        public Dictionary<String, StatementEntry> ReadEntries()
        {
            Dictionary<String, StatementEntry> result = new Dictionary<String, StatementEntry>();

            XmlNodeList entries = doc.SelectNodes("//ds:op1", nsmgr);
            foreach (XmlNode entry in entries)
            {
                string type = entry.Attributes["tip"].Value;
                string partenerCUI = entry.Attributes["cuiP"].Value;
                string partenerName = entry.Attributes["denP"].Value;
                int invoiceCount = Convert.ToInt32(entry.Attributes["nrFact"].Value);
                int baseValue = Convert.ToInt32(entry.Attributes["baza"].Value);
                int vatValue = Convert.ToInt32(entry.Attributes["tva"].Value);
                StatementEntry e = new StatementEntry(type, partenerCUI, partenerName, invoiceCount, baseValue, vatValue);
                result.Add(e.Type + e.PartenerCUI, e);
            }
            return result;
        }

        public StatementHeader ReadHeaderInto()
        {
            StatementHeader header = new StatementHeader();
            XmlNode d394 = doc.SelectSingleNode("//ds:declaratie394", nsmgr);
            header.Month = d394.Attributes["luna"].Value;
            header.Year = d394.Attributes["an"].Value;
            header.Type = d394.Attributes["tip_D394"].Value[0];

            Issuer issuer = new Issuer();
            issuer.Name = d394.Attributes["nume_declar"].Value;
            issuer.Surname = d394.Attributes["prenume_declar"].Value;
            issuer.Position = d394.Attributes["functie_declar"].Value;

            header.Issuer = issuer;

            XmlNode comp = doc.SelectSingleNode("//ds:identificare", nsmgr);
            Company company = new Company();
            company.CUI = comp.Attributes["cui"].Value;
            company.Name = comp.Attributes["den"].Value;
            company.Adress = comp.Attributes["adresa"].Value;
            company.Phone = comp.Attributes["telefon"].Value;
            company.Fax = comp.Attributes["fax"].Value;
            company.Email = comp.Attributes["mail"].Value;
            header.Company = company;
            return header;
        }

        public void Save(Statement statement)
        {
            XmlDocument doc = GetPremadeXmlDocument();
            AppendHeader(statement.Header, doc);
            AppendRecords(statement.Entries, doc);
            doc.Save(filename);
        }

        private static XmlDocument GetPremadeXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlElement root = doc.CreateElement("declaratie394");
            doc.AppendChild(root);

            root.SetAttribute("xmlns", "mfp:anaf:dgti:d394:declaratie:v2");
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            root.SetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "mfp:anaf:dgti:d394:declaratie:v2 D394.xsd");
            return doc;
        }

        private void AppendHeader(StatementHeader header, XmlDocument xml)
        {
            AppendGeneralData(header, xml);
            AppendCompanyData(header, xml);
            AppendSummary(header, xml);
        }

        private static void AppendGeneralData(StatementHeader header, XmlDocument xml)
        {
            XmlElement root = xml.DocumentElement;
            root.SetAttribute("luna", header.Month);
            root.SetAttribute("an", header.Year);
            root.SetAttribute("tip_D394", header.Type.ToString());
            root.SetAttribute("nume_declar", header.Issuer.Name);
            root.SetAttribute("prenume_declar", header.Issuer.Surname);
            root.SetAttribute("functie_declar", header.Issuer.Position);
        }

        private static void AppendSummary(StatementHeader header, XmlDocument xml)
        {
            XmlElement summary = xml.CreateElement("rezumat");
            xml.DocumentElement.AppendChild(summary);
            summary.SetAttribute("nrCui", header.Summary.UniquePartenerCount.ToString());

            summary.SetAttribute("bazaL", header.Summary.LType.Base.ToString());
            summary.SetAttribute("tvaL", header.Summary.LType.VAT.ToString());

            summary.SetAttribute("bazaA", header.Summary.AType.Base.ToString());
            summary.SetAttribute("tvaA", header.Summary.AType.VAT.ToString());

            summary.SetAttribute("bazaV", header.Summary.VType.Base.ToString());
            summary.SetAttribute("tvaV", header.Summary.VType.VAT.ToString());

            summary.SetAttribute("bazaVc", header.Summary.VcType.Base.ToString());
            summary.SetAttribute("tvaVc", header.Summary.VcType.VAT.ToString());

            summary.SetAttribute("bazaC", header.Summary.CType.Base.ToString());
            summary.SetAttribute("tvaC", header.Summary.CType.VAT.ToString());

            summary.SetAttribute("bazaCc", header.Summary.CcType.Base.ToString());
            summary.SetAttribute("tvaCc", header.Summary.CcType.VAT.ToString());

            summary.SetAttribute("nrFactL", header.Summary.LTypeCount.ToString());
            summary.SetAttribute("nrFactA", header.Summary.ATypeCount.ToString());
            summary.SetAttribute("nrFactV", header.Summary.VTypeCount.ToString());
            summary.SetAttribute("nrFactC", header.Summary.CTypeCount.ToString());
        }

        private static void AppendCompanyData(StatementHeader header, XmlDocument xml)
        {
            XmlElement ident = xml.CreateElement("identificare");
            xml.DocumentElement.AppendChild(ident);
            ident.SetAttribute("cui", header.Company.CUI);
            ident.SetAttribute("den", header.Company.Name);
            ident.SetAttribute("adresa", header.Company.Adress);
            ident.SetAttribute("telefon", header.Company.Phone);
            ident.SetAttribute("fax", header.Company.Fax);
            ident.SetAttribute("totalPlata_A", header.Summary.GetCRCSum().ToString());
        }

        private void AppendRecords(Dictionary<string, StatementEntry> entries, XmlDocument xml)
        {
            foreach (KeyValuePair<String, StatementEntry> pair in entries)
            {
                StatementEntry entry = pair.Value;
                XmlElement node = xml.CreateElement("op1");
                xml.DocumentElement.AppendChild(node);

                node.SetAttribute("tip", entry.Type.ToString());
                node.SetAttribute("cuiP", entry.PartenerCUI);
                node.SetAttribute("denP", entry.PartenerName);
                node.SetAttribute("nrFact", entry.InvoiceCount.ToString());
                node.SetAttribute("baza", entry.BaseValue.ToString());
                node.SetAttribute("tva", entry.VATValue.ToString());
            }
        }
    }
}
