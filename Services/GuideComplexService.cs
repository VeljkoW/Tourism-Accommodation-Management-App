using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.View.Guide.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Services
{
    public class GuideComplexService
    {
        public static GuideComplexService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuideComplexService>();
        }

        public TourService tourService = TourService.GetInstance();
        public TourScheduleService tourScheduleService = TourScheduleService.GetInstance();
        public TourImageService tourImageService = TourImageService.GetInstance();
        public ImageService imageService = ImageService.GetInstance();
        public KeyPointService keyPointService = KeyPointService.GetInstance();
        public LocationService locationService = LocationService.GetInstance();

        public GuideComplexService() { }

        public Dictionary<TourSchedule,Tour> LoadTours(User user)
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
                tour = tourService.GetById(schedule.TourId);
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