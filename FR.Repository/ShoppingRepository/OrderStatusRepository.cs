using System.Linq;
using System;
using System.Data;
using System.Collections.Generic;
using FR.Repository.Interfaces;
using FR.Domain.Model.Entities;
namespace FR.Repository.ShoppingRepository
{
    public class OrderStatusRepository : BaseShoppingCartRepository, IOrderStatusRepository
    {
        #region IOrderStatusRepository Members

        public List<OrderStatus> GetAllOrderStatuses(bool vActive)
        {
            throw new NotImplementedException();
        }

        public OrderStatus GetOrderStatusById(int vOrderStatusID)
        {
            throw new NotImplementedException();
        }

        public int GetAllOrderStatusCount(bool vActive)
        {
            throw new NotImplementedException();
        }

        public OrderStatus AddOrderStatus(int vOrderStatusID, string vTitle, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive)
        {
            throw new NotImplementedException();
        }

        public OrderStatus AddOrderStatus(OrderStatus vOrderStatus)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrderStatus(OrderStatus vOrderStatus)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrderStatus(int vOrderStatusID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteOrderStatus(OrderStatus vOrderStatus)
        {
            throw new NotImplementedException();
        }

        public OrderStatus UpdateOrderStatus(OrderStatus vOrderStatus)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
