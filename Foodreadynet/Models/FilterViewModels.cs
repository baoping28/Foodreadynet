using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FR.Domain.Model.Entities;
namespace FoodReady.WebUI.Models
{
    public class AssistanceViewModel
    {
        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
    }

    public class FilterViewModels
    {
        public FilterViewModels()
        {

            ScheduleAssistances = new[]
        {
            new SelectListItem{Text="Order for Today",Value="1"},
            new SelectListItem {Text="Order for Tomorrow",Value="2"}
        };
            TimeAssistances = new[]
        {
            new SelectListItem{Text="ASAP",Value="ASAP"},
            new SelectListItem{Text="12:00 AM",Value="12:00 AM"},
            new SelectListItem {Text="12:10 AM",Value="12:10 AM"},
            new SelectListItem {Text="12:20 AM",Value="12:20 AM"},
            new SelectListItem{Text="12:30 AM",Value="12:30 AM"},
            new SelectListItem {Text="12:40 AM",Value="12:40 AM"},
            new SelectListItem{Text="12:50 AM",Value="12:50 AM"},
            new SelectListItem {Text="1:00 AM",Value="1:00 AM"},
            new SelectListItem {Text="1:10 AM",Value="1:10 AM"},
            new SelectListItem {Text="1:20 AM",Value="1:20 AM"},
            new SelectListItem{Text="1:30 AM",Value="1:30 AM"},
            new SelectListItem {Text="1:40 AM",Value="1:40 AM"},
            new SelectListItem{Text="1:50 AM",Value="1:50 AM"},
            new SelectListItem {Text="2:00 AM",Value="2:00 AM"},
            new SelectListItem {Text="2:10 AM",Value="2:10 AM"},
            new SelectListItem {Text="2:20 AM",Value="2:20 AM"},
            new SelectListItem{Text="2:30 AM",Value="2:30 AM"},
            new SelectListItem {Text="2:40 AM",Value="2:40 AM"},
            new SelectListItem{Text="2:50 AM",Value="2:50 AM"},
            new SelectListItem {Text="3:00 AM",Value="3:00 AM"},
            new SelectListItem {Text="3:10 AM",Value="3:10 AM"},
            new SelectListItem {Text="3:20 AM",Value="3:20 AM"},
            new SelectListItem{Text="3:30 AM",Value="3:30 AM"},
            new SelectListItem {Text="3:40 AM",Value="3:40 AM"},
            new SelectListItem{Text="3:50 AM",Value="3:50 AM"},
            new SelectListItem {Text="4:00 AM",Value="4:00 AM"},
            new SelectListItem {Text="4:10 AM",Value="4:10 AM"},
            new SelectListItem {Text="4:20 AM",Value="4:20 AM"},
            new SelectListItem{Text="4:30 AM",Value="4:30 AM"},
            new SelectListItem {Text="4:40 AM",Value="4:40 AM"},
            new SelectListItem{Text="4:50 AM",Value="4:50 AM"},
            new SelectListItem {Text="5:00 AM",Value="5:00 AM"},
            new SelectListItem {Text="5:10 AM",Value="5:10 AM"},
            new SelectListItem {Text="5:20 AM",Value="5:20 AM"},
            new SelectListItem{Text="5:30 AM",Value="5:30 AM"},
            new SelectListItem {Text="5:40 AM",Value="5:40 AM"},
            new SelectListItem{Text="5:50 AM",Value="5:50 AM"},
            new SelectListItem {Text="6:00 AM",Value="6:00 AM"},
            new SelectListItem {Text="6:10 AM",Value="6:10 AM"},
            new SelectListItem {Text="6:20 AM",Value="6:20 AM"},
            new SelectListItem{Text="6:30 AM",Value="6:30 AM"},
            new SelectListItem {Text="6:40 AM",Value="6:40 AM"},
            new SelectListItem{Text="6:50 AM",Value="6:50 AM"},
            new SelectListItem {Text="7:00 AM",Value="7:00 AM"},
            new SelectListItem {Text="7:10 AM",Value="7:10 AM"},
            new SelectListItem {Text="7:20 AM",Value="7:20 AM"},
            new SelectListItem{Text="7:30 AM",Value="7:30 AM"},
            new SelectListItem {Text="7:40 AM",Value="7:40 AM"},
            new SelectListItem{Text="7:50 AM",Value="7:50 AM"},
            new SelectListItem {Text="8:00 AM",Value="8:00 AM"},
            new SelectListItem {Text="8:10 AM",Value="8:10 AM"},
            new SelectListItem {Text="8:20 AM",Value="8:20 AM"},
            new SelectListItem{Text="8:30 AM",Value="8:30 AM"},
            new SelectListItem {Text="8:40 AM",Value="8:40 AM"},
            new SelectListItem{Text="8:50 AM",Value="8:50 AM"},
            new SelectListItem {Text="9:00 AM",Value="9:00 AM"},
            new SelectListItem {Text="9:10 AM",Value="9:10 AM"},
            new SelectListItem {Text="9:20 AM",Value="9:20 AM"},
            new SelectListItem{Text="9:30 AM",Value="9:30 AM"},
            new SelectListItem {Text="9:40 AM",Value="9:40 AM"},
            new SelectListItem{Text="9:50 AM",Value="9:50 AM"},
            new SelectListItem {Text="10:00 AM",Value="10:00 AM"},
            new SelectListItem {Text="10:10 AM",Value="10:10 AM"},
            new SelectListItem {Text="10:20 AM",Value="10:20 AM"},
            new SelectListItem{Text="10:30 AM",Value="10:30 AM"},
            new SelectListItem {Text="10:40 AM",Value="10:40 AM"},
            new SelectListItem{Text="10:50 AM",Value="10:50 AM"},
            new SelectListItem {Text="11:00 AM",Value="11:00 AM"},
            new SelectListItem {Text="11:10 AM",Value="11:10 AM"},
            new SelectListItem {Text="11:20 AM",Value="11:20 AM"},
            new SelectListItem{Text="11:30 AM",Value="11:30 AM"},
            new SelectListItem {Text="11:40 AM",Value="11:40 AM"},
            new SelectListItem{Text="11:50 AM",Value="11:50 AM"},
            new SelectListItem {Text="12:00 PM",Value="12:00 PM"},
            new SelectListItem {Text="12:10 PM",Value="12:10 PM"},
            new SelectListItem {Text="12:20 PM",Value="12:20 PM"},
            new SelectListItem{Text="12:30 PM",Value="12:30 PM"},
            new SelectListItem {Text="12:40 PM",Value="12:40 PM"},
            new SelectListItem{Text="12:50 PM",Value="12:50 PM"},
            new SelectListItem {Text="1:00 PM",Value="1:00 PM"},
            new SelectListItem {Text="1:10 PM",Value="1:10 PM"},
            new SelectListItem {Text="1:20 PM",Value="1:20 PM"},
            new SelectListItem{Text="1:30 PM",Value="1:30 PM"},
            new SelectListItem {Text="1:40 PM",Value="1:40 PM"},
            new SelectListItem{Text="1:50 PM",Value="1:50 PM"},
            new SelectListItem {Text="2:00 PM",Value="2:00 PM"},
            new SelectListItem {Text="2:10 PM",Value="2:10 PM"},
            new SelectListItem {Text="2:20 PM",Value="2:20 PM"},
            new SelectListItem{Text="2:30 PM",Value="2:30 PM"},
            new SelectListItem {Text="2:40 PM",Value="2:40 PM"},
            new SelectListItem{Text="2:50 PM",Value="2:50 PM"},
            new SelectListItem {Text="3:00 PM",Value="3:00 PM"},
            new SelectListItem {Text="3:10 PM",Value="3:10 PM"},
            new SelectListItem {Text="3:20 PM",Value="3:20 PM"},
            new SelectListItem{Text="3:30 PM",Value="3:30 PM"},
            new SelectListItem {Text="3:40 PM",Value="3:40 PM"},
            new SelectListItem{Text="3:50 PM",Value="3:50 PM"},
            new SelectListItem {Text="4:00 PM",Value="4:00 PM"},
            new SelectListItem {Text="4:10 PM",Value="4:10 PM"},
            new SelectListItem {Text="4:20 PM",Value="4:20 PM"},
            new SelectListItem{Text="4:30 PM",Value="4:30 PM"},
            new SelectListItem {Text="4:40 PM",Value="4:40 PM"},
            new SelectListItem{Text="4:50 PM",Value="4:50 PM"},
            new SelectListItem {Text="5:00 PM",Value="5:00 PM"},
            new SelectListItem {Text="5:10 PM",Value="5:10 PM"},
            new SelectListItem {Text="5:20 PM",Value="5:20 PM"},
            new SelectListItem{Text="5:30 PM",Value="5:30 PM"},
            new SelectListItem {Text="5:40 PM",Value="5:40 PM"},
            new SelectListItem{Text="5:50 PM",Value="5:50 PM"},
            new SelectListItem {Text="6:00 PM",Value="6:00 PM"},
            new SelectListItem {Text="6:10 PM",Value="6:10 PM"},
            new SelectListItem {Text="6:20 PM",Value="6:20 PM"},
            new SelectListItem{Text="6:30 PM",Value="6:30 PM"},
            new SelectListItem {Text="6:40 PM",Value="6:40 PM"},
            new SelectListItem{Text="6:50 PM",Value="6:50 PM"},
            new SelectListItem {Text="7:00 PM",Value="7:00 PM"},
            new SelectListItem {Text="7:10 PM",Value="7:10 PM"},
            new SelectListItem {Text="7:20 PM",Value="7:20 PM"},
            new SelectListItem{Text="7:30 PM",Value="7:30 PM"},
            new SelectListItem {Text="7:40 PM",Value="7:40 PM"},
            new SelectListItem{Text="7:50 PM",Value="7:50 PM"},
            new SelectListItem {Text="8:00 PM",Value="8:00 PM"},
            new SelectListItem {Text="8:10 PM",Value="8:10 PM"},
            new SelectListItem {Text="8:20 PM",Value="8:20 PM"},
            new SelectListItem{Text="8:30 PM",Value="8:30 PM"},
            new SelectListItem {Text="8:40 PM",Value="8:40 PM"},
            new SelectListItem{Text="8:50 PM",Value="8:50 PM"},
            new SelectListItem {Text="9:00 PM",Value="9:00 PM"},
            new SelectListItem {Text="9:10 PM",Value="9:10 PM"},
            new SelectListItem {Text="9:20 PM",Value="9:20 PM"},
            new SelectListItem{Text="9:30 PM",Value="9:30 PM"},
            new SelectListItem {Text="9:40 PM",Value="9:40 PM"},
            new SelectListItem{Text="9:50 PM",Value="9:50 PM"},
            new SelectListItem {Text="10:00 PM",Value="10:00 PM"},
            new SelectListItem {Text="10:10 PM",Value="10:10 PM"},
            new SelectListItem {Text="10:20 PM",Value="10:20 PM"},
            new SelectListItem{Text="10:30 PM",Value="10:30 PM"},
            new SelectListItem {Text="10:40 PM",Value="10:40 PM"},
            new SelectListItem{Text="10:50 PM",Value="10:50 PM"},
            new SelectListItem {Text="11:00 PM",Value="11:00 PM"},
            new SelectListItem {Text="11:10 PM",Value="11:10 PM"},
            new SelectListItem {Text="11:20 PM",Value="11:20 PM"},
            new SelectListItem{Text="11:30 PM",Value="11:30 PM"},
            new SelectListItem {Text="11:40 PM",Value="11:40 PM"},
            new SelectListItem{Text="11:50 PM",Value="11:50 PM"},
        };

            RatingAssistances = new[]
        {
            new AssistanceViewModel {GroupName="Rating", Name = "5 Stars" ,Value="5",Id="5Stars"},
            new AssistanceViewModel {GroupName="Rating", Name = "4 Stars" ,Value="4",Id="4Stars"},
            new AssistanceViewModel {GroupName="Rating", Name = "3 Stars" ,Value="3",Id="3Stars"},
            new AssistanceViewModel {GroupName="Rating", Name = "2 Stars" ,Value="2",Id="2Stars"},
            new AssistanceViewModel {GroupName="Rating", Name = "1 Stars" ,Value="1",Id="1Stars"}
        }.ToList();

            FeatherAssistances = new[]
        {
            new AssistanceViewModel {GroupName="freeDelivery", Name = "Free Delivery" ,Value="unchecked"},
            new AssistanceViewModel {GroupName="breakfast", Name = "Breakfast",Value="unchecked" },
            new AssistanceViewModel {GroupName="lunchSpecial", Name = "Lunch Special",Value="unchecked" },
            new AssistanceViewModel {GroupName="coupons", Name = "Coupons" ,Value="unchecked"},
            new AssistanceViewModel {GroupName="freeItems", Name = "Free Items",Value="unchecked" }
        }.ToList();

            MinimumAssistances = new[]
        {
            new AssistanceViewModel {GroupName="Price", Name = "< $5 " ,Value="5",Id="min5"},
            new AssistanceViewModel {GroupName="Price", Name = "< $10" ,Value="10",Id="min10"},
            new AssistanceViewModel {GroupName="Price", Name = "< $15" ,Value="15",Id="min15"},
            new AssistanceViewModel {GroupName="Price", Name = "< $20" ,Value="20",Id="min20"},
            new AssistanceViewModel {GroupName="Price", Name = "< $30" ,Value="30",Id="min30"}
        }.ToList();

            DistanceAssistances = new[]
        {
            new AssistanceViewModel {GroupName="Distance", Name = "< 0.5mi",Value="0.5",Id="Distance1" },
            new AssistanceViewModel {GroupName="Distance", Name = "< 1.0mi" ,Value="1.0",Id="Distance2"},
            new AssistanceViewModel {GroupName="Distance", Name = "< 3.0mi",Value="3.0",Id="Distance3" },
            new AssistanceViewModel {GroupName="Distance", Name = "< 5.0mi" ,Value="5.0",Id="Distance4"},
            new AssistanceViewModel {GroupName="Distance", Name = "< 7.0mi",Value="7.0",Id="Distance5" },
            new AssistanceViewModel {GroupName="Distance", Name = "< 10.0mi",Value="10.0",Id="Distance6" }
        }.ToList();
        }

        
        public string Subject { get; set; }
        public IEnumerable<SelectListItem> SubjectValues
        {
            get
            {
                return new[]
            {
                new SelectListItem { Value = "General Inquiry", Text = "General Inquiry" },
                new SelectListItem { Value = "Full Wedding Package", Text = "Full Wedding Package" },
                new SelectListItem { Value = "Day of Wedding", Text = "Day of Wedding" },
                new SelectListItem { Value = "Hourly Consultation", Text = "Hourly Consultation" }  
            };
            }
        }

        public IList<AssistanceViewModel> RatingAssistances { get; set; }
        public IList<AssistanceViewModel> FeatherAssistances { get; set; }
        public IList<AssistanceViewModel> MinimumAssistances { get; set; }
        public IList<AssistanceViewModel> DistanceAssistances { get; set; }
        public IEnumerable<SelectListItem> ScheduleAssistances { get; set; }
        public string CuisineDropDown { get; set; }
        public IEnumerable<SelectListItem> TimeAssistances { get; set; }
        public List<SelectListItem> CuisineAssistances { get; set; }
        public List<BizInfo> BizInfos { get; set; }
        public List<BizInfo> BizOpenSet { get; set; }
        public List<BizInfo> BizCloseSet { get; set; }
        public List<BizInfo> BizHiddenSet { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string cuisine { get; set; }
        public string userFullAddress { get; set; }
        public BrowseHistory History;
        public BizDescriptionModel BizDescription { get; set; }
        public string MapMarkers { get; set; }
    }
}