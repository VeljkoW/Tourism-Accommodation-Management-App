using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourReviewService
    {
        private ITourReviewRepository tourReviewRepository = TourReviewRepository.GetInstance();
        public TourReviewService() { }
        public static TourReviewService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReviewService>();
        }
        public TourReview Add(TourReview newTourReview)
        {
            return tourReviewRepository.Add(newTourReview);
        }

        public List<TourReview> GetAll()
        {
            return tourReviewRepository.GetAll();
        }

        public TourReview? GetById(int Id)
        {
            return tourReviewRepository.GetById(Id);
        }
    }
}
