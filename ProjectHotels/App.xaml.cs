using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectHotels.Views;
using ProjectHotels.ViewModels;
using ProjectHotels.Models;
using Newtonsoft.Json;

using System.Collections.Generic;
namespace ProjectHotels
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); 
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "settings.json");            
            try
            {
                var item = JsonConvert.DeserializeObject<SettingsItem>(File.ReadAllText(path));
                CurrentSettings = new SettingsViewModel(item.Language, item.IsDarkMode);
            }
            catch
            {
                CurrentSettings = new SettingsViewModel("RU", false);
            }
            Accounts = new AccountsListViewModel();
            MainPage = new SearchPage(); 
        }

        protected override void OnStart()
        {
            
        }
        protected override void OnSleep()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "accounts.json");
            var accountItem = JsonConvert.SerializeObject(Accounts.Current.Account);
            File.WriteAllText(path, accountItem);
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static SettingsViewModel CurrentSettings { get; set; }

        public static AccountsListViewModel Accounts
        {
            get; set;
        }

        static HotelItemDatabase database;

        public static HotelItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new HotelItemDatabase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                      "HotelSQLite.db3"));
                }
                return database;
            }
        }
    }
}
