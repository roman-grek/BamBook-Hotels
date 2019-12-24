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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage(AccountViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            registerButton.IsEnabled = false;
            MessagingCenter.Subscribe<AccountsListViewModel>(this, "Invalid register", async (arg) =>
            {
                await DisplayAlert(vm.Resources["RegisterCreateErrorTitle"], vm.Resources["RegisterCreateErrorText"], "OK");
            });
            MessagingCenter.Subscribe<AccountsListViewModel>(this, "Email check failed", async (arg) =>
            {
                await DisplayAlert(vm.Resources["RegisterEmailErrorTitle"], vm.Resources["RegisterEmailErrorText"], "OK");
            });
        }

        private void PasswordRepeatEntry_Completed(object sender, EventArgs e)
        {
            if (passwordEntry.Text == passwordRepeatEntry.Text)
                registerButton.IsEnabled = true;
            else
            {
                var resources = (BindingContext as AccountViewModel).Resources;
                DisplayAlert(resources["RegisterPasswordErrorTitle"], resources["RegisterPasswordErrorText"], "OK");
                passwordRepeatEntry.Text = "";
            }
        }
    }
}