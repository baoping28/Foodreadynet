using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface IBizImageRepository
    {
        List<BizImage> GetAllBizImages();
        List<BizImage> GetAllBizImages(bool vActive);
        BizImage GetBizImageById(int vBizImageID);
        int GetAllBizImageCount();
        int GetAllBizImageCount(bool vActive);
        List<BizImage> GetBizImagesByBizInfoId(bool vActive, int vBizInfoId);
        BizImage AddBizImage(int vBizImageID, int vBizInfoID, string vSmallImageName, string vBigImageName, bool vActive);
        BizImage AddBizImage(BizImage vBizImage);
        bool LockBizImage(BizImage vBizImage);
        bool UnlockBizImage(BizImage vBizImage);
        bool DeleteBizImage(BizImage vBizImage);
        bool DeleteBizImage(int vBizImageID);
        bool UnDeleteBizImage(BizImage vBizImage);
        BizImage UpdateBizImage(BizImage vBizImage);
    }
}
