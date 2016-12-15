using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;

namespace FolderDoc
{
    public class Item
    {
        public Guid Id { get; set; }

        [Required]
        //[Index("NameIndex", IsUnique = true)]
        //[StringLength(200)]
        public string Name { get; set; }

        public string Ext { get; set; }

        public string Text { get; set; }

        public string FileName { get; set; }

        // Add your custom properties above this line

        public int? Order { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }

        // Navigation Properties

        public virtual ICollection<Link> Parents { get; set; }
        public virtual ICollection<Link> Children { get; set; }
    }

    public class Link
    {
        [Key, Column(Order = 1)]
        public Guid ParentId { get; set; }

        [Key, Column(Order = 2)]
        public Guid ChildId { get; set; }

        // Navigation Properties

        public virtual Item Parent { get; set; }
        public virtual Item Child { get; set; }
    }

    public class ItemDbContext : DbContext
    {
        static ItemDbContext()
        {
            Database.SetInitializer(new MyDocumentsInitializer());
        }

        public ItemDbContext() : base("FolderDocData")
        { }

        public IDbSet<Item> Items { get; set; }
        public IDbSet<Link> Links { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>()
                .HasRequired(a => a.Parent)
                .WithMany(b => b.Children)
                .HasForeignKey(c => c.ParentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Link>()
                .HasRequired(a => a.Child)
                .WithMany(b => b.Parents)
                .HasForeignKey(c => c.ChildId);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedFolder(DirectoryInfo dir, Guid parentId, bool subFolder = true)
        {
            Trace.TraceInformation(dir.Name);
            int order = 0;

            foreach (FileInfo fi in dir.GetFiles())
            {
                if (fi.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                Item item = new Item
                {
                    Id = Guid.NewGuid(),
                    Order = ++order,
                    Name = fi.Name,
                    Ext = fi.Extension.ToLower(),
                    Text = fi.LastWriteTime.ToShortDateString(),
                    FileName = fi.FullName
                };
                Items.Add(item);
                if (subFolder)
                {
                    Links.Add(new Link { ParentId = parentId, ChildId = item.Id });
                }
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                if (di.Attributes.HasFlag(FileAttributes.Hidden))
                    continue;

                Item item = new Item
                {
                    Id = Guid.NewGuid(),
                    Order = ++order,
                    Name = di.Name,
                    Ext = @"\",
                    Text = "Folder",
                    FileName = di.FullName
                };
                Items.Add(item);
                if (subFolder)
                {
                    Links.Add(new Link { ParentId = parentId, ChildId = item.Id });
                }
                SeedFolder(di, item.Id);
            }
        }

        private class MyDocumentsInitializer : DropCreateDatabaseIfModelChanges<ItemDbContext>
        {
            protected override void Seed(ItemDbContext db)
            {
                base.Seed(db);
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                DirectoryInfo di = new DirectoryInfo(path);
                db.SeedFolder(di, Guid.Empty, false);
                db.SaveChanges();

                db.Configuration.AutoDetectChangesEnabled = true;
                db.Configuration.ValidateOnSaveEnabled = true;
            }
        }
    }
}