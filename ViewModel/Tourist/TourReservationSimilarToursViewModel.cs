using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Repository;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.IRepositories;
using BookingApp.Services;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReservationSimilarToursViewModel
    {
        public TourReservationSimilarTours TourReservationSimilarTours { get; set; }
        public Tour SelectedTour { get; set; }
        public List<Tour> Tours { get; set; }
        public TourReservationSimilarToursViewModel(TourReservationSimilarTours tourReservationSimilarTours,Tour selectedTour)
        { 
            this.TourReservationSimilarTours = tourReservationSimilarTours;

            SelectedTour = selectedTour;

            Tours = new List<Tour>();
            List<Tour> ToursAll = new List<Tour>();

            List<KeyPoint> keyPointsForward = new List<KeyPoint>();
            List<Image> imagesForward = new List<Image>();

            List<TourSchedule> Schedules = TourScheduleService.GetInstance().GetAll();
            List<Tour> IndividualTours = TourService.GetInstance().GetAll();
            List<KeyPoint> keyPoints = KeyPointService.GetInstance().GetAll();
            List<Location> locations = LocationService.GetInstance().GetAll();
            List<TourImage> tourImages = TourImageService.GetInstance().GetAll();
            List<Image> images = ImageService.GetInstance().GetAll();

            foreach (Tour tour in IndividualTours)
            {
                foreach (TourSchedule tourSchedule in Schedules)
                {
                    if (tour.Id == tourSchedule.TourId)
                    {
                        if (tour.Id == SelectedTour.Id && tourSchedule.Date == SelectedTour.DateTime)
                        {
                            continue;
                        }
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
                        foreach (Location location in locations)
                        {
                            if (location.Id == tour1.LocationId)
                            {
                                tour1.Location = location;
                            }
                        }

                        //injecting keypoints
                        foreach (KeyPoint keyPoint in keyPoints)
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
                        foreach (TourImage tourImage in tourImages)
                        {
                            if (tourImage.TourId == tour1.Id)
                            {
                                foreach (Image image in images)
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

                        ToursAll.Add(tour1);
                    }
                }
            }

            foreach (Tour t in ToursAll)
            {
                if (t.Location != null && selectedTour.Location != null)
                {

                    if (t.Location.State == selectedTour.Location.State && t.Location.City == selectedTour.Location.City)
                    {
                        Tours.Add(t);
                    }

                }
            }



            TourReservationSimilarTours.TourTextBlock.Text = "\"" + SelectedTour.Name + "\"";
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            TourReservationSimilarTours.Close();
        }
    }
}
