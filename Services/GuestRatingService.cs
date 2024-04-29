using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace BookingApp.Services
{
    public class GuestRatingService
    {
        private IGuestRatingRepository GuestRatingRepository { get; set; }
        public GuestRatingService(IGuestRatingRepository guestRatingRepository) { GuestRatingRepository = guestRatingRepository; }
        public static GuestRatingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestRatingService>();
        }
        public void Add(GuestRating newGuestRating)
        {
            GuestRatingRepository.Add(newGuestRating);
        }

        public List<GuestRating> GetAll()
        {
            return GuestRatingRepository.GetAll();
        }

        public ObservableCollection<GuestRating> Update(User user)
        {
            ObservableCollection<GuestRating> guestRatings = new ObservableCollection<GuestRating>();
            foreach(GuestRating guestRating in GetAll())
            {
                if(guestRating.GuestId == user.Id)
                    if(IsOwnerRated(guestRating))
                        guestRatings.Add(guestRating);
            }
            return guestRatings;
        }

        public bool IsOwnerRated(GuestRating guestRating)
        {
            foreach (OwnerRating ownerRating in OwnerRatingService.GetInstance().GetAll())
            {
                if (guestRating.ownerId == ownerRating.ownerId && guestRating.guestId == ownerRating.guestId)
                {
                    return true;
                }
            }
            return false;
        }

        public double GetAverageGrade(User user)
        {
            ObservableCollection<GuestRating> ratings = GuestRatingService.GetInstance().Update(user);
            double AverageGrade = 0;
            foreach (GuestRating guestRating in ratings)
            {
                AverageGrade += (double)(guestRating.Cleanliness + guestRating.FollowingGuidelines) / 2;
            }
            AverageGrade /= ratings.Count();
            return AverageGrade;
        }

        public bool IsSuperGuest(User user)
        {
            int numberOfReservation = 0;
            foreach(ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
            {
                if (user.Id == reservedAccommodation.GuestId && DateTime.Now > reservedAccommodation.checkInDate && DateTime.Now.AddYears(-1) < reservedAccommodation.checkInDate)
                    numberOfReservation++;
            }

            if (numberOfReservation >= 10)
                return true;
            
            return false;
        }

        public int GetBonus(User user)
        {
            int numberOfBonus = 0;
            foreach(GuestBonus guestBonus in GuestBonusService.GetInstance().GetAll())
            {
                if(guestBonus.GuestId == user.Id)
                {
                    numberOfBonus = guestBonus.Bonus;
                }
            }
            return numberOfBonus;
        }
    }
}
