using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationStatisticsService
    {
        public static AccommodationStatisticsService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<AccommodationStatisticsService>();
        }
        public void UpdateYears(int accommodationId, ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            ReservedAccommodationService.GetInstance().ReservationCountByYear(accommodationId, AccommodationStatisticsByYears);
            ReservationCancellationService.GetInstance().CancellationCountByYear(accommodationId, AccommodationStatisticsByYears);
            AcceptedReservationReschedulingService.GetInstance().ReschedulingCountByYear(accommodationId, AccommodationStatisticsByYears);
            RenovationRequestService.GetInstance().RenovationRequestCountByYear(accommodationId, AccommodationStatisticsByYears);

            SortAccommodationStatisticsByYears(AccommodationStatisticsByYears);
        }
        private void SortAccommodationStatisticsByYears(ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            for (int i = 0; i < AccommodationStatisticsByYears.Count - 1; i++)
            {
                for (int j = i + 1; j < AccommodationStatisticsByYears.Count; j++)
                {
                    if (AccommodationStatisticsByYears[i].Year < AccommodationStatisticsByYears[j].Year)
                    {
                        AccommodationStatisticsByYear tempAccommodationStatisticsByYear = new AccommodationStatisticsByYear(AccommodationStatisticsByYears[i]);
                        AccommodationStatisticsByYears[i] = AccommodationStatisticsByYears[j];
                        AccommodationStatisticsByYears[j] = tempAccommodationStatisticsByYear;
                    }
                }
            }
        }
        public void UpdateMonths(int year, int accommodationId, ObservableCollection<AccommodationStatisticsByMonth> AccommodationStatisticsByMonths)
        {
            for(int i = 1;i <= 12; i++)
            {
                AccommodationStatisticsByMonth AccommodationStatisticsByMonth = new AccommodationStatisticsByMonth(i);
                AccommodationStatisticsByMonth.AccommodationId = accommodationId;
                AccommodationStatisticsByMonths.Add(AccommodationStatisticsByMonth);
            }
            ReservedAccommodationService.GetInstance().ReservationCountByMonth(year, accommodationId, AccommodationStatisticsByMonths);
            ReservationCancellationService.GetInstance().CancellationCountByMonth(year, accommodationId, AccommodationStatisticsByMonths);
            AcceptedReservationReschedulingService.GetInstance().ReschedulingCountByMonth(year, accommodationId, AccommodationStatisticsByMonths);
            RenovationRequestService.GetInstance().RenovationRequestCountByMonth(year, accommodationId, AccommodationStatisticsByMonths);
        }

        public ObservableCollection<AccommodationsStatisticsByLocation> UpdateLocations(User user)
        {
            ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations = new ObservableCollection<AccommodationsStatisticsByLocation>();
            AccommodationService.GetInstance().AccommodationsByLocation(user, AccommodationsStatisticsByLocations);
            ReservedAccommodationService.GetInstance().SortAccommodationStatisticsByLocation(AccommodationsStatisticsByLocations);

            return AccommodationsStatisticsByLocations;
        }
    }
}
