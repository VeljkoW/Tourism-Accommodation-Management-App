using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/tourreservations.csv";

        private readonly Serializer<TourReservation> _serializer;

        private List<TourReservation> _tourReservations;

        public TourReservationRepository()
        {
            _serializer = new Serializer<TourReservation>();
            _tourReservations = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            if (_tourReservations.Count < 1)
            {
                return 1;
            }
            return _tourReservations.Max(c => c.Id) + 1;
        }
        public TourReservation Add(TourReservation newTourReservation)
        {
            newTourReservation.Id = NextId();
            _tourReservations.Add(newTourReservation);
            _serializer.ToCSV(FilePath, _tourReservations);
            return newTourReservation;
        }
        public List<TourReservation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourReservation? GetById(int Id)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            return _tourReservations.Find(c => c.Id == Id);
        }
        public TourReservation? GetByScheduleId(int Id)
        {
            _tourReservations = _serializer.FromCSV(FilePath);
            return _tourReservations.Find(c => c.TourScheduleId == Id);
        }
    }
}
