using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GallPrintD394Exporter
{
    class Company
    {
        public string CUI;
        public string Name;
        public string Adress;
        public string Phone;
        public string Fax;
        public string Email;

        public Company Clone()
        {
            Company clone = new Company();
            clone.CUI = this.CUI;
            clone.Name = this.Name;
            clone.Adress = this.Adress;
            clone.Phone = this.Phone;
            clone.Fax = this.Fax;
            clone.Email = this.Email;
            return clone;
        }
    }
}
