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
    }
}
