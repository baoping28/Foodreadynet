using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IOrderItemRepository
    {
        List<OrderItem> GetAllOrderItems();
        List<OrderItem> GetAllOrderItems(bool vActive);
        OrderItem GetOrderItemById(int vOrderItemID);
        int GetOrderItemsCount(bool vActive);
        bool DeleteOrderItem(OrderItem vOrderItem);
        bool DeleteOrderItem(int vOrderItemID);
        bool UnDeleteOrderItem(OrderItem vOrderItem);
    }
}
