using FinalProgramAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.DTO
{
    public class ProductDTO
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



        public ProductDTO()
        {
            Name = string.Empty;
        }

        public ProductDTO(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            price = p.Price;
        }

        public ProductDTO(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            price = p.Price;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}