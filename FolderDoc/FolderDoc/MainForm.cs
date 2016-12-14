using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
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
                d.Id
            }).ToList();

            db.Links.Load();
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            //Guid id = (Guid)e.Row.Cells["Id"].Value;
            //listBox1.DataSource = db.Links.Include(p => p.Parent.Name).Where(d => d.ChildId == id);
            //listBox2.DataSource = db.Links.Include(c => c.Child.Name).Where(d => d.ParentId == id);
        }
    }
}
