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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for GuideMainPage.xaml
    /// </summary>
    public partial class GuideMainPage : Page
    {
        public TourRepository tourRepository = new TourRepository();
        public TourScheduleRepository scheduleRepository = new TourScheduleRepository();
        public TourImageRepository tourImageRepository = new TourImageRepository();
        public ImageRepository imageRepository = new ImageRepository();
        public KeyPointRepository keyPointRepository = new KeyPointRepository();
        public LocationRepository locationRepository = new LocationRepository();
        public User User { get; set; }
        public string UserName { get; set; }
        public List<Tour> Tours { get; set; }
        public List<UserControlTourCard> SelectedItems { get; set; } = new List<UserControlTourCard>();
        public GuideMainPage(User user)
        {
            InitializeComponent();
            User = user;
            UserName = user.Username;
            DataContext = this;
            LoadTours();

        }
        public void LoadTours()
        {
            List<TourSchedule> schedules = new List<TourSchedule>();
            List<KeyPoint> keypoints = keyPointRepository.GetAll();
            List<Image> images = imageRepository.GetAll();
            List<TourImage> tourImages = tourImageRepository.GetAll();
            schedules = scheduleRepository.GetAll();
            Tours = new List<Tour>();
            foreach (TourSchedule schedule in schedules)
            {
                if(schedule.Date.Date != DateTime.Now.Date)
                {
                    continue;
                }
                Tour tour = new Tour();
                tour = tourRepository.GetById(schedule.TourId);
                tour.DateTime = schedule.Date;
                tour.Location = locationRepository.GetById(tour.LocationId);
                foreach (KeyPoint kp in keypoints)
                {
                    if (kp.TourId == tour.Id)
                    {
                        tour.KeyPoints.Add(kp);
                    }
                }
                foreach(TourImage ti in tourImages)
                {
                    if(ti.TourId == schedule.TourId)
                    {
                        tour.Images.Add(imageRepository.GetById(ti.ImageId));
                    }
                }
                Tours.Add(tour);
            }
            ListOfTours.Children.Clear();
            foreach (Tour t in Tours)
            {
                UserControlTourCard userControlTourCard = new UserControlTourCard(t,User);
                userControlTourCard.Margin = new Thickness(0, 0, 0, 15);
                ListOfTours.Children.Add(userControlTourCard);
            }
        }

        private void ClickCreateTour(object sender, RoutedEventArgs e)
        {
            CreateTourForm createTourForm = new CreateTourForm();
            NavigationService.Navigate(createTourForm);
            LoadTours();
        }

        private void ClickMonitorTour(object sender, RoutedEventArgs e)
        {

        }
    }
}
