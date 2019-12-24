using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using ProjectHotels.Views;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

namespace ProjectHotels.ViewModels
{
    public class HotelsListViewModel : INotifyPropertyChanged, IEnumerable<HotelViewModel>
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<HotelViewModel> allHotels = new List<HotelViewModel>();

        private ObservableCollection<HotelViewModel> hotels;
        public ObservableCollection<HotelViewModel> Hotels
        {
            get { return hotels; }
            set
            {
                if (hotels != value)
                {
                    hotels = value;
                    OnPropertyChanged("Hotels");
                }
            }
        }

        public List<string> Sortings { get; private set; }

        HotelViewModel selectedHotel;

        public INavigation Navigation { get; set; }

        public HotelsListViewModel(string city, DateTime dateTo, DateTime dateFrom, int adults, int children)
        {
            var hotelsList = App.Database.GetHotels().Where(i => i.City == city);
            foreach (var hotel in hotelsList)
                allHotels.Add(new HotelViewModel(hotel));
            Hotels = new ObservableCollection<HotelViewModel>(allHotels);
            Resources = new LocalizedResources(typeof(Resources.Resources), App.CurrentSettings.SelectedLanguage);
            Sortings = new List<string>() { Resources["SortingName"], Resources["SortingRating"], Resources["SortingStars"] };
            SelectedSorting = Resources["SortingName"];
            City = city;
            Dates = string.Format("{0} - {1}", dateTo.ToString("MM/dd/yyyy"), dateFrom.ToString("MM/dd/yyyy"));
            People = string.Format("{0} {1}, {2} {3}", adults, Resources["SearchAdults"], children, Resources["SearchChildren"]);
        }

        public HotelsListViewModel(List<int> favoritesIDs)
        {
            foreach (var id in favoritesIDs)
                allHotels.Add(new HotelViewModel(App.Database.GetHotel(id)));
            Hotels = new ObservableCollection<HotelViewModel>(allHotels);
            Resources = new LocalizedResources(typeof(Resources.Resources), App.CurrentSettings.SelectedLanguage);
            Sortings = new List<string>();
        }

        public HotelViewModel SelectedHotel
        {
            get { return selectedHotel; }
            set
            {
                if (selectedHotel != value)
                {
                    HotelViewModel temp = value;
                    temp.ListViewModel = this;
                    selectedHotel = null;
                    OnPropertyChanged("SelectedHotel");
                    Navigation.PushAsync(new HotelPage(temp));
                }
            }
        }

        public string City { get; private set; }
        public string Dates { get; private set; }
        public string People { get; private set; }

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

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void FilterStars()
        {
            Hotels = new ObservableCollection<HotelViewModel>(allHotels.Where(i => i.StarsInt >= 4));
        }

        public void FilterRating()
        {
            Hotels = new ObservableCollection<HotelViewModel>(allHotels.Where(i => i.Rating >= 8));
        }

        public void FilterNone()
        {
            Hotels = new ObservableCollection<HotelViewModel>(allHotels);
        }

        public void SortName()
        {
            var temp = Hotels.OrderBy(i => i.Name);
            Hotels = new ObservableCollection<HotelViewModel>(temp);
        }
        public void SortStars()
        {
            var temp = Hotels.OrderByDescending(i => i.StarsInt);
            Hotels = new ObservableCollection<HotelViewModel>(temp);
        }
        public void SortRating()
        {
            var temp = Hotels.OrderByDescending(i => i.Rating);
            Hotels = new ObservableCollection<HotelViewModel>(temp);
        }

        string selectedSorting;
        public string SelectedSorting
        {
            get { return selectedSorting; }
            set
            {
                selectedSorting = value;
                if (value == Resources["SortingName"])
                    SortName();
                else if (value == Resources["SortingStars"])
                    SortStars();
                else
                    SortRating();
                OnPropertyChanged("SelectedSorting");
            }
        }
        public IEnumerator<HotelViewModel> GetEnumerator()
        {
            return Hotels.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
