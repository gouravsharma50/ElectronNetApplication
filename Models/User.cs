using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApplication.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public int CorporationId { get; set; }

        public int BranchId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsSync { get; set; } = false;

        [ForeignKey("CorporationId")]
        public Corporation Corporation { get; set; }

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; }
    }
}
