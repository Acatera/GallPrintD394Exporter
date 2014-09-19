using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallPrintD394Exporter
{
    class Issuer
    {
        public string Name;
        public string Surname;
        public string Position;

        public Issuer Clone()
        {
            Issuer clone = new Issuer();
            clone.Name = this.Name;
            clone.Surname = this.Surname;
            clone.Position = this.Position;
            return clone;
        }
    }
}
