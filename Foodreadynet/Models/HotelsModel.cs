using System.Collections.Generic;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class HotelsModel
    {
        [HiddenInput(DisplayValue = false)]
        public int HoteTypeID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int HoteID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string AddressLine { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9][0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public List<Hotel> ListHotels { get; set; }
    }
}