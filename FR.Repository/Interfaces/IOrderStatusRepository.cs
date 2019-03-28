using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IOrderStatusRepository
    {
        List<OrderStatus> GetAllOrderStatuses(bool vActive);
        OrderStatus GetOrderStatusById(int vOrderStatusID);
        int GetAllOrderStatusCount(bool vActive);
        OrderStatus AddOrderStatus(int vOrderStatusID, string vTitle, DateTime vAddedDate, string vAddedBy,
                     DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        OrderStatus AddOrderStatus(OrderStatus vOrderStatus);
        bool DeleteOrderStatus(OrderStatus vOrderStatus);
        bool DeleteOrderStatus(int vOrderStatusID);
        bool UnDeleteOrderStatus(OrderStatus vOrderStatus);
        OrderStatus UpdateOrderStatus(OrderStatus vOrderStatus);
    }
}