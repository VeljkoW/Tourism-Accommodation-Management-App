using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.View.Owner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;

namespace BookingApp.Services
{
    public class OwnerRatingService
    {
        public IOwnerRatingRepository OwnerRatingRepository {get;set;}
        public OwnerRatingService(IOwnerRatingRepository ownerRatingRepository) { OwnerRatingRepository = ownerRatingRepository; }

        public static OwnerRatingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerRatingService>();
        }
        public void Add(OwnerRating ownerRating)
        {
            OwnerRatingRepository.Add(ownerRating);
            //OwnerNotification ownerNotification = new OwnerNotification();
            //ownerNotification.ReservedAccommodationId = reservedAccommodation.Id;
            //ownerNotification.Root = "OwnerRating";
            //OwnerNotificationService.GetInstance().Add(ownerNotification);
        }

        public List<OwnerRating> GetAll()
        {
            return OwnerRatingRepository.GetAll();
        }

        public OwnerRating? GetById(int Id)
        {
            return OwnerRatingRepository.GetById(Id);
        }

        public ObservableCollection<OwnerRating> GetOwnerRatings(int userId)
        {
            ObservableCollection<OwnerRating> OwnerRatings = new ObservableCollection<OwnerRating>();
            foreach (OwnerRating ownerRating in GetAll())
            {
                if (userId == ownerRating.OwnerId)
                {
                    if(IsGuestRated(ownerRating))
                    {
                        OwnerRatings.Add(ownerRating);
                    }
                }
            }
            return OwnerRatings;
        }
        public bool IsGuestRated(OwnerRating ownerRating)
        {
            foreach (GuestRatingModel guestRating in GuestRatingService.GetInstance().GetAll())
            {
                if (guestRating.OwnerId == ownerRating.OwnerId && guestRating.GuestId == ownerRating.GuestId)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
