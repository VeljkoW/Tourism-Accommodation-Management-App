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
    public class KeyPointService
    {
        public IKeyPointRepository KeyPointRepository { get; set; }
        public KeyPointService(IKeyPointRepository keyPointRepository) { KeyPointRepository = keyPointRepository; }
        public static KeyPointService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<KeyPointService>();
        }
        public KeyPoint Add(KeyPoint newKeyPoint)
        {
            return KeyPointRepository.Add(newKeyPoint);
        }

        public List<KeyPoint> GetAll()
        {
            return KeyPointRepository.GetAll();
        }

        public KeyPoint? GetById(int Id)
        {
            return KeyPointRepository.GetById(Id);
        }
    }
}
