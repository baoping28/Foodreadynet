using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class SubMenuViewModel
    {
        public List<BizCuisine> MenuList { get; set; }
        public List<Category> CategoryList { get; set; }
        public string sortParameter { get; set; }
        public int CurrentBizCuisineId { get; set; }
    }
}