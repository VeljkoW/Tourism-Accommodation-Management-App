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

        private readonly Serializer<TourImage> _serializer;

        private List<TourImage> _tourImages;
        public TourScheduleRepository()
        {
            _serializer = new Serializer<TourImage>();
            _tourImages = _serializer.FromCSV(FilePath);
        }
        internal void Add(TourImage newTour)
        {
            _tourImages.Add(newTour);
            _serializer.ToCSV(FilePath, _tourImages);
        }
        public List<TourImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
