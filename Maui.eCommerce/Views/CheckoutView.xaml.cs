using Library.eCommerce.Services;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class CheckoutView : ContentPage
{
	public CheckoutView()
	{
		InitializeComponent();
		BindingContext = new ShoppingManagementViewModel();
	}

	private void BackToShopClicked(object sender, EventArgs e)
    {
		(BindingContext as ShoppingManagementViewModel)?.Checkout();
		(BindingContext as ShoppingManagementViewModel)?.RefreshUX();
		Shell.Current.GoToAsync("//ShoppingManagement");
    }
	
	 protected override void OnAppearing()
    {
            base.OnAppearing();
            (BindingContext as ShoppingManagementViewModel)?.RefreshUX();
			(BindingContext as ShoppingManagementViewModel)?.refreshPrices();
    }
}
