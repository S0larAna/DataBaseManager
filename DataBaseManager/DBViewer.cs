using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using Npgsql;

namespace DataBaseManager
{
    public partial class DBViewer : Form
    {
        public DBViewer()
        {
            InitializeComponent();
        }

        private void DBViewer_Load(object sender, EventArgs e)
        {
            Form1 obj = (Form1)Application.OpenForms["Form1"];
            obj.Hide();
        }


        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            list = PGManage.listTheCatalogs();
            for (int i=0; i<list.Count; i++)
            {
                comboBox1.Items.Add(list[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PGManage.table = comboBox1.Text;
            GridView form = new GridView();
            form.Show();
        }
    }
}
