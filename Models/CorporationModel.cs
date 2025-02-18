using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DesktopApplication.Utilities;

namespace DesktopApplication.Models
{
    public class CorporationModel
    {
        [Required(ErrorMessage = "Please enter the Corporation Name.")]
        public string CorporationName { get; set; }
        public int CorporationId { get; set; }
        public DateTime CorporationCreatedOn { get; set; }
        public bool IsSync { get; set; } = false;
        public string CreatedOn => DateTimeFormatter.DateTimeStrMMM(CorporationCreatedOn);
    }
   

}
