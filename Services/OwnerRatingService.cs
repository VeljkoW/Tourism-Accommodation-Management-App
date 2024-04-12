using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class OwnerRatingService
    {
        private IOwnerRatingRepository ownerRatingRepository = OwnerRatingRepository.GetInstance();
        public OwnerRatingService() { }

        public static OwnerRatingService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerRatingService>();
        }
        public void Add(OwnerRating ownerRating)
        {
            ownerRatingRepository.Add(ownerRating);
        }

        public List<OwnerRating> GetAll()
        {
            return ownerRatingRepository.GetAll();
        }

        public OwnerRating? GetById(int Id)
        {
            return ownerRatingRepository.GetById(Id);
        }

    }
}
