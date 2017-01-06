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
            //TODO: fix some crushes on Load
            db.Items.Load();
            dataGridView1.DataSource = db.Items.Local.Select(d => new
            {
                d.Name,
                d.Ext,
                d.Text,
                d.FileName,
                d.Id
            }).ToList();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if ((sender as DataGridView).SelectedRows.Count != 1)
                return;

            Guid id = (Guid)((sender as DataGridView).CurrentRow.Cells["Id"].Value);

            db.Links.Load();
            var parents = db.Links
                .Where(c => c.ChildId == id)
                .Select(p => p.Parent.Name)
                .ToList();
            var children = db.Links
                .Where(p => p.ParentId == id)
                .Select(c => c.Child.Name)
                .ToList();

            listBox1.DataSource = parents;
            listBox2.DataSource = children;
        }
    }
}
