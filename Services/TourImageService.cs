using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourImageService
    {
        private TourImageRepository tourImageRepository = TourImageRepository.GetInstance();
        public TourImageService() { }
        public TourImageService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourImageService>();
        }
        public void Add(TourImage newImage)
        {
            tourImageRepository.Add(newImage);
        }
        public List<TourImage> GetAll()
        {
            return tourImageRepository.GetAll();
        }
    }
}
