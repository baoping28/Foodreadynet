using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class OrderItemRepository : BaseShoppingCartRepository, IOrderItemRepository
    {
        #region IOrderItemRepository Members

        public List<OrderItem> GetAllOrderItems()
        {
            List<OrderItem> lOrderItem = default(List<OrderItem>);
            string lOrderItemKey = CacheKey + "_AllOrderItems";

            if (base.EnableCaching && (Cache[lOrderItemKey] != null))
            {
                lOrderItem = (List<OrderItem>)Cache[lOrderItemKey];
            }
            else
            {
                lOrderItem = Shoppingctx.OrderItems.ToList();
                if (base.EnableCaching)
                {
                    CacheData(lOrderItemKey, lOrderItem, CacheDuration);
                }
            }
            return lOrderItem;
        }
        public List<OrderItem> GetAllOrderItems(bool vActive)
        {
            return GetAllOrderItems().Where(e => e.Active == vActive).ToList();
        }

        public OrderItem GetOrderItemById(int vOrderItemID)
        {
            return GetAllOrderItems().FirstOrDefault(e => e.OrderItemId == vOrderItemID);
        }

        public int GetOrderItemsCount(bool vActive)
        {
            throw new NotImplementedException();
        }
        public bool DeleteOrderItem(OrderItem vOrderItem)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrderItem(int vOrderItemID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteOrderItem(OrderItem vOrderItem)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
