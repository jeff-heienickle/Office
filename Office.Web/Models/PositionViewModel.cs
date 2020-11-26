using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Office.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Office.Web.Models
{
    public class PositionViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name="Title")]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string Name { get; set; }

        public int Bonus { get; set; }

        public DateTimeOffset? Created { get; set; }
     
        public virtual ICollection<Spot> DirectReports { get; set; }
        public byte[] Image { get; set; }

        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }

        [Display(Name = "Manager")]
        public Guid? ManagerId { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }
    }
}
