using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.ViewModel.Owner;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationStatistics.xaml
    /// </summary>
    public partial class AccommodationStatistics : Page
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public AccommodationStatisticsViewModel AccommodationStatisticsViewModel {  get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public User User { get; set; }
        public AccommodationStatistics(OwnerMainWindow ownerMainWindow)
        {
            OwnerMainWindow = ownerMainWindow;
            User = ownerMainWindow.user;
            InitializeComponent();
            AccommodationStatisticsViewModel = new AccommodationStatisticsViewModel(this);
            DataContext = AccommodationStatisticsViewModel;
            GridsVisibility();
            StatisticsByYearTable.IsEnabled = false;
            StatisticsByMonthTable.IsEnabled = false;
            MostOccupiedYearTextBlock.Visibility = Visibility.Collapsed;
            MostOccupiedMonthTextBlock.Visibility = Visibility.Collapsed;
            YearSelectedValidation.Visibility = Visibility.Collapsed;
            AccommodationSelectedValidation.Visibility = Visibility.Visible;
            App.LanguageChanged += OnLanguageChanged;
            App.ThemeChanged += OnThemeChanged;
            OnThemeChanged();
            if (App.currentLanguage() == ENG)
                AccommodationSelectedValidation.Text = "Select the accommodation!";
            else
                AccommodationSelectedValidation.Text = "Izaberi smeštaj!";
        }

        private void AccommodationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatisticsByYearTable.IsEnabled = true;
            StatisticsByMonthTable.IsEnabled = false;
            AccommodationStatisticsViewModel.AccommodationStatisticsByYears.Clear();
            AccommodationStatisticsViewModel.AccommodationStatisticsByMonths.Clear();
            AccommodationStatisticsViewModel.UpdateYears();
            MostOccupiedYearTextBlock.Visibility = Visibility.Visible;
            MostOccupiedMonthTextBlock.Visibility = Visibility.Collapsed;

            AccommodationSelectedValidation.Visibility = Visibility.Collapsed;
            YearSelectedValidation.Visibility = Visibility.Visible;
            if (App.currentLanguage() == ENG)
                YearSelectedValidation.Text = "Select the year!";
            else
                YearSelectedValidation.Text = "Izaberi godinu!";

        }

        private void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AccommodationStatisticsViewModel.SelectedAccommodationStatisticsByYear != null)
            {
                StatisticsByMonthTable.IsEnabled = true;
                AccommodationStatisticsViewModel.AccommodationStatisticsByMonths.Clear();
                AccommodationStatisticsViewModel.UpdateMonths();
                MostOccupiedMonthTextBlock.Visibility = Visibility.Visible;
                YearSelectedValidation.Visibility= Visibility.Collapsed;
            }
        }

        private void AddAccommodationsClick1(object sender, RoutedEventArgs e)
        {
            AddACcommodationClickGeneral(AccommodationStatisticsViewModel.MostPopularLocationId1);
        }
        private void AddAccommodationsClick2(object sender, RoutedEventArgs e)
        {
            AddACcommodationClickGeneral(AccommodationStatisticsViewModel.MostPopularLocationId2);
        }
        private void AddAccommodationsClick3(object sender, RoutedEventArgs e)
        {
            AddACcommodationClickGeneral(AccommodationStatisticsViewModel.MostPopularLocationId3);
        }
        private void AddACcommodationClickGeneral(int MostPopularLocationId)
        {
            AccommodationRegistration AccommodationRegistration = new AccommodationRegistration(User);
            Location location = LocationService.GetInstance().GetById(MostPopularLocationId);

            var SelectedState = AccommodationRegistration.AccommodationManagementViewModel.States.FirstOrDefault(t => t.Equals(location.State));
            AccommodationRegistration.AccommodationManagementViewModel.SelectedState = SelectedState;
            LocationService.GetInstance().GetCitiesForState(AccommodationRegistration.AccommodationManagementViewModel.Cities, SelectedState);

            for(int i=0; i< AccommodationRegistration.AccommodationManagementViewModel.Cities.Count(); i++)
            {
                if (AccommodationRegistration.AccommodationManagementViewModel.Cities[i].Id == location.Id)
                {
                    AccommodationRegistration.AccommodationManagementViewModel.SelectedLocation = AccommodationRegistration.AccommodationManagementViewModel.Cities[i];
                    AccommodationRegistration.CityComboBox.SelectedIndex = i;
                    break;
                }
            }
            //var SelectedLocation = AccommodationRegistration.AccommodationManagementViewModel.Cities.FirstOrDefault(t => t.Id == location.Id);
            //AccommodationRegistration.AccommodationManagementViewModel.SelectedLocation = SelectedLocation;
            //AccommodationRegistration.CityComboBox.SelectedItem = SelectedLocation;

            OwnerMainWindow.mainFrame.Navigate(AccommodationRegistration);
            OwnerMainWindow.CurrentNavigationButton = "AccommodationManagementButton";
        }

        private void CloseAccommodationsClick1(object sender, RoutedEventArgs e)
        {
            CloseAccommodationsClickGeneral(AccommodationStatisticsViewModel.LeastPopularLocationId1);
        }
        private void CloseAccommodationsClick2(object sender, RoutedEventArgs e)
        {
            CloseAccommodationsClickGeneral(AccommodationStatisticsViewModel.LeastPopularLocationId2);
        }
        private void CloseAccommodationsClick3(object sender, RoutedEventArgs e)
        {
            CloseAccommodationsClickGeneral(AccommodationStatisticsViewModel.LeastPopularLocationId3);
        }
        private void CloseAccommodationsClickGeneral(int LeastPopularLocationId)
        {
            AccommodationRegistration AccommodationRegistration = new AccommodationRegistration(User, true);
            Location location = LocationService.GetInstance().GetById(LeastPopularLocationId);

            var SelectedState = AccommodationRegistration.AccommodationManagementViewModel.States.FirstOrDefault(t => t.Equals(location.State));
            AccommodationRegistration.AccommodationManagementViewModel.SelectedChosenState = SelectedState;
            AccommodationRegistration.AccommodationManagementViewModel.StateChosen();

            for (int i = 0; i < AccommodationRegistration.AccommodationManagementViewModel.CitiesForChoosing.Count(); i++)
            {
                if (AccommodationRegistration.AccommodationManagementViewModel.CitiesForChoosing[i].Id == location.Id)
                {
                    AccommodationRegistration.AccommodationManagementViewModel.SelectedChosenCity = AccommodationRegistration.AccommodationManagementViewModel.CitiesForChoosing[i];
                    AccommodationRegistration.ChooseCityComboBox.SelectedIndex = i + 1;
                    AccommodationRegistration.AccommodationManagementViewModel.CityChosen();
                    break;
                }
            }
            AccommodationRegistration.Dispatcher.Invoke(() =>
            {
                
                AccommodationRegistration.ScrollViewerName.ScrollToVerticalOffset(AccommodationRegistration.ScrollViewerName.VerticalOffset + 510);
            });
            OwnerMainWindow.mainFrame.Navigate(AccommodationRegistration);
            OwnerMainWindow.CurrentNavigationButton = "AccommodationManagementButton";
        }

        public void GridsVisibility()
        {
            switch (AccommodationStatisticsViewModel.CheckLocations())
            {
                case 0:
                    MostPopularGrid1.Visibility = Visibility.Collapsed;
                    MostPopularGrid2.Visibility = Visibility.Collapsed;
                    MostPopularGrid3.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid1.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid2.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid3.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    MostPopularGrid1.Visibility = Visibility.Visible;
                    MostPopularGrid2.Visibility = Visibility.Collapsed;
                    MostPopularGrid3.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid1.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid2.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid3.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    MostPopularGrid1.Visibility = Visibility.Visible;
                    MostPopularGrid2.Visibility = Visibility.Visible;
                    MostPopularGrid3.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid1.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid2.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid3.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    MostPopularGrid1.Visibility = Visibility.Visible;
                    MostPopularGrid2.Visibility = Visibility.Visible;
                    MostPopularGrid3.Visibility = Visibility.Visible;
                    MostUnpopularGrid1.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid2.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid3.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    MostPopularGrid1.Visibility = Visibility.Visible;
                    MostPopularGrid2.Visibility = Visibility.Visible;
                    MostPopularGrid3.Visibility = Visibility.Visible;
                    MostUnpopularGrid1.Visibility = Visibility.Visible;
                    MostUnpopularGrid2.Visibility = Visibility.Collapsed;
                    MostUnpopularGrid3.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    MostPopularGrid1.Visibility = Visibility.Visible;
                    MostPopularGrid2.Visibility = Visibility.Visible;
                    MostPopularGrid3.Visibility = Visibility.Visible;
                    MostUnpopularGrid1.Visibility = Visibility.Visible;
                    MostUnpopularGrid2.Visibility = Visibility.Visible;
                    MostUnpopularGrid3.Visibility = Visibility.Collapsed;
                    break;
                default:
                    MostPopularGrid1.Visibility = Visibility.Visible;
                    MostPopularGrid2.Visibility = Visibility.Visible;
                    MostPopularGrid3.Visibility = Visibility.Visible;
                    MostUnpopularGrid1.Visibility = Visibility.Visible;
                    MostUnpopularGrid2.Visibility = Visibility.Visible;
                    MostUnpopularGrid3.Visibility = Visibility.Visible;
                    break;
            }
        }
        public void OnThemeChanged()
        {
            if (App.currentTheme() == "Light")
            {
                var newColor = (Color)Application.Current.Resources["BorderLightBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
            }
            else
            {
                var newColor = (Color)Application.Current.Resources["BorderDarkBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
            }
        }
        private void OnLanguageChanged()
        {
            if (App.currentLanguage() == ENG)
            {
                YearSelectedValidation.Text = "Select the year!";
                AccommodationSelectedValidation.Text = "Select the accommodation!";
            }
            else
            {
                YearSelectedValidation.Text = "Izaberi godinu!";
                AccommodationSelectedValidation.Text = "Izaberi smeštaj!";
            }
        }
        ~AccommodationStatistics()
        {
            App.LanguageChanged -= OnLanguageChanged;
        }
    }
}
