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
    public class TourService
    {
        private TourRepository tourRepository = TourRepository.GetInstance();
        public TourService() { }
        public static TourService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourService>();
        }
        public Tour Add(Tour newTour)
        {
            return tourRepository.Add(newTour);
        }

        public List<Tour> GetAll()
        {
            return tourRepository.GetAll();
        }

        public Tour? GetById(int Id)
        {
            return tourRepository.GetById(Id);
        }
    }
}
