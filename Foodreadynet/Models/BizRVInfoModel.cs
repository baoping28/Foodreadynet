using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class BizRVInfoModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizRVInfoId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [Required]
        [StringLength(4, ErrorMessage = "The {0} must be at least {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Restaurant Price")]
        public string BizPrice { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Standing Capacity")]
        public int StandingCapacity { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Seated Capacity")]
        public int SeatedCapacity { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Dining Style")]
        public string DiningStyle { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Payment Options")]
        public string PaymentOptions { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Accept Walk In")]
        public string AcceptWalkIn { get; set; }

        [Display(Name = "Executive Chef (optional)")]
        public string ExecutiveChef { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Private Party Facilities (optional)")]
        public string Facilities { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Special Events (optional)")]
        public string Events { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Parking (optional)")]
        public string Parking { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Additional Details (optional)")]
        public string Details { get; set; }

        public BizInfo Bizinfo { get; set; }
    }
}