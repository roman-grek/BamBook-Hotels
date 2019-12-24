using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectHotels.ViewModels;

namespace ProjectHotels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HotelPage : ContentPage
    {
        public HotelViewModel ViewModel { get; private set; }
        public HotelPage(HotelViewModel vm)
        {
            InitializeComponent();
            ViewModel = vm;
            BindingContext = ViewModel;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            if (App.CurrentSettings.SelectedLanguage == "RU")
                DisplayAlert("Заказ принят",
                    string.Format("Письмо с сообщением о бронировании отправлено на почтовый адрес {0}", App.Accounts.Current.Email), "OK");
            else
                DisplayAlert("Your booking is accepted",
                    string.Format("A letter with information has been sent on {0}", App.Accounts.Current.Email), "OK");
        }
    }
}