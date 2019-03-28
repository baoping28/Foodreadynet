using System.Collections.Generic;
using FR.Domain.Model.Entities;
using System.Web.Mvc;

namespace FoodReady.WebUI.Models
{
    public class OrderDriversModel
    {
        public Order Order { get; set; }
        public Driver Driver { get; set; }
        public List<Driver> LDrivers { get; set; }

    }
}