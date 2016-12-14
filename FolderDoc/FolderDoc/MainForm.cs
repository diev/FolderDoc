using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace FolderDoc
{
    public partial class MainForm : Form
    {
        ItemDbContext db;

        public MainForm()
        {
            InitializeComponent();

            db = new ItemDbContext();
            db.Items.Load();
            dataGridView1.DataSource = db.Items.Local.Select(d => new
            {
                d.Name,
                d.Ext,
                d.Text,
                d.FileName,
                d.Id
            }).ToList();

            db.Links.Load();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            Guid id = (Guid)((sender as DataGridView).CurrentRow.Cells["Id"].Value);

            var parents = db.Links.Where(c => c.ChildId == id).Select(p => p.Parent).ToList();
            var children = db.Links.Where(p => p.ParentId == id).Select(c => c.Child).ToList();

            listBox1.DataSource = parents.Select(i => i.Name).ToList();
            listBox2.DataSource = children.Select(i => i.Name).ToList();
        }
    }
}
