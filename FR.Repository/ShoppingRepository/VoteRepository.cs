using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class VoteRepository : BaseShoppingCartRepository, IVoteRepository
    {
        #region IVoteRepository Members

        public List<Vote> GetAllVotes()
        {
            List<Vote> lVotes = default(List<Vote>);
            string lVotesKey = CacheKey + "_AllVotes";

            if (base.EnableCaching && (Cache[lVotesKey] != null))
            {
                lVotes = (List<Vote>)Cache[lVotesKey];
            }
            else
            {
                lVotes = (from lVote in Shoppingctx.Votes
                             orderby lVote.Title
                             select lVote).ToList();
                if (base.EnableCaching)
                {
                    CacheData(lVotesKey, lVotes, CacheDuration);
                }
            }
            return lVotes;
        }
        public List<Vote> GetAllVotes(bool vActive)
        {
          return  GetAllVotes().Where(e => e.Active == vActive).ToList();
        }
        public Vote GetVoteById(int vVoteID)
        {
            return GetAllVotes().Where(e => e.VoteId == vVoteID).FirstOrDefault();
        }
        public List<Vote> GetAllVotesByBizInfoId(int vBizInfoId)
        {
            return GetAllVotes().Where(e => e.BizInfoId == vBizInfoId).ToList();
        }
        public List<Vote> GetAllVotesByUserId(string vUserId)
        {
            return GetAllVotes().Where(e => e.UserId== vUserId).ToList();
        }
        public List<Vote> GetAllVotesByUserId(string vUserId, bool vActive)
        {
            return GetAllVotes().Where(e => e.UserId == vUserId && e.Active==vActive).ToList();
        }
        public int GetAllVoteCount()
        {
            return Shoppingctx.Votes.Count();
        }
        public int GetAllVoteCount(bool vActive)
        {
            return Shoppingctx.Votes.Where(e => e.Active == vActive).Count();
        }

        public Vote AddVote(int vVoteID, int vBizInfoID, string vTitle, string vPenName, string vUserID, int vRatingVote,
                     string vIpAddress, string vComment, DateTime vAddedDate, string vAddedBy,
                     DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            Vote lVote = new Vote();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vVoteID > 0)
                {
                    lVote = frctx.Votes.FirstOrDefault(u => u.VoteId == vVoteID);
                    lVote.BizInfoId = vBizInfoID;
                    lVote.Title = vTitle;
                    lVote.PenName = vPenName;
                    lVote.UserId = vUserID;
                    lVote.RatingVote = vRatingVote;

                    lVote.IpAddress = vIpAddress;
                    lVote.Comment = vComment;

                    lVote.UpdatedDate = vUpdatedDate;
                    lVote.UpdatedBy = vUpdatedBy;
                    lVote.Active = vActive;
                    return frctx.SaveChanges() > 0 ? lVote : null;
                }
                else
                {
                    lVote.BizInfoId = vBizInfoID;
                    lVote.Title = vTitle;
                    lVote.PenName = vPenName;
                    lVote.UserId = vUserID;
                    lVote.RatingVote = vRatingVote;

                    lVote.IpAddress = vIpAddress;
                    lVote.Comment = vComment;

                    lVote.AddedDate = vAddedDate;
                    lVote.AddedBy = vAddedBy;
                    lVote.UpdatedDate = vUpdatedDate;
                    lVote.UpdatedBy = vUpdatedBy;
                    lVote.Active = vActive;
                    return AddVote(lVote);
                }
            }
        }

        public Vote AddVote(Vote vVote)
        {
            try
            {
                    Shoppingctx.Votes.Add(vVote);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vVote : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool LockVote(Vote vVote)
        {
            return ChangeLockState(vVote, false);
        }
        public bool UnlockVote(Vote vVote)
        {
            return ChangeLockState(vVote, true);
        }
        private bool ChangeLockState(Vote vVote, bool vState)
        {
            using (FRShoppingEntities frenty = new FRShoppingEntities())
            {
                Vote up = frenty.Votes.FirstOrDefault(e => e.VoteId == vVote.VoteId);
                up.UpdatedDate = DateTime.Now;
                up.Active = vState;
                return frenty.SaveChanges() > 0 ? true : false;
            }
        }
        public bool DeleteVote(Vote vVote)
        {
            throw new NotImplementedException();
        }

        public bool DeleteVote(int vVoteID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteVote(Vote vVote)
        {
            throw new NotImplementedException();
        }

        public Vote UpdateVote(Vote vVote)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
