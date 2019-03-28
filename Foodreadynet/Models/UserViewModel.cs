using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class UserViewModel
    {
        public string[] Alphabet { get; set; }
        public List<FR.Domain.Model.Entities.AspNetUser> Users { get; set; }
    }
}