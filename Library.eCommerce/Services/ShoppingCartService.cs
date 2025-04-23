using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;
using FinalProgramAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Util;
namespace Library.eCommerce.Services
{
    public class ShoppingCartService
    {
        private ProductServiceProxy _prodSvc = ProductServiceProxy.Current;
        private List<Item> items;

        public List<Item> CartItems
        {
            get
            {
                return items;
            }
        }

        public double tax = 0.07;

        public double Subtotal
        {
            get
            {
                double total = 0;
                foreach (var item in CartItems)
                {
                    total += (double)(item.Product.Price * item.Quantity);
                }
                total = Math.Round(total, 2);
                return total;
            }
        }
        public double Totalprice
        {
            get
            {
                double total = 0;
                total = Subtotal + (Subtotal * tax);
                total = Math.Round(total, 2);
                return total;
            }
        }
        public static ShoppingCartService Current {  
            get
            {
                if(instance == null)
                {
                    instance = new ShoppingCartService();
                }

                return instance;
            } 
        }
        private static ShoppingCartService? instance;
        private ShoppingCartService() { 
            var CartPayload = new WebRequestHandler().Get("/ShoppingCart").Result;
            items = JsonConvert.DeserializeObject<List<Item>>(CartPayload) ?? new List<Item>();

           // items = new List<Item>();
        }

        public async Task<IEnumerable<Item?>> Search(string? query)
        {
            if (query == null)
            {
                return new List<Item>();
            }
            var response = await new WebRequestHandler().Post("/ShoppingCart/Search", new QueryRequest { Query = query});
            items = JsonConvert.DeserializeObject<List<Item?>>(response) ?? new List<Item?>();
            return items;
        }

        public Item? AddOrUpdate(Item item)
        {
            var response = new WebRequestHandler().Post("/ShoppingCart", item).Result;
            var newItem = JsonConvert.DeserializeObject<Item>(response);
            var existingInvItem = _prodSvc.GetById(item.Id);
            if(existingInvItem == null || existingInvItem.Quantity == 0) {
                return null;
            }

           

            var existingItem = CartItems.FirstOrDefault(i => i.Product.Name == item.Product.Name);
            if(existingItem == null)
            {
                //add
                var newCartitem = new Item(newItem);
                newCartitem.Quantity = 1;
                existingInvItem.Quantity--;
                CartItems.Add(newCartitem);
            } else
            {
                existingInvItem.Quantity--;
                existingItem.Quantity++;
            }


            return existingInvItem;
        }

        public Item? ReturnItem(Item? item)
        {
            if (item?.Id <= 0 || item == null)
            {
                return null;
            }
            var result = new WebRequestHandler().Delete($"/ShoppingCart/{item.Id}").Result;
            var itemToReturn = CartItems.FirstOrDefault(c => c.Product.Name == item.Product.Name);
            if (itemToReturn != null)
            {
                itemToReturn.Quantity--;
                var inventoryItem = _prodSvc.Products.FirstOrDefault(p => p.Id == itemToReturn.Id); ;
                if(inventoryItem == null)
                {
                    _prodSvc.AddOrUpdate(new Item(itemToReturn));
                } else
                {
                    inventoryItem.Quantity++;
                }
            }


            return JsonConvert.DeserializeObject<Item>(result);
        }

        public void ClearCart()
        {
            items.Clear();
            new WebRequestHandler().Delete("/ShoppingCart/Checkout").Wait();
            var result = new WebRequestHandler().Get("/ShoppingCart").Result;
            var cart = JsonConvert.DeserializeObject<List<Item>>(result);
            new WebRequestHandler().Post("/ShoppingCart", cart);
            
        }   



        
    }
}