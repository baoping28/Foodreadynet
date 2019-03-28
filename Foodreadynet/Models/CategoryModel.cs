using System.Collections.Generic;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class CategoryModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizCuisineID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Description")]
        public string Description { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public string MenuName { get; set; }
        public List<Category> ListCategories { get; set; }
        public BizInfo Bizinfo { get; set; }
    }
}