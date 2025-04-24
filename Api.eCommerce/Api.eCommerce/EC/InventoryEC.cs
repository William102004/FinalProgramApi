using Api.eCommerce.Database;
using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using FinalProgramAPI.Models;

namespace Api.eCommerce.EC
{

    public class InventoryEC
    {

        public List<Item?> Get()
        {
            return Filebase.Current.Inventory;
            //return FakeDatabase.Inventory;
        }

        public IEnumerable<Item> Get(string? query)
        {
            return Filebase.Current.Search(query).Take(100) ?? new List<Item>();
        }

        public Item? Delete(int id)
        {
            var itemToDelete = Filebase.Current.Inventory.FirstOrDefault(i => i?.Id == id);
            //var itemToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i?.Id == id);
            if (itemToDelete != null)
            {
                  Filebase.Current.Delete(itemToDelete);
               // FakeDatabase.Inventory.Remove(itemToDelete);
            }

            return itemToDelete;
        }

        public Item? AddOrUpdate(Item item)
        {
           // if (item.Id == 0)
           // {
           //     item.Id = FakeDatabase.LastKey_Item + 1;
           //     item.Product.Id = item.Id;
          //      FakeDatabase.Inventory.Add(item);
          //  } else {
          //      var existingItem = FakeDatabase.Inventory.FirstOrDefault(p => p.Id == item.Id);
          //      var index = FakeDatabase.Inventory.IndexOf(existingItem);
           //     FakeDatabase.Inventory.RemoveAt(index);
           //     FakeDatabase.Inventory.Insert(index, new Item(item));
          //  }

           // return item;
            return Filebase.Current.AddOrUpdate(item);
        }
    }
}