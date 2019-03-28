using FR.Domain.Model.Entities;
using FR.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FR.Repository.ShoppingRepository
{
    public class OrderRepository : BaseShoppingCartRepository, IOrderRepository
    {
        #region IOrderRepository Members

        public List<Order> GetAllOrders()
        {
            string key = CacheKey + "_AllOrderList";

            if (EnableCaching && (Cache[key] != null))
            {
                return (List<Order>)Cache[key];
            }

            List<Order> lOrders = (from lOrder in Shoppingctx.Orders.Include("OrderItems")
                                   select lOrder).ToList();

            if (EnableCaching)
            {
                CacheData(key, lOrders, CacheDuration);
            }

            return lOrders;
        }

        public List<Order> GetAllOrders(bool vActive)
        {
            return GetAllOrders().Where(e => e.Active == vActive).ToList();
        }

        public Order GetOrderById(int vOrderID)
        {
            return GetAllOrders().Where(e => e.OrderId == vOrderID).FirstOrDefault();
        }
        public List<Order> GetOrdersByEmail(string vEmail)
        {
            return GetAllOrders().Where(e => e.Email == vEmail).ToList();
        }
        public List<Order> GetUserOrdersByEmail(string vLogonName,string vEmail)
        {
            return GetAllOrders().Where(e => e.Email == vEmail && e.LogonName==vLogonName).ToList();
        }
        public List<Order> GetOrdersByInvoiceNumber(string vInvoiceNumber)
        {
            int n = 0;
            if (int.TryParse(vInvoiceNumber,out n))
            {
                n = n - 800000000;
                return GetAllOrders().Where(e => e.OrderId == n).ToList();
            }
            return null;
        }
        public List<Order> GetUserOrdersByInvoiceNumber(string vLogonName, string vInvoiceNumber)
        {
            int n = 0;
            if (int.TryParse(vInvoiceNumber, out n))
            {
                n = n - 800000000;
                return GetAllOrders().Where(e => e.OrderId == n && e.LogonName==vLogonName).ToList();
            }
            return null;
        }
        public List<Order> GetOrdersByTransactionId(string vTransactionId)
        {
            return GetAllOrders().Where(e => e.TransactionId == vTransactionId).ToList();
        }
        public List<Order> GetUserOrdersByTransactionId(string vLogonName,string vTransactionId)
        {
            return GetAllOrders().Where(e => e.TransactionId == vTransactionId && e.LogonName==vLogonName).ToList();
        }
        public List<Order> GetOrdersByOrderStatusId(int vOrderStatusId)
        {
            return GetAllOrders().Where(e => e.OrderStatusId == vOrderStatusId).ToList();
        }

        public List<Order> GetOrdersByOrderStatusId(int vOrderStatusID, bool vActive)
        {
            return GetOrdersByOrderStatusId(vOrderStatusID).Where(e => e.Active == vActive).ToList();
        }

        public List<Order> GetOrdersByDateRange(DateTime vFromDate, DateTime vToDate)
        {
            List<Order> lOrders = default(List<Order>);

            if (vFromDate == null)
            {
                vFromDate = DateTime.MinValue;
            }
            if (vToDate == null)
            {
                vToDate = DateTime.MaxValue;
            }
            if (vFromDate > DateTime.MinValue && vToDate > DateTime.MinValue && vToDate >= vFromDate)
            {
                lOrders = (from lOrder in Shoppingctx.Orders.Include("OrderItems")
                           where lOrder.AddedDate >= vFromDate &&
                                 lOrder.AddedDate < vToDate 
                           orderby lOrder.AddedDate descending
                           select lOrder).ToList();
                return lOrders;
            }

            return GetAllOrders();
        }

        public List<Order> GetOrdersByDateRange(DateTime vFromDate, DateTime vToDate, bool vActive)
        {
            return GetOrdersByDateRange(vFromDate, vToDate).Where(e => e.Active == vActive).ToList();
        }

        public List<Order> GetMissedOrdersByBizId(int bizid)
        {
            List<Order> lOrders = default(List<Order>);

            DateTime dt = DateTime.Now.AddDays(-1);
            lOrders = (from lOrder in Shoppingctx.Orders.Include("OrderItems")
                       where lOrder.AddedDate > dt && lOrder.BizInfoId == bizid && lOrder.OrderStatusId != 9
                       orderby lOrder.AddedDate
                       select lOrder).ToList();
            return lOrders; 
        }

        public List<Order> GetMissedOrdersByBizId(int bizid,DateTime fromtime)
        {
            List<Order> lOrders = default(List<Order>);

            DateTime dt = fromtime.AddDays(-1);
            lOrders = (from lOrder in Shoppingctx.Orders.Include("OrderItems")
                       where lOrder.AddedDate >= dt && lOrder.AddedDate <= fromtime && lOrder.BizInfoId == bizid && lOrder.OrderStatusId != 9
                       orderby lOrder.AddedDate
                       select lOrder).ToList();
            return lOrders;
        }
        public List<Order> GetOrdersByGiftCardId(int vGiftCardId)
        {
            return vGiftCardId==0?null: GetAllOrders().Where(e => e.GiftCardId == vGiftCardId).ToList();
        }

        public int GetAllOrderCount()
        {
            return Shoppingctx.Orders.Count();
        }

        public int GetAllOrderCount(bool vActive)
        {
            return Shoppingctx.Orders.Where(e => e.Active == vActive).Count();
        }

        public List<Order> GetLastNdaysOrdersByLogonName(int ndays, string vLogonName)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetLastNdaysOrdersByLogonName(int ndays, string vLogonName, bool vActive)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrdersByDateRangeWithLogonName(DateTime vFromDate, DateTime vToDate, string vLogonName)
        {
            return GetOrdersByDateRange(vFromDate, vToDate).Where(e => e.LogonName == vLogonName).ToList();
        }

        public List<Order> GetOrdersByDateRangeWithLogonName(DateTime vFromDate, DateTime vToDate, string vLogonName, bool vActive)
        {
            return GetOrdersByDateRangeWithLogonName(vFromDate, vToDate, vLogonName).Where(e => e.Active == vActive).ToList();
        }

        public List<Order> GetOrdersByLogonName(string vLogonName)
        {
            return GetAllOrders().Where(e => e.LogonName == vLogonName).ToList();
        }

        public List<Order> GetOrdersByLogonName(string vLogonName, bool vActive)
        {
            return GetOrdersByLogonName(vLogonName).Where(e => e.Active == vActive).ToList();
        }
        public List<Order> GetOrdersByBizId(int vBizInfoID)
        {
            return GetAllOrders().Where(e => e.BizInfoId == vBizInfoID).ToList();
        }
        public List<Order> GetOrdersByBizId(int vBizInfoID,bool vActive)
        {
            return GetOrdersByBizId(vBizInfoID).Where(e => e.Active == vActive).ToList();
        }

        public List<Order> GetOrdersByDateRangeWithBizId(DateTime vFromDate, DateTime vToDate, int vBizInfoID)
        {

            if (vFromDate == null)
            {
                vFromDate = DateTime.MinValue;
            }
            if (vToDate == null)
            {
                vToDate = DateTime.MaxValue;
            }
            if (vFromDate > DateTime.MinValue && vToDate > DateTime.MinValue)
            {
                return GetAllOrders().Where(e => e.AddedDate >= vFromDate &&
                                 e.AddedDate < vToDate &&
                                 e.BizInfoId == vBizInfoID).OrderByDescending(e=>e.AddedDate).ToList();
            }

            return GetOrdersByBizId(vBizInfoID).OrderByDescending(e => e.AddedDate).ToList(); ;
        }

        public List<Order> GetOrdersByDateRangeWithBizId(DateTime vFromDate, DateTime vToDate, int vBizInfoID, bool vActive)
        {
            return GetOrdersByDateRangeWithBizId(vFromDate, vToDate, vBizInfoID).Where(e => e.Active == vActive).ToList();
        }

        public Order UpdateOrderByEmail(int vOrderID,string vDriverName,DateTime vUpdatedDate, string vUpdatedBy)
        {
            Order lOrder = new Order();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vOrderID > 0)
                {
                    lOrder = frctx.Orders.FirstOrDefault(u => u.OrderId == vOrderID);
                    lOrder.DriverName = vDriverName;
                    lOrder.UpdatedDate = vUpdatedDate;
                    lOrder.UpdatedBy = vUpdatedBy;
                    return frctx.SaveChanges() > 0 ? lOrder : null;
                }
                else
                {
                    return null;
                }
            }
        }
        public bool UpdateOrderStatus(int vOrderID)
        {
            Order lOrder = new Order();
            using (FRShoppingEntities frctx = new FRShoppingEntities())
            {
                if (vOrderID > 0)
                {
                    lOrder = frctx.Orders.FirstOrDefault(u => u.OrderId == vOrderID);
                    lOrder.OrderStatusId = 9; // Confirmed
                    lOrder.UpdatedDate = DateTime.Now;
                    return frctx.SaveChanges() > 0 ;
                }
                else
                {
                    return false;
                }
            }
        }
        public Order AddOrder(Order vOrder)
        {
            try
            {
                    Shoppingctx.Orders.Add(vOrder);
                base.PurgeCacheItems(CacheKey);

                return Shoppingctx.SaveChanges() > 0 ? vOrder : null;
            }
            catch
            {
                return null;
            }
        }

        public Order InsertOrder(ShoppingCart vShoppingCart, string vLogonName, int vBizInfoID, int vOrderStatusID, int vCreditCardID,
               bool vIsLoggedUser, bool vIsdelivery, decimal vDeliveryCharge, decimal vSubTotal, decimal vOrderTax, decimal vOrderTotal,
               decimal vServiceCharge, decimal vDriverTip, string vFirstName, string vLastName,
               string vStreet, string vCity, string vState, string vZipCode, string vPhone, string vEmail,
               string vTransactionID, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive,
                string vScheduleDate, string vScheduleTime, string vCouponChoice, bool vIsPayPalPayment, decimal vBizDiscountAmount, decimal vDiscountAmount,
               decimal vBizSubTotal, decimal vBizOrderTotal, string vCustomerMessage, string vDriverName, string vIpAddress,
               bool vIsGiftCardPayment, int vGiftCardId, decimal vGiftCardAmountPay)
        {
            Order order = new Order
            {
                LogonName = vLogonName,
                BizInfoId = vBizInfoID,
                OrderStatusId = vOrderStatusID,
                CreditCardId = vCreditCardID,
                IsLoggedUser = vIsLoggedUser,
                IsDelivery = vIsdelivery,
                DeliveryCharge = vDeliveryCharge,
                SubTotal = vSubTotal,
                OrderTax = vOrderTax,
                OrderTotal = vOrderTotal,
                ServiceCharge = vServiceCharge,
                DriverTip = vDriverTip,
                FirstName = vFirstName,
                LastName = vLastName,
                Street = vStreet,
                City = vCity,
                State = vState,
                ZipCode = vZipCode,
                Phone = vPhone,
                Email = vEmail,
                TransactionId = vTransactionID,
                AddedDate = vAddedDate,
                AddedBy = vAddedBy,
                UpdatedDate = vUpdatedDate,
                UpdatedBy = vUpdatedBy,
                Active = vActive,
                ScheduleDate = vScheduleDate,
                ScheduleTime = vScheduleTime,
                CouponChoice = vCouponChoice,
                IsPayPalPayment = vIsPayPalPayment,
                BizDiscountAmount = vBizDiscountAmount,
                DiscountAmount = vDiscountAmount,
                BizSubTotal = vBizSubTotal,
                BizOrderTotal = vBizOrderTotal,
                IsGiftCardPayment = vIsGiftCardPayment,
                GiftCardId = vGiftCardId,
                GiftCardAmountPay = vGiftCardAmountPay,

                CustomerMessage = vCustomerMessage,
                IpAddress = vIpAddress,
                DriverName = vDriverName
            }; 
            using (FRShoppingEntities ctx = new FRShoppingEntities())
            {
                // insert the master order

               order= ctx.Orders.Add(order);

                //insert the child order items

                foreach (ShoppingCartItem item in vShoppingCart.Items)
                {
                    OrderItem oi = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ID,
                        Title = item.Title,
                        Quantity = item.Quantity,
                        AddedDate = vAddedDate,
                        AddedBy = vAddedBy,
                        UpdatedDate = vUpdatedDate,
                        UpdatedBy = vUpdatedBy,
                        Active = vActive,
                        FinalSalePrice = item.FinalPrice,
                        BizUnitPrice = item.BizUnitPrice,
                        BizFinalPrice = item.BizFinalPrice,
                        DiscountPercentage = item.DiscountPercentage,
                        ExtraListTotal = item.ExtraPriceTotal,
                        ProductSizePrice = item.ProductSizePrice,
                        BizSizePrice = item.BizSizePrice,
                        BizItemTotal = item.BizItemTotal,
                        ItemTotal = item.ItemTotal,
                        IsFamilyMeal = item.IsFamilyMeal,
                        UnitPrice = item.UnitPrice,
                        AddSideListPrice = item.AddSidePriceTotal,
                        BizAddSideListPrice = item.BizAddSidePriceTotal,
                        CrustChoicePrice = item.CrustChoicePrice,
                        CrustChoiceBizPrice = item.CrustChoiceBizPrice,
                        cheeseAmountPrice = item.CheeseChoicePrice,
                        CheeseAmountBizPrice = item.CheeseChoiceBizPrice,
                        ProductSizeTitle = item.ProductSizeTitle,
                        ProductSize = item.ProductSize,
                        HowSpicy = item.HowSpicy,
                        SelectedFreeToppings = item.SelectedFreeToppings,
                        SideChoice = item.SideChoice,
                        SauceChoice = item.SauceChoice,
                        DressingChoice = item.DressingChoice,
                        SelectedToppings = item.SelectedToppings,
                        SelectedAddSides = item.SelectedAddSides,
                        CrustChoice = item.CrustChoice,
                        CheeseAmount = item.CheeseChoice,
                        Instruction = item.Instruction
                    };
                    ctx.OrderItems.Add(oi);
                    order.OrderItems.Add(oi);

                }

                order = AddOrder(order);
                return order;
            }
        }
       
        public bool DeleteOrder(Order vOrder)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(int vOrderID)
        {
            throw new NotImplementedException();
        }

        public bool UnDeleteOrder(Order vOrder)
        {
            throw new NotImplementedException();
        }

        public Order UpdateOrder(Order vOrder)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
