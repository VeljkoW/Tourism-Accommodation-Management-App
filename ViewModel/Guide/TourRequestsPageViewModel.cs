using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BookingApp.ViewModel.Guide
{
    public class TourRequestsPageViewModel : INotifyPropertyChanged
    {
        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());
        public ObservableCollection<UserControlTourSuggestion> Cards { get; set; } = new ObservableCollection<UserControlTourSuggestion>();
        public TourRequestsPage TourRequestsPage { get; }
        private int userId = GuideMainWindow.UserId;
        private bool _stateBoxIsEnabled = true;
        public bool StateBoxIsEnabled
        {
            get { return _stateBoxIsEnabled; }
            set
            {
                _stateBoxIsEnabled = value;
                OnPropertyChanged(nameof(StateBoxIsEnabled));
            }
        }
        private bool _cityBoxIsEnabled = false;
        public bool CityBoxIsEnabled
        {
            get { return _cityBoxIsEnabled; }
            set
            {
                _cityBoxIsEnabled = value;
                OnPropertyChanged(nameof(CityBoxIsEnabled));
            }
        }
        private UserControlAcceptTourRequest _acceptTourCard;
        public UserControlAcceptTourRequest AcceptTourCard
        {
            get { return _acceptTourCard; }
            set
            {
                if (_acceptTourCard != value)
                {
                    _acceptTourCard = value;
                    OnPropertyChanged(nameof(AcceptTourCard));
                    ClearPopup();
                    TourRequestsPage.VAU.Children.Add(AcceptTourCard);
                }
            }
        }
        private int _touristCount;
        public int TouristCount
        {
            get { return _touristCount; }
            set
            {
                if(value == null)
                {
                    _touristCount = 0;
                    OnPropertyChanged();
                }
                if (_touristCount != value)
                {
                    _touristCount = value;
                    OnPropertyChanged();
                    FilterUpdated();
                }
            }
        }
        private DateTime _selectedFromDate= DateTime.Now.AddMonths(-5);
        public DateTime SelectedFromDate
        {
            get { return _selectedFromDate; }
            set
            {
                if (_selectedFromDate != value)
                {
                    _selectedFromDate = value;
                    OnPropertyChanged();
                    if(_selectedFromDate!= DateTime.MinValue)
                    {
                        SelectedToDate = _selectedFromDate.AddDays(1);
                        TourRequestsPage.toDatePicker.DisplayDateStart= _selectedFromDate.AddDays(1);
                        IsEnabledToDate =true;
                    }
                }
            }
        }
        private DateTime _selectedToDate = DateTime.Now.AddMonths(-5);
        public DateTime SelectedToDate
        {
            get { return _selectedToDate; }
            set
            {
                if (_selectedToDate != value)
                {
                    _selectedToDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedLanguage = "";
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _selectedState = "";
        public string SelectedState
        {
            get { return _selectedState; }
            set
            {
                if (_selectedState != value)
                {
                    _selectedState = value;
                    OnPropertyChanged();
                }
                if(SelectedState != "" && SelectedState != null)
                {
                    Cities.Clear();
                    foreach (Location location in Locations)
                    {
                        if (SelectedState.Equals(location.State) && !Cities.Contains(location.City))
                        {
                            Cities.Add(location.City);
                        }
                    }
                    SelectedCity = Cities.First();
                    CityBoxIsEnabled = true;
                }
            }
        }
        private string _selectedCity="";
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _isEnabledToDate;
        public bool IsEnabledToDate
        {
            get { return _isEnabledToDate; }
            set
            {
                if (_isEnabledToDate != value)
                {
                    _isEnabledToDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public List<Location> Locations = new List<Location>();
        public ObservableCollection<string> States { get; set;} = new ObservableCollection<string>();
        public ObservableCollection<string> Cities { get; set;} = new ObservableCollection<string>();
        public ObservableCollection<string> Languages { get; set;} = new ObservableCollection<string>();
        private void ClickGoBackExecute()
        {
            Cards.Clear();
            TourRequestsPage.NavigationService.GoBack();
        }
        public TourRequestsPageViewModel(TourRequestsPage tourRequestsPage)
        {
            TourRequestsPage = tourRequestsPage;
            TourRequestsPage.FilterUpdateEvent += FilterUpdated;
            Load();
        }
        public void ClearPopup()
        {
            TourRequestsPage.VAU.Children.Clear();
            Load();
        }
        private DateTime oldestDate;
        public void Load()
        {
            Languages.Clear();
            List<string> languages = new List<string>();
            List<int> locationIds = new List<int>();
            Cards.Clear();
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAll().ToList();
            DateTime oldestDate = tourSuggestions.Min(suggestion => suggestion.FromDate);
            this.oldestDate = oldestDate.AddDays(-1);
            SelectedFromDate = oldestDate.AddDays(-1);
            this.TourRequestsPage.fromDatePicker.DisplayDateStart = oldestDate;
            IsEnabledToDate = false;
            foreach (var tourSuggestion in tourSuggestions)
            {
                if(tourSuggestion.Status == TourSuggestionStatus.Pending)
                {
                    Cards.Add(new UserControlTourSuggestion(this,tourSuggestion));
                    languages.Add(tourSuggestion.Language);
                    locationIds.Add(tourSuggestion.LocationId);
                }
            }
            foreach(string language in languages.Distinct())
            {
                Languages.Add(language);
            }
            foreach (int locationId in locationIds)
            {
                Locations.Add(LocationService.GetInstance().GetById(locationId));
            }
            foreach(Location location in Locations) 
            {
                if(!States.Contains(location.State))
                {
                States.Add(location.State);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void FilterUpdated()
        {
            Cards.Clear();
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAll().ToList();
            DateTime oldestDate = tourSuggestions.Min(suggestion => suggestion.FromDate);
            foreach (var tourSuggestion in tourSuggestions)
            {
                Location location = LocationService.GetInstance().GetById(tourSuggestion.LocationId);
                if(SelectedLanguage != null)
                if (SelectedLanguage != "" && !SelectedLanguage.Equals(tourSuggestion.Language))
                {
                    continue;
                }
                if(SelectedCity != null)
                if (SelectedCity != "" && !SelectedCity.Equals(location.City))
                {
                    continue;
                }
                if(SelectedState != null)
                if (SelectedState != "" && !SelectedState.Equals(location.State))
                {
                    continue;
                }
                if(TouristCount > 0 && tourSuggestion.NumberOfPeople < TouristCount) 
                {
                    continue;
                }
                if(IsEnabledToDate) 
                { 
                    if (tourSuggestion.ToDate < SelectedFromDate || tourSuggestion.FromDate > SelectedToDate)
                    {
                        continue;
                    }
                }
                {
                    if (tourSuggestion.Status == TourSuggestionStatus.Pending)
                    {
                        Cards.Add(new UserControlTourSuggestion(this, tourSuggestion));
                    }
                }
            }
        }
        public RelayCommand ClearFilter => new RelayCommand(execute => ClearFilterExecute());

        private void ClearFilterExecute()
        {
            SelectedCity = "";
            SelectedState= "";
            TouristCount= 0;
            SelectedFromDate = oldestDate;
            SelectedToDate = SelectedFromDate;
            IsEnabledToDate = false;
            SelectedLanguage = "";
            FilterUpdated();
            TourRequestsPage.StateCombobox.SelectedIndex=-1;
            TourRequestsPage.CityCombobox.SelectedIndex=-1;
            TourRequestsPage.LanguageCombobox.SelectedIndex=-1;
        }
    }
}
