using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
using FR.Infrastructure.Helpers;
namespace FR.Services.ViewServices
{
    public class AllCitiesView
    {
        public static string ShowAllCities(List<BizInfo> bi, int vNumberOfCity, string vCuisineName, int Cols = 4)
        {
            if (Cols <= 0) { Cols = 4; }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""float-left citytable"">");
            var bz = from b in bi
                     orderby b.Address.State, b.Address.City
                     group b by b.Address.State into g
                     select new { State = g.Key, Group = g, Count = g.Count() };
            int tl = vNumberOfCity + bz.Count();
            int row = tl / Cols;
            if (tl % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in bz)
            {
                var ct = from i in item.Group
                         orderby i.Address.City
                         group i by i.Address.City into c
                         select new { City = c.Key, Group = c, Count = c.Count() };
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""float-left citytable"">"));
                }
                sb.Append(string.Format(@"<div class=""citydiv""><b>{0}</b> ({1})</div>", item.State, item.Count));

                foreach (var e in ct)
                {
                    now++;
                    if (now > row)
                    {
                        now = 1;
                        sb.Append(string.Format(@"</div><div class=""float-left citytable"">"));
                    }

                    if (string.IsNullOrEmpty(vCuisineName))
                    {
                        sb.Append(string.Format(@"<div class=""citydiv""><a href=""/AllCities/City/{0}"">{1}</a> ({2})</div>", e.City.Replace(" ", "-"), e.City, e.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div class=""citydiv""><a href=""/CityCuisine?city={0}&cuisine={1}"">{2}</a> ({3})</div>", e.City.Replace(" ", "-"), vCuisineName, e.City, e.Count));
                    }
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public static string ShowAllCities_Bootstrap(List<BizInfo> bi, int vNumberOfCity, string vCuisineName)
        {
            int Cols = 4;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""col-sm-3 col-md-3"">");
            var bz = from b in bi
                     orderby b.Address.State, b.Address.City
                     group b by b.Address.State into g
                     select new { State = g.Key, Group = g, Count = g.Count() };
            int tl = vNumberOfCity + bz.Count();
            int row = tl / Cols;
            if (tl % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in bz)
            {
                var ct = from i in item.Group
                         orderby i.Address.City
                         group i by i.Address.City into c
                         select new { City = c.Key, Group = c, Count = c.Count() };
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""col-sm-3 col-md-3"">"));
                }
                sb.Append(string.Format(@"<div><b>{0}</b> ({1})</div>", item.State, item.Count));

                foreach (var e in ct)
                {
                    now++;
                    if (now > row)
                    {
                        now = 1;
                        sb.Append(string.Format(@"</div><div class=""col-sm-3 col-md-3"">"));
                    }

                    if (string.IsNullOrEmpty(vCuisineName))
                    {
                        sb.Append(string.Format(@"<div><a href=""/AllCities/City/{0}"">{1}</a> ({2})</div>", e.City.Replace(" ", "-"), e.City, e.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div><a href=""/CityCuisine?city={0}&cuisine={1}"">{2}</a> ({3})</div>", e.City.Replace(" ", "-"), vCuisineName, e.City, e.Count));
                    }
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public static string ShowMostPopularCities(List<BizInfo> bi, int Cols = 4)
        {
            if (Cols <= 0) { Cols = 4; }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""clearBoth inlineList""><ul>");
            var cus = from b in bi
                      orderby b.Address.City
                      group b by b.Address.City into g
                      select new { City = g.Key, Group = g, Count = g.Count() };
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > Cols)
                {
                    now = 1;
                    sb.Append(string.Format(@"</ul></div><div class=""clearBoth inlineList""><ul>"));
                }
                sb.Append(string.Format(@"<li><a href=""/AllCities/City/{0}""><b>{1}</b></a> ({2})</li> ", item.City.Replace(" ", "-"), item.City, item.Count));
            }
            sb.Append("</ul></div>");
            return sb.ToString();
        }
        public static string ShowMostPopularCities_bootatrap(List<BizInfo> bi)
        {
            int Cols = 4;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""col-sm-3 col-md-3"">");
            var cus = from b in bi
                      orderby b.Address.City
                      group b by b.Address.City into g
                      select new { City = g.Key, Group = g, Count = g.Count() };
            int row = cus.Count() / Cols;
            if (cus.Count() % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""col-sm-3 col-md-3"">"));
                }
                sb.Append(string.Format(@"<div><a href=""/AllCities/City/{0}""><b>{1}</b></a> ({2})</div> ", item.City.Replace(" ", "-"), item.City, item.Count));
            }
            sb.Append("</div>");
            return sb.ToString();
        }
    }
    public class AllCuisinesView
    {
        public static string ShowCuisinesView(List<BizCuisine> bc, string vCity, string vZip, int Cols = 4)
        {
            if (Cols <= 0) { Cols = 4; }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""float-left citytable"">");
            var cus = from b in bc
                      orderby b.CuisineType.Title
                      group b by b.CuisineType.Title into g
                      select new { CuisineType = g.Key, Group = g, Count = g.Count() };
            int tl = cus.Count();
            int row = tl / Cols;
            if (tl % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""float-left citytable"">"));
                }

                if (string.IsNullOrEmpty(vZip))
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<div class=""citydiv""><a href=""/AllCuisines/Cuisine/{0}""><b>{1}</b></a> ({2})</div>", item.CuisineType.Replace(" ", "-"), item.CuisineType, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div class=""citydiv""><a href=""/CityCuisine?city={0}&cuisine={1}""><b>{2}</b></a> ({3})</div>", vCity.Replace(" ", "-"), item.CuisineType, item.CuisineType, item.Count));
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<div class=""citydiv""><a href=""/Restaurants?zip={0}&cuisine={1}""><b>{2}</b></a> ({3})</div>", vZip, item.CuisineType.Replace(" ", "-"), item.CuisineType, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div class=""citydiv""><a href=""/Restaurants?city={0}&zip={1}&cuisine={2}""><b>{3}</b></a> ({4})</div>", vCity, vZip, item.CuisineType.Replace(" ", "-"), item.CuisineType, item.Count));
                    }
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public static string ShowCuisinesView_Bootatrap(List<BizCuisine> bc, string vCity, string vZip)
        {
            int Cols = 4;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""col-sm-3 col-md-3"">");
            var cus = from b in bc
                      orderby b.CuisineType.Title
                      group b by b.CuisineType.Title into g
                      select new { CuisineType = g.Key, Group = g, Count = g.Count() };
            int tl = cus.Count();
            int row = tl / Cols;
            if (tl % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""col-sm-3 col-md-3"">"));
                }

                if (string.IsNullOrEmpty(vZip))
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<div><a href=""/AllCuisines/Cuisine/{0}""><b>{1}</b></a> ({2})</div>", item.CuisineType.Replace(" ", "-"), item.CuisineType, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div><a href=""/CityCuisine?city={0}&cuisine={1}""><b>{2}</b></a> ({3})</div>", vCity.Replace(" ", "-"), item.CuisineType, item.CuisineType, item.Count));
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<div><a href=""/Restaurants?zip={0}&cuisine={1}""><b>{2}</b></a> ({3})</div>", vZip, item.CuisineType.Replace(" ", "-"), item.CuisineType, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div><a href=""/Restaurants?city={0}&zip={1}&cuisine={2}""><b>{3}</b></a> ({4})</div>", vCity, vZip, item.CuisineType.Replace(" ", "-"), item.CuisineType, item.Count));
                    }
                }
            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public static string ShowMostPopularCuisines(List<BizCuisine> bc, int Cols = 4)
        {
            if (Cols <= 0) { Cols = 4; }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""clearBoth inlineList""><ul>");
            var cus = from b in bc
                      orderby b.CuisineTypeName
                      group b by b.CuisineTypeName into g
                      select new { Cuisine = g.Key, Group = g, Count = g.Count() };
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > Cols)
                {
                    now = 1;
                    sb.Append(string.Format(@"</ul></div><div class=""clearBoth inlineList""><ul>"));
                }
                sb.Append(string.Format(@"<li><a href=""/AllCuisines/Cuisine/{0}""><b>{1}</b></a> ({2})</li> ", item.Cuisine.Replace(" ", "-"), item.Cuisine, item.Count));
            }
            sb.Append("</ul></div>");
            return sb.ToString();
        }
        public static string ShowMostPopularCuisines_Bootstarp(List<BizCuisine> bc)
        {
            int Cols = 4;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""col-sm-3 col-md-3"">");
            var cus = from b in bc
                      orderby b.CuisineTypeName
                      group b by b.CuisineTypeName into g
                      select new { Cuisine = g.Key, Group = g, Count = g.Count() };
            int row = cus.Count() / Cols;
            if (cus.Count() % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""col-sm-3 col-md-3"">"));
                }
                sb.Append(string.Format(@"<div><a href=""/AllCuisines/Cuisine/{0}""><b>{1}</b></a> ({2})</div> ", item.Cuisine.Replace(" ", "-"), item.Cuisine, item.Count));
            }
            sb.Append("</div>");
            return sb.ToString();
        }
    }
    public class AllZopCodesView
    {
        public static string ShowZipCodesView_Bootstarp(List<BizInfo> bc, string vCity, string vCuisineName)
        {
            int Cols = 6;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""col-sm-2 col-md-2"">");
            var cus = from b in bc
                      orderby b.Address.ZipCode
                      group b by b.Address.ZipCode into g
                      select new { Zipcode = g.Key, Group = g, Count = g.Count() };
            int tl = cus.Count();
            int row = tl / Cols;
            if (tl % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(@"</div><div class=""col-sm-2 col-md-2"">");
                }

                if (string.IsNullOrEmpty(vCuisineName))
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<div><a href=""/CityZip?zip={0}""><b>{1}</b></a> ({2})<br /></div>", item.Zipcode, item.Zipcode, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div><a href=""/CityZip?city={0}&zip={1}""><b>{2}</b></a> ({3})<br /></div>", vCity.Replace(" ", "-"), item.Zipcode, item.Zipcode, item.Count));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<div><a href=""/Restaurants?zip={0}&cuisine={1}""><b>{2}</b></a> ({3})<br /></div>", item.Zipcode, vCuisineName, item.Zipcode, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<div><a href=""/Restaurants?city={0}&zip={1}&cuisine={2}""><b>{3}</b></a> ({4})<br /></div>", vCity.Replace(" ", "-"), item.Zipcode, vCuisineName, item.Zipcode, item.Count));
                    }
                }



            }
            sb.Append("</div>");
            return sb.ToString();
        }
        public static string ShowZipCodesView(List<BizInfo> bc, string vCity, string vCuisineName, int Cols = 6)
        {
            if (Cols <= 0) { Cols = 6; }
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class=""float-left ziptable"">");
            var cus = from b in bc
                      orderby b.Address.ZipCode
                      group b by b.Address.ZipCode into g
                      select new { Zipcode = g.Key, Group = g, Count = g.Count() };
            int tl = cus.Count();
            int row = tl / Cols;
            if (tl % Cols != 0) { row++; }
            int now = 0;
            foreach (var item in cus)
            {
                now++;

                if (now > row)
                {
                    now = 1;
                    sb.Append(string.Format(@"</div><div class=""float-left ziptable"">"));
                }

                if (string.IsNullOrEmpty(vCuisineName))
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<a href=""/CityZip?zip={0}""><b>{1}</b></a> ({2})<br />", item.Zipcode, item.Zipcode, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<a href=""/CityZip?city={0}&zip={1}""><b>{2}</b></a> ({3})<br />", vCity.Replace(" ", "-"), item.Zipcode, item.Zipcode, item.Count));
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(vCity))
                    {
                        sb.Append(string.Format(@"<a href=""/Restaurants?zip={0}&cuisine={1}""><b>{2}</b></a> ({3})<br />", item.Zipcode, vCuisineName, item.Zipcode, item.Count));
                    }
                    else
                    {
                        sb.Append(string.Format(@"<a href=""/Restaurants?city={0}&zip={1}&cuisine={2}""><b>{3}</b></a> ({4})<br />", vCity.Replace(" ", "-"), item.Zipcode, vCuisineName, item.Zipcode, item.Count));
                    }
                }



            }
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
