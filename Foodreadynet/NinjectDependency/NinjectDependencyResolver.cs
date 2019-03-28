using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Ninject.Syntax;
using FR.Repository.Interfaces;
using FR.Repository.ShoppingRepository;

namespace FoodReady.WebUI.NinjectDependency
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        public IBindingToSyntax<T> Bind<T>()
        {
            return kernel.Bind<T>();
        }
        public IKernel Kernel
        {
            get { return kernel; }
        }
        private void AddBindings()
        {
            // put additional bindings here
            Bind<IProductRepository>().To<ProductRepository>();
            Bind<IBizInfoRepository>().To<BizInfoRepository>();
            Bind<IBizCuisineRepository>().To<BizCuisineRepository>();
            Bind<ICuisineTypeRepository>().To<CuisineTypeRepository>();
            Bind<ICategoryRepository>().To<CategoryRepository>();
            Bind<IProductToppingRepository>().To<ProductToppingRepository>();
            Bind<IProductSizeRepository>().To<ProductSizeRepository>();
            Bind<ISideChoiceRepository>().To<SideChoiceRepository>();
            Bind<IProductDressingRepository>().To<ProductDressingRepository>();
            Bind<IDiscountCouponRepository>().To<DiscountCouponRepository>();
            Bind<IFreeItemCouponRepository>().To<FreeItemCouponRepository>();
            Bind<IUserDetailRepository>().To<UserDetailRepository>();
            Bind<ICreditCardTypeRepository>().To<CreditCardTypeRepository>();
            Bind<IVoteRepository>().To<VoteRepository>();
            Bind<IOrderRepository>().To<OrderRepository>();
            Bind<ICreditCardRepository>().To<CreditCardRepository>();
            Bind<IAddressRepository>().To<AddressRepository>();
            Bind<IContactInfoRepository>().To<ContactInfoRepository>();
            Bind<IZoneNameRepository>().To<ZoneNameRepository>();
            Bind<IBizHourRepository>().To<BizHourRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserLoginRepository>().To<UserLoginRepository>();
            Bind<IDayOfCloseRepository>().To<DayOfCloseRepository>();
            Bind<IMealSectionRepository>().To<MealSectionRepository>();
            Bind<IFoodTypeRepository>().To<FoodTypeRepository>();
            Bind<ICookMethodRepository>().To<CookMethodRepository>();
            Bind<IToppingRepository>().To<ToppingRepository>();
            Bind<IDressingRepository>().To<DressingRepository>();
            Bind<IOrderItemRepository>().To<OrderItemRepository>();
            Bind<IGiftCardRepository>().To<GiftCardRepository>();
            Bind<IFavorRestaurantRepository>().To<FavorRestaurantRepository>();
            Bind<IFamilyMealRepository>().To<FamilyMealRepository>();
            Bind<IConvertedPointRepository>().To<ConvertedPointRepository>();
            Bind<IRewardVoucherRepository>().To<RewardVoucherRepository>();
            Bind<IAddSideRepository>().To<AddSideRepository>();
            Bind<ICrustChoiceRepository>().To<CrustChoiceRepository>();
            Bind<ICheeseAmountRepository>().To<CheeseAmountRepository>();
            Bind<ISauceChoiceRepository>().To<SauceChoiceRepository>();
            Bind<IBizImageRepository>().To<BizImageRepository>();
            Bind<IBizRVInfoRepository>().To<BizRVInfoRepository>();
            Bind<IReservationRepository>().To<ReservationRepository>();
            Bind<IHotelRepository>().To<HotelRepository>();
            Bind<IHotelTypeRepository>().To<HotelTypeRepository>();
            Bind<IDriverRepository>().To<DriverRepository>();
            // create the email settings object
            /*
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(
                ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            Bind<IOrderProcessor>()
            .To<EmailOrderProcessor>()
            .WithConstructorArgument("settings", emailSettings);
             */
        }
    }
}