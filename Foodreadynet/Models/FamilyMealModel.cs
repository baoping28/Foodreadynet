using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class FamilyMealModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int FamilyMealID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(2,100)]
        [Display(Name = "Number of People")]
        public int NumberOfPeople { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Meal to Add")]
        public string MealToAdd { get; set; }

        public Product Product { get; set; }
        public BizInfo Bizinfo { get; set; }
        public List<FamilyMeal> LFamilyMeal { get; set; }
    }
}