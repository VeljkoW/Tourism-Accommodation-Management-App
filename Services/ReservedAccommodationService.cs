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

        public ReservedAccommodation? GetByAccommodationId(int Id)
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
            foreach (ReservedAccommodation tempReservedAccommodation in GetAll())
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
                    if (tempReservedAccommodation.Accommodation.Id == accommodation.Id && user.Id == accommodation.OwnerId)
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
                if (GuestRatingModel.GuestId == tempReservedAccommodation.guestId && GuestRatingModel.OwnerId == user.Id)
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
    }
}
