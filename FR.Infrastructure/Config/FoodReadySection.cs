using System;
using System.Configuration;
using System.Web.Configuration;
using FR.Infrastructure.Helpers;
namespace FR.Infrastructure.Config
{
    public class FoodReadySection : ConfigurationSection
    {
        [ConfigurationProperty("defaultConnectionStringName", DefaultValue = "DefaultConnection")]
        public string DefaultConnectionStringName
        {
            get { return (string)this["defaultConnectionStringName"]; }
            set { this["DefaultConnectionStringName"] = value; }
        }

        [ConfigurationProperty("devSiteName", DefaultValue = "FoodReady")]
        public string devSiteName
        {
            get { return (string)this["devSiteName"]; }
            set { this["devSiteName"] = value; }
        }
        [ConfigurationProperty("appRoot", DefaultValue = "FoodReady")]
        public string AppRoot
        {
            get { return (string)this["appRoot"]; }
            set { this["appRoot"] = value; }
        }

        [ConfigurationProperty("siteDomainName", DefaultValue = "localhost")]
        public string SiteDomainName
        {
            get { return (string)this["siteDomainName"]; }
            set { this["siteDomainName"] = value; }
        }

        [ConfigurationProperty("siteName", DefaultValue = "FoodReady")]
        public string SiteName
        {
            get { return (string)this["siteName"]; }
            set { this["siteName"] = value; }
        }

