using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
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
            foreach (OwnerRating ownerRating in OwnerRatingService.GetInstance().GetAll())
            {
                if (userId == ownerRating.ownerId)
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
                if (guestRating.ownerId == ownerRating.ownerId && guestRating.guestId == ownerRating.guestId)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
