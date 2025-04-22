 using Library.eCommerce.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProgramAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        private double price;
        public double Price 
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Price must be greater than or equal to zero.");
                }
                price = Math.Round(value, 2);
            }
        }
        public string? Display
        {
            get
            {
                return $"{Id}. {Name}";
            }
        }

       public string LegacyProperty1 { get; set; }
       public string LegacyProperty2 { get; set; }
       public string LegacyProperty3 { get; set; }
       public string LegacyProperty4 { get; set; }
       public string LegacyProperty5 { get; set; }
       public string LegacyProperty6 { get; set; }

        public Product()
        {
            Name = string.Empty;
        }

        public Product(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            price = p.Price;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }

        public Product(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            price = p.Price;
            LegacyProperty1 = string.Empty;
        }
    }
}