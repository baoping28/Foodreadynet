using System.Collections.Generic;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;
namespace FoodReady.WebUI.Models
{
    public class HotelTypeModel
    {

        [HiddenInput(DisplayValue = false)]
        public int HoteTypeID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public List<HotelType> ListHotelTypes { get; set; }
    }
}