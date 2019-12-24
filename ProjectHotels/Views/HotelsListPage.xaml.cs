using ProjectHotels.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectHotels.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HotelsListPage : ContentPage
    {
        public HotelsListPage(HotelsListViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void FiltersPicker_Clicked(object sender, System.EventArgs e)
        {
            var vm = BindingContext as HotelsListViewModel;
            var action = await DisplayActionSheet(vm.Resources["FiltersTitle"],
                vm.Resources["Cancel"], null, vm.Resources["FilterStars"],
                vm.Resources["FilterRating"], vm.Resources["FilterNone"]);
            if (action == vm.Resources["FilterRating"])
                vm.FilterRating();
            else if (action == vm.Resources["FilterStars"])
                vm.FilterStars();
            else if (action == vm.Resources["FilterNone"])
                vm.FilterNone();
            else
                return;
        }
    }
}