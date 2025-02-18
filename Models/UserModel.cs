using DesktopApplication.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApplication.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please select a corporation.")]
        public int CorporationId { get; set; }
        [Required(ErrorMessage = "Please select a  Branch.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Please enter the User Name.")]
        public string Username { get; set; }

        [Required]
        public string Role { get; set; } = "USER";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedOn => DateTimeFormatter.DateTimeStrMMM(CreatedDate);

        public bool IsSync { get; set; } = false;

        [ForeignKey("CorporationId")]
        public Corporation? Corporation { get; set; }

        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }
    }
}
