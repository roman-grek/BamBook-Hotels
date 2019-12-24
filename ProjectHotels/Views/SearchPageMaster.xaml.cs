using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System;

namespace ProjectHotels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPageMaster : ContentPage
    {
        public ListView ListView;

        public SearchPageMaster()
        {
            InitializeComponent();
            ListView = MenuItemsListView;
            Init(null, null);
            MessagingCenter.Subscribe<object, CultureChangedMessage>(this,
                string.Empty, Init);
        }

        void Init(object s, CultureChangedMessage ccm)
        {
            var res = App.Accounts.Resources;
            ListView.ItemsSource = new ArraySegment<SearchPageMasterMenuItem>(new SearchPageMasterMenuItem[]
            {
                new SearchPageMasterMenuItem(){ Title=res["SearchMenuMain"], IconSourcePath="https://image.flaticon.com/icons/png/512/38/38298.png", TargetType = typeof(SearchPageDetail), Id = 0},
                new SearchPageMasterMenuItem(){ Title=res["SearchMenuFavorites"], IconSourcePath="https://image.flaticon.com/icons/png/512/69/69904.png", TargetType = typeof(MenuItems.FavoritesPage), Id = 2},
                new SearchPageMasterMenuItem(){ Title=res["SearchMenuAccount"], IconSourcePath="https://image.flaticon.com/icons/png/512/64/64096.png", TargetType = typeof(MenuItems.AccountPage), Id = 3},
                new SearchPageMasterMenuItem(){ Title=res["SearchMenuSettings"], IconSourcePath="https://img.icons8.com/cotton/2x/settings--v1.png", TargetType = typeof(MenuItems.SettingsPage), Id = 4},
            });
        }
    }
}