using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReservationSimilarTours.xaml
    /// </summary>
    public partial class TourReservationSimilarTours : Window
    {
        public Tour SelectedTour {  get; set; }
        public List<Tour> Tours {  get; set; }
        public TourRepository tourRepository { get; set; }
        public TourScheduleRepository tourScheduleRepository { get; set; }
        public KeyPointRepository keyPointRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public TourImageRepository tourImageRepository { get; set; }
        public ImageRepository imageRepository {  get; set; }

        public TourReservationSimilarTours(Tour selectedTour)
        {
            InitializeComponent();
            SelectedTour = selectedTour;
            DataContext = this;

            tourRepository = new TourRepository();
            tourScheduleRepository = new TourScheduleRepository();
            keyPointRepository = new KeyPointRepository();

            tourScheduleRepository = new TourScheduleRepository();
            tourRepository = new TourRepository();
            keyPointRepository = new KeyPointRepository();
            locationRepository = new LocationRepository();
            tourImageRepository = new TourImageRepository();
            imageRepository = new ImageRepository();


            Tours = new List<Tour>();
            List<Tour> ToursAll = new List<Tour>();

            List<KeyPoint> keyPointsForward = new List<KeyPoint>();
            List<Image> imagesForward = new List<Image>();

            List<TourSchedule> Schedules = tourScheduleRepository.GetAll();
            List<Tour> IndividualTours =  tourRepository.GetAll();
            List<KeyPoint> keyPoints = keyPointRepository.GetAll();
            List<Location> locations = locationRepository.GetAll();
            List<TourImage> tourImages = tourImageRepository.GetAll();
            List<Image> images = imageRepository.GetAll();

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

            foreach(Tour t in ToursAll)
            {
                if (t.Location != null && selectedTour.Location != null)
                {

                    if (t.Location.State == selectedTour.Location.State && t.Location.City == selectedTour.Location.City)
                    {
                        Tours.Add(t);
                    }

                }
            }



            TourTextBlock.Text = "\"" + SelectedTour.Name + "\"";
        }

        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double SWidth = SystemParameters.PrimaryScreenWidth;
            double SHeight = SystemParameters.PrimaryScreenHeight;
            double WWidth = this.Width;
            double WHeight = this.Height;

            this.Left = (SWidth - WWidth) / 2;
            this.Top = (SHeight - WHeight) / 2;
        }
        public void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
