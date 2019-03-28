using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class BizImageRepository : BaseShoppingCartRepository, IBizImageRepository
    {
        #region IBizImageRepository Members

        public List<BizImage> GetAllBizImages()
        {
            List<BizImage> lBizImages = default(List<BizImage>);
            string lBizImagesKey = CacheKey + "_AllBizImages";

            if (base.EnableCaching && (Cache[lBizImagesKey] != null))
            {
                lBizImages = (List<BizImage>)Cache[lBizImagesKey];
            }
            else
            {
                lBizImages = (from lBizImage in Shoppingctx.BizImages.Include("BizInfo")
                                 select lBizImage).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lBizImagesKey, lBizImages, CacheDuration);
                }
            }
            return lBizImages;
        }

        public List<BizImage> GetAllBizImages(bool vActive)
        {
            return GetAllBizImages().Where(e => e.Active == vActive).ToList();
        }

        public BizImage GetBizImageById(int vBizImageID)
        {
            return GetAllBizImages().Where(e => e.BizImageId == vBizImageID).FirstOrDefault();
        }

        public int GetAllBizImageCount()
        {
            return Shoppingctx.BizImages.Count();
        }

        public int GetAllBizImageCount(bool vActive)
        {
            return Shoppingctx.BizImages.Where(e => e.Active == vActive).Count();
        }

        public List<BizImage> GetBizImagesByBizInfoId(bool vActive, int vBizInfoId)
        {
            return GetAllBizImages().Where(e => e.BizInfoId == vBizInfoId && e.Active == vActive).ToList();
        }


        public BizImage AddBizImage(int vBizImageID, int vBizInfoID, string vSmallImageName, string vBigImageName, bool vActive)
        {
            BizImage lBizImage = new BizImage();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vBizImageID > 0)
                {
                    lBizImage = frctx.BizImages.FirstOrDefault(u => u.BizImageId == vBizImageID);
                    lBizImage.SmallImageName = vSmallImageName;
                    lBizImage.BigImageName = vBigImageName;
                    lBizImage.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lBizImage : null;
                }
                else
                {
                    lBizImage.BizInfoId = vBizInfoID;
                    lBizImage.SmallImageName = vSmallImageName;
                    lBizImage.BigImageName = vBigImageName;
                    lBizImage.Active = vActive;
                    return AddBizImage(lBizImage);
                }
            }
        }
        public BizImage AddBizImage(BizImage vBizImage)
        {
            try
            {
                 Shoppingctx.BizImages.Add(vBizImage);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vBizImage : null;
            }
            catch
            {
                return null;
            }
        }

        public bool LockBizImage(BizImage vBizImage)
        {
            return ChangeLockState(vBizImage, false);
        }

        public bool UnlockBizImage(BizImage vBizImage)
        {
            return ChangeLockState(vBizImage, true);
        }

        private bool ChangeLockState(BizImage vBizImage, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                BizImage up = frenty.BizImages.FirstOrDefault(e => e.BizImageId == vBizImage.BizImageId);
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteBizImage(BizImage vBizImage)
        {
            throw new NotImplementedException();
        }

        public bool DeleteBizImage(int vBizImageID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteBizImage(BizImage vBizImage)
        {
            throw new NotImplementedException();
        }

        public BizImage UpdateBizImage(BizImage vBizImage)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
