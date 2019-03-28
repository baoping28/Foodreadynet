using System;
using FR.Domain.Model.Abstract;

namespace FR.Domain.Model.Entities
{
    public partial class BizHour : IBaseEntity
    {
        private string _setName = "BizHours";
        public string SetName
        {
            get { return _setName; }
            set { _setName = value; }
        }
        public string BizTimeZoneName
        {
            get
            {
                if ((this.ZoneName != null))
                {
                    return this.ZoneName.Title;
                }
                return string.Empty;
            }
        }

        private DateTime ConvertMyTimeToLocal(string time)
        {
            TimeZoneInfo myZone = TimeZoneInfo.FindSystemTimeZoneById(this.BizTimeZoneName);
            return TimeZoneInfo.ConvertTime(Convert.ToDateTime(time), myZone, TimeZoneInfo.Local);
        }

        public DateTime LocalMonStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.MonStart);
            }
        }
        public DateTime LocalMonClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.MonClose);
            }
        }
        public DateTime LocalTueStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.ThuStart);
            }
        }
        public DateTime LocalTueClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.ThuClose);
            }
        }
        public DateTime LocalWedStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.WedStart);
            }
        }
        public DateTime LocalWedClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.WedClose);
            }
        }

        public DateTime LocalThuStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.ThuStart);
            }
        }
        public DateTime LocalThuClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.ThuClose);
            }
        }
        public DateTime LocalFriStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.FriStart);
            }
        }
        public DateTime LocalFriClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.FriClose);
            }
        }
        public DateTime LocalSatStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.SatStart);
            }
        }
        public DateTime LocalSatClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.SatClose);
            }
        }
        public DateTime LocalSunStart
        {
            get
            {
                return ConvertMyTimeToLocal(this.SunStart);
            }
        }
        public DateTime LocalSunClose
        {
            get
            {
                return ConvertMyTimeToLocal(this.SunColse);
            }
        }
        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        bool bIsDirty = false;
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDirty
        {
            get { return bIsDirty; }
            set { bIsDirty = value; }
        }


        public bool CanAdd
        {
            get { return true; }
        }

        public bool CanDelete
        {
            get { return true; }
        }

        public bool CanEdit
        {
            get { return true; }
        }

        public bool CanRead
        {
            get { return true; }
        }

    }
}
