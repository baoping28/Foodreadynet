using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;


namespace FoodReady.WebUI.Models
{
    public class ProductSizeModel
    {
        public ProductSizeModel()
        {
            SizeProducts = new List<SelectListItem>();
        }
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ProductSizeID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0, 100)]
        [Display(Name = "Size in Inch")]
        public int Size { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 1000.00)]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Restaurant Price")]
        public decimal BizPrice { get; set; }

        public Product Product { get; set; }
        public BizInfo Bizinfo { get; set; }
        public List<ProductSize> LProductsize { get; set; }
        public List<SelectListItem> SizeProducts { get; set; }
    }
}