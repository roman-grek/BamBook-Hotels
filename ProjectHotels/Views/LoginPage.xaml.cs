using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectHotels.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectHotels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public AccountViewModel ViewModel { get; private set; }
        public LoginPage(AccountViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            BindingContext = ViewModel;
            MessagingCenter.Subscribe<AccountsListViewModel>(this, "Account not found", async (arg) =>
            {
                await DisplayAlert(ViewModel.Resources["LoginErrorTitle"], ViewModel.Resources["LoginErrorText"], "OK");
            });
        }
    }
}