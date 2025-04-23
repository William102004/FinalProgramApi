using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private ShoppingCartService _cartSvc = ShoppingCartService.Current;
       public ItemViewModel? SelectedItem { get; set; }
       public ItemViewModel? SelectedCartItem { get; set; }

       public string? Query { get; set; }


        public ObservableCollection<ItemViewModel?> Inventory
        {
            get
            {
                return new ObservableCollection<ItemViewModel?>(_invSvc.Products
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m))
                    );
            }
        }

        public ObservableCollection<ItemViewModel?> ShoppingCart
        {
            get
            {
                return new ObservableCollection<ItemViewModel?>(_cartSvc.CartItems
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m))
                    );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshUX()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Model.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem.Model);

                if(updatedItem != null && shouldRefresh) {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }

            }
        }

        public void ReturnItem()
        {
            if (SelectedCartItem != null) {
                var shouldRefresh = SelectedCartItem.Model.Quantity >= 1;
                
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem.Model);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }

        public void Checkout()
        {
            _cartSvc.ClearCart();
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }
        public void refreshPrices()
        {
            NotifyPropertyChanged(nameof(tax));
            NotifyPropertyChanged(nameof(Subtotal));
            NotifyPropertyChanged(nameof(total));
        }


        public double tax
        {
            get
            {
                return ShoppingCartService.Current.tax;
            }
        }
        public double Subtotal
        {
            get
            {
                return ShoppingCartService.Current.Subtotal;
            }
        }
        public double total
        {
            get
            {
                return ShoppingCartService.Current.Totalprice;;
            }
        }

         public async Task<bool> Search()
        {
            await _cartSvc.Search(Query);
            NotifyPropertyChanged(nameof(ShoppingCart));
            return true;
        }

        public async Task<bool> ClearSearchQuery()
        {
                Query = string.Empty;
                NotifyPropertyChanged(nameof(Query));
                await _cartSvc.Search(Query);
                NotifyPropertyChanged(nameof(ShoppingCart));
                return true;
        }

        
    }
}