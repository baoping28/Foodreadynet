using FR.Domain.Model.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class GroupInfoModel
    {
        [Required(ErrorMessage = "*")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "*")]
        [Display(Name = "Lable Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9][0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Maximum Order")]
        public decimal MaximumOrder { get; set; }

        public List<BizInfo> LBizInfo { get; set; }
        public ShoppingCart Cart { get; set; }

    }
    public class GroupGuestModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Group Code")]
        public string GroupId { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "*")]
        [Display(Name = "Lable Name")]
        public string LableName { get; set; }

        public List<BizInfo> LBizInfo { get; set; }
        public ShoppingCart Cart { get; set; }

    }
}