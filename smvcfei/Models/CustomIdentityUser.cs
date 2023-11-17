using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace smvcfei.Models
{
    public class CustomIdentityUser: IdentityUser
    {
        [PersonalData]
        [Display(Name = "Nombre")]
        public string Nombrecompleto { get; set; }
    }
}
