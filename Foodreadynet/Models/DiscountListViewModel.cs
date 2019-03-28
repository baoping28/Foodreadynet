using System.Collections.Generic;
using System.Linq;
using FR.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace FoodReady.WebUI.Models
{
    public class DiscountListViewModel
    {
        public List<BizInfo> LBizInfos { get; set; }
        public BrowseHistory History { get; set; }
    }
}