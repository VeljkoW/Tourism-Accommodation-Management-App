using BookingApp.Domain.Model;
using BookingApp.Services;
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
using System.Windows.Media;
using System.Windows;
using System.Globalization;
using VirtualKeyboard.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class TourRequestStatisticsPageViewModel : INotifyPropertyChanged
    {
        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());
        private void ClickGoBackExecute()
        {
            TourRequestStatisticsPage.NavigationService.GoBack();
        }
        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                if (value == null)
                {
                    _year = 0;
                    OnPropertyChanged();
                    FilterStatistics();
                }
                if (_year != value)
                {
                    _year = value;
                    OnPropertyChanged();
                    FilterStatistics();
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
                    if (!string.IsNullOrEmpty(SelectedLanguage))
                    {
                        SelectedState = "";
                        SelectedCity = "";
                    }
                }
                FilterStatistics();
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
                    if (SelectedState != null && SelectedState != "")
                    {
                        // Clear city when a state is selected
                        SelectedCity = "";
                    }
                    SelectedLanguage = "";
                }
                if (SelectedState != "" && SelectedState != null)
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
                    SelectedLanguage = "";
                }
                    FilterStatistics();
            }
        }
        private string _selectedCity = "";
        public string SelectedCity
        {
            get { return _selectedCity; }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged();
                    FilterStatistics();
                    SelectedLanguage = "";
                }
            }
        }
        public List<Location> Locations = new List<Location>();

        public TourRequestStatisticsPageViewModel(TourRequestStatisticsPage tourRequestStatisticsPage)
        {
            TourRequestStatisticsPage = tourRequestStatisticsPage;
            TourRequestStatisticsPage.UpdateTrigger += LoadStatistics;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            TourRequestStatisticsPage.Statistics.Children.Clear();
            Languages.Clear();
            List<string> languages = new List<string>();
            List<int> locationIds = new List<int>();
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAll().ToList();
            foreach (var tourSuggestion in tourSuggestions)
            {
                languages.Add(tourSuggestion.Language);
                locationIds.Add(tourSuggestion.LocationId);
            }
            foreach (string language in languages.Distinct())
            {
                Languages.Add(language);
            }
            foreach (int locationId in locationIds)
            {
                Locations.Add(LocationService.GetInstance().GetById(locationId));
            }
            foreach (Location location in Locations)
            {
                if (!States.Contains(location.State))
                {
                    States.Add(location.State);
                }
            }
            FilterStatistics();
        }

        private void FilterStatistics()
        {
            TourRequestStatisticsPage.Statistics.Children.Clear();
            List<TourSuggestion> tourSuggestions = TourSuggestionService.GetInstance().GetAll().ToList();
            if (Year == 0)
            {
                Dictionary<int, int> yearRequestCounts = new Dictionary<int, int>();
                foreach (var tourSuggestion in tourSuggestions)
                {
                    Location location = LocationService.GetInstance().GetById(tourSuggestion.LocationId);
                    if (SelectedLanguage != null && SelectedLanguage != "")
                    {
                        if (!tourSuggestion.Language.Equals(SelectedLanguage))
                        {
                            continue;
                        }
                    }
                    if (SelectedState != null && SelectedState != "")
                    {
                        if (!location.State.Equals(SelectedState))
                        {
                            continue;
                        }
                    }
                    if (SelectedCity != null && SelectedCity != "")
                    {
                        if (!location.City.Equals(SelectedCity))
                        {
                            continue;
                        }
                    }
                    int year = tourSuggestion.FromDate.Year;
                    if (!yearRequestCounts.ContainsKey(year))
                    {
                        yearRequestCounts[year] = 1;
                    }
                    else
                    {
                        yearRequestCounts[year]++;
                    }
                }

                // Display statistics for each year
                foreach (var entry in yearRequestCounts)
                {
                    TourRequestStatisticsPage.Statistics.Children.Add(new UserControlRequestStatistics(entry.Key.ToString(), entry.Value.ToString()));
                }
            }
            else
            {
                var filteredTourSuggestions = tourSuggestions.Where(suggestion => suggestion.FromDate.Year == Year).ToList();
                Dictionary<int, int> monthRequestCounts = new Dictionary<int, int>();

                foreach (var tourSuggestion in filteredTourSuggestions)
                {
                    Location location = LocationService.GetInstance().GetById(tourSuggestion.LocationId);
                    if (SelectedLanguage != null && SelectedLanguage != "")
                    {
                        if (!tourSuggestion.Language.Equals(SelectedLanguage))
                        {
                            continue;
                        }
                    }
                    if (SelectedState != null && SelectedState != "")
                    {
                        if (!location.State.Equals(SelectedState))
                        {
                            continue;
                        }
                    }
                    if (SelectedCity != null && SelectedCity != "")
                    {
                        if (!location.City.Equals(SelectedCity))
                        {
                            continue;
                        }
                    }
                    int month = tourSuggestion.FromDate.Month;
                    if (!monthRequestCounts.ContainsKey(month))
                    {
                        monthRequestCounts[month] = 1;
                    }
                    else
                    {
                        monthRequestCounts[month]++;
                    }
                }
                foreach (var entry in monthRequestCounts)
                {
                    TourRequestStatisticsPage.Statistics.Children.Add(new UserControlRequestStatistics(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(entry.Key), entry.Value.ToString()));
                }
            }
        }

        public ObservableCollection<string> States { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Cities { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Languages { get; set; } = new ObservableCollection<string>();
        public TourRequestStatisticsPage TourRequestStatisticsPage { get; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
