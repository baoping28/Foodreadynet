using System;
using System.Collections.Generic;
using FR.Domain.Model.Entities;

namespace FR.Repository.Interfaces
{
    public interface IToppingRepository
    {
        List<Topping> GetAllToppings();
        List<Topping> GetAllToppings(bool vActive);
        Topping GetToppingById(int vToppingID);
        int GetAllToppingCount(bool vActive);
        Topping AddTopping(int vToppingID, int vTitle,  DateTime vAddedDate, string vAddedBy,
                              DateTime vUpdatedDate, string vUpdatedBy, bool vActive);
        Topping AddTopping(Topping vTopping);
        bool DeleteTopping(Topping vTopping);
        bool DeleteTopping(int vToppingID);
        bool UnDeleteTopping(Topping vTopping);
        Topping UpdateTopping(Topping vTopping);
    }
}

