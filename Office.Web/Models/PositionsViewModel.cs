using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Office.Core.Entities;
using Office.Web;

namespace Office.Web.Models
{
    public class PositionsViewModel
    {
       [Display (Name="Search by name")]
       public string name  { get; set; }
       public PagedResult<Spot> positions { get; set; }
    }
}
