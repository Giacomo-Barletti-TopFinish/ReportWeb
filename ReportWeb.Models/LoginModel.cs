using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
