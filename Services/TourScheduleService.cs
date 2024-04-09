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
    public class TourScheduleService
    {
        private TourScheduleRepository tourScheduleRepository = TourScheduleRepository.GetInstance();
        public TourScheduleService() { }
        public TourScheduleService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourScheduleService>();
        }
        public void Add(TourSchedule newTourSchedule)
        {
            tourScheduleRepository.Add(newTourSchedule);
        }

        public List<TourSchedule> GetAll()
        {
            return tourScheduleRepository.GetAll();
        }

        public TourSchedule? GetById(int Id)
        {
            return tourScheduleRepository.GetById(Id);
        }
        public TourSchedule? Update(TourSchedule tourSchedule)
        {
            return tourScheduleRepository.Update(tourSchedule);
        }
    }
}
