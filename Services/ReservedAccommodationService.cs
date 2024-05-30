using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using GuestRatingPage = BookingApp.View.Owner.GuestRating;
using BookingApp.View.Guest.Pages;
using System.Collections;

namespace BookingApp.Services
{
    public class ReservedAccommodationService
    {
        public IReservedAccommodationRepository ReservedAccommodationRepository { get; set; }
        public ReservedAccommodationService(IReservedAccommodationRepository reservedAccommodationRepository) {
            ReservedAccommodationRepository = reservedAccommodationRepository;
        }
        public static ReservedAccommodationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservedAccommodationService>();
        }
        public void Add(ReservedAccommodation newReservedAccommodation)
        {
            ReservedAccommodationRepository.Add(newReservedAccommodation);
        }

        public List<ReservedAccommodation>? GetByAccommodationId(int Id)
        {
            return ReservedAccommodationRepository.GetByAccommodationId(Id);
        }

        public ReservedAccommodation? GetById(int Id)
        {
            return ReservedAccommodationRepository.GetById(Id);
        }
        public List<ReservedAccommodation> GetAll()
        {
            return ReservedAccommodationRepository.GetAll();
        }

        public void Delete(ReservedAccommodation reservedAccommodation)
        {
            ReservedAccommodationRepository.Delete(reservedAccommodation);
        }
        public void UpdateDatesByReschedulingRequest(GuestReschedulingRequest GuestReschedulingRequest)
        {
            ReservedAccommodationRepository.UpdateDatesByReschedulingRequest(GuestReschedulingRequest);
        }

        public ObservableCollection<ReservedAccommodation> Update(User user)
        {
            ObservableCollection<ReservedAccommodation> reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            foreach (ReservedAccommodation reserved in GetAll())
            {
                if (user.Id == reserved.GuestId) reservedAccommodations.Add(reserved);
            }
            return reservedAccommodations;
        }
        public ObservableCollection<ReservedAccommodation> NotificationUpdate(User user, ObservableCollection<ReservedAccommodation> ReservedAccommodations)
        {
            ReservedAccommodations.Clear();
            List<Accommodation> accommodations = AccommodationService.GetInstance().GetAll().Where(t => t.OwnerId == user.Id).ToList();
            foreach (ReservedAccommodation tempReservedAccommodation in GetAll())
                foreach (Accommodation accommodation in accommodations)
                    if (tempReservedAccommodation.Accommodation.Id == accommodation.Id)
                    {
                        if (GuestRatingService.GetInstance().GetAll().Count == 0)
                            AvailableForRating(tempReservedAccommodation, ReservedAccommodations);
                        else
                        {
                            if (!IsAlreadyRated(tempReservedAccommodation, user))
                                AvailableForRating(tempReservedAccommodation, ReservedAccommodations);
                        }
                    }
            return ReservedAccommodations;
        }
        public bool IsAlreadyRated(ReservedAccommodation tempReservedAccommodation, User user)
        {
            foreach (GuestRatingModel GuestRatingModel in GuestRatingService.GetInstance().GetAll())
                if (GuestRatingModel.GuestId == tempReservedAccommodation.GuestId && GuestRatingModel.OwnerId == user.Id)
                    return true;
            return false;
        }
        public void AvailableForRating(ReservedAccommodation ReservedAccommodation, ObservableCollection<ReservedAccommodation> ReservedAccommodations)
        {
            if ((DateTime.Now > ReservedAccommodation.CheckOutDate) &&
                (DateTime.Now - ReservedAccommodation.CheckOutDate).Days <= 5)
                ReservedAccommodations.Add(ReservedAccommodation);
        }
        public void ReservationCountByYear(int accommodationId, ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            List<ReservedAccommodation> ReservedAccommodations = new List<ReservedAccommodation>();
            ReservedAccommodations = GetAll().Where(t=>t.Accommodation.Id == accommodationId).ToList();
            foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodations)
            {
                if (AccommodationStatisticsByYears.Count == 0)
                    AddAccommodationStatisticsByYear(reservedAccommodation, AccommodationStatisticsByYears);
                else
                {
                    bool alreadyExists = false;
                    foreach(AccommodationStatisticsByYear AccommodationStatisticsByYear in AccommodationStatisticsByYears)
                        if(AccommodationStatisticsByYear.Year == reservedAccommodation.CheckInDate.Year)
                        {
                            AccommodationStatisticsByYear.Reservations++;
                            alreadyExists = true;
                            break;
                        }
                    if (!alreadyExists)
                        AddAccommodationStatisticsByYear(reservedAccommodation, AccommodationStatisticsByYears);
                }
            }
        }
        private void AddAccommodationStatisticsByYear(ReservedAccommodation reservedAccommodation, 
                                                      ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            AccommodationStatisticsByYear AccommodationStatisticsByYear = new AccommodationStatisticsByYear();
            AccommodationStatisticsByYear.AccommodationId = reservedAccommodation.Accommodation.Id;
            AccommodationStatisticsByYear.Year = reservedAccommodation.CheckInDate.Year;
            AccommodationStatisticsByYear.Reservations++;
            AccommodationStatisticsByYears.Add(AccommodationStatisticsByYear);
        }
        public void ReservationCountByMonth(int year, int accommodationId, ObservableCollection<AccommodationStatisticsByMonth> AccommodationStatisticsByMonths)
        {
            List<ReservedAccommodation> ReservedAccommodations = new List<ReservedAccommodation>();
            ReservedAccommodations = GetAll().Where(t => t.Accommodation.Id == accommodationId && t.CheckInDate.Year == year).ToList();
            foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodations)
                foreach (AccommodationStatisticsByMonth AccommodationStatisticsByMonth in AccommodationStatisticsByMonths)
                    if (AccommodationStatisticsByMonth.Month == reservedAccommodation.CheckInDate.Month)
                        AccommodationStatisticsByMonth.Reservations++;
        }

        public void SortAccommodationStatisticsByLocation(ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations)
        {
            ReservationCountByLocation(AccommodationsStatisticsByLocations);
            BusynessByLocation(AccommodationsStatisticsByLocations);
            Dictionary<int, int> LocationsByPopularity = new Dictionary<int, int>();
            foreach (AccommodationsStatisticsByLocation accommodationsStatisticsByLocation in AccommodationsStatisticsByLocations)
            {
                LocationsByPopularity.Add(accommodationsStatisticsByLocation.LocationId, 0);
            }

            var sortedListByReservations = AccommodationsStatisticsByLocations.OrderByDescending(x => x.Reservations).ToList();
            for(int i=0; i<sortedListByReservations.Count(); i++)
            {
                LocationsByPopularity[sortedListByReservations[i].LocationId] += i + 1;
            }

            var sortedListByBusyness = AccommodationsStatisticsByLocations.OrderByDescending(x => x.Busyness).ToList();
            for (int i = 0; i < sortedListByBusyness.Count(); i++)
            {
                LocationsByPopularity[sortedListByBusyness[i].LocationId] += i + 1;
            }

            ObservableCollection<AccommodationsStatisticsByLocation> TempAccommodationsStatisticsByLocations = new ObservableCollection<AccommodationsStatisticsByLocation>();
            var sortedByValueDescending = LocationsByPopularity.OrderBy(kvp => kvp.Value).ToList();
            foreach (var kvp in sortedByValueDescending)
            {
                var temporaryAccommodationStatistics = AccommodationsStatisticsByLocations.ToList().Find(t => t.LocationId == kvp.Key);
                TempAccommodationsStatisticsByLocations.Add(temporaryAccommodationStatistics);
            }
            AccommodationsStatisticsByLocations.Clear();
            foreach(var tempItem in TempAccommodationsStatisticsByLocations)
            {
                AccommodationsStatisticsByLocations.Add(tempItem);
            }
        }
        private void ReservationCountByLocation(ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations)
        {
            foreach (AccommodationsStatisticsByLocation accommodationsStatisticsByLocation in AccommodationsStatisticsByLocations)
                foreach (ReservedAccommodation reservedAccommodation in GetAll())
                    if (accommodationsStatisticsByLocation.Accommodations.Where(t => t.Id == reservedAccommodation.Accommodation.Id).Count() != 0)
                        accommodationsStatisticsByLocation.Reservations++;
        }
        private void BusynessByLocation(ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations)
        {
            foreach (AccommodationsStatisticsByLocation accommodationsStatisticsByLocation in AccommodationsStatisticsByLocations)
            {
                double busyness = 0;
                foreach(Accommodation accommodation in accommodationsStatisticsByLocation.Accommodations)
                {
                    double guestNumberSum = 0;
                    int numberOfReservations = 0;
                    foreach(ReservedAccommodation reservedAccommodation in GetAll())
                    {
                        if(reservedAccommodation.Accommodation.Id == accommodation.Id)
                        {
                            numberOfReservations++;
                            guestNumberSum += reservedAccommodation.GuestNumber;
                        }
                    }
                    if (numberOfReservations == 0)
                        continue;
                    guestNumberSum /= numberOfReservations;
                    busyness += guestNumberSum / accommodation.MaxGuestNumber;
                }
                accommodationsStatisticsByLocation.Busyness = busyness / accommodationsStatisticsByLocation.Accommodations.Count();
            }
        }
    }
}
