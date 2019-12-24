using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectHotels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : MasterDetailPage
    {
        public SearchPage()
        {
            InitializeComponent();
            MasterPage.MenuItemsListView.ItemSelected += ListView_ItemSelected;

        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as SearchPageMasterMenuItem;
            if (item == null)
                return;

            if (App.Accounts.Current == null && (item.Id == 1 || item.Id == 2 || item.Id == 3))
            {
                if (App.CurrentSettings.SelectedLanguage == "RU")
                    DisplayAlert("Не выполнен вход", "Войдите в аккаунт, чтобы совершить это действие", "OK");
                else
                    DisplayAlert("No current account", "Enter account to continue", "OK");
                return;
            }

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}