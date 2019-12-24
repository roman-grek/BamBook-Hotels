using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ProjectHotels.Models;
using Xamarin.Forms;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.Linq;
using Xamarin.Essentials;
using System.Net.Mail;
using System.Net;

namespace ProjectHotels.ViewModels
{
    public class HotelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        HotelsListViewModel lvm;

        Geocoder geocoder;

        public ICommand ChangeFavoritesCommand { get; private set; }
        public ICommand ShowOnMapCommand { get; private set; }
        public ICommand BookHotelCommand { get; private set; }

        public Hotel Hotel { get; private set; }

        public HotelViewModel()
        {
            Hotel = new Hotel();
            
            ChangeFavoritesCommand = new Command(ChangeFavorites);
            ShowOnMapCommand = new Command(ShowOnMap);
            BookHotelCommand = new Command(BookHotel);
            geocoder = new Geocoder();
            Resources = new LocalizedResources(typeof(Resources.Resources), App.CurrentSettings.SelectedLanguage);
        }

        public HotelViewModel(Hotel hotel) : this()
        {
            Hotel = hotel;
            IsFavorite = App.Accounts.Current.Favorites.Contains(Hotel.ID);
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

        public HotelsListViewModel ListViewModel
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
            get { return Hotel.Name; }
            set
            {
                if (Hotel.Name != value)
                {
                    Hotel.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        public string City
        {
            get { return Hotel.City; }
            set
            {
                if (Hotel.City != value)
                {
                    Hotel.City = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        public ImageSource Stars
        {
            get
            {
                if (Hotel.Stars == 1)
                    return ImageSource.FromFile("star.png");
                if (Hotel.Stars == 2)
                    return ImageSource.FromFile("star2.png");
                if (Hotel.Stars == 3)
                    return ImageSource.FromFile("star3.png");
                if (Hotel.Stars == 4)
                    return ImageSource.FromFile("star4.png");
                else
                    return ImageSource.FromFile("star5.png");
            }
        }

        public int StarsInt { get => Hotel.Stars; }
        public double Rating
        {
            get { return Hotel.Rating; }
            set
            {
                if (Hotel.Rating != value)
                {
                    Hotel.Rating = value;
                    OnPropertyChanged("Rating");
                }
            }
        }

        public string RatingString
        {
            get { return string.Format("Rating: {0}", Rating); }
        }
        public string Address
        {
            get { return Hotel.Address; }
            set
            {
                if (Hotel.Address != value)
                {
                    Hotel.Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        public ImageSource Picture1
        {
            get { return ImageSource.FromUri(new Uri(Hotel.Picture1)); }
        }
        public ImageSource Picture2
        {
            get { return ImageSource.FromUri(new Uri(Hotel.Picture2)); }
        }
        public ImageSource Picture3
        {
            get { return ImageSource.FromUri(new Uri(Hotel.Picture3)); }
        }
        public ImageSource Picture4
        {
            get => ImageSource.FromUri(new Uri(Hotel.Picture4)); 
        }

        public ImageSource WaypointPicture
        {
            get => ImageSource.FromFile("waypoint.png");
        }

        public bool IsValid
        {
            get
            {
                return (!string.IsNullOrEmpty(Name.Trim())) &&
                    (!string.IsNullOrEmpty(City.Trim())) &&
                    Hotel.Stars > 0 && Rating > 0 && (!string.IsNullOrEmpty(Address.Trim()));
            }
        }

        public bool IsFavorite { get; private set; }

        public ImageSource FavoritePicture
        {
            get => IsFavorite ? ImageSource.FromFile("heart_full.png") : ImageSource.FromFile("heart_empty.png");
        }

        void ChangeFavorites()
        {
            if (App.Accounts.Current == null)
                return;
            if (IsFavorite)
            {
                App.Accounts.Current.RemoveFromFavorites(Hotel.ID);
            }
            else
            {
                App.Accounts.Current.AddToFavorites(Hotel.ID);
            }
            IsFavorite = !IsFavorite;
            OnPropertyChanged("FavoritePicture");
        }

        void BookHotel()
        {
            try
            {
                MailAddress from = new MailAddress("bambookhotels@gmail.com", "BamBook Hotels");
                MailAddress to = new MailAddress(App.Accounts.Current.Email, App.Accounts.Current.Name);
                MailMessage msg = new MailMessage(from, to);
                msg.Subject = "Бронирование отеля через приложение BamBook Hotels";
                msg.Body = string.Format(("<h2>Ваш заказ принят</h2> <p>С вашего аккаунта было совершено бронирование отеля {0}" +
                    "на даты {1}. {2}.</p>"), Name, ListViewModel.Dates, ListViewModel.People);
                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("bambookhotels@gmail.com", "isp.project2019");
                smtp.EnableSsl = true;
                smtp.SendMailAsync(msg);
            }
            catch { }
        }

        async void ShowOnMap()
        {
            IEnumerable<Position> approximateLocations = await geocoder.GetPositionsForAddressAsync(Address);
            Position position = approximateLocations.FirstOrDefault();
            await Xamarin.Essentials.Map.OpenAsync(position.Latitude, position.Longitude, new MapLaunchOptions
            {
                Name = this.Name,
                NavigationMode = NavigationMode.None
            });
        }
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
