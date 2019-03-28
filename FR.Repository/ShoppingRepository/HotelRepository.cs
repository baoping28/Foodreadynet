using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class HotelRepository : BaseShoppingCartRepository, IHotelRepository
    {
        public List<Hotel> GetAllHotels()
        {
            List<Hotel> lHotel = default(List<Hotel>);
            string lHotelKey = CacheKey + "_AllHotels";

            if (base.EnableCaching && (Cache[lHotelKey] != null))
            {
                lHotel = (List<Hotel>)Cache[lHotelKey];
            }
            else
            {
                lHotel = Shoppingctx.Hotels.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lHotelKey, lHotel, CacheDuration);
                }
            }
            return lHotel;
        }

        public List<Hotel> GetAllHotels(bool vActive)
        {
            return GetAllHotels().Where(e => e.Active == vActive).ToList();
        }

        public Hotel GetHotelById(int vHotelID)
        {
            return GetAllHotels().Where(e => e.HotelId == vHotelID).FirstOrDefault();
        }
        public Dictionary<string, List<Hotel>> GetCityHotelsByTypeId(int vHotelTypeId, bool vActive)
        {
            List<Hotel> lh = Shoppingctx.Hotels.Where(e => e.HotelTypeId == vHotelTypeId && e.Active== vActive).ToList();
            Dictionary<string, List<Hotel>> cityhotel = new Dictionary<string, List<Hotel>>();
            var bz = from b in lh
                     orderby b.City
                     group b by b.City into g
                     select new { City = g.Key, Group = g, Count = g.Count() };
            foreach (var item in bz)
            {
                List<Hotel> hl = new List<Hotel>();
                
                foreach (var e in item.Group)
                {
                    hl.Add(e);
                }
                cityhotel.Add(item.City + " -- " + hl.FirstOrDefault().State + " (" + hl.Count + ")", hl.OrderBy(e=>e.Name).ToList());
            }
            return cityhotel;
        }
        public int GetAllHotelCount(bool vActive)
        {
            return Shoppingctx.Hotels.Where(e => e.Active == vActive).Count();
        }

        public Hotel AddHotel(int vHotelID, int vHotelTypeID, string vName, string vAddressLine, string vCity, string vState, string vZipCode, string vImageUrl, string vLatitude, string vLongitude, string vDescription, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            Hotel lHotel = new Hotel();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vHotelID > 0)
                {
                    lHotel = frctx.Hotels.FirstOrDefault(u => u.HotelId == vHotelID);
                    lHotel.Name = vName;
                    lHotel.AddressLine = vAddressLine;
                    lHotel.City = vCity;
                    lHotel.State = vState;

                    lHotel.ZipCode = vZipCode;
                    lHotel.ImageUrl = vImageUrl;
                    lHotel.Latitude = vLatitude;
                    lHotel.Longitude = vLongitude;
                    lHotel.Description = vDescription;
                    lHotel.UpdatedDate = vUpdatedDate;
                    lHotel.UpdatedBy = vUpdatedBy;
                    lHotel.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lHotel : null;
                }
                else
                {
                    lHotel.HotelTypeId = vHotelTypeID;
                    lHotel.Name = vName;
                    lHotel.AddressLine = vAddressLine;
                    lHotel.City = vCity;
                    lHotel.State = vState;

                    lHotel.ZipCode = vZipCode;
                    lHotel.ImageUrl = vImageUrl;
                    lHotel.Latitude = vLatitude;
                    lHotel.Longitude = vLongitude;
                    lHotel.Description = vDescription;
                    lHotel.AddedDate = vAddedDate;
                    lHotel.AddedBy = vAddedBy;
                    lHotel.UpdatedDate = vUpdatedDate;
                    lHotel.UpdatedBy = vUpdatedBy;
                    lHotel.Active = vActive;
                    return AddHotel(lHotel);
                }
            }
        }

        public Hotel AddHotel(Hotel vHotel)
        {
            try
            {
                Shoppingctx.Hotels.Add(vHotel);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vHotel : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteHotel(Hotel vHotel)
        {
            throw new NotImplementedException();
        }

        public bool DeleteHotel(int vHotelID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteHotel(Hotel vHotel)
        {
            throw new NotImplementedException();
        }

        public Hotel UpdateHotel(Hotel vHotel)
        {
            throw new NotImplementedException();
        }
    }
}
