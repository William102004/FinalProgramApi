
using Library.eCommerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.eCommerce.Database
{
    public class Filebase
    {
        private string _root;
        private string _productRoot;

        private string _shoppingCartRoot;
        private static Filebase _instance;


        public static Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
             _root = @"/Users/williamalmaguer/Downloads";
            _productRoot = $"{_root}/Products";
            _shoppingCartRoot = $"{_root}/Cart"; 
        }

        public int LastKey
        {
            get
            {
                if (Inventory.Any())
                {
                    return Inventory.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public int LastKey_Cart
        {
            get
            {
                if (ShoppingCart.Any())
                {
                    return ShoppingCart.Select(x => x.Id).Max();
                }
                return 0;
            }
        }

        public Item AddOrUpdate(Item item)
        {
            //set up a new Id if one doesn't already exist
            if(item.Id <= 0)
            {
                item.Id = LastKey + 1;
            }
            //go to the right place
            string path = $"{_productRoot}/{item.Id}.json";
            

            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(item));

            //return the item, which now has an id
            return item;
        }
        
        public List<Item?> Inventory
        {
            get
            {
                var root = new DirectoryInfo(_productRoot);
                var _patients = new List<Item>();
                foreach(var patientFile in root.GetFiles())
                {
                    var patient = JsonConvert
                        .DeserializeObject<Item>
                        (File.ReadAllText(patientFile.FullName));
                    if(patient != null)
                    {
                        _patients.Add(patient);
                    }

                }
                return _patients;
            }
        }


        public bool Delete(Item item)
        {
           
           string path = $"{_productRoot}/{item.Id}.json";
           if(File.Exists(path))
            {
                File.Delete(path);
            }
            File.Delete($"{_productRoot}/{item.Id}.json");
            //TODO: refer to AddOrUpdate for an idea of how you can implement this.
            
            return true;
        }

        public IEnumerable<Item?> Search(string? query)
        {
            return Inventory.Where(p => p?.Product?.Name?.ToLower()
                      .Contains(query?.ToLower() ?? string.Empty) ?? false);
        }


        public List<Item?> ShoppingCart
        {
            get
            {
                var root = new DirectoryInfo(_shoppingCartRoot);
                var _patients = new List<Item>();
                foreach(var patientFile in root.GetFiles())
                {
                    var patient = JsonConvert
                        .DeserializeObject<Item>
                        (File.ReadAllText(patientFile.FullName));
                    if(patient != null)
                    {
                        _patients.Add(patient);
                    }

                }
                return _patients;
            }
        }

        public Item ShoppingCartAddOrUpdate(Item item,Item cartItem, Item inventoryItem)
        {
            //set up a new Id if one doesn't already exist
            inventoryItem.Quantity--;
            Filebase.Current.AddOrUpdate(inventoryItem);
            Item? resultItem;
            
            if(cartItem == null)
            {
                
                item.Id = LastKey_Cart + 1;
                item.Quantity = 1;
                resultItem = item;
            }
            else
            {
                cartItem.Quantity++;
                resultItem = cartItem;
            } 
            resultItem.Product.Id = item.Id;
            //resultItem.Product.Id = resultItem.Id;

            string path = $"{_shoppingCartRoot}/{resultItem.Id}.json";
            //go to the right place
            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(resultItem));

            //return the item, which now has an id
            return resultItem;
        }


        public bool ShoppingCartDelete(Item item)
        {
            var existingInvItem = Filebase.Current.Inventory.FirstOrDefault(i => i?.Product.Name == item.Product.Name);
            if(existingInvItem != null)
            {
                existingInvItem.Quantity++;
                existingInvItem.Id = Filebase.Current.Inventory.FirstOrDefault(i => i?.Product.Name == item.Product.Name).Id;
                Filebase.Current.AddOrUpdate(existingInvItem);
            }
            else
            {
                Item? Item1 = item;
                Item1.Quantity = 1;
                Filebase.Current.AddOrUpdate(Item1);
            }
            var existingCartItem = Filebase.Current.ShoppingCart.FirstOrDefault(i => i?.Product.Name == item.Product.Name);
            if(existingCartItem == null)
            {
                return false;
            }
            string path = $"{_shoppingCartRoot}/{existingCartItem.Id}.json";
            if(existingCartItem?.Quantity > 1)
            {
                existingCartItem.Quantity--;
                File.WriteAllText(path, JsonConvert.SerializeObject(existingCartItem));
            }
            else
            {
                if(File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            
            return true;
        }

            
        public void CheckOut()
        {
            var ItemstoCheckOut = ShoppingCart.ToList();
         
            foreach(var item in ItemstoCheckOut)
            {
                var path = $"{_shoppingCartRoot}/{ItemstoCheckOut.FirstOrDefault()?.Id}.json";
                if (File.Exists(path))
                {
                     File.Delete(path);
                } 
                File.Delete($"{_shoppingCartRoot}/{item?.Id}.json");
                   
            }
            
        }

        public IEnumerable<Item?> SearchCart(string? query)
        {
            return ShoppingCart.Where(p => p?.Product?.Name?.ToLower()
                      .Contains(query?.ToLower() ?? string.Empty) ?? false);
        }

    }
}