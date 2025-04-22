using Library.eCommerce.Models;
using Library.eCommerce.Services;
using FinalProgramAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ProductViewModel
    {
        private Item? cachedModel { get; set; }
        public string? Name { 
            get
            {
                return Model?.Product?.Name ?? string.Empty;
            }

            set
            {
                if(Model != null && Model.Product?.Name != value && Model.Product != null)
                {
                    Model.Product.Name = value;
                }
            }
        }

        public double Price
        {
            get
            {
                return Model?.Product?.Price ?? 0;
            }

            set
            {
                if(Model != null &&  Model.Product.Price != value )
                {
                    value = Math.Round(value, 2);
                    Model.Product.Price = value;
                }
            }
        }

        public int? Quantity
        {
            get
            {
                return Model?.Quantity;
            }

            set
            {
                if( Model != null && Model.Quantity != value)
                {
                    Model.Quantity = value;
                }
            }
        }

        public Item? Model { get; set; }

        public void AddOrUpdate()
        {
            ProductServiceProxy.Current.AddOrUpdate(Model);
        }

        public void Undo()
        {
            ProductServiceProxy.Current.AddOrUpdate(cachedModel);
        }

        public ProductViewModel() {
            Model = new Item();
            cachedModel = null;
        }

        public ProductViewModel(Item? model)
        {
            Model = model;
            if (model != null)
            {
                cachedModel = new Item(model);
            }
        }

        
    }
}