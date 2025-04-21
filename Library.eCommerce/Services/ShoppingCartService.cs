using System.Text.RegularExpressions;
using Library.eCommerce.Models;

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
            items = new List<Item>();
        }

        public Item? AddOrUpdate(Item item)
        {
            var existingInvItem = _prodSvc.GetById(item.Id);
            if(existingInvItem == null || existingInvItem.Quantity == 0) {
                return null;
            }

            if (existingInvItem != null)
            {
                existingInvItem.Quantity --;
            }

            var existingItem = CartItems.FirstOrDefault(i => i.Id == item.Id);
            if(existingItem == null)
            {
                //add
                var newItem = new Item(item);
                newItem.Quantity = 1;
                CartItems.Add(newItem);
            } else
            {
                //update
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

            var itemToReturn = CartItems.FirstOrDefault(c => c.Id == item.Id);
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


            return itemToReturn;
        }

        public void ClearCart()
        {
            items.Clear();
        }   



        
    }
}