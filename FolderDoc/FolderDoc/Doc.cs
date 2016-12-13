using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FolderDoc
{
    class Doc
    {
        [StringLength(36)] // Guid
        public string Id { get; set; }

        [StringLength(36)]
        public string ParentId { get; set; }

        public int? Order { get; set; }

        // Navigation
        public virtual Doc Parent { get; set; }
        public virtual ICollection<Doc> Children { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }

        // Data
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength]
        public string Text { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
    }
}
