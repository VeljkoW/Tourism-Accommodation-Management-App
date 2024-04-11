using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Owner;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class OwnerRatingRepository : IOwnerRatingRepository
    {

        public static OwnerRatingRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerRatingRepository>();
        }
        private const string FilePath = "../../../Resources/Data/ownerRating.csv";

        private readonly Serializer<OwnerRating> _serializer;

        private List<OwnerRating> _ownerRatings;
        public OwnerRatingRepository()
        {
            _serializer = new Serializer<OwnerRating>();
            _ownerRatings = _serializer.FromCSV(FilePath);
        }
        public void Add(OwnerRating ownerRating)
        {
            _ownerRatings.Add(ownerRating);
            _serializer.ToCSV(FilePath, _ownerRatings);
        }

        public List<OwnerRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
