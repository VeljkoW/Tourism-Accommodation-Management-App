using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourScheduleRepository
    {
        private const string FilePath = "../../../Resources/Data/tourschedules.csv";

        private readonly Serializer<TourSchedule> _serializer;

        private List<TourSchedule> _tourImages;
        public TourScheduleRepository()
        {
            _serializer = new Serializer<TourSchedule>();
            _tourImages = _serializer.FromCSV(FilePath);
        }
        internal void Add(TourSchedule newTour)
        {
            _tourImages.Add(newTour);
            _serializer.ToCSV(FilePath, _tourImages);
        }
        public List<TourSchedule> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
