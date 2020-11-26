using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Office.Core.Entities
{
    public class Spot
    {
        public Guid Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public int Bonus { get; set; }

        public DateTimeOffset? Created { get; set; }

        public Guid? ManagerId { get; set; }
        public virtual Spot Manager { get; set; }
        public virtual ICollection<Spot> DirectReports { get; set; }
        public byte[] Image { get; set; }

    }
}
