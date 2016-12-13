using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FolderDoc
{
    class Folder
    {
        [StringLength(36)] // Guid
        public string Id { get; set; }

        // Navigation
        public virtual ICollection<Doc> Docs { get; set; }

        // Data
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
