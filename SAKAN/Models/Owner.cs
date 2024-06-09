using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAKAN.Models
{
    public class Owner:ApplicationUser
    {
        
        public virtual ICollection<Building> Buildings { get; set; }

    }
}
