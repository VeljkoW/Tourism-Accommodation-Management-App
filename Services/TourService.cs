using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Services
{
    public class TourService
    {
        public TourScheduleService tourScheduleService = TourScheduleService.GetInstance();
        public TourImageService tourImageService = TourImageService.GetInstance();
        public ImageService imageService = ImageService.GetInstance();
        public KeyPointService keyPointService = KeyPointService.GetInstance();
        public LocationService locationService = LocationService.GetInstance();

        public ITourRepository TourRepository { get; set; }
        public TourService(ITourRepository TourRepositorya) {
            this.TourRepository= TourRepositorya;
        }
        public static TourService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourService>();
        }
        public Tour Add(Tour newTour)
        {
            return TourRepository.Add(newTour);
        }

        public List<Tour> GetAll()
        {
            return TourRepository.GetAll();
        }

        public Tour? GetById(int Id)
        {
            return TourRepository.GetById(Id);
        }
        public Dictionary<TourSchedule, Tour> LoadToursForGuide(User user)
        {
            Dictionary<TourSchedule, Tour> t = new Dictionary<TourSchedule, Tour>();
            List<TourSchedule> schedules = tourScheduleService.GetAll();
            List<KeyPoint> keyPoints = keyPointService.GetAll();
            List<TourImage> tourImages = tourImageService.GetAll();

            foreach (TourSchedule schedule in schedules)
            {
                if (schedule.ScheduleStatus == ScheduleStatus.Finished || schedule.ScheduleStatus == ScheduleStatus.Canceled || schedule.Date.Date < DateTime.Now.Date){
                    continue;}
                Tour tour = GetById(schedule.TourId);
                if (tour.OwnerId != user.Id){
                    continue;}
                tour.DateTime = schedule.Date;
                tour.Location = locationService.GetById(tour.LocationId);
                foreach (KeyPoint kp in keyPoints){
                    if (kp.TourId == tour.Id){
                        tour.KeyPoints.Add(kp);
                    }
                }
                foreach (TourImage ti in tourImages){
                    if (ti.TourId == schedule.TourId){
                        tour.Images.Add(imageService.GetById(ti.ImageId));
                    }
                }
                t.Add(schedule, tour);
            }
            return t;
        }
        public void HandoutCoupons(int scheduleId)
        {
            TourReservation? tr =TourReservationService.GetInstance().GetByScheduleId(scheduleId);
            if(tr != null)
            {
                TourCoupon tourCoupon = new TourCoupon(tr.UserId, "FIX THIS PLS IN TOURCUPON ADD", "Coupon awarded because the guide has canelled the tour", DateTime.Now, 12, CouponStatus.Valid);
                TourCouponService.GetInstance().Add(tourCoupon);
                TourSchedule? t = TourScheduleService.GetInstance().GetById(scheduleId);
                if(t != null)
                {
                t.ScheduleStatus = ScheduleStatus.Canceled;
                TourScheduleService.GetInstance().Update(t);
                }
            }
            else
            {
                TourSchedule? t = TourScheduleService.GetInstance().GetById(scheduleId);
                if (t != null)
                {
                    t.ScheduleStatus = ScheduleStatus.Canceled;
                    TourScheduleService.GetInstance().Update(t);
                }
            }
        }
        //TODO: get data for statistics
        public Dictionary<Tour,TourStatistics> GetTourStatistics(int year=0)
        {
            Dictionary < Tour,TourStatistics > ret=new Dictionary<Tour,TourStatistics >();

            foreach (Tour tour in GetAll())
            {
                int underage = 0;
                int adult = 0;
                int elderly = 0;
                List<TourSchedule> tourSchedules = TourScheduleService.GetInstance().GetScheduleByTourId(tour.Id).Where(t=> t.ScheduleStatus == ScheduleStatus.Finished).ToList();
                if(year >0)
                {
                    tourSchedules = tourSchedules.Where(t=>t.Date.Year == year).ToList();
                }
                List<TourReservation> tourReservations =TourScheduleService.GetInstance().GetReservationsFromSchedules(tourSchedules);
                foreach(TourReservation tourReservation in tourReservations)
                {
                    underage += TourPersonService.GetInstance().GetUnderageCount(tourReservation.People);
                    adult += TourPersonService.GetInstance().GetAdultCount(tourReservation.People);
                    elderly += TourPersonService.GetInstance().GetElderlyCount(tourReservation.People);
                }
                TourStatistics tourStatistics = new TourStatistics(underage, adult, elderly);
                if (underage + adult + elderly > 0)
                {
                    ret.Add(tour, tourStatistics);
                }
            }
            return ret;
        }
        public Tour MakeTour(Tour tour, TourSchedule tourSchedule)
        {
            List<KeyPoint> keyPointsForward = new List<KeyPoint>();
            List<Image> imagesForward = new List<Image>();
            Tour tour1 = new Tour();

            tour1.DateTime = tourSchedule.Date;
            tour1.OwnerId = tour.OwnerId;
            tour1.Name = tour.Name;
            tour1.Description = tour.Description;
            tour1.Duration = tour.Duration;
            tour1.Id = tour.Id;
            tour1.LocationId = tour.LocationId;
            tour1.Language = tour.Language;
            tour1.MaxTourists = tour.MaxTourists;

            //injecting locations
            foreach (Location location in LocationService.GetInstance().GetAll())
            {
                if (location.Id == tour1.LocationId)
                {
                    tour1.Location = location;
                }
            }

            //injecting keypoints
            foreach (KeyPoint keyPoint in KeyPointService.GetInstance().GetAll())
            {
                if (keyPoint.TourId == tour1.Id)
                {
                    KeyPoint keyPoint1 = new KeyPoint();
                    keyPoint1.Id = keyPoint.Id;
                    keyPoint1.TourId = keyPoint.TourId;
                    keyPoint1.Point = keyPoint.Point;
                    keyPoint1.IsVisited = keyPoint.IsVisited;

                    keyPointsForward.Add(keyPoint1);
                }
            }

            tour1.KeyPoints = keyPointsForward;
            keyPointsForward = new List<KeyPoint>();

            //injecting images
            foreach (TourImage tourImage in TourImageService.GetInstance().GetAll())
            {
                if (tourImage.TourId == tour1.Id)
                {
                    foreach (Image image in ImageService.GetInstance().GetAll())
                    {
                        if (image.Id == tourImage.ImageId)
                        {
                            Image image1 = new Image();
                            image1.Id = image.Id;
                            image1.Path = image.Path;

                            imagesForward.Add(image1);
                        }
                    }
                }
            }
            tour1.Images = imagesForward;
            imagesForward = new List<Image>();
            return tour1;
        }
    }
}