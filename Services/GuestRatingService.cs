using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class GuestRatingService
    {
        private GuestRatingRepository guestRatingRepository = GuestRatingRepository.GetInstance();
        public GuestRatingService() { }
        public static GuestRatingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestRatingService>();
        }
        public void Add(GuestRating newGuestRating)
        {
            guestRatingRepository.Add(newGuestRating);
        }

        public List<GuestRating> GetAll()
        {
            return guestRatingRepository.GetAll();
        }
    }
}
