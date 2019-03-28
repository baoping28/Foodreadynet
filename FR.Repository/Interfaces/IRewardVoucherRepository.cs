using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IRewardVoucherRepository
    {
        List<RewardVoucher> GetAllRewardVouchers();
        List<RewardVoucher> GetAllRewardVouchers(bool vActive);
        RewardVoucher GetRewardVoucherById(int vRewardVoucherID);
        int GetAllRewardVoucherCount();
        int GetAllRewardVoucherCount(bool vActive);
        RewardVoucher AddRewardVoucher(int vRewardVoucherID, string vTitle, string vDescription, string vImageUrl,
              decimal vValue, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        RewardVoucher AddRewardVoucher(RewardVoucher vRewardVoucher);
        bool LockRewardVoucher(RewardVoucher vRewardVoucher);
        bool UnlockRewardVoucher(RewardVoucher vRewardVoucher);
        bool DeleteRewardVoucher(RewardVoucher vRewardVoucher);
        bool DeleteRewardVoucher(int vRewardVoucherID);
        bool UnDeleteRewardVoucher(RewardVoucher vRewardVoucher);
        RewardVoucher UpdateRewardVoucher(RewardVoucher vRewardVoucher);
    }
}