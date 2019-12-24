using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.Settings;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectHotels.Views.MenuItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = App.CurrentSettings;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as ViewModels.SettingsViewModel).ChangeStyle(e.Value);
        }
    }
}