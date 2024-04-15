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
    public class TourReviewImageService
    {
        public ITourReviewImageRepository TourReviewImageRepository {get;set;}
        public TourReviewImageService(ITourReviewImageRepository tourReviewImageRepository) {
            TourReviewImageRepository = tourReviewImageRepository;
                }
        public static TourReviewImageService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourReviewImageService>();
        }
        public void Add(TourReviewImage newImage)
        {
            TourReviewImageRepository.Add(newImage);
        }
        public List<TourReviewImage> GetAll()
        {
            return TourReviewImageRepository.GetAll();
        }
    }
}
