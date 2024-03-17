using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;
        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }
        public void Add(Accommodation newAccommodation)
        {
            newAccommodation.Id = NextId();
            _accommodations.Add(newAccommodation);
            _serializer.ToCSV(FilePath, _accommodations);
        }
        public List<Accommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public Accommodation? GetById(int Id)
        {
            _accommodations = _serializer.FromCSV(FilePath);
            return _accommodations.Find(c => c.Id == Id);
        }

    }
}
