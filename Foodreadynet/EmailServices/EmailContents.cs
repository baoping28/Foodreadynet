using System;
using FR.Infrastructure.Helpers;


namespace FoodReady.WebUI.EmailServices
{
    public class EmailContents
    {
        public string To { get; set; }
        public string FromName { get; set; }
        public string FromEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailContents()
        {
        }

        public EmailContents(string vFromName, string vTo, string vFromEmailAddress, string vSubject, string vBody)
        {
            FromName = vFromName;
            To = vTo;
            FromEmailAddress = vFromEmailAddress;
            Subject = vSubject;
            Body = vBody;
        }
        public EmailContents(EmailManager em)
        {
            FromEmailAddress = Globals.Settings.ContactForm.MailFrom;
            FromName = "foodready.net";
            Subject = "New online order";
            To = Globals.Settings.ContactForm.MailTo;
            Body = em.BuildEmailHtmlBodyForOrder();
        }

    }

}