using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourimages.csv";

        private readonly Serializer<TourImage> _serializer;

        private List<TourImage> _tourImages;
        public TourImageRepository()
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
