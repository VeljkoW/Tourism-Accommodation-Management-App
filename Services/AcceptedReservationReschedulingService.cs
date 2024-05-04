using BookingApp.Domain.IRepositories;
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
    public class AcceptedReservationReschedulingService
    {
        public IAcceptedReservationReschedulingRepository AcceptedReservationReschedulingRepository { get; set; }
        public AcceptedReservationReschedulingService(IAcceptedReservationReschedulingRepository acceptedReservationReschedulingRepository) { AcceptedReservationReschedulingRepository = acceptedReservationReschedulingRepository; }
        public static AcceptedReservationReschedulingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<AcceptedReservationReschedulingService>();
        }
        public void Add(AcceptedReservationRescheduling acceptedReservationRescheduling)
        {
            AcceptedReservationReschedulingRepository.Add(acceptedReservationRescheduling);
        }

        public List<AcceptedReservationRescheduling> GetAll()
        {
            return AcceptedReservationReschedulingRepository.GetAll();
        }
        public void ReschedulingCountByYear(int accommodationId, ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            List<AcceptedReservationRescheduling> AcceptedReservationReschedulings = new List<AcceptedReservationRescheduling>();
            AcceptedReservationReschedulings = GetAll().Where(t => t.AccommodationId == accommodationId).ToList();
            foreach (AcceptedReservationRescheduling acceptedReservationRescheduling in AcceptedReservationReschedulings)
            {
                if (AccommodationStatisticsByYears.Count == 0)
                    AddAccommodationStatisticsByYear(acceptedReservationRescheduling, AccommodationStatisticsByYears);
                else
                {
                    bool alreadyExists = false;
                    foreach (AccommodationStatisticsByYear AccommodationStatisticsByYear in AccommodationStatisticsByYears)
                        if (AccommodationStatisticsByYear.Year == acceptedReservationRescheduling.AcceptedDate.Year)
                        {
                            AccommodationStatisticsByYear.Reschedulings++;
                            alreadyExists = true;
                            break;
                        }
                    if (!alreadyExists)
                        AddAccommodationStatisticsByYear(acceptedReservationRescheduling, AccommodationStatisticsByYears);
                }
            }
        }
        private void AddAccommodationStatisticsByYear(AcceptedReservationRescheduling acceptedReservationRescheduling,
                                                      ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            AccommodationStatisticsByYear AccommodationStatisticsByYear = new AccommodationStatisticsByYear();
            AccommodationStatisticsByYear.AccommodationId = acceptedReservationRescheduling.AccommodationId;
            AccommodationStatisticsByYear.Year = acceptedReservationRescheduling.AcceptedDate.Year;
            AccommodationStatisticsByYear.Reschedulings++;
            AccommodationStatisticsByYears.Add(AccommodationStatisticsByYear);
        }
        public void ReschedulingCountByMonth(int year, int accommodationId, ObservableCollection<AccommodationStatisticsByMonth> AccommodationStatisticsByMonths)
        {
            List<AcceptedReservationRescheduling> AcceptedReservationReschedulings = new List<AcceptedReservationRescheduling>();
            AcceptedReservationReschedulings = GetAll().Where(t => t.AccommodationId == accommodationId && t.AcceptedDate.Year == year).ToList();
            foreach (AcceptedReservationRescheduling acceptedReservationRescheduling in AcceptedReservationReschedulings)
                foreach (AccommodationStatisticsByMonth AccommodationStatisticsByMonth in AccommodationStatisticsByMonths)
                    if (AccommodationStatisticsByMonth.Month == acceptedReservationRescheduling.AcceptedDate.Month)
                        AccommodationStatisticsByMonth.Reschedulings++;
        }
    }
}
