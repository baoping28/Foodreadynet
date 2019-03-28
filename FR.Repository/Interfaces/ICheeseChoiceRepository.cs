using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;


namespace FR.Repository.Interfaces
{
    public interface ICheeseAmountRepository
    {
        List<CheeseAmount> GetAllCheeseAmounts();
        List<CheeseAmount> GetAllCheeseAmounts(bool vActive);
        CheeseAmount GetCheeseAmountById(int vCheeseAmountID);
        int GetAllCheeseAmountCount();
        int GetAllCheeseAmountCount(bool vActive);
        List<CheeseAmount> GetCheeseAmountsByProductId(bool vActive, int vProductId);
        CheeseAmount AddCheeseAmount(int vCheeseAmountID, int vProductID, string vTitle, string vDescription, decimal vPrice, decimal vBizPrice,
                DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        CheeseAmount AddCheeseAmount(CheeseAmount vCheeseAmount);
        bool LockCheeseAmount(CheeseAmount vCheeseAmount);
        bool UnlockCheeseAmount(CheeseAmount vCheeseAmount);
        bool DeleteCheeseAmount(CheeseAmount vCheeseAmount);
        bool DeleteCheeseAmount(int vCheeseAmountID);
        bool UnDeleteCheeseAmount(CheeseAmount vCheeseAmount);
        CheeseAmount UpdateCheeseAmount(CheeseAmount vCheeseAmount);
    }
}

