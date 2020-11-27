using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Office.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Office.Web.Models
{
    public class EditPositionViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name="Title")]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(5, 1000, ErrorMessage = "Bonus must be at between $5 and $1000")]
        public int Bonus { get; set; }

        public DateTimeOffset? Created { get; set; }
     
        public virtual ICollection<Spot> DirectReports { get; set; }
        public byte[] Image { get; set; }

        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Manager")]
        public Guid? ManagerId { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
    }
}
