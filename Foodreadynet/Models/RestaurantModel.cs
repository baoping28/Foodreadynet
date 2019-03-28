using System.ComponentModel.DataAnnotations;
using System.Linq;
using FR.Domain.Model.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace FoodReady.WebUI.Models
{
    public class RestaurantModel
    {
        [HiddenInput(DisplayValue=false)]
        public int UserDetailId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int BizHourID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int AddressID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ContactInfoID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Restaurant Name")]
        public string BizTitle { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Display(Name = "Yelp Biz ID")]
        [MaxLength(126)]
        public string YelpBizID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mon Lunch Start")]
        public string LMonStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mon Lunch End")]
        public string LMonClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Tue Lunch Start")]
        public string LTueStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Tue Lunch End")]
        public string LTueClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Wed Lunch Start")]
        public string LWedStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Wed Lunch End")]
        public string LWedClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Thu Lunch Start")]
        public string LThuStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Thu Lunch End")]
        public string LThuClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fri Lunch Start")]
        public string LFriStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fri Lunch End")]
        public string LFriClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sat Lunch Start")]
        public string LSatStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sat Lunch End")]
        public string LSatClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sun Lunch Start")]
        public string LSunStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sun Lunch End")]
        public string LSunClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mon Dinner Start")]
        public string MonStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Mon Dinner End")]
        public string MonClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Tue Dinner Start")]
        public string TueStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Tue Dinner End")]
        public string TueClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Wed Dinner Start")]
        public string WedStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Wed Dinner End")]
        public string WedClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Thu Dinner Start")]
        public string ThuStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Thu Dinner End")]
        public string ThuClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fri Dinner Start")]
        public string FriStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Fri Dinner End")]
        public string FriClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sat Dinner Start")]
        public string SatStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sat Dinner End")]
        public string SatClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sun Dinner Start")]
        public string SunStart { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Sun Dinner End")]
        public string SunClose { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Time Zone")]
        public int ZoneNameId { get; set; }

        [Required(ErrorMessage = "*")]
        [RegularExpression("^[01]?[- .]?(\\([2-9]{3}\\)|[2-9][0-9]{2})[- .]?[0-9]{3}[- .]?[0-9]{4}$", ErrorMessage = "*")]
        [Display(Name = "Restaurant Fax")]
        public string Fax { get; set; }

        [Display(Name = "Food Cost")]
        public string FoodCost { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Has Discount Coupons?")]
        public string HasDiscountCoupons { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Has Free Item Coupons?")]
        public string HasFreeItemCoupons { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Has Breakfast?")]
        public string HasBreakfast { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Has Lunch Special?")]
        public string HasLunchSpecial { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Can Pre-Order for Tomorrow")]
        public string CanOrderForNextday { get; set; }

        [Display(Name = "Website Address")]
        public string Website { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Do You Deliver?")]
        public string Delivery { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Delivery Radius(miles)")]
        public int Radius { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, double.MaxValue)]
        [Display(Name = "Delivery Charge")]
        public decimal DeliveryFee { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, double.MaxValue)]
        [Display(Name = "Delivery Minimum")]
        public decimal OrderMinimum { get; set; }

        [Required(ErrorMessage = "*")]
        [Range(0.00, 100.00)]
        [Display(Name = "Tax Percentage(%)")]
        public decimal TaxPercentageRate { get; set; }

        public BizInfo Bizinfo { get; set; }
        public List<ZoneName> LZoneNames { get; set; }
        public List<SelectListItem> ZoneNameChoices(List<ZoneName> lzc)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var z in lzc)
            {
                l.Add(new SelectListItem { Text = z.Title, Value = z.ZoneNameId.ToString() });
            }
            return l;
        }
        public List<SelectListItem> ZoneNameChoices(List<ZoneName> lzc,string selectedZoneId)
        {
            List<SelectListItem> l = new List<SelectListItem>();
            foreach (var z in lzc)
            {
                l.Add(new SelectListItem { Text = z.Title, Value = z.ZoneNameId.ToString(), Selected = selectedZoneId == z.ZoneNameId.ToString() });
            }
            return l;
        }
    }
}