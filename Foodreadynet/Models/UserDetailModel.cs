using System;

namespace FoodReady.WebUI.Models
{
    public class UserDetailModel
    {
        public FR.Domain.Model.Entities.AspNetUser User { get; set; }
        public FR.Domain.Model.Entities.UserDetail Userdetail { get; set; }
        public FR.Domain.Model.Entities.AspNetUserLogin UserLogin { get; set; }
    }
}