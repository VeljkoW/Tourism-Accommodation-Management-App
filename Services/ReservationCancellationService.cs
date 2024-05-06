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
    public class ReservationCancellationService
    {
        public IReservationCancellationRepository ReservationCancellationRepository { get; set; }
        public ReservationCancellationService(IReservationCancellationRepository reservationCancellationRepository) { ReservationCancellationRepository = reservationCancellationRepository; }
        public static ReservationCancellationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservationCancellationService>();
        }
        public void Add(ReservationCancellation reservationCancellation)
        {
            ReservationCancellationRepository.Add(reservationCancellation);
        }

        public List<ReservationCancellation> GetAll()
        {
            return ReservationCancellationRepository.GetAll();
        }
        public void CancellationCountByYear(int accommodationId, ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            List<ReservationCancellation> ReservationCancellations = new List<ReservationCancellation>();
            ReservationCancellations = GetAll().Where(t => t.AccommodationId == accommodationId).ToList();
            foreach (ReservationCancellation reservationCancellation in ReservationCancellations)
            {
                if (AccommodationStatisticsByYears.Count == 0)
                    AddAccommodationStatisticsByYear(reservationCancellation, AccommodationStatisticsByYears);
                else
                {
                    bool alreadyExists = false;
                    foreach (AccommodationStatisticsByYear AccommodationStatisticsByYear in AccommodationStatisticsByYears)
                        if (AccommodationStatisticsByYear.Year == reservationCancellation.CancelDate.Year)
                        {
                            AccommodationStatisticsByYear.Cancellations++;
                            alreadyExists = true;
                            break;
                        }
                    if (!alreadyExists)
                        AddAccommodationStatisticsByYear(reservationCancellation, AccommodationStatisticsByYears);
                }
            }
        }
        private void AddAccommodationStatisticsByYear(ReservationCancellation reservationCancellation,
                                                      ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            AccommodationStatisticsByYear AccommodationStatisticsByYear = new AccommodationStatisticsByYear();
            AccommodationStatisticsByYear.AccommodationId = reservationCancellation.AccommodationId;
            AccommodationStatisticsByYear.Year = reservationCancellation.CancelDate.Year;
            AccommodationStatisticsByYear.Cancellations++;
            AccommodationStatisticsByYears.Add(AccommodationStatisticsByYear);
        }
        public void CancellationCountByMonth(int year, int accommodationId, ObservableCollection<AccommodationStatisticsByMonth> AccommodationStatisticsByMonths)
        {
            List<ReservationCancellation> ReservationCancellations = new List<ReservationCancellation>();
            ReservationCancellations = GetAll().Where(t => t.AccommodationId == accommodationId && t.CancelDate.Year == year).ToList();
            foreach (ReservationCancellation reservationCancellation in ReservationCancellations)
                foreach (AccommodationStatisticsByMonth AccommodationStatisticsByMonth in AccommodationStatisticsByMonths)
                    if (AccommodationStatisticsByMonth.Month == reservationCancellation.CancelDate.Month)
                        AccommodationStatisticsByMonth.Cancellations++;
        }
    }
}
