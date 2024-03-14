using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class ReservedAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/reservedAccommodation.csv";

        private readonly Serializer<ReservedAccommodation> _serializer;

        private List<ReservedAccommodation> _reservedAccommodations;

        public ReservedAccommodationRepository()
        {
            _serializer = new Serializer<ReservedAccommodation>();
            _reservedAccommodations = _serializer.FromCSV(FilePath);
        }

        public void Add(ReservedAccommodation newReservedAccommodation)
        {
            _reservedAccommodations.Add(newReservedAccommodation);
            _serializer.ToCSV(FilePath, _reservedAccommodations);
        }
        public List<ReservedAccommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public ReservedAccommodation? GetById(int Id)
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            return _reservedAccommodations.Find(c => c.accommodationId == Id);
        }
    }
}
