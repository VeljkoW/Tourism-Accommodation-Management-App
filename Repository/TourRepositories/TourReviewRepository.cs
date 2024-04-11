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
    public class TourReviewRepository : ITourReviewRepository
    { 
        public static TourReviewRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReviewRepository>();
        }
        private const string FilePath = "../../../Resources/Data/tourreviews.csv";

        private readonly Serializer<TourReview> _serializer;

        private List<TourReview> _tourReviews;

        public TourReviewRepository()
        {
            _serializer = new Serializer<TourReview>();
            _tourReviews = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourReviews = _serializer.FromCSV(FilePath);
            if (_tourReviews.Count < 1)
            {
                return 1;
            }
            return _tourReviews.Max(c => c.Id) + 1;
        }
        public TourReview Add(TourReview newTourReview)
        {
            newTourReview.Id = NextId();
            _tourReviews.Add(newTourReview);
            _serializer.ToCSV(FilePath, _tourReviews);
            return newTourReview;
        }
        public List<TourReview> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourReview? GetById(int Id)
        {
            _tourReviews = _serializer.FromCSV(FilePath);
            return _tourReviews.Find(c => c.Id == Id);
        }
    }
}
