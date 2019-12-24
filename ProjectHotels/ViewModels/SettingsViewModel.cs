using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Globalization;
using System.ComponentModel;
using System.IO;

namespace ProjectHotels.ViewModels
{
  public class SettingsViewModel : INotifyPropertyChanged
    {
        public List<string> Languages { get; set; } = new List<string>()
        {
            "EN",
            "RU"
        };

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                SetLanguage();
                OnPropertyChanged("SelectedLanguage");
            }
        }

        public SettingsViewModel(string language, bool isDarkmode)
        {
            SelectedLanguage = language;
            Resources = new LocalizedResources(typeof(Resources.Resources), language);
            ChangeStyle(isDarkmode);
        }

        private LocalizedResources resources;

        public event PropertyChangedEventHandler PropertyChanged;

        public LocalizedResources Resources
        {
            get { return resources; }
            private set
            {
                if (resources != value)
                {
                    resources = value;
                    OnPropertyChanged("Resources");
                }
            }
        }

        private void SetLanguage()
        {
            MessagingCenter.Send<object, CultureChangedMessage>(this,
                    string.Empty, new CultureChangedMessage(SelectedLanguage));
            SaveChanges();
        }

        public static bool IsDarkMode { get; private set; }

        public void ChangeStyle(bool isDarkMode)
        {
            if (isDarkMode)
            {
                App.Current.Resources["buttonStyle"] = App.Current.Resources["buttonDark"];
                App.Current.Resources["labelStyle"] = App.Current.Resources["labelDark"];
                App.Current.Resources["pageStyle"] = App.Current.Resources["pageDark"];
                App.Current.Resources["entryStyle"] = App.Current.Resources["entryDark"];
                App.Current.Resources["imageButtonStyle"] = App.Current.Resources["imageButtonDark"];
                App.Current.Resources["searchBarStyle"] = App.Current.Resources["searchBarDark"];
            }
            else
            {
                App.Current.Resources["buttonStyle"] = App.Current.Resources["buttonLight"];
                App.Current.Resources["labelStyle"] = App.Current.Resources["labelLight"];
                App.Current.Resources["pageStyle"] = App.Current.Resources["pageLight"];
                App.Current.Resources["entryStyle"] = App.Current.Resources["entryLight"];
                App.Current.Resources["imageButtonStyle"] = App.Current.Resources["imageButtonLight"];
                App.Current.Resources["searchBarStyle"] = App.Current.Resources["searchBarLight"];
            }
            IsDarkMode = isDarkMode;
            SaveChanges();
        }

        private void SaveChanges()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "settings.json");
            var item = new SettingsItem() { Language = SelectedLanguage, IsDarkMode = SettingsViewModel.IsDarkMode };
            File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(item));
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
