using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class GuestRatingRepository
    {
        private const string FilePath = "../../../Resources/Data/guestRating.csv";

        private readonly Serializer<GuestRating> _serializer;

        private List<GuestRating> _guestRatings;
        public GuestRatingRepository()
        {
            _serializer = new Serializer<GuestRating>();
            _guestRatings = _serializer.FromCSV(FilePath);
        }
        public void Add(GuestRating newGuestRating)
        {
            _guestRatings.Add(newGuestRating);
            _serializer.ToCSV(FilePath, _guestRatings);
        }
        public List<GuestRating> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        /*public int GetUserById(int id)
        {
            _guestRatings = _serializer.FromCSV(FilePath);
            return _guestRatings.Find(c => c.guestId == id);
        }*/
    }
}
