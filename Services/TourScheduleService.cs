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
        public ITourScheduleRepository TourScheduleRepository {get;set;}
        public TourScheduleService(ITourScheduleRepository tourScheduleRepository) {
            TourScheduleRepository=tourScheduleRepository;
        }
        public static TourScheduleService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourScheduleService>();
        }
        public void Add(TourSchedule newTourSchedule)
        {
            TourScheduleRepository.Add(newTourSchedule);
        }

        public List<TourSchedule> GetAll()
        {
            return TourScheduleRepository.GetAll();
        }

        public TourSchedule? GetById(int Id)
        {
            return TourScheduleRepository.GetById(Id);
        }
        public TourSchedule? Update(TourSchedule tourSchedule)
        {
            return TourScheduleRepository.Update(tourSchedule);
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
        public List<TourSchedule> GetFinishedTourSchedulesThatUserAttended(int userId)
        {
            DateTime currentDate = DateTime.Now;
            DateTime oneYearAgo = currentDate.AddYears(-1);

            List<TourSchedule> finishedTourSchedules = GetAll().Where(t => t.ScheduleStatus == ScheduleStatus.Finished).ToList();
            List<TourSchedule> tourSchedules = new List<TourSchedule>();
            List<TourReservation> tourReservations = TourReservationService.GetInstance().GetReservationsByUserId(userId);
            foreach (TourSchedule tourSchedule in finishedTourSchedules)
            {
                if (tourSchedule.Date >= oneYearAgo && tourSchedule.Date <= currentDate)    //checks only the tours in the past year
                {
                    bool attended = false;

                    foreach (TourReservation tourReservation in tourReservations)
                    {
                        if (tourReservation.TourScheduleId == tourSchedule.Id)
                        {
                            foreach (TourPerson tourPerson in tourReservation.People)
                            {
                                if (tourPerson.KeyPointId != -1)
                                {
                                    attended = true;
                                    break;
                                }
                            }
                        }
                        if (attended)
                        {
                            break;
                        }
                    }
                    if (attended)
                    {
                        tourSchedules.Add(tourSchedule);
                    }
                }
            }
            return tourSchedules;
        }
        public List<TourSchedule> GetSchedulesForCouponAwards(int userId)
        {
            List<TourSchedule> tourSchedules = new List<TourSchedule>();

            foreach(TourSchedule tourSchedule in GetFinishedTourSchedulesThatUserAttended(userId))
            {
                bool exists = false;
                foreach(TourCouponAward tourCouponAward in TourCouponAwardService.GetInstance().GetAll())
                {
                    foreach(TourSchedule tourSchedule1 in tourCouponAward.AttendedSchedules)
                    {
                        if(tourSchedule1.Id == tourSchedule.Id)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if(exists)
                    {
                        break;
                    }
                }
                if(!exists)
                {
                    tourSchedules.Add(tourSchedule);
                }
            }
            return tourSchedules;
        }
    }
}
