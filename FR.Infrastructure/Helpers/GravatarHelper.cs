using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FR.Infrastructure.Helpers
{
    public class GravatarHelper
    {
        private string _localAvatarPath = string.Empty;

        public HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        public string LocalAvatarPath(string sAvatar)
        {
            if (string.IsNullOrEmpty(_localAvatarPath))
            {
                _localAvatarPath = Path.Combine(Path.Combine(Request.PhysicalApplicationPath, "Avatars"),
                                                sAvatar + ".jpg");
            }
            return _localAvatarPath;
        }

        public string AvatarPath(string sAvatar)
        {
            if (CheckForAvatar(sAvatar))
            {
            }
            //TODO: Add default avatar here.
            return string.Empty;
        }


        public string GetAvatar(string eMail)
        {
            return GetAvatar(eMail, 80);
        }


        public string GetAvatar(string eMail, int width)
        {
            string gravatarHash = GetGravatarHash(eMail);
            if (CheckForAvatar(gravatarHash) == false)
            {
                DownloadGravatar(gravatarHash, width);
            }


            return gravatarHash + ".jpg";
        }

        private void DownloadGravatar(string sGravatarHash)
        {
            DownloadGravatar(sGravatarHash, 80);
        }

        private void DownloadGravatar(string sGravatarHash, int width)
        {
            string sGravatar = string.Format("http://www.gravatar.com/avatar/{0}.jpg?d=wavatar&s={1}", sGravatarHash, width);

            writeImage(sGravatar, sGravatarHash);
        }

        private void writeImage(string imageFilePath, string sGravatarHash)
        {
            var request = (HttpWebRequest)WebRequest.Create(imageFilePath);
            request.Timeout = 5000;
            request.ReadWriteTimeout = 20000;
            var response = (HttpWebResponse)request.GetResponse();
            Image img = Image.FromStream(response.GetResponseStream());

            using (img)
            {
                img.Save(LocalAvatarPath(sGravatarHash + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }


        private bool CheckForAvatar(string sAvatar)
        {
            string lAvatarPath = LocalAvatarPath(sAvatar);
            if (lAvatarPath.Contains("default.jpg") == false)
            {
                return File.Exists(lAvatarPath);
            }
            return false;
        }

        public static string GetGravatarHash(string sEmail)
        {
            MD5 md5Hasher = MD5.Create();

            //Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(sEmail));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            int i = 0;
            for (i = 0; i <= data.Length - 1; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.


            return sBuilder.ToString();
        }
    }
}
