using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApplication.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public int CorporationId { get; set; }
        public int BranchId { get; set; }
        public int CreatedByUserId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsSync { get; set; } = false;

        [ForeignKey("CorporationId")]
        public Corporation Corporation { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }

        [ForeignKey("CreatedByUserId")]
        public User User { get; set; }
    }
}



