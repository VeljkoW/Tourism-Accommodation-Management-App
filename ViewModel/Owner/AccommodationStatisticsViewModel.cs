using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Owner;

namespace BookingApp.ViewModel.Owner
{
    public class AccommodationStatisticsViewModel
    {
        public User User { get; set; }
        public ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations {  get; set; }
        public ObservableCollection<AccommodationStatisticsByMonth> AccommodationStatisticsByMonths { get; set; }
        public ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears { get; set; }
        public AccommodationStatisticsByYear SelectedAccommodationStatisticsByYear {  get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public Accommodation SelectedAccommodation {  get; set; }
        public AccommodationStatistics AccommodationStatistics {  get; set; }
        public int MostPopularLocationId1 {  get; set; }
        public int MostPopularLocationId2 { get; set; }
        public int MostPopularLocationId3 { get; set; }
        public int LeastPopularLocationId1 {  get; set; }
        public int LeastPopularLocationId2 { get; set; }
        public int LeastPopularLocationId3 { get; set; }
        public AccommodationStatisticsViewModel(AccommodationStatistics accommodationStatistics)
        {
            User = accommodationStatistics.User;
            AccommodationStatistics = accommodationStatistics;
            Accommodations = AccommodationService.GetInstance().GetAllByUser(User);
            AccommodationStatisticsByYears = new ObservableCollection<AccommodationStatisticsByYear>();
            AccommodationStatisticsByMonths = new ObservableCollection<AccommodationStatisticsByMonth>();
            AccommodationsStatisticsByLocations = AccommodationStatisticsService.GetInstance().UpdateLocations(User);
            CheckLocations();
            //MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
            //MostPopularLocationId2 = AccommodationsStatisticsByLocations[1].LocationId;
            //MostPopularLocationId3 = AccommodationsStatisticsByLocations[2].LocationId;
            //LeastPopularLocationId1 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 1].LocationId;
            //LeastPopularLocationId2 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 2].LocationId;
            //LeastPopularLocationId3 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 3].LocationId;
        }
        public int CheckLocations()
        {
            switch (AccommodationsStatisticsByLocations.Count)
            {
                case 0:
                    break;
                case 1:
                    MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
                    break;
                case 2:
                    MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
                    MostPopularLocationId2 = AccommodationsStatisticsByLocations[1].LocationId;
                    break;
                case 3:
                    MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
                    MostPopularLocationId2 = AccommodationsStatisticsByLocations[1].LocationId;
                    MostPopularLocationId3 = AccommodationsStatisticsByLocations[2].LocationId;
                    break;
                case 4:
                    MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
                    MostPopularLocationId2 = AccommodationsStatisticsByLocations[1].LocationId;
                    MostPopularLocationId3 = AccommodationsStatisticsByLocations[2].LocationId;
                    LeastPopularLocationId1 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 1].LocationId;
                    break;
                case 5:
                    MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
                    MostPopularLocationId2 = AccommodationsStatisticsByLocations[1].LocationId;
                    MostPopularLocationId3 = AccommodationsStatisticsByLocations[2].LocationId;
                    LeastPopularLocationId1 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 1].LocationId;
                    LeastPopularLocationId2 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 2].LocationId;
                    break;
                default:
                    MostPopularLocationId1 = AccommodationsStatisticsByLocations[0].LocationId;
                    MostPopularLocationId2 = AccommodationsStatisticsByLocations[1].LocationId;
                    MostPopularLocationId3 = AccommodationsStatisticsByLocations[2].LocationId;
                    LeastPopularLocationId1 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 1].LocationId;
                    LeastPopularLocationId2 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 2].LocationId;
                    LeastPopularLocationId3 = AccommodationsStatisticsByLocations[AccommodationsStatisticsByLocations.Count() - 3].LocationId;
                    break;
            }
            return AccommodationsStatisticsByLocations.Count;
        }
        public void UpdateYears()
        {
            AccommodationStatisticsService.GetInstance().UpdateYears(SelectedAccommodation.Id, AccommodationStatisticsByYears);
            int popularYearIndex = 0;
            double maxOccupancy=0;
            for(int i=0;  i<AccommodationStatisticsByYears.Count; i++)
            {
                double tempOccupancy;
                if (DateTime.IsLeapYear(AccommodationStatisticsByYears[i].Year))
                    tempOccupancy = (double)AccommodationStatisticsByYears[i].Reservations / 366;
                else
                    tempOccupancy = (double)AccommodationStatisticsByYears[i].Reservations / 365;
                if(maxOccupancy < tempOccupancy)
                {
                    popularYearIndex = i;
                    maxOccupancy = tempOccupancy;
                }
            }
            if(AccommodationStatisticsByYears.Count != 0)
                AccommodationStatistics.PopularYearLabel.Text = AccommodationStatisticsByYears[popularYearIndex].Year.ToString();
        }
        public void UpdateMonths()
        {
            AccommodationStatisticsService.GetInstance().UpdateMonths(SelectedAccommodationStatisticsByYear.Year, SelectedAccommodation.Id, AccommodationStatisticsByMonths);
            int popularMonthIndex = 0;
            double maxOccupancy = 0;
            for (int i = 0; i < AccommodationStatisticsByMonths.Count; i++)
            {
                double tempOccupancy;
                int monthDays = DateTime.DaysInMonth(SelectedAccommodationStatisticsByYear.Year, AccommodationStatisticsByMonths[i].Month);
                tempOccupancy = (double)AccommodationStatisticsByMonths[i].Reservations / monthDays;
                if (maxOccupancy < tempOccupancy)
                {
                    popularMonthIndex = i;
                    maxOccupancy = tempOccupancy;
                }
            }
            AccommodationStatistics.PopularMonthLabel.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(AccommodationStatisticsByMonths[popularMonthIndex].Month); //AccommodationStatisticsByMonths[popularMonthIndex].Month.ToString();
        }
    }
}
