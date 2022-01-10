using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GammaltGlimmer.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}