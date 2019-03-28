using System;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class EmailGroupKeyModel
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Friend Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}