﻿using DesktopApplication.Utilities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesktopApplication.Models
{
    public class CategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [DisplayName("Corporation")]
        public int CorporationId { get; set; }
        public int BranchId { get; set; }
        public int CreatedByUserId { get; set; }

        [Required(ErrorMessage = "Please enter the Category Name.")]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        [DisplayName("Parent Category ")]
        public string? ParentCategoryName { get; set; }
        public int? ParentCategoryId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string CreatedOn => DateTimeFormatter.DateTimeStrMMM(CreatedDate);

        public bool IsSync { get; set; } = false;

        public Corporation? Corporation { get; set; }

        public Branch? Branch { get; set; }

        public User? User { get; set; }
    }
}



