using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectHotels.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectHotels.Views.MenuItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        public FavoritesPage()
        {
            InitializeComponent();
            BindingContext = new HotelsListViewModel(App.Accounts.Current.Favorites);
        }
    }
}