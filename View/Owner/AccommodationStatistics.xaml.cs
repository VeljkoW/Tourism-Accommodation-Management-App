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
            StatisticsByYearTable.IsEnabled = false;
            StatisticsByMonthTable.IsEnabled = false;
        }

        private void AccommodationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatisticsByYearTable.IsEnabled = true;
            StatisticsByMonthTable.IsEnabled = false;
            AccommodationStatisticsViewModel.AccommodationStatisticsByYears.Clear();
            AccommodationStatisticsViewModel.AccommodationStatisticsByMonths.Clear();
            AccommodationStatisticsViewModel.UpdateYears();
        }

        private void YearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AccommodationStatisticsViewModel.SelectedAccommodationStatisticsByYear != null)
            {
                StatisticsByMonthTable.IsEnabled = true;
                AccommodationStatisticsViewModel.AccommodationStatisticsByMonths.Clear();
                AccommodationStatisticsViewModel.UpdateMonths();
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
            OwnerMainWindow.NavigationButtonBarPressed("AccommodationManagementButton");
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
                OwnerMainWindow.mainFrame.Navigate(AccommodationRegistration);
                OwnerMainWindow.NavigationButtonBarPressed("AccommodationManagementButton");
                AccommodationRegistration.ScrollViewerName.ScrollToVerticalOffset(AccommodationRegistration.ScrollViewerName.VerticalOffset + 510);
            });
        }
    }
}
