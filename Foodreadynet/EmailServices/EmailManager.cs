using System;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Web;
using System.Web.Configuration;
using FR.Infrastructure.Helpers;
using FR.Domain.Model.Entities;
using System.Text.RegularExpressions;
using FoodReady.WebUI.Models;
using IdentitySample.Models;
using System.Collections.Generic;
namespace FoodReady.WebUI.EmailServices
{
    public class EmailManager
    {
        public BizInfo EMBizInfo { get; set; }
        public ShoppingCart EMShoppingCart { get; set; }
        public string OrderTime { get; set; }
        public string OrderType { get; set; }
        public string PaymentType { get; set; }
        public string ScheduleDateTime { get; set; }
        public string OrderNumber { get; set; }
        public string Name { get; set; }
        public string RoomNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string IpAddress { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
        public string Subtotal { get; set; }
        public string Tax { get; set; }
        public string ServiceCharge { get; set; }
        public string DeliveryCharge { get; set; }
        public string Tip { get; set; }
        public string DriveName { get; set; }
        public string CouponChoice { get; set; }
        public string Total { get; set; }
        public string Instruction { get; set; }
        public string FaxBody { get; set; }
        public bool IsSent { get; set; }
        public bool IsFaxSent { get; set; }
        public EmailManager()
        {

        }
        public EmailManager(BizInfo vEMBizInfo, ShoppingCart vEMShoppingCart, string vOrderTime, string vOrderType, string vPaymentType, string vScheduleDateTime, string vOrderNumber,
               string vName, string vRoomNumber, string vAddress, string vCity, string vState, string vZip, string vPhone, string vIpAddress, string vCreditCardType,
               string vCreditCardNumber, string vExpirationDate, string vSecurityCode, string vSubtotal, string vTax, string vServiceCharge, string vDeliveryCharge, string vTip, string vDriveName, string vCouponChoice,
               string vTotal, string vInstruction)
        {
            EMBizInfo = vEMBizInfo; EMShoppingCart = vEMShoppingCart; OrderTime = vOrderTime; OrderType = vOrderType;
            PaymentType = vPaymentType; ScheduleDateTime = vScheduleDateTime; OrderNumber = vOrderNumber; Name = vName; RoomNumber = vRoomNumber;
            Address = vAddress; City = vCity; State = vState; Zip = vZip; Phone = vPhone; IpAddress = vIpAddress;
            CreditCardType = vCreditCardType; CreditCardNumber = vCreditCardNumber; ExpirationDate = vExpirationDate;
            SecurityCode = vSecurityCode; Subtotal = vSubtotal; Tax = vTax; ServiceCharge = vServiceCharge; DeliveryCharge = vDeliveryCharge; Tip = vTip;
            DriveName = vDriveName; CouponChoice = vCouponChoice; Total = vTotal; Instruction = vInstruction;

        }
        private string OnlineFaxUserName
        {
            get { return WebConfigurationManager.AppSettings["OnlineFaxUserName"].ToString(); }
        }
        private string OnlineFaxPassword
        {
            get { return WebConfigurationManager.AppSettings["OnlineFaxPassword"].ToString(); }
        }
        private string OnlineFaxNumber
        {
            get { return WebConfigurationManager.AppSettings["OnlineFaxNumber"].ToString(); }
        }
        private string SMTPServerName
        {
            get { return Globals.Settings.ContactForm.smtpServer; }
        }
        public void SendFax(string vFaxNumber)
        {
            try
            {
                IsFaxSent = false;
                net.interfax.ws.InterFax x = new net.interfax.ws.InterFax();
                long st = x.SendCharFax(this.OnlineFaxUserName, this.OnlineFaxPassword, this.FormatFaxNumber(vFaxNumber), this.FaxBody, "html");
                if (st > 0)
                {
                    IsFaxSent = true;
                }
            }
            catch (Exception ex)
            {
                IsFaxSent = false;
                EmailContents ec = new EmailContents();
                ec.FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
                ec.FromName = "FoodReady.net Fax";
                ec.Subject = "New online order fax failed";
                ec.To = Globals.Settings.ContactForm.MailTo;
                ec.Body = "<b>Online Fax Failed</b><br />Name: " + Name + " Phone: " + Phone + "<br /> OrderNumber: " + OrderNumber + " Total: " + Total + "<br /> Order Type: " + OrderType + " Payment Type: " + PaymentType;
                SmtpClient client = new SmtpClient();//(will use the Web.config settings)
                // client.UseDefaultCredentials = true;
                MailAddress from = new MailAddress(ec.FromEmailAddress, ec.FromName);
                MailAddress to = new MailAddress(ec.To);

                MailMessage message = new MailMessage(from, to);

                message.Subject = ec.Subject;
                message.Body = FormatText(ec.Body, true);
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;

                try
                {
                    IsSent = false;
                    client.Send(message);
                    IsSent = true;
                }
                catch
                {

                    return;
                }
            }
        }
        public void SendOrderFax(string vFaxNumber)
        {
            if (string.IsNullOrEmpty(vFaxNumber))
            {
                IsFaxSent = false;
                return;
            }
            try
            {
                IsFaxSent = false;
                net.interfax.ws.InterFax x = new net.interfax.ws.InterFax();
                long st = x.SendCharFax(this.OnlineFaxUserName, this.OnlineFaxPassword, this.FormatFaxNumber(vFaxNumber), this.FaxBody, "html");
               if (st > 0)
               {
                IsFaxSent = true;
               }
            }
            catch
            {
                IsFaxSent = false;
                return;
            }
        }
         public string FormatFaxNumber(string vFaxNumber)

        {
            return "+1" +  Regex.Replace(vFaxNumber, @"[^0-9]", "");
         }
        public void Send(EmailContents emailcontents)
        {
            SmtpClient client = new SmtpClient();//(will use the Web.config settings)
            // client.UseDefaultCredentials = true;
            MailAddress from = new MailAddress(emailcontents.FromEmailAddress, emailcontents.FromName);
            MailAddress to = new MailAddress(emailcontents.To);

            MailMessage message = new MailMessage(from, to);

            message.Subject = emailcontents.Subject;
            message.Body = FormatText(emailcontents.Body, true);
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;

            try
            {
                IsSent = false;
                client.Send(message);
                IsSent = true;
            }
            catch
            {

                //throw ex;
            }
        }
        public string FormatText(string text, bool allow)
        {
            string formatted = "";

            StringBuilder sb = new StringBuilder(text);
            sb.Replace("  ", " &nbsp;");

            if (!allow)
            {
                sb.Replace("<br />", Environment.NewLine);
                sb.Replace("&nbsp;", " ");
                formatted = sb.ToString();
            }
            else
            {
                StringReader sr = new StringReader(sb.ToString());
                StringWriter sw = new StringWriter();

                while (sr.Peek() > -1)
                {
                    string temp = sr.ReadLine();
                    sw.Write(temp + "<br />");
                }

                formatted = sw.GetStringBuilder().ToString();
            }

            return formatted;
        }

        public string GetEmailForOrderItems()
        {
            StringBuilder sb = new StringBuilder();
            if (EMShoppingCart != null)
            {
                foreach (ShoppingCartItem sci in EMShoppingCart.Items)
                {
                    sb.Append("<tr>");
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                        sb.Append("<td style='border-bottom:solid 1px black;border-right:solid 1px black;padding:6px 2px 6px 2px;text-align:center;'>");
                        sb.Append("<b>" + sci.LabelName + "</b>");
                        sb.Append("</td>");
                    }
                    sb.Append("<td style='border-bottom:solid 1px black;border-right:solid 1px black;padding:6px 2px 6px 2px;text-align:center;'>");
                    sb.Append("<b>" + sci.Quantity.ToString() + "</b>");
                    sb.Append("</td><td style='border-bottom:solid 1px black;border-right:solid 1px black;padding:6px 10px 6px 10px;text-align:left;'>");
                    sb.Append("<b>" + sci.Title + "</b>");
                    if (string.IsNullOrEmpty(sci.ProductSizeTitle) == false)
                    {
                        sb.Append("(" + (sci.ProductSizeTitle) + ")");
                    }
                    if (string.IsNullOrEmpty(GetItemNote(sci)) == false)
                    {
                        sb.Append("<br/>" + GetItemNote(sci));
                    }
                    sb.Append("</td><td style='border-bottom:solid 1px black;padding:6px 10px 6px 2px;text-align:right;'>");
                    sb.Append(Helper.ToUSD(sci.ItemTotal.ToString("N2")));
                    sb.Append("</td></tr>");
                }
                if (string.IsNullOrEmpty(EMShoppingCart.FreeItem) == false)
                {
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                        sb.Append("<tr><td colspan='4' style='border-bottom:solid 1px black;padding:6px 10px 6px 10px;text-align:left;'>");
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='3' style='border-bottom:solid 1px black;padding:6px 10px 6px 10px;text-align:left;'>");
                    }
                    sb.Append(EMShoppingCart.FreeItem + "</td></tr>");
                }
                if (string.IsNullOrEmpty(this.Instruction) == false)
                {
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                        sb.Append("<tr><td colspan='4' style='padding:6px 10px 6px 10px;text-align:left;'>Instructions: ");
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='3' style='padding:6px 10px 6px 10px;text-align:left;'>Instructions: ");
                    }
                    sb.Append(this.Instruction + "</td></tr>");
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        public string GetHtmlForOrderItems()
        {
            StringBuilder sb = new StringBuilder();
            if (EMShoppingCart != null)
            {
                foreach (ShoppingCartItem sci in EMShoppingCart.Items)
                {
                    sb.Append("<tr>");
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                       sb.Append("<td style='width:130px;border-bottom:solid 1px black; border-right:solid 1px black; padding:10px; text-align:right;'>");
                    sb.Append("<b>" + sci.LabelName + "</b>");
                    sb.Append("</td>"); 
                    }
                    sb.Append("<td style='width:60px;border-bottom:solid 1px black; border-right:solid 1px black; padding:10px; text-align:right;'>");
                    sb.Append("<b>" + sci.Quantity.ToString() + "</b>");
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                        sb.Append("</td><td style='width:250px; border-bottom:solid 1px black; border-right:solid 1px black; padding:10px; text-align:left;'>");
                    }
                    else
                    {
                        sb.Append("</td><td style='width:400px; border-bottom:solid 1px black; border-right:solid 1px black; padding:10px; text-align:left;'>");
                    }
                    sb.Append("<b>" +sci.Title+"</b>");
                    if (string.IsNullOrEmpty(sci.ProductSizeTitle) == false)
                                                {
                                                    sb.Append("(" + (sci.ProductSizeTitle) + ")");
                                                }
                    if (string.IsNullOrEmpty(GetItemNote(sci))==false)
                    {
                        sb.Append("<br/>" + GetItemNote(sci));
                    }
                    sb.Append("</td><td style='width:100px; border-bottom:solid 1px black; padding:10px; text-align:right;'>");
                    sb.Append(Helper.ToUSD(sci.ItemTotal.ToString("N2")));
                    sb.Append("</td></tr>");
                }
                if (string.IsNullOrEmpty(EMShoppingCart.FreeItem)==false)
                {
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                        sb.Append("<tr><td colspan='4' style='border-bottom:solid 1px black;padding:10px; text-align:left;'>");
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='3' style='border-bottom:solid 1px black;padding:10px; text-align:left;'>");
                    }
                 sb.Append(EMShoppingCart.FreeItem + "</td></tr>");
                }
                if (string.IsNullOrEmpty(this.Instruction) == false)
                {
                    if (EMShoppingCart.IsFinalSharedCart)
                    {
                        sb.Append("<tr><td colspan='4' style='border-bottom:solid 1px black; padding:10px; text-align:left;'>Instructions: ");
                    }
                    else
                    {
                        sb.Append("<tr><td colspan='3' style='border-bottom:solid 1px black; padding:10px; text-align:left;'>Instructions: ");
                    }
                     sb.Append(this.Instruction + "</td></tr>");
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        public static string GetFaxForOrderItems(Order od)
        {
            StringBuilder sb = new StringBuilder();
            if (od != null)
            {
                foreach (OrderItem sci in od.OrderItems)
                {
                    sb.Append("<tr><td style='width:60px;border-bottom:solid 1px black; border-right:solid 1px black; padding:10px; text-align:right;'>");
                    sb.Append("<b>" + sci.Quantity.ToString() + "</b>");
                    sb.Append("</td><td style='width:400px; border-bottom:solid 1px black; border-right:solid 1px black; padding:10px; text-align:left;'>");
                    sb.Append("<b>" + sci.Title + "</b>");
                    if (string.IsNullOrEmpty(sci.ProductSizeTitle) == false)
                    {
                        sb.Append("(" + (sci.ProductSizeTitle) + ")");
                    }
                    if (string.IsNullOrEmpty(sci.GetItemNote) == false)
                    {
                        sb.Append("<br/>" + sci.GetItemNote);
                    }
                    sb.Append("</td><td style='width:100px; border-bottom:solid 1px black; padding:10px; text-align:right;'>");
                    sb.Append(Helper.ToUSD(sci.ItemTotal.ToString("N2")));
                    sb.Append("</td></tr>");
                }
                if (string.IsNullOrEmpty(od.CouponChoice) == false && od.DiscountAmount==0.00m)
                {
                    sb.Append("<tr><td colspan='3' style='border-bottom:solid 1px black;padding:10px; text-align:left;'>");
                    sb.Append(od.CouponChoice + "</td></tr>");
                }
                if (string.IsNullOrEmpty(od.CustomerMessage) == false)
                {
                    sb.Append("<tr><td colspan='3' style='border-bottom:solid 1px black; padding:10px; text-align:left;'>Instructions: ");
                    sb.Append(od.CustomerMessage + "</td></tr>");
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        public static string GetOrderItemsForDriver(Order od)
        {
            int num = 0;
            StringBuilder sb = new StringBuilder();
            if (od != null)
            {
                foreach (OrderItem sci in od.OrderItems)
                {
                    num++;
                    sb.Append("<b>" + num + ":</b> &nbsp; " + "[" + sci.Quantity.ToString() + "] " + sci.Title + "</b>");
                    if (string.IsNullOrEmpty(sci.ProductSizeTitle) == false)
                    {
                        sb.Append("(" + (sci.ProductSizeTitle) + ")");
                    }
                    if (string.IsNullOrEmpty(sci.GetItemNote) == false)
                    {
                        sb.Append("<br/>" + sci.GetItemNote);
                    }
                    sb.Append("<br/>");
                }
                sb.Append("<hr />");


                if (string.IsNullOrEmpty(od.CouponChoice) == false && od.DiscountAmount == 0.00m)
                {
                    sb.Append("Coupon: " + od.CouponChoice + "<hr />");
                }
                if (string.IsNullOrEmpty(od.CustomerMessage) == false)
                {
                    sb.Append("Message: " + od.CustomerMessage + "<hr />");
                }
                return sb.ToString();
            }
            return string.Empty;
        }
        public string GetItemNote(ShoppingCartItem line)
        {
            string str=string.Empty;
            if (string.IsNullOrEmpty(line.DressingChoice) == false)
            {

                str = "Dressing:" + line.DressingChoice;
            }
            if (string.IsNullOrEmpty(line.SelectedFreeToppings) == false)
            {
                str += "Free toppings:<br />" + line.SelectedFreeToppings + "<br/>";
            }
            if (line.ToppingList.Count > 0)
            {
                str += "Extra toppings for each:<br />" + line.ToppingListNote;
            }
            if (line.IsSpicy && string.IsNullOrEmpty(line.HowSpicy) == false)
            {
                str += "Spicy: " + line.HowSpicy + "<br />";
            }
            if (string.IsNullOrEmpty(line.SideChoice) == false)
            {
                str += "Side choice: " + line.SideChoice + "<br />";
            }
            if (string.IsNullOrEmpty(line.SauceChoice) == false)
            {
                str += "Sauce choice: " + line.SauceChoice + "<br />";
            }
            if (string.IsNullOrEmpty(line.CrustChoice) == false)
            {
                str += "Crust choice: " + line.CrustChoice + "<br />";
            }
            if (string.IsNullOrEmpty(line.CheeseChoice) == false)
            {
                str += "Cheese Amount: " + line.CheeseChoice + "<br />";
            }
            if (line.AddSideList.Count > 0)
            {
                str +=  line.AddSideListNote;
            }
            if (string.IsNullOrEmpty(line.Instruction) == false)
            {
                str += "Instructions: " + line.Instruction;
            }
            return str;                  

        }
        public string GetHtmlForCreditCardInfo()
        {
            if (PaymentType.ToLower() == "PayPal".ToLower()) //PayPal express payment
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>Payment Type: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(PaymentType);
            sb.Append("</td></tr><tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>Credit Card Type: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(CreditCardType);
            sb.Append("</td></tr><tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>Credit Card Number: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(CreditCardNumber);
            sb.Append("</td></tr><tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>Expiration Date: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(ExpirationDate);
            sb.Append("</td></tr><tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>Security Code: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(SecurityCode);
            sb.Append("</td></tr>");
            return sb.ToString();

        }
        public string GetHtmlForAddressInfo()
        {
            if (PaymentType.ToLower() == "PayPal".ToLower()) //PayPal express payment
            {
                return string.Empty;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>Address: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(Address);
            sb.Append("</td></tr><tr><td style='width:350px; border:solid 1px black; padding:10px; text-align:right;'>City, State: </td><td style='width:350px; border:solid 1px black; padding:10px; text-align:left;'>");
            sb.Append(City + ", " + State + " " + Zip);
            sb.Append("</td></tr>");
            return sb.ToString();
        }

        public string BuildFaxHtmlBodyForOrder()
        {
            string pagepath = "";
            if (EMShoppingCart.IsFinalSharedCart)
            {
                pagepath = EMShoppingCart.IsDelivery ? "~/Content/HTMLPages/SharedOrderSumary.htm" : "~/Content/HTMLPages/SharedOrderPickupSumary.htm";
            }
            else
            {
                pagepath = EMShoppingCart.IsDelivery ? "~/Content/HTMLPages/OrderSumary.htm" : "~/Content/HTMLPages/OrderPickupSumary.htm";
            }
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            msgbody = msgbody.Replace("+BizName+", EMBizInfo.BizTitle);
            msgbody = msgbody.Replace("+BizInfos+", EMBizInfo.BizAddressString + " | " + "Phone: " + EMBizInfo.ContactInfo.Phone + " | Fax: " + EMBizInfo.Fax) + "<br/>";
            msgbody = msgbody.Replace("+orderTime+", OrderTime);
            msgbody = msgbody.Replace("+orderType+", OrderType);
            msgbody = msgbody.Replace("+paymentType+", PaymentType);
            msgbody = msgbody.Replace("+orderNumber+", OrderNumber);
            msgbody = msgbody.Replace("+name+", Name);
            // msgbody = msgbody.Replace("+address+", GetHtmlForAddressInfo());
            if (string.IsNullOrEmpty(RoomNumber) == false)
            {
                msgbody = msgbody.Replace("+address+", "#" + RoomNumber + " " + Address);
            }
            else
            {
                msgbody = msgbody.Replace("+address+",  Address);
            }
            msgbody = msgbody.Replace("+CityStateZip+", City + ", " + State + " " + Zip);
            msgbody = msgbody.Replace("+CrossStreet+", "");
            msgbody = msgbody.Replace("+Phone+", Phone);
            msgbody = msgbody.Replace("+TimeDate+", EMShoppingCart.ScheduleTime + ", " + EMShoppingCart.ScheduleDate);
            // msgbody = msgbody.Replace("+creditCard+", GetHtmlForCreditCardInfo());
            msgbody = msgbody.Replace("+orderItems+", GetHtmlForOrderItems());
            msgbody = msgbody.Replace("+Subtotal+", Subtotal);
            msgbody = msgbody.Replace("+ServiceCharge+", "$" + EMShoppingCart.serviceCharge.ToString("N2"));
            msgbody = msgbody.Replace("+DeliveryFee+", "$" + EMShoppingCart.DeliveryFee.ToString("N2"));
            msgbody = msgbody.Replace("+Discount+", EMShoppingCart.DiscountPercentage == 0 ? "$0.00" : EMShoppingCart.DiscountPercentage + "%( -$" + EMShoppingCart.DiscountAmount.ToString("N2") + " )");
            msgbody = msgbody.Replace("+DriverTip+", "$" + EMShoppingCart.DriverTip.ToString("N2"));
            msgbody = msgbody.Replace("+Tax+", Tax);
            msgbody = msgbody.Replace("+Total+", Total);
            return msgbody;
        }
        public string BuildEmailHtmlBodyForOrder()
        {
            string pagepath = "";
            if (EMShoppingCart.IsFinalSharedCart)
            {
               pagepath = EMShoppingCart.IsDelivery ? "~/Content/HTMLPages/EmailSharedOrderSumary.htm" : "~/Content/HTMLPages/EmailSharedPickupSumary.htm";
            }
            else
            {
               pagepath = EMShoppingCart.IsDelivery ? "~/Content/HTMLPages/EmailOrderSumary.htm" : "~/Content/HTMLPages/EmailPickupSumary.htm";
            }
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            msgbody = msgbody.Replace("+BizName+", EMBizInfo.BizTitle);
            msgbody = msgbody.Replace("+BizInfos+", EMBizInfo.BizAddressString + " | " + "Phone: " + EMBizInfo.ContactInfo.Phone + " | Fax: " + EMBizInfo.Fax) + "<br/>";
            msgbody = msgbody.Replace("+orderTime+", OrderTime);
            msgbody = msgbody.Replace("+orderType+", OrderType);
            msgbody = msgbody.Replace("+paymentType+", PaymentType);
            msgbody = msgbody.Replace("+orderNumber+", OrderNumber);
            msgbody = msgbody.Replace("+name+", Name);
            // msgbody = msgbody.Replace("+address+", GetHtmlForAddressInfo());
            if (string.IsNullOrEmpty(RoomNumber) == false)
            {
                msgbody = msgbody.Replace("+address+", "#" + RoomNumber + " " + Address);
            }
            else
            {
                msgbody = msgbody.Replace("+address+", Address);
            }
            msgbody = msgbody.Replace("+CityStateZip+", City + ", " + State + " " + Zip);
            msgbody = msgbody.Replace("+CrossStreet+","");
            msgbody = msgbody.Replace("+Phone+", Phone);
            msgbody = msgbody.Replace("+TimeDate+", EMShoppingCart.ScheduleTime + ", " + EMShoppingCart.ScheduleDate);
           // msgbody = msgbody.Replace("+creditCard+", GetHtmlForCreditCardInfo());
            msgbody = msgbody.Replace("+orderItems+", GetEmailForOrderItems());
            msgbody = msgbody.Replace("+Subtotal+", Subtotal);
            msgbody = msgbody.Replace("+ServiceCharge+", "$" + EMShoppingCart.serviceCharge.ToString("N2"));
            msgbody = msgbody.Replace("+DeliveryFee+", "$" + EMShoppingCart.DeliveryFee.ToString("N2"));
            msgbody = msgbody.Replace("+Discount+", EMShoppingCart.DiscountPercentage == 0 ? "$0.00" : EMShoppingCart.DiscountPercentage + "%( -$" + EMShoppingCart.DiscountAmount.ToString("N2") + " )");
            msgbody = msgbody.Replace("+DriverTip+", "$" + EMShoppingCart.DriverTip.ToString("N2"));
            msgbody = msgbody.Replace("+Tax+", Tax);
            msgbody = msgbody.Replace("+Total+", Total);
            return msgbody;
        }
        public static string BuildOrderFaxBody(Order od)
        {
            string pagepath = od.IsDelivery ? "~/Content/HTMLPages/OrderSumary.htm" : "~/Content/HTMLPages/OrderPickupSumary.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            msgbody = msgbody.Replace("+BizName+", od.BizName);
            msgbody = msgbody.Replace("+BizInfos+", od.BizInfo.BizAddressString + " | " + "Phone: " + od.BizInfo.ContactInfo.Phone + " | Fax: " + od.BizInfo.Fax) + "<br/>";
            msgbody = msgbody.Replace("+orderTime+", od.AddedDate.ToString());
            msgbody = msgbody.Replace("+orderType+", od.OrderType);
            msgbody = msgbody.Replace("+paymentType+",  od.IsPayPalPayment ? "PayPal Payment" : od.IsGiftCardPayment == false ? "Credit Card Payment" : od.GiftCardAmountPay == od.OrderTotal ? "Gift Card Payment" : "Credit Card and Gift Card Payment");
            msgbody = msgbody.Replace("+orderNumber+", od.InvioceNumber);
            msgbody = msgbody.Replace("+name+", od.FirstName + " " + od.LastName);
            // msgbody = msgbody.Replace("+address+", GetHtmlForAddressInfo());
            if (string.IsNullOrEmpty(od.RoomNumber) == false)
            {
                msgbody = msgbody.Replace("+address+", "#" + od.RoomNumber + " " + od.Street);
            }
            else
            {
                msgbody = msgbody.Replace("+address+", od.Street);
            }
            msgbody = msgbody.Replace("+CityStateZip+", od.City + ", " + od.State + " " + od.ZipCode);
            msgbody = msgbody.Replace("+CrossStreet+", "");
            msgbody = msgbody.Replace("+Phone+", od.Phone);
            msgbody = msgbody.Replace("+TimeDate+", od.ScheduleTime + ", " + od.ScheduleDate);
            // msgbody = msgbody.Replace("+creditCard+", GetHtmlForCreditCardInfo());
            msgbody = msgbody.Replace("+orderItems+", GetFaxForOrderItems(od));
            msgbody = msgbody.Replace("+Subtotal+", "$" + od.SubTotal.ToString("N2"));
            msgbody = msgbody.Replace("+ServiceCharge+", "$" + od.ServiceCharge.ToString("N2"));
            msgbody = msgbody.Replace("+DeliveryFee+", "$" + od.DeliveryCharge.ToString("N2"));
            msgbody = msgbody.Replace("+Discount+", od.DiscountAmount == 0.00m ? "$0.00" : od.CouponChoice + "%( -$" + od.DiscountAmount.ToString("N2") + " )");
            msgbody = msgbody.Replace("+DriverTip+", "$" + od.DriverTip.ToString("N2"));
            msgbody = msgbody.Replace("+Tax+", "$" + od.OrderTax.ToString("N2"));
            msgbody = msgbody.Replace("+Total+", "$" + od.OrderTotal.ToString("N2"));
            return msgbody;
        }
        public static string BuildEmailToDriverHtmlBody(Order od,string vDriverName)
        {
            string pagepath = "~/Content/HTMLPages/EmailOrderToDriver.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+drivername+", vDriverName);
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+orderTime+", od.AddedDate.ToString());
            msgbody = msgbody.Replace("+orderType+", od.OrderType);
            msgbody = msgbody.Replace("+orderNumber+", od.InvioceNumber);
            msgbody = msgbody.Replace("+TimeDate+", od.ScheduleTime + ", " + od.ScheduleDate);
            msgbody = msgbody.Replace("+Total+", "$" + od.OrderTotal.ToString("N2"));
            msgbody = msgbody.Replace("+name+", od.FirstName + " " + od.LastName);
            if (string.IsNullOrEmpty(od.RoomNumber) == false)
            {
                msgbody = msgbody.Replace("+address+", "#" + od.RoomNumber + " " + od.Street);
            }
            else
            {
                msgbody = msgbody.Replace("+address+", od.Street);
            }
            msgbody = msgbody.Replace("+CityStateZip+", od.City + ", " + od.State + " " + od.ZipCode);
            msgbody = msgbody.Replace("+Phone+", od.Phone);
            msgbody = msgbody.Replace("+BizName+", od.BizName);
            msgbody = msgbody.Replace("+BizInfos+", od.BizInfo.BizAddressString + "<br />" + "Phone: " + od.BizInfo.ContactInfo.Phone) + "<br/>";
            msgbody = msgbody.Replace("+orderItems+", GetOrderItemsForDriver(od));
            return msgbody;
        }
        public static string BuildRVtoCustomerHtmlBody(RVConfirmationModel model)
        {
            string pagepath = "~/Content/HTMLPages/ReservationToCustomer.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+name+", model.FirstName + " " + model.LastName);
            msgbody = msgbody.Replace("+bizname+", model.Bizinfo.BizTitle);
            msgbody = msgbody.Replace("+datetime+", model.RVDate + " " + model.RVTime);
            msgbody = msgbody.Replace("+num+", model.RVNum.ToString());
            msgbody = msgbody.Replace("+message+", model.Message);
            msgbody = msgbody.Replace("+bizaddress+", model.Bizinfo.BizAddressString);
            msgbody = msgbody.Replace("+bizphone+", model.Bizinfo.ContactInfo.Phone);
            return msgbody;
        }
        public static string BuildRVCancelToCustomer(Reservation model)
        {
            string pagepath = "~/Content/HTMLPages/RVCancelToCustomer.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+name+", model.FirstName + " " + model.LastName);
            msgbody = msgbody.Replace("+bizname+", model.BizName);
            msgbody = msgbody.Replace("+datetime+", model.RVDate + " " + model.RVTime);
            msgbody = msgbody.Replace("+num+", model.PartySize.ToString());
            msgbody = msgbody.Replace("+message+", model.Message);
            msgbody = msgbody.Replace("+bizaddress+", model.BizRVInfo.BizInfo.BizAddressString);
            msgbody = msgbody.Replace("+bizphone+", model.BizRVInfo.BizInfo.ContactInfo.Phone);
            return msgbody;
        }
        public static string BuildRVtoRestaurantHtmlBody(RVConfirmationModel model)
        {
            string pagepath = "~/Content/HTMLPages/ReservationToRestaurant.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+bizname+", model.Bizinfo.BizTitle);
            msgbody = msgbody.Replace("+cname+", model.FirstName + " " + model.LastName);
            msgbody = msgbody.Replace("+datetime+", model.RVDate + " " + model.RVTime);
            msgbody = msgbody.Replace("+num+", model.RVNum.ToString());
            msgbody = msgbody.Replace("+cphone+", model.Phone);
            msgbody = msgbody.Replace("+cemail+", model.Email);
            msgbody = msgbody.Replace("+message+", model.Message);
            return msgbody;
        }
        public static string BuildRVCancelToRestaurant(Reservation model)
        {
            string pagepath = "~/Content/HTMLPages/RVCancelToRestaurant.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+bizname+", model.BizName);
            msgbody = msgbody.Replace("+cname+", model.FirstName + " " + model.LastName);
            msgbody = msgbody.Replace("+datetime+", model.RVDate + " " + model.RVTime);
            msgbody = msgbody.Replace("+num+", model.PartySize.ToString());
            msgbody = msgbody.Replace("+cphone+", model.Phone);
            msgbody = msgbody.Replace("+cemail+", model.Email);
            msgbody = msgbody.Replace("+message+", model.Message);
            return msgbody;
        }
        public static string BuildPartnerInfoHtmlBody(RestaurantJoinModel model)
        {
            string pagepath ="~/Content/HTMLPages/BackToIMEAL.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+name+", model.Name);
            msgbody = msgbody.Replace("+address+",model.Address + ", " + model.City + ", " + model.State + " " + model.ZipCode);
            msgbody = msgbody.Replace("+phone+", model.Phone);
            msgbody = msgbody.Replace("+mobile+", model.Mobile);
            msgbody = msgbody.Replace("+Fax+", model.Fax);
            msgbody = msgbody.Replace("+email+", model.Email);
            msgbody = msgbody.Replace("+delivery+", model.Delivery);
            msgbody = msgbody.Replace("+radius+", model.Radius.ToString("N2") + " miles");
            msgbody = msgbody.Replace("+deliveryMinimum+", "$" + model.OrderMinimum.ToString("N2"));
            msgbody = msgbody.Replace("+TaxRate+", model.TaxPercentage.ToString("N2") + "%");
            msgbody = msgbody.Replace("+Cuisine+", model.CuisineType);
            msgbody = msgbody.Replace("+message+", model.Message);
            msgbody = msgbody.Replace("+time+", DateTime.Now.ToString());
            return msgbody;
        }
        public static string BuildImealHtmlBody()
        {
            string pagepath = "~/Content/HTMLPages/BackToPartner.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            return msgbody;
        }
        public static string BuildRegisterEmailBody(RegisterViewModel model)
        {
            string pagepath = "~/Content/HTMLPages/RegisterBizEmail.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            msgbody = msgbody.Replace("+name+", model.FirstName + " " + model.LastName);
            msgbody = msgbody.Replace("+userid+", model.Email);
            msgbody = msgbody.Replace("+password+", model.Password);
            return msgbody;
        }
        public static string BuildCustomerEmailBody(ContactModel model)
        {
            string pagepath = "~/Content/HTMLPages/CustomerEmail.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+name+", model.ContactName);
            msgbody = msgbody.Replace("+email+", model.Email);
            msgbody = msgbody.Replace("+number+", model.OrderNumber.ToString());
            msgbody = msgbody.Replace("+date+", model.OrderDate);
            msgbody = msgbody.Replace("+restaurantname+", model.RestaurantName);
            msgbody = msgbody.Replace("+message+", model.Message);
            return msgbody;
        }
        public static string BuildRegisterDriverEmailBody(DriverModel model)
        {
            string pagepath = "~/Content/HTMLPages/DriverEmail.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+name+", model.Name);
            msgbody = msgbody.Replace("+password+", model.Password);
            return msgbody;
        }
        public string BuildGiftCardFaxHtmlBody(BuyGiftCardModel model)
        {
            return BuildGiftCardEmailHtmlBody(model);
        }
        public  string BuildGiftCardEmailHtmlBody(BuyGiftCardModel model)
        {
            string pagepath = "~/Content/HTMLPages/BuyGiftCardEmail.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            msgbody = msgbody.Replace("+buyeremail+", model.BuyerEmail);
            msgbody = msgbody.Replace("+phone+", model.Phone);
            msgbody = msgbody.Replace("+message+", model.Message);
            msgbody = msgbody.Replace("+code+", OrderNumber);
            return msgbody;
        }
        public static string BuildSendGiftBody(PointSumaryModel model,string gift)
        {
            string pagepath = "~/Content/HTMLPages/GiftToFoodready.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+message+", model.Message);
            msgbody = msgbody.Replace("+name+", model.Name);
            msgbody = msgbody.Replace("+gift+", gift);
            msgbody = msgbody.Replace("+addrss+", model.AddressLine);
            msgbody = msgbody.Replace("+city+", model.City);
            msgbody = msgbody.Replace("+state+", model.State);
            msgbody = msgbody.Replace("+zipcode+", model.ZipCode);
            return msgbody;
        }
        public static string BuildGiftToCustomerBody(PointSumaryModel model, string gift)
        {
            string pagepath = "~/Content/HTMLPages/GiftToCustomer.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+name+", model.Name);
            msgbody = msgbody.Replace("+gift+", gift);
            msgbody = msgbody.Replace("+addrss+", model.AddressLine);
            msgbody = msgbody.Replace("+city+", model.City);
            msgbody = msgbody.Replace("+state+", model.State);
            msgbody = msgbody.Replace("+zipcode+", model.ZipCode);
            return msgbody;
        }
        public static string BuildRVChangetoCustomer(Reservation newRV,Reservation oldRV)
        {
            return BuildResult(newRV, oldRV, "~/Content/HTMLPages/RVChangetoCustomer.htm");
        }
        public static string BuildRVChangetoRestaurant(Reservation newRV, Reservation oldRV)
        {
            return BuildResult(newRV, oldRV, "~/Content/HTMLPages/RVChangetoRestaurant.htm");
        }
        public static string BuildResult(Reservation newRV, Reservation oldRV, string path)
        {
            string pagepath = path;
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+name+", newRV.FirstName + " " + newRV.LastName);
            msgbody = msgbody.Replace("+bizname+", newRV.BizName);
            msgbody = msgbody.Replace("+datetime+", newRV.RVDate + " " + newRV.RVTime);
            msgbody = msgbody.Replace("+num+", newRV.PartySize.ToString());
            msgbody = msgbody.Replace("+message+", newRV.Message);
            msgbody = msgbody.Replace("+olddatetime+", oldRV.RVDate + " " + oldRV.RVTime);
            msgbody = msgbody.Replace("+oldnum+", oldRV.PartySize.ToString());
            msgbody = msgbody.Replace("+oldmessage+", oldRV.Message);
            msgbody = msgbody.Replace("+bizaddress+", newRV.BizRVInfo.BizInfo.BizAddressString);
            msgbody = msgbody.Replace("+bizphone+", newRV.BizRVInfo.BizInfo.ContactInfo.Phone);
            return msgbody;
        }
        public  string BuildEmailHtmlOrderForBoss(List<BizInfo> lcarts)
        {
            string pagepath = "~/Content/HTMLPages/EmailToBoss.htm";
            string msgbody = string.Empty;
            using (StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(pagepath)))
            {
                msgbody = sr.ReadToEnd();
            }
            msgbody = msgbody.Replace("+sitephone+", WebConfigurationManager.AppSettings["ServicePhone"].ToString());
            msgbody = msgbody.Replace("+sitefax+", WebConfigurationManager.AppSettings["ServiceFax"].ToString());
            msgbody = msgbody.Replace("+orderTime+", OrderTime);
            msgbody = msgbody.Replace("+orderType+", OrderType);
            msgbody = msgbody.Replace("+orderTotal+", Helper.FormatPriceWithDollar(Total));
            StringBuilder sb = new StringBuilder("<ul>");
            foreach (var b in lcarts)
            {
                sb.Append("<li>" + b.BizTitle + "<br />" + b.BizTwoLineAddressString + "</li>");
            }
            sb.Append("</ul>");
            msgbody = msgbody.Replace("+restaurants+", sb.ToString());
            return msgbody;
        }
    }

}