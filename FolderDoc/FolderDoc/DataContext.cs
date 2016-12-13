using System.Data.Entity;

namespace FolderDoc
{
    class DataContext : DbContext
    {
        public DataContext() : base("FolderDocData")
        { }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<Doc> Docs { get; set; }
    }
}
