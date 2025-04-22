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
        return FakeDatabase.ShoppingCart;
    }
   
    public IEnumerable<Item> Get(string? query)
    {
        return FakeDatabase.SearchCart(query).Take(100) ?? new List<Item>();
    }

    public Item? Delete(int id)
    {
        var itemToReturn = FakeDatabase.ShoppingCart.FirstOrDefault(i => i?.Id == id);
        if (itemToReturn == null)
        {
            return null;
        }
        if (itemToReturn != null)
        {
            var existingInvItem = FakeDatabase.Inventory.FirstOrDefault(i => i?.Product.Name == itemToReturn.Product.Name);
            FakeDatabase.ShoppingCart.Remove(itemToReturn);
           if(existingInvItem != null)
           {
                existingInvItem.Quantity++;
           }
           else
           {
                FakeDatabase.Inventory.Add(itemToReturn);
           }
           
        }

        return itemToReturn;
    }

    public Item? AddOrUpdate(Item item)
    {
        var existingInvItem = FakeDatabase.Inventory.FirstOrDefault(i => i?.Id == item.Id);
        if(existingInvItem == null || existingInvItem.Quantity == 0) {
                return null;
        }

       
        existingInvItem.Quantity --;
        var existingCartItem = FakeDatabase.ShoppingCart.FirstOrDefault(i => i?.Product.Name == item.Product.Name);
        if(existingCartItem == null)
        {
            item.Id = FakeDatabase.LastKey_Cart + 1;
            item.Product.Id = item.Id;
            item.Quantity = 1;
            FakeDatabase.ShoppingCart.Add(item);
        }
        else
        {
            existingCartItem.Quantity++;
        }
        return existingInvItem;
        
    }
}
