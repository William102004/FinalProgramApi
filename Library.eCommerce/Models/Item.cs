using Library.eCommerce.DTO;
using Library.eCommerce.Services;
using FinalProgramApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; }
        public int? Quantity { get; set; }



        public override string ToString()
        {
            return $"{Product} Quantity:{Quantity} Price: {Product?.Price}";
        }

        public string Display { 
            get
            {
                return $"{Product?.Display ?? string.Empty}, Price: ${Product?.Price} ,{Quantity} in stock";
            }
        }

        public Item()
        {
            Product = new ProductDTO();
            Quantity = 0;
            
        }
        public Item(Item i)
        {
            Product = new ProductDTO(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;
        }
    }
}