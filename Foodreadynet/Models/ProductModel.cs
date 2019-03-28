using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;


namespace FoodReady.WebUI.Models
{
    public class ProductModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizCuisineID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CategoryID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Cook Method")]
        public int CookMethodID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Food Type")]
        public int FoodTypeID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Meal Section")]
        public int MealSectionID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Small Image")]
        public string SmallImage { get; set; }

        [Display(Name = "Big Image")]
        public string BigImage { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, 99)]
        [Display(Name = "Max Number Of Free Topping")]
        public int MaxNumOfFreeTopping { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Unit Price")]
        [Range(0.00, 9999.99)]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Restaurant Price")]
        [Range(0.00, 9999.99)]
        public decimal BizPrice { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, 100)]
        [Display(Name = "Discount Percentage(%)")]
        public int DiscountPercentage { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Is Spicy")]
        public string IsSpicy { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Is Vegetarian")]
        public string IsVegetarian { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Is Popular")]
        public string IsMostPopular { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Is Family Dinner")]
        public string IsFamilyDinner { get; set; }

        public List<Product> LProducts { get; set; }
        public List<CookMethod> LCookMethods { get; set; }
        public List<FoodType> LFoodTypes { get; set; }
        public List<MealSection> LMealSections { get; set; }
        public BizInfo Bizinfo { get; set; }
        public Product Product { get; set; }
        public List<SelectListItem> CookMethods(List<CookMethod> lc)
        {
            return CookMethods(lc, null);
        }
        public List<SelectListItem> CookMethods(List<CookMethod> lc, string selectedCookMethodId)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            if (string.IsNullOrEmpty(selectedCookMethodId))
            {
                foreach (var z in lc)
                {
                    l.Add(new SelectListItem { Text = z.Title, Value = z.CookMethodId.ToString() });
                }
            }
            else
            {
                foreach (var z in lc)
                {
                    l.Add(new SelectListItem { Text = z.Title, Value = z.CookMethodId.ToString(), Selected = selectedCookMethodId == z.CookMethodId.ToString() });
                }
            }
            return l;
        }
        public List<SelectListItem> FoodTypes(List<FoodType> lc)
        {
            return FoodTypes(lc, null);
        }
        public List<SelectListItem> FoodTypes(List<FoodType> lc, string selectedFoodTypeId)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            if (string.IsNullOrEmpty(selectedFoodTypeId))
            {
                foreach (var z in lc)
                {
                    l.Add(new SelectListItem { Text = z.Title, Value = z.FoodTypeId.ToString() });
                }
            }
            else
            {
                foreach (var z in lc)
                {
                    l.Add(new SelectListItem { Text = z.Title, Value = z.FoodTypeId.ToString(), Selected = selectedFoodTypeId == z.FoodTypeId.ToString() });
                }
            }
            return l;
        }
        public List<SelectListItem> MealSections(List<MealSection> lc)
        {
            return MealSections(lc, null);
        }
        public List<SelectListItem> MealSections(List<MealSection> lc, string selectedMealSectionId)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            if (string.IsNullOrEmpty(selectedMealSectionId))
            {
                foreach (var z in lc)
                {
                    l.Add(new SelectListItem { Text = z.Title, Value = z.MealSectionId.ToString() });
                }
            }
            else
            {
                foreach (var z in lc)
                {
                    l.Add(new SelectListItem { Text = z.Title, Value = z.MealSectionId.ToString(), Selected = selectedMealSectionId == z.MealSectionId.ToString() });
                }
            }
            return l;
        }
    }
}