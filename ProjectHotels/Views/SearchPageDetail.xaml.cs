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
    public partial class SearchPageDetail : ContentPage
    {
        public SearchPageDetail()
        {          
            InitializeComponent();
            App.Accounts.Navigation = Navigation;
            BindingContext = App.Accounts;

            searchDateTo.MinimumDate = DateTime.Now;
            searchDateTo.MaximumDate = DateTime.Now.AddMonths(3);
            searchDateFrom.MinimumDate = DateTime.Now.AddDays(1);
            searchDateFrom.MaximumDate = DateTime.Now.AddMonths(3).AddDays(1);
        }

        private void searchBtnClicked(object sender, EventArgs e)
        {
            if (searchDateFrom.Date <= searchDateTo.Date)
            {
                if (App.CurrentSettings.SelectedLanguage == "RU")
                    DisplayAlert("Неверная дата", "Дата возвращения должна быть после даты отъезда", "OK");
                else
                    DisplayAlert("Wrong date", "Depart date must be after arrival date", "OK");
                return;
            }
            var hvm = new HotelsListViewModel(searchCity.Text, searchDateTo.Date, searchDateFrom.Date,
                (int)searchAdults.Value, (int)searchChildren.Value)
            { Navigation = this.Navigation};
            Navigation.PushAsync(new HotelsListPage(hvm));
        }
    }
}