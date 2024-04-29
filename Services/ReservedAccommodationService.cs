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
        public ObservableCollection<ReservedAccommodation> NotificationUpdate(User user)
        {
            ObservableCollection<ReservedAccommodation> ReservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            foreach (ReservedAccommodation tempReservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
                    if (tempReservedAccommodation.accommodationId == accommodation.Id && user.Id == accommodation.OwnerId)
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
                if (GuestRatingModel.guestId == tempReservedAccommodation.guestId && GuestRatingModel.ownerId == user.Id)
                    return true;
            return false;
        }
        public void AvailableForRating(ReservedAccommodation ReservedAccommodation, ObservableCollection<ReservedAccommodation> ReservedAccommodations)
        {
            if ((DateTime.Now > ReservedAccommodation.checkOutDate) &&
                (DateTime.Now - ReservedAccommodation.checkOutDate).Days <= 5)
                ReservedAccommodations.Add(ReservedAccommodation);
        }
    }
}
