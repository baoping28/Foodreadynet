using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Web.Configuration;



namespace FR.Domain.Model.Entities
{
    public class BrowseHistory
    {
        private List<BizInfo> _groupBizOption = new List<BizInfo>();

        public List<BizInfo> GroupBizOption
        {
            get { return this._groupBizOption; }
            set { this._groupBizOption = value; }
        }
        private List<BizInfo> _filterSet = new List<BizInfo>();

        public List<BizInfo> FilterSet
        {
            get { return this._filterSet; }
            set { this._filterSet = value; }
        }
        private List<BizInfo> _filterOpenSet = new List<BizInfo>();

        public List<BizInfo> FilterOpenSet
        {
            get { return this._filterOpenSet; }
            set { this._filterOpenSet = value; }
        }
        private List<BizInfo> _filterCloseSet = new List<BizInfo>();

        public List<BizInfo> FilterCloseSet
        {
            get { return this._filterCloseSet; }
            set { this._filterCloseSet = value; }
        }
        private List<BizInfo> _filterHiddenSet = new List<BizInfo>();

        public List<BizInfo> FilterHiddenSet
        {
            get { return this._filterHiddenSet; }
            set { this._filterHiddenSet = value; }
        }

        private bool _isDelivery = true;

        public bool IsDelivery
        {
            get { return this._isDelivery; }
            set { this._isDelivery = value; }
        }

        private string _addressCityState = string.Empty;

        public string AddressCityState
        {
            get { return this._addressCityState; }
            set { this._addressCityState = value; }
        }

        private string _address = string.Empty;

        public string Address
        {
            get { return this._address; }
            set { this._address = value; }
        }

        private string _city = string.Empty;

        public string City
        {
            get { return this._city; }
            set { this._city = value; }
        }

        private string _state = string.Empty;

        public string State
        {
            get { return this._state; }
            set { this._state = value; }
        }

        private string _zip = string.Empty;

        public string Zip
        {
            get { return this._zip; }
            set { this._zip = value; }
        }

        private string _cuisine = string.Empty;

        public string Cuisine
        {
            get { return this._cuisine; }
            set { this._cuisine = value; }
        }

        public string FullAddress
        {
            get
            {
                return this.AddressCityState + " " + this.Zip;
            }
        }
        private int _tryGiftCodeTimes = 0;

        public int TryGiftCodeTimes
        {
            get { return this._tryGiftCodeTimes; }
            set { this._tryGiftCodeTimes = value; }
        }
        private DateTime _lastTryGiftCodeTime = DateTime.Now;

        public DateTime LastTryGiftCodeTime
        {
            get { return this._lastTryGiftCodeTime; }
            set { this._lastTryGiftCodeTime = value; }
        }
    }
}
