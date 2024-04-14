using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
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
        private ITourScheduleRepository tourScheduleRepository = TourScheduleRepository.GetInstance();
        public TourScheduleService() { }
        public static TourScheduleService GetInstance()
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
        public List<TourSchedule>? GetScheduleByTourId(int TourId)
        {
            return GetAll().Where(tourSchedule => tourSchedule.TourId == TourId).ToList();
        }
        public List<TourReservation> GetReservationsFromSchedules(List<TourSchedule> tourSchedules)
        {
            var scheduleIds = tourSchedules.Select(s => s.Id).ToList();
            return TourReservationService.GetInstance().GetAll().Where(t => scheduleIds.Contains(t.TourScheduleId)).ToList();
        }
    }
}
