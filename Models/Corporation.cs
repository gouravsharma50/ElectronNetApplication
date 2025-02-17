using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApplication.Models
{
    public class Corporation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CorporationId { get; set; }

        [Required]
        public string CorporationName { get; set; }
        //public DateTime CorporationCreatedOn { get; set; }

        public bool IsSync { get; set; } = false;

        public ICollection<Branch> Branches { get; set; }
    }
}
