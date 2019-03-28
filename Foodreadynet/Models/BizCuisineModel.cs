using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
namespace FoodReady.WebUI.Models
{
    public class BizCuisineModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Choose a Cuisine")]
        public int CuisineID { get; set; }

        public List<BizCuisine> Bizcuisines { get; set; }

        public BizInfo Bizinfo { get; set; }

        public List<SelectListItem> CuisineTypeDropdown()
        {
            return CuisineTypeDropdown(0);
        }
        public List<SelectListItem> CuisineTypeDropdown(int vCuisineId)
        {
            if (vCuisineId<1)
            {
                vCuisineId = 0;
            }
                List<SelectListItem> lt = new List<SelectListItem>();
                using (FRShoppingEntities frenty = new FRShoppingEntities())
                {
                    List<CuisineType> lct = frenty.CuisineTypes.Where(e => e.Active == true).ToList();
                    lt.Add(new SelectListItem { Text = "Choose a cuisine", Value = "", Selected = vCuisineId==0 });
                    foreach (var c in lct)
                    {
                        lt.Add(new SelectListItem { Text = c.Title, Value = c.CuisineTypeId.ToString(), Selected = c.CuisineTypeId == vCuisineId});
                    }
                    return lt;
                }
        }
    }
}