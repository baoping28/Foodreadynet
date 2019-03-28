using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class AddSideRepository : BaseShoppingCartRepository, IAddSideRepository
    {
        #region IAddSideRepository Members

        public List<AddSide> GetAllAddSides()
        {
            List<AddSide> lAddSides = default(List<AddSide>);
            string lAddSidesKey = CacheKey + "_AllAddSides";

            if (base.EnableCaching && (Cache[lAddSidesKey] != null))
            {
                lAddSides = (List<AddSide>)Cache[lAddSidesKey];
            }
            else
            {
                lAddSides = (from lAddSide in Shoppingctx.AddSides.Include("Product")
                             select lAddSide).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lAddSidesKey, lAddSides, CacheDuration);
                }
            }
            return lAddSides;
        }

        public List<AddSide> GetAllAddSides(bool vActive)
        {
            return GetAllAddSides().Where(e => e.Active == vActive).ToList();
        }

        public AddSide GetAddSideById(int vAddSideID)
        {
            return GetAllAddSides().Where(e => e.AddSideId == vAddSideID).FirstOrDefault();
        }

        public int GetAllAddSideCount()
        {
            return Shoppingctx.AddSides.Count();
        }

        public int GetAllAddSideCount(bool vActive)
        {
            return Shoppingctx.AddSides.Where(e => e.Active == vActive).Count();
        }

        public List<AddSide> GetAddSidesByProductId(bool vActive, int vProductId)
        {
            return GetAllAddSides().Where(e => e.ProductId == vProductId && e.Active == vActive).ToList();
        }

        public AddSide AddAddSide(int vAddSideID, int vProductID, string vTitle, string vDescription, decimal vPrice, decimal vBizPrice, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            AddSide lAddSide = new AddSide();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vAddSideID > 0)
                {
                    lAddSide = frctx.AddSides.FirstOrDefault(u => u.AddSideId == vAddSideID);
                    lAddSide.Title = vTitle;
                    lAddSide.Description = vDescription;
                    lAddSide.Price = vPrice;
                    lAddSide.BizPrice = vBizPrice;

                    lAddSide.UpdatedDate = vUpdatedDate;
                    lAddSide.UpdatedBy = vUpdatedBy;
                    lAddSide.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lAddSide : null;
                }
                else
                {
                    lAddSide.Title = vTitle;
                    lAddSide.Description = vDescription;
                    lAddSide.Price = vPrice;
                    lAddSide.BizPrice = vBizPrice;

                    lAddSide.ProductId = vProductID;
                    lAddSide.AddedDate = vAddedDate;
                    lAddSide.AddedBy = vAddedBy;
                    lAddSide.UpdatedDate = vUpdatedDate;
                    lAddSide.UpdatedBy = vUpdatedBy;
                    lAddSide.Active = vActive;
                    return AddAddSide(lAddSide);
                }
            }
        }

        public AddSide AddAddSide(AddSide vAddSide)
        {
            try
            {
                Shoppingctx.AddSides.Add(vAddSide);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vAddSide : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockAddSide(AddSide vAddSide)
        {
            return ChangeLockState(vAddSide, false);
        }

        public bool UnlockAddSide(AddSide vAddSide)
        {
            return ChangeLockState(vAddSide, true);
        }

        private bool ChangeLockState(AddSide vAddSide, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                AddSide up = frenty.AddSides.FirstOrDefault(e => e.AddSideId == vAddSide.AddSideId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteAddSide(AddSide vAddSide)
        {
            throw new NotImplementedException();
        }

        public bool DeleteAddSide(int vAddSideID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteAddSide(AddSide vAddSide)
        {
            throw new NotImplementedException();
        }

        public AddSide UpdateAddSide(AddSide vAddSide)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
