using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;
namespace FR.Repository.Interfaces
{
    public interface IVoteRepository
    {
        List<Vote> GetAllVotes();
        List<Vote> GetAllVotes(bool vActive);
        Vote GetVoteById(int vVoteID);
        List<Vote> GetAllVotesByBizInfoId(int vBizInfoId);
        List<Vote> GetAllVotesByUserId(string vUserId);
        List<Vote> GetAllVotesByUserId(string vUserId, bool vActive);
        int GetAllVoteCount();
        int GetAllVoteCount(bool vActive);
        Vote AddVote(int vVoteID, int vBizInfoID, string vTitle, string vPenName, string vUserID, int vRatingVote, 
                     string vIpAddress,string vComment,DateTime vAddedDate, string vAddedBy,
                     DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Vote AddVote(Vote vVote);
        bool LockVote(Vote vVote);
        bool UnlockVote(Vote vVote);
        bool DeleteVote(Vote vVote);
        bool DeleteVote(int vVoteID);
        bool UnDeleteVote(Vote vVote);
        Vote UpdateVote(Vote vVote);
    }
}