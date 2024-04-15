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
        private IOwnerRatingRepository OwnerRatingRepository {get;set;}
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

    }
}
