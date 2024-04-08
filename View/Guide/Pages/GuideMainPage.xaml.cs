using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
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
using Image = BookingApp.Domain.Model.Image;

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
        public ImageService imageService = ImageService.GetInstance();
        public KeyPointRepository keyPointRepository = new KeyPointRepository();
        public LocationRepository locationRepository = new LocationRepository();
        public EventHandler? ListUpdater {  get; set; }
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
        private void ClickCreateTour(object sender, RoutedEventArgs e)
        {
            CreateTourForm createTourForm = new CreateTourForm(User);
            NavigationService.Navigate(createTourForm);
            LoadTours();
        }
        // ide u servis
        public void LoadTours()
        {
            //List<TourSchedule> schedules = new List<TourSchedule>();
            List<KeyPoint> keypoints = keyPointRepository.GetAll();
            //List<Image> images = imageRepository.GetAll();
            List<TourImage> tourImages = tourImageRepository.GetAll();
            List<TourSchedule> schedules = scheduleRepository.GetAll();
            Tours = new List<Tour>();
            Tours.Clear();
            ListOfTours.Children.Clear();
            foreach (TourSchedule schedule in schedules)
            {
                if(schedule.ScheduleStatus == ScheduleStatus.Finished || schedule.Date.Date != DateTime.Now.Date)
                {
                    continue;
                }
                Tour tour = new Tour();
                tour = tourRepository.GetById(schedule.TourId);
                if(tour.OwnerId != User.Id)
                {
                    continue;
                }
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
                        tour.Images.Add(imageService.GetById(ti.ImageId));
                    }
                }
                Tours.Add(tour);
                UserControlTourCard userControlTourCard = new UserControlTourCard(tour, User,schedule);
                CreateTourCard(tour, schedule);
                if (schedule.ScheduleStatus == ScheduleStatus.Ongoing)
                {
                    Tours.Clear();
                    ListOfTours.Children.Clear();
                    ListOfTours.Children.Add(userControlTourCard);
                    break;
                }
            }
        }
        private void CreateTourCard(Tour tour,TourSchedule schedule)
        {
            UserControlTourCard userControlTourCard = new UserControlTourCard(tour, User, schedule);
            userControlTourCard.Margin = new Thickness(0, 0, 0, 15);
            userControlTourCard.OnFinishedTour += UserControlTourCard_OnFinishedTour;
            userControlTourCard.OnClickedGoBackMonitoringTour += UserControlTourCard_OnFinishedTour;
            ListOfTours.Children.Add(userControlTourCard);
        }

        private void UserControlTourCard_OnFinishedTour(object? sender, EventArgs e)
        {
            LoadTours();
        }

        private void ClickLogout(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            OnLogoutHandler?.Invoke(sender, e);
            signInForm.Show();
        }
        public EventHandler OnLogoutHandler { get; set; }
    }
}
