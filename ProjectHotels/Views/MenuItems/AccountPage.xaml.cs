using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectHotels.Views.MenuItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : ContentPage
    {
        private ViewModels.AccountViewModel vm;

        private int tapCount = 0;
        public AccountPage()
        {
            InitializeComponent();
            vm = App.Accounts.Current;
            BindingContext = vm;
        }

        private async void OnImageButton_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(vm.Resources["AccountChoosePhotoTitle"],
                vm.Resources["Cancel"], null, vm.Resources["AccountChoosePhotoGallery"],
                vm.Resources["AccountChoosePhotoTake"]);
            if (action == vm.Resources["AccountChoosePhotoTake"])
                vm.TakePhotoImageCommand.Execute(null);
            else if (action == vm.Resources["Cancel"])
                return;
            else
                vm.ChoosePhotoImageCommand.Execute(null);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var image = sender as Image;
            tapCount++;
            image.RotateTo(360 * tapCount);
            if (tapCount % 2 == 1)
            {
                gridAccount.IsVisible = false;
                changeButton.IsVisible = false;
                leaveButton.IsVisible = false;
                image.ScaleTo(1.7);
                accountImage.HeightRequest = 300;
                accountImage.WidthRequest = 300;
                accountImage.VerticalOptions = LayoutOptions.FillAndExpand;
            }
            else
            {
                gridAccount.IsVisible = true;
                changeButton.IsVisible = true;
                leaveButton.IsVisible = true;
                image.ScaleTo(1);
                accountImage.HeightRequest = 150;
                accountImage.WidthRequest = 150;
                accountImage.VerticalOptions = LayoutOptions.Start;
            }
        }

        private void LeaveButton_Clicked(object sender, EventArgs e)
        {
            App.Accounts.LeaveAccount();
            var page = (Page)Activator.CreateInstance(typeof(SearchPageDetail));
            Navigation.PushAsync(page);
            Navigation.RemovePage(this);
            
        }
    }
}