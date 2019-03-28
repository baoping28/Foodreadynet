using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace FR.Repository.ShoppingRepository
{
    public class MealSectionRepository : BaseShoppingCartRepository, IMealSectionRepository
    {
        #region IMealSectionRepository Members
        public List<MealSection> GetAllMealSections()
        {
            List<MealSection> lMealSection = default(List<MealSection>);
            string lMealSectionKey = CacheKey + "_AllMealSections";

            if (base.EnableCaching && (Cache[lMealSectionKey] != null))
            {
                lMealSection = (List<MealSection>)Cache[lMealSectionKey];
            }
            else
            {
                lMealSection = Shoppingctx.MealSections.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lMealSectionKey, lMealSection, CacheDuration);
                }
            }
            return lMealSection;
        }
        public List<MealSection> GetAllMealSections(bool vActive)
        {
            return GetAllMealSections().Where(e => e.Active == vActive).ToList();
        }

        public MealSection GetMealSectionById(int vMealSectionID)
        {
            return GetAllMealSections().FirstOrDefault(e => e.MealSectionId == vMealSectionID);
        }

        public int GetAllMealSectionCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public MealSection AddMealSection(int vMealSectionID, string vTitle, string vDescription, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            MealSection lMealSection = new MealSection();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vMealSectionID > 0)
                {
                    lMealSection = frctx.MealSections.FirstOrDefault(u => u.MealSectionId == vMealSectionID);
                    lMealSection.Title = vTitle;
                    lMealSection.Description = vDescription;

                    lMealSection.UpdatedDate = vUpdatedDate;
                    lMealSection.UpdatedBy = vUpdatedBy;
                    lMealSection.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lMealSection : null;
                }
                else
                {
                    lMealSection.Title = vTitle;
                    lMealSection.Description = vDescription;

                    lMealSection.AddedDate = vAddedDate;
                    lMealSection.AddedBy = vAddedBy;
                    lMealSection.UpdatedDate = vUpdatedDate;
                    lMealSection.UpdatedBy = vUpdatedBy;
                    lMealSection.Active = vActive;
                    return AddMealSection(lMealSection);
                }
            }
        }

        public MealSection AddMealSection(MealSection vMealSection)
        {
            try
            {

                Shoppingctx.MealSections.Add(vMealSection);

                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vMealSection : null;
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteMealSection(MealSection vMealSection)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMealSection(int vMealSectionID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteMealSection(MealSection vMealSection)
        {
            throw new NotImplementedException();
        }

        public MealSection UpdateMealSection(MealSection vMealSection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
