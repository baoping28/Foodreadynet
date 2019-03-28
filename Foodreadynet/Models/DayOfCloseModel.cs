using System.Collections.Generic;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace FoodReady.WebUI.Models
{
    public class DayOfCloseModel
    {
        [HiddenInput(DisplayValue = false)]
        public int BizInfoID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DayOfCloseID { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Item Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Close Date")]
        public string CloseDay { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Time Zone")]
        public int ZoneNameId { get; set; }

        public List<DayOfClose> CloseDays { get; set; }

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
        public List<SelectListItem> ZoneNameChoices(List<ZoneName> lzc, string selectedZoneId)
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