using System.Collections.Generic;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class BizImageModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizImageID { get; set; }

        [Display(Name = "Small Image")]
        public string SmallImageName { get; set; }

        [Display(Name = "Big Image")]
        public string BigImageName { get; set; }

        public List<BizImage> BizImages { get; set; }
        public BizInfo Bizinfo { get; set; }
        public BizImage Bizimage { get; set; }
    }
}