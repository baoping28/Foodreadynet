using System;
using FR.Domain.Model.Entities;
using FR.Repository.Abstract;
using System.Configuration;
namespace FR.Repository.ShoppingRepository
{
    public class BaseShoppingCartRepository : BaseRepository
    {
        private FRShoppingEntities _Shoppingctx;
        private string  _connString=ConfigurationManager.ConnectionStrings["FRShoppingEntities"].ConnectionString;
        private bool disposedValue;

        public string ConnString
        {
            get
            {
                return this._connString;
            }
            set
            {
                this._connString = value;
            }
        }
        public BaseShoppingCartRepository()
        {
            this.disposedValue = false;
            this.ConnectionString = GetActualConnectionString();
            this.CacheKey = "ShoppingCart";
        }

        public BaseShoppingCartRepository(string sConnectionString)
        {
            this.disposedValue = false;
            this.ConnectionString = sConnectionString;
            this.CacheKey = "ShoppingCart";
        }

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if ((!this.disposedValue && disposing) && null != this.Shoppingctx)
            {
                this.Shoppingctx.Dispose();
            }
            this.disposedValue = true;
        }

        public FRShoppingEntities Shoppingctx
        {
            get
            {
                if (null == this._Shoppingctx)
                {
                    this._Shoppingctx = new FRShoppingEntities();
                }
                return this._Shoppingctx;
            }
            set
            {
                this._Shoppingctx = value;
            }
        }
    }
}


