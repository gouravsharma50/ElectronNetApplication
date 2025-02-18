using DesktopApplication.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApplication.Models
{
    public class BranchModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Please enter the Branch Name.")]
        public string BranchName { get; set; }
        [Required(ErrorMessage = "Please select a corporation.")]
        public int CorporationId { get; set; }
        public DateTime BranchCreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsSync { get; set; } = false;

        public Corporation? Corporation { get; set; }

        public string CreatedOn => DateTimeFormatter.DateTimeStrMMM(BranchCreatedDate);
    }
}

