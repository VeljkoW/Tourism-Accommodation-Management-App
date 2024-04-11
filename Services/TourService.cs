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
        public TourScheduleService tourScheduleService = TourScheduleService.GetInstance();
        public TourImageService tourImageService = TourImageService.GetInstance();
        public ImageService imageService = ImageService.GetInstance();
        public KeyPointService keyPointService = KeyPointService.GetInstance();
        public LocationService locationService = LocationService.GetInstance();

        private ITourRepository tourRepository = TourRepository.GetInstance();
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
        public Dictionary<TourSchedule, Tour> LoadToursForGuide(User user)
        {
            Dictionary<TourSchedule, Tour> t = new Dictionary<TourSchedule, Tour>();
            t.Clear();
            List<TourSchedule> schedules = tourScheduleService.GetAll();
            List<KeyPoint> keyPoints = keyPointService.GetAll();
            List<TourImage> tourImages = tourImageService.GetAll();
            List<Image> images = imageService.GetAll();

            foreach (TourSchedule schedule in schedules)
            {
                if (schedule.ScheduleStatus == ScheduleStatus.Finished || schedule.Date.Date != DateTime.Now.Date)
                {
                    continue;
                }
                Tour tour = new Tour();
                tour = GetById(schedule.TourId);
                if (tour.OwnerId != user.Id)
                {
                    continue;
                }
                tour.DateTime = schedule.Date;
                tour.Location = locationService.GetById(tour.LocationId);
                foreach (KeyPoint kp in keyPoints)
                {
                    if (kp.TourId == tour.Id)
                    {
                        tour.KeyPoints.Add(kp);
                    }
                }
                foreach (TourImage ti in tourImages)
                {
                    if (ti.TourId == schedule.TourId)
                    {
                        tour.Images.Add(imageService.GetById(ti.ImageId));
                    }
                }
                t.Add(schedule, tour);
            }
            return t;
        }
    }
}
