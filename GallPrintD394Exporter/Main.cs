using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GallPrintD394Exporter
{
    public partial class Main : Form
    {
        
        public Main()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileListBox.Items.AddRange(fileDialog.FileNames);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (fileListBox.Items.Count == 0)
            {
                MessageBox.Show("Nu exista nimic de concatenat. \nFolositi butonul de adaugare pentru a adauga fisiere"); //No files exist. Use add button to add some
                return;
            }
            if (exportDialog.ShowDialog() == DialogResult.OK)
            {
                Statement statement = new Statement(); 
                DataAccess export = new DataAccess(exportDialog.FileName);
                
                foreach (string file in fileListBox.Items)
                {
                    try
                    {
                        DataAccess da = new DataAccess(file);
                        Statement s = new Statement();
                        s.LoadDataUsing(da);
                        statement.Append(s);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(ex.Message, "Eroare");
                    }
                }
                statement.SaveDataUsing(export);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            int index = fileListBox.SelectedIndex;

            if (index < 0)
            {
                MessageBox.Show("Nu aveti selectat nici un fisier"); //No file selected
                return;
            }

            fileDialog.FileName = fileListBox.Items[index].ToString();
            fileDialog.InitialDirectory = Path.GetDirectoryName(fileDialog.FileName);
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileListBox.Items[index] = fileDialog.FileName;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = fileListBox.SelectedIndex;
            if (index >= 0 && fileListBox.Items.Count > 0)
            {
                fileListBox.Items.RemoveAt(index);
                if (fileListBox.Items.Count >= index + 1)
                {
                    fileListBox.SelectedIndex = index;
                }
                else
                {
                    fileListBox.SelectedIndex = index - 1;
                }
            }
        }
    }
}
