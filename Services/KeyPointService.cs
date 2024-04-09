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
    public class KeyPointService
    {
        private KeyPointRepository keyPointRepository = KeyPointRepository.GetInstance();
        public KeyPointService() { }
        public static KeyPointService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<KeyPointService>();
        }
        public KeyPoint Add(KeyPoint newKeyPoint)
        {
            return keyPointRepository.Add(newKeyPoint);
        }

        public List<KeyPoint> GetAll()
        {
            return keyPointRepository.GetAll();
        }

        public KeyPoint? GetById(int Id)
        {
            return keyPointRepository.GetById(Id);
        }
    }
}
