using System;
using System.Collections.Generic;
using System.Text;
using ProjectHotels.Models;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace ProjectHotels.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand TakePhotoImageCommand { get; protected set; }
        public ICommand ChoosePhotoImageCommand { get; protected set; }
        public Account Account { get; private set; }

        AccountsListViewModel lvm;
        public AccountViewModel()
        {
            Account = new Account();
            Name = "";
            Email = "";
            Password = "";
            Favorites = new List<int>();
            TakePhotoImageCommand = new Command(TakePhotoImage);
            ChoosePhotoImageCommand = new Command(ChoosePhotoImage);
            Resources = new LocalizedResources(typeof(Resources.Resources), App.CurrentSettings.SelectedLanguage);
        }

        public AccountViewModel(Account account)
        {
            Account = account;
            TakePhotoImageCommand = new Command(TakePhotoImage);
            ChoosePhotoImageCommand = new Command(ChoosePhotoImage);
            Resources = new LocalizedResources(typeof(Resources.Resources), App.CurrentSettings.SelectedLanguage);
        }

        public AccountsListViewModel ListViewModel
        {
            get { return lvm; }
            set
            {
                if (lvm != value)
                {
                    lvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public string Name
        {
            get { return Account.Name; }
            set
            {
                if (Account.Name != value)
                {
                    Account.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string Email
        {
            get { return Account.Email; }
            set
            {
                if (Account.Email != value)
                {
                    Account.Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }
        public string Password
        {
            get { return Account.Password; }
            set
            {
                if (Account.Password != value)
                {
                    Account.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public ImageSource Image
        {
            get
            {
                return Account.Image != null ? Account.Image : ImageSource.FromUri(new Uri("https://image.flaticon.com/icons/png/512/64/64096.png"));
            }
            set
            {
                if (Account.Image != value)
                {
                    Account.Image = value;
                    OnPropertyChanged("Image");
                }
            }
        }

        public List<int> Favorites
        {
            get
            {
                return Account.Favorites;
            }
            set
            {
                if (Account.Favorites != value)
                {
                    Account.Favorites = value;
                    OnPropertyChanged("Favorites");
                }
            }
        }

        public void AddToFavorites(int id)
        {
            if (!Favorites.Contains(id))
                Account.Favorites.Add(id);
        }

        public void RemoveFromFavorites(int id)
        {
            if (Favorites.Contains(id))
                Account.Favorites.Remove(id);
        }

        private LocalizedResources resources;
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

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name.Trim())) &&
                    (!string.IsNullOrEmpty(Email.Trim())) &&
                    (!string.IsNullOrEmpty(Password.Trim()));
            }
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void TakePhotoImage()
        {
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Directory = "BamBookHotels",
                    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                });

                if (file == null)
                    return;
                Image = ImageSource.FromFile(file.Path);
            }
        }

        private async void ChoosePhotoImage()
        {
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                Image = ImageSource.FromFile(photo.Path);
            }
        }
        public override bool Equals(object obj)
        {
            return Account.Email == (obj as AccountViewModel).Email;
        }

        public override int GetHashCode()
        {
            return Account.Email.GetHashCode();
        }
    }
}
