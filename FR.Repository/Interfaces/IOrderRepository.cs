using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        List<Order> GetAllOrders(bool vActive);
        Order GetOrderById(int vOrderID);
        List<Order> GetOrdersByEmail(string vEmail);
        List<Order> GetUserOrdersByEmail(string vLogonName, string vEmail);
        List<Order> GetOrdersByInvoiceNumber(string vInvoiceNumber);
        List<Order> GetUserOrdersByInvoiceNumber(string vLogonName, string vInvoiceNumber);
        List<Order> GetOrdersByTransactionId(string vTransactionId);
        List<Order> GetUserOrdersByTransactionId(string vLogonName, string vTransactionId);
        List<Order> GetOrdersByOrderStatusId(int vOrderStatusId);
        List<Order> GetOrdersByDateRange(DateTime vFromDate, DateTime vToDate);
        List<Order> GetOrdersByDateRange(DateTime vFromDate, DateTime vToDate, bool vActive);
        List<Order> GetOrdersByOrderStatusId(int vOrderStatusID, bool vActive);
        List<Order> GetMissedOrdersByBizId(int bizid);
        List<Order> GetMissedOrdersByBizId(int bizid, DateTime fromtime);
        List<Order> GetOrdersByGiftCardId(int vGiftCardId);
        int GetAllOrderCount();
        int GetAllOrderCount(bool vActive);
        List<Order> GetLastNdaysOrdersByLogonName(int ndays, string vLogonName);
        List<Order> GetLastNdaysOrdersByLogonName(int ndays, string vLogonName, bool vActive);
        List<Order> GetOrdersByDateRangeWithLogonName(DateTime vFromDate, DateTime vToDate, string vLogonName);
        List<Order> GetOrdersByDateRangeWithLogonName(DateTime vFromDate, DateTime vToDate, string vLogonName, bool vActive);
         List<Order> GetOrdersByLogonName(string vLogonName);
        List<Order> GetOrdersByLogonName(string vLogonName, bool vActive);
        List<Order> GetOrdersByBizId(int vBizInfoID);
        List<Order> GetOrdersByBizId(int vBizInfoID, bool vActive);
        List<Order> GetOrdersByDateRangeWithBizId(DateTime vFromDate, DateTime vToDate, int vBizInfoID);
        List<Order> GetOrdersByDateRangeWithBizId(DateTime vFromDate, DateTime vToDate, int vBizInfoID, bool vActive);
        Order UpdateOrderByEmail(int vOrderID, string vDriverName, DateTime vUpdatedDate, string vUpdatedBy);
        bool UpdateOrderStatus(int vOrderID);
        Order AddOrder(Order vOrder);
        Order InsertOrder(ShoppingCart vShoppingCart, string vLogonName, int vBizInfoID, int vOrderStatusID, int vCreditCardID,
              bool vIsLoggedUser, bool vIsdelivery, decimal vDeliveryCharge, decimal vSubTotal, decimal vOrderTax, decimal vOrderTotal,
              decimal vServiceCharge, decimal vDriverTip, string vFirstName, string vLastName,
              string vStreet, string vCity, string vState, string vZipCode, string vPhone, string vEmail,
              string vTransactionID, DateTime vAddedDate, string vAddedBy, DateTime vUpdatedDate, string vUpdatedBy, bool vActive,
               string vScheduleDate, string vScheduleTime, string vCouponChoice, bool vIsPayPalPayment, decimal vBizDiscountAmount, decimal vDiscountAmount,
              decimal vBizSubTotal, decimal vBizOrderTotal, string vCustomerMessage, string vDriverName, string vIpAddress,
              bool vIsGiftCardPayment, int vGiftCardId, decimal vGiftCardAmountPay);
        bool DeleteOrder(Order vOrder);
        bool DeleteOrder(int vOrderID);
        bool UnDeleteOrder(Order vOrder);
        Order UpdateOrder(Order vOrder);
    }
}