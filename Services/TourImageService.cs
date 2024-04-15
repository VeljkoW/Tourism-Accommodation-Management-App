using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
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
        public ITourImageRepository TourImageRepository { get; set; }
        public TourImageService(ITourImageRepository tourImageRepository) { TourImageRepository = tourImageRepository; }
        public static TourImageService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourImageService>();
        }
        public void Add(TourImage newImage)
        {
            TourImageRepository.Add(newImage);
        }
        public List<TourImage> GetAll()
        {
            return TourImageRepository.GetAll();
        }
    }
}
