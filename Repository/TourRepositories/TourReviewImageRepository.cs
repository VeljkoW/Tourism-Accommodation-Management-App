using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourReviewImageRepository : ITourReviewImageRepository
    {
        public static TourReviewImageRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReviewImageRepository>();
        }
        private const string FilePath = "../../../Resources/Data/tourreviewimages.csv";

        private readonly Serializer<TourReviewImage> _serializer;

        private List<TourReviewImage> _tourReviewImages;
        public TourReviewImageRepository()
        {
            _serializer = new Serializer<TourReviewImage>();
            _tourReviewImages = _serializer.FromCSV(FilePath);
        }
        public void Add(TourReviewImage newTourReviewImage)
        {
            _tourReviewImages.Add(newTourReviewImage);
            _serializer.ToCSV(FilePath, _tourReviewImages);
        }
        public List<TourReviewImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