        [ConfigurationProperty("pageSize", DefaultValue = "8")]
        public int PageSize
        {
            get { return (int)this["pageSize"]; }
            set { this["pageSize"] = value; }
        }

        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)this["enableCaching"]; }
            set { this["enableCaching"] = value; }
        }

        [ConfigurationProperty("defaultCacheDuration", DefaultValue = "600")]
        public int DefaultCacheDuration
        {
            get { return (int)this["defaultCacheDuration"]; }
            set { this["defaultCacheDuration"] = value; }
        }

        [ConfigurationProperty("contactForm", IsRequired = true)]
        public ContactFormElement ContactForm
        {
            get { return (ContactFormElement)this["contactForm"]; }
        }

        [ConfigurationProperty("articles", IsRequired = true)]
        public ArticlesElement Articles
        {
            get { return (ArticlesElement)this["articles"]; }
        }

        [ConfigurationProperty("polls", IsRequired = true)]
        public PollsElement Polls
        {
            get { return (PollsElement)this["polls"]; }
        }


        [ConfigurationProperty("payPalSettings", IsRequired = true)]
        public payPalSettingsElement PayPalSettings
        {
            get { return (payPalSettingsElement)this["payPalSettings"]; }
        }


    }

    public class ContactFormElement : ConfigurationElement
    {
        [ConfigurationProperty("smtpServer", DefaultValue = "localhost")]
        public string smtpServer
        {
            get { return (string)this["smtpServer"]; }
            set { this["smtpServer"] = value; }
        }
        [ConfigurationProperty("mailSubject", DefaultValue = "Mail from FoodReady: {0}")]
        public string MailSubject
        {
            get { return (string)this["mailSubject"]; }
            set { this["mailSubject"] = value; }
        }

        [ConfigurationProperty("mailTo", IsRequired = true)]
        public string MailTo
        {
            get { return (string)this["mailTo"]; }
            set { this["mailTo"] = value; }
        }

        [ConfigurationProperty("mailCC")]
        public string MailCC
        {
            get { return (string)this["mailCC"]; }
            set { this["mailCC"] = value; }
        }
        [ConfigurationProperty("mailFrom")]
        public string MailFrom
        {
            get { return (string)this["mailFrom"]; }
            set { this["mailFrom"] = value; }
        }


    }

    public class ArticlesElement : ConfigurationElement
    {

        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["ConnectionStringName"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                string connStringName = null;
                if (string.IsNullOrEmpty(this.ConnectionStringName))
                {
                    connStringName = Globals.Settings.DefaultConnectionStringName;
                }
                else
                {
                    connStringName = this.ConnectionStringName;
                }
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        [Obsolete("No longer a factor with the Entity Framework.")]
        public string ProviderType
        {
            get { return (string)this["providerType"]; }
            set { this["providerType"] = value; }
        }

        [ConfigurationProperty("ratingLockInterval", DefaultValue = "15")]
        public int RatingLockInterval
        {
            get { return (int)this["ratingLockInterval"]; }
            set { this["ratingLockInterval"] = value; }
        }

        [ConfigurationProperty("pageSize", DefaultValue = "10")]
        public int PageSize
        {
            get { return (int)this["pageSize"]; }
            set { this["pageSize"] = value; }
        }

        [ConfigurationProperty("rssItems", DefaultValue = "5")]
        public int RssItems
        {
            get { return (int)this["rssItems"]; }
            set { this["rssItems"] = value; }
        }

        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)this["enableCaching"]; }
            set { this["enableCaching"] = value; }
        }

        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)this["cacheDuration"];
                if (duration > 0)
                {
                    return duration;
                }
                else
                {
                    return Globals.Settings.DefaultCacheDuration;
                }
            }
            set { this["cacheDuration"] = value; }
        }

        [ConfigurationProperty("urlIndicator")]
        public string URLIndicator
        {
            get
            {
                string lurlIndicator = this["urlIndicator"].ToString();
                if (string.IsNullOrEmpty(lurlIndicator))
                {
                    lurlIndicator = "Article";
                }
                return lurlIndicator;
            }
            set { this["urlIndicator"] = value; }
        }

        [ConfigurationProperty("categoryUrlIndicator")]
        public string CategoryUrlIndicator
        {
            get
            {
                string lurlIndicator = this["categoryUrlIndicator"].ToString();
                if (string.IsNullOrEmpty(lurlIndicator))
                {
                    lurlIndicator = "Category";
                }
                return lurlIndicator;
            }
            set { this["categoryUrlIndicator"] = value; }
        }
        [ConfigurationProperty("facebookUrl")]
        public string FacebookUrl
        {
            get
            {
                string fbUrl = this["facebookUrl"].ToString();
                if (string.IsNullOrEmpty(fbUrl))
                {
                    fbUrl = "";
                }
                return fbUrl;
            }
            set { this["facebookUrl"] = value; }
        }
        [ConfigurationProperty("akismetKey")]
        public string AkismetKey
        {
            get
            {
                string lakismetKey = this["akismetKey"].ToString();
                if (string.IsNullOrEmpty(lakismetKey))
                {
                    lakismetKey = "";
                }
                return lakismetKey;
            }
            set { this["akismetKey"] = value; }
        }

        [ConfigurationProperty("enableAkismet", DefaultValue = "false")]
        public bool EnableAkismet
        {
            get { return (bool)this["enableAkismet"]; }
            set { this["enableAkismet"] = value; }
        }

        [ConfigurationProperty("reportAkismet", DefaultValue = "false")]
        public bool ReportAkismet
        {
            get { return (bool)this["reportAkismet"]; }
            set { this["reportAkismet"] = value; }
        }


    }

    public class PollsElement : ConfigurationElement
    {

        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                string connStringName = null;
                if (string.IsNullOrEmpty(this.ConnectionStringName))
                {
                    connStringName = Globals.Settings.DefaultConnectionStringName;
                }
                else
                {
                    connStringName = this.ConnectionStringName;
                }
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        [Obsolete("No longer a factor with the Entity Framework.")]
        public string ProviderType
        {
            get { return (string)this["providerType"]; }
            set { this["providerType"] = value; }
        }

        [ConfigurationProperty("votingLockInterval", DefaultValue = "15")]
        public int VotingLockInterval
        {
            get { return (int)this["votingLockInterval"]; }
            set { this["votingLockInterval"] = value; }
        }

        [ConfigurationProperty("votingLockByCookie", DefaultValue = "true")]
        public bool VotingLockByCookie
        {
            get { return (bool)this["votingLockByCookie"]; }
            set { this["votingLockByCookie"] = value; }
        }

        [ConfigurationProperty("votingLockByIP", DefaultValue = "true")]
        public bool VotingLockByIP
        {
            get { return (bool)this["votingLockByIP"]; }
            set { this["votingLockByIP"] = value; }
        }

        [ConfigurationProperty("archiveIsPublic", DefaultValue = "false")]
        public bool ArchiveIsPublic
        {
            get { return (bool)this["archiveIsPublic"]; }
            set { this["archiveIsPublic"] = value; }
        }

        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)this["enableCaching"]; }
            set { this["enableCaching"] = value; }
        }

        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)this["cacheDuration"];
                if (duration <= 0)
                {
                    duration = Globals.Settings.DefaultCacheDuration;
                }
                return duration;
            }
            set { this["cacheDuration"] = value; }
        }

        [ConfigurationProperty("urlIndicator")]
        public string URLIndicator
        {
            get
            {
                string lurlIndicator = this["urlIndicator"].ToString();
                if (string.IsNullOrEmpty(lurlIndicator))
                {
                    lurlIndicator = "Poll";
                }
                return lurlIndicator;
            }
            set { this["urlIndicator"] = value; }
        }

    }

    public class NewslettersElement : ConfigurationElement
    {

        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return (string)this["connectionStringName"]; }
            set { this["connectionStringName"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                string connStringName = null;
                if (string.IsNullOrEmpty(this.ConnectionStringName))
                {
                    connStringName = Globals.Settings.DefaultConnectionStringName;
                }
                else
                {
                    connStringName = this.ConnectionStringName;
                }
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        [Obsolete("No longer a factor with the Entity Framework.")]
        public string ProviderType
        {
            get { return (string)this["providerType"]; }
            set { this["providerType"] = value; }
        }

        [ConfigurationProperty("fromEmail", IsRequired = true)]
        public string FromEmail
        {
            get { return (string)this["fromEmail"]; }
            set { this["fromEmail"] = value; }
        }

        [ConfigurationProperty("fromDisplayName", IsRequired = true)]
        public string FromDisplayName
        {
            get { return (string)this["fromDisplayName"]; }
            set { this["fromDisplayName"] = value; }
        }

        [ConfigurationProperty("hideFromArchiveInterval", DefaultValue = "15")]
        public int HideFromArchiveInterval
        {
            get { return (int)this["hideFromArchiveInterval"]; }
            set { this["hideFromArchiveInterval"] = value; }
        }

        [ConfigurationProperty("archiveIsPublic", DefaultValue = "false")]
        public bool ArchiveIsPublic
        {
            get { return (bool)this["archiveIsPublic"]; }
            set { this["archiveIsPublic"] = value; }
        }

        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)this["enableCaching"]; }
            set { this["enableCaching"] = value; }
        }

        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)this["cacheDuration"];
                if (duration > 0)
                {
                    return duration;
                }
                else
                {
                    return Globals.Settings.DefaultCacheDuration;
                }
            }
            set { this["cacheDuration"] = value; }
        }

        [ConfigurationProperty("urlIndicator")]
        public string URLIndicator
        {
            get
            {
                string lurlIndicator = this["urlIndicator"].ToString();
                if (string.IsNullOrEmpty(lurlIndicator))
                {
                    lurlIndicator = "Newsletter";
                }
                return lurlIndicator;
            }
            set { this["urlIndicator"] = value; }
        }

    }


    public class payPalSettingsElement : ConfigurationElement
    {

        [ConfigurationProperty("connectionStringName")]
        public string ConnectionStringName
        {
            get { return this["connectionStringName"].ToString(); }
            set { this["connectionStringName"] = value; }
        }

        public string ConnectionString
        {
            get
            {
                string connStringName = this.ConnectionStringName;
                if (string.IsNullOrEmpty(this.ConnectionStringName))
                {
                    connStringName = Globals.Settings.DefaultConnectionStringName;
                }
                return WebConfigurationManager.ConnectionStrings[connStringName].ConnectionString;
            }
        }

        [Obsolete("No longer a factor with the Entity Framework.")]
        public string ProviderType
        {
            get { return (string)this["providerType"]; }
            set { this["providerType"] = value; }
        }

        [ConfigurationProperty("ratingLockInterval", DefaultValue = "15")]
        public int RatingLockInterval
        {
            get { return (int)this["ratingLockInterval"]; }
            set { this["ratingLockInterval"] = value; }
        }

        [ConfigurationProperty("pageSize", DefaultValue = "10")]
        public int PageSize
        {
            get { return (int)this["pageSize"]; }
            set { this["pageSize"] = value; }
        }

        [ConfigurationProperty("rssItems", DefaultValue = "5")]
        public int RssItems
        {
            get { return (int)this["rssItems"]; }
            set { this["rssItems"] = value; }
        }

        [ConfigurationProperty("defaultOrderListInterval", DefaultValue = "7")]
        public int DefaultOrderListInterval
        {
            get { return (int)this["defaultOrderListInterval"]; }
            set { this["defaultOrderListInterval"] = value; }
        }

        [ConfigurationProperty("sandboxMode", DefaultValue = "false")]
        public bool SandboxMode
        {
            get { return (bool)this["sandboxMode"]; }
            set { this["sandboxMode"] = value; }
        }

        [ConfigurationProperty("businessEmail", IsRequired = true)]
        public string BusinessEmail
        {
            get { return (string)this["businessEmail"]; }
            set { this["businessEmail"] = value; }
        }
        [ConfigurationProperty("payPalReceiverEmail", IsRequired = true)]
        public string PayPalReceiverEmail
        {
            get { return (string)this["payPalReceiverEmail"]; }
            set { this["payPalReceiverEmail"] = value; }
        }
        [ConfigurationProperty("paypalCertID", IsRequired = true)]
        public string PaypalCertID
        {
            get { return (string)this["paypalCertID"]; }
            set { this["paypalCertID"] = value; }
        }
        [ConfigurationProperty("p12Password", IsRequired = true)]
        public string P12Password
        {
            get { return (string)this["p12Password"]; }
            set { this["p12Password"] = value; }
        }
        [ConfigurationProperty("pdtToken", IsRequired = true)]
        public string PDTToken
        {
            get { return (string)this["pdtToken"]; }
            set { this["pdtToken"] = value; }
        }
        [ConfigurationProperty("payPalAPIVersion", IsRequired = true)]
        public string PayPalAPIVersion
        {
            get { return (string)this["payPalAPIVersion"]; }
            set { this["payPalAPIVersion"] = value; }
        }

        [ConfigurationProperty("payPalAPIUsername", IsRequired = true)]
        public string PayPalAPIUsername
        {
            get
            {
                string enppas = (string)this["payPalAPIUsername"];
                return CryptionClass.Decrypt(enppas);
            }
            set { this["payPalAPIUsername"] = value; }
        }
        [ConfigurationProperty("payPalAPIPassword", IsRequired = true)]
        public string PayPalAPIPassword
        {
            get
            {
                string enppas = (string)this["payPalAPIPassword"];
                return CryptionClass.Decrypt(enppas);
            }
            set { this["payPalAPIPassword"] = value; }
        }
        [ConfigurationProperty("payPalAPISignature", IsRequired = true)]
        public string PayPalAPISignature
        {
            get
            {
                string enppas = (string)this["payPalAPISignature"];
                return CryptionClass.Decrypt(enppas);
            }
            set { this["payPalAPISignature"] = value; }
        }
        [ConfigurationProperty("environment", IsRequired = true)]
        public string Environment
        {
            get { return (string)this["environment"]; }
            set { this["environment"] = value; }
        }
        [ConfigurationProperty("currencyCode", DefaultValue = "USD")]
        public string CurrencyCode
        {
            get { return (string)this["currencyCode"]; }
            set { this["currencyCode"] = value; }
        }

        [ConfigurationProperty("lowAvailability", DefaultValue = "10")]
        public int LowAvailability
        {
            get { return (int)this["lowAvailability"]; }
            set { this["lowAvailability"] = value; }
        }

        [ConfigurationProperty("enableCaching", DefaultValue = "true")]
        public bool EnableCaching
        {
            get { return (bool)this["enableCaching"]; }
            set { this["enableCaching"] = value; }
        }

        [ConfigurationProperty("cacheDuration")]
        public int CacheDuration
        {
            get
            {
                int duration = (int)this["cacheDuration"];
                if (duration > 0)
                {
                    return duration;
                }
                else
                {
                    return Globals.Settings.DefaultCacheDuration;
                }
            }
            set { this["cacheDuration"] = value; }
        }

        [ConfigurationProperty("productURLIndicator", DefaultValue = "Product")]
        public string ProductURLIndicator
        {
            get
            {
                string lurlIndicator = this["productURLIndicator"].ToString();
                if (string.IsNullOrEmpty(lurlIndicator))
                {
                    lurlIndicator = "Product";
                }
                return lurlIndicator;
            }
            set { this["ProductURLIndicator"] = value; }
        }

        [ConfigurationProperty("departmentURLIndicator", DefaultValue = "Department")]
        public string DepartmentURLIndicator
        {
            get
            {
                string lurlIndicator = this["departmentURLIndicator"].ToString();
                if (string.IsNullOrEmpty(lurlIndicator))
                {
                    lurlIndicator = "Department";
                }
                return lurlIndicator;
            }
            set { this["departmentURLIndicator"] = value; }
        }

    }
}
