﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DesktopApplication.Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        [Required]
        public string BranchName { get; set; }

        public int CorporationId { get; set; }

        public DateTime BranchCreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsSync { get; set; } = false;

        [ForeignKey("CorporationId")]
        public Corporation Corporation { get; set; }
    }
}
