using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FoodReady.WebUI.Models
{
    public class PointSumaryModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Choice of Gift")]
        public string GiftChoice { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Street")]
        public string AddressLine { get; set; }
        
        [Required(ErrorMessage = "*")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[1-9][0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Postal Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "*")]
        [RegularExpression("^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$", ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Message")]
        public string Message { get; set; }

        public int TotalPoints { get; set; }
        public int UsedPoints { get; set; }
        public int AvailablePoints { get; set; }
        public int QualifiedPoints { get; set; }
        public List<RewardVoucher>AvailableGiftList { get; set; }
        public List<SelectListItem> GiftChoices(List<RewardVoucher> lrv)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var r in lrv)
            {
                l.Add(new SelectListItem { Text = r.Title + " (" + r.Points + " points)", Value = r.RewardVoucherId.ToString() });
            }
            return l;
        }
    }
}