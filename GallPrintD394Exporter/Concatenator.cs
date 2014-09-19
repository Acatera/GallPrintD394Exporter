using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GallPrintD394Exporter
{
    class Concatenator
    {
        private DataAccess export;
        private Statement statement = new Statement();
        public string Filename;
        public Concatenator(string filename)
        {
            this.Filename = filename;
            export = new DataAccess(filename);
        }

        public void Append(string file)
        {
            DataAccess da = new DataAccess(file);
            Statement s = new Statement();
            s.LoadDataUsing(da);
            statement.Append(s);
        }

        public void Save()
        {
            statement.SaveDataUsing(export);
        }
    }
}
