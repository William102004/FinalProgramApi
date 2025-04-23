using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShoppingManagementView : ContentPage
{
	public ShoppingManagementView()
	{
		InitializeComponent();
		BindingContext = new ShoppingManagementViewModel();
        
	}

    private void RemoveFromCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.ReturnItem();
    }
    private void AddToCartClicked(object sender, EventArgs e)
    {
		(BindingContext as ShoppingManagementViewModel)?.PurchaseItem();
    }

    private void InlineAddClicked(object sender, EventArgs e)
    {
       (BindingContext as ShoppingManagementViewModel)?.RefreshUX();
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Checkout");
    }
    private void BackToMainMenuClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ShoppingManagementViewModel)?.RefreshUX();
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel)?.Search();
    }

    private void ClearClicked(object sender, EventArgs e)
    {  
        (BindingContext as ShoppingManagementViewModel)?.ClearSearchQuery();
    }
}