using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Owner;
using BookingApp.View.Guest.Pages;

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
            ownerRating.Id = NextId();
            _ownerRatings.Add(ownerRating);
            _serializer.ToCSV(FilePath, _ownerRatings);
        }
        public int NextId()
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            if (_ownerRatings.Count < 1)
            {
                return 1;
            }
            return _ownerRatings.Max(c => c.Id) + 1;
        }
        public OwnerRating? GetById(int Id)
        {
            _ownerRatings = _serializer.FromCSV(FilePath);
            return _ownerRatings.Find(c => c.Id == Id);
        }
        public List<OwnerRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
