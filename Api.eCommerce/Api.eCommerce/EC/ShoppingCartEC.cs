using System;
using Api.eCommerce.Controllers;
using Api.eCommerce.Database;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc.Formatters;


namespace Api.eCommerce.EC;

public class ShoppingCartEC
{


    public List<Item?> Get()
    {
        return Filebase.Current.ShoppingCart;
    }
   
    public IEnumerable<Item> Get(string? query)
    {
        return Filebase.Current.SearchCart(query).Take(100) ?? new List<Item>();
    }

    public Item? Delete(int id)
    {
        var itemToReturn = Filebase.Current.ShoppingCart.FirstOrDefault(i => i?.Id == id);
        if (itemToReturn == null)
        {
            return null;
        }
        if (itemToReturn != null)
        {
            var existingInvItem = Filebase.Current.Inventory.FirstOrDefault(i => i?.Product.Name == itemToReturn.Product.Name);
            //FakeDatabase.ShoppingCart.Remove(itemToReturn);
           //if(existingInvItem != null)
           //{
           //    existingInvItem.Quantity++;
          //}
           //else
           //{
           //     FakeDatabase.Inventory.Add(itemToReturn);
          // }
         // Filebase.Current.ShoppingCartDelete(itemToReturn,existingInvItem);
            Filebase.Current.ShoppingCartDelete(itemToReturn);
        }

        return itemToReturn;
    }

    public Item? AddOrUpdate(Item item)
    {
        var existingInvItem = Filebase.Current.Inventory.FirstOrDefault(i => i?.Id == item.Id);
        if(existingInvItem == null || existingInvItem.Quantity == 0) {
                return null;
        }

       
        //existingInvItem.Quantity --;
        var existingCartItem = Filebase.Current.ShoppingCart.FirstOrDefault(i => i?.Product.Name == item.Product.Name);
        //if(existingCartItem == null)
        //{
          //  item.Id = FakeDatabase.LastKey_Cart + 1;
           // item.Product.Id = item.Id;
          //  item.Quantity = 1;
           // FakeDatabase.ShoppingCart.Add(item);
       // }
       // else
        //{
          //  existingCartItem.Quantity++;
       // }
        //return existingInvItem;

        return Filebase.Current.ShoppingCartAddOrUpdate(item,existingCartItem,existingInvItem);
    }
    public void CheckOut()
    {
        //var ItemstoCheckOut = FakeDatabase.ShoppingCart.ToList();
        //foreach (var item in ItemstoCheckOut)
        //{
          //  FakeDatabase.ShoppingCart.Remove(item);
        //}
       Filebase.Current.CheckOut();
    }

}
