using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {
        public List<Tour> IndividualTours { get; set; }
        public List<Tour> Tours {  get; set; }
        public List<TourSchedule> Schedules { get; set; }
        public TourRepository tourRepository { get; set; }
        public TourScheduleRepository tourScheduleRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public KeyPointRepository keyPointRepository { get; set; }
        public TourImageRepository tourImageRepository { get; set; }
        public ImageRepository imageRepository { get; set; }
        public User User { get; set; }
        public string Username {  get; set; }
        public TouristMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
            Username = User.Username;

            if(Username.Length > 10)
            {
                Username = Username.Substring(0,10) + "...";    
            }

            tourRepository = new TourRepository();
            tourScheduleRepository = new TourScheduleRepository();
            locationRepository = new LocationRepository();
            keyPointRepository = new KeyPointRepository();
            tourImageRepository = new TourImageRepository();
            imageRepository = new ImageRepository();

            List<Location> locations = new List<Location>();
            List<KeyPoint> keyPoints = new List<KeyPoint>();
            List<KeyPoint> keyPointsForeward = new List<KeyPoint>();
            List<Image> images = new List<Image>();
            List<Image> imagesForeward = new List<Image>();
            List<TourImage> tourImages = new List<TourImage>();

            locations = locationRepository.GetAll();
            keyPoints = keyPointRepository.GetAll();
            images = imageRepository.GetAll();
            tourImages = tourImageRepository.GetAll();

            Tours = new List<Tour>();

            IndividualTours = tourRepository.GetAll();
            Schedules = tourScheduleRepository.GetAll();
            
            foreach(Tour tour in IndividualTours)
            {
                foreach(TourSchedule tourSchedule in Schedules)
                {
                    if(tour.Id == tourSchedule.TourId)
                    {
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
                        foreach(KeyPoint keyPoint in keyPoints)
                        {
                            if(keyPoint.TourId == tour1.Id)
                            {
                                KeyPoint keyPoint1 = new KeyPoint();
                                keyPoint1.Id = keyPoint.Id;
                                keyPoint1.TourId = keyPoint.TourId;
                                keyPoint1.Point = keyPoint.Point;
                                keyPoint1.IsVisited = keyPoint.IsVisited;

                                keyPointsForeward.Add(keyPoint1);
                            }
                        }

                        tour1.KeyPoints = keyPointsForeward;
                        keyPointsForeward = new List<KeyPoint>();

                        //injecting images
                        foreach (TourImage tourImage in tourImages)
                        {
                            if(tourImage.TourId == tour1.Id) 
                            {
                                foreach(Image image in images)
                                {
                                    if(image.Id == tourImage.ImageId)
                                    {
                                        Image image1 = new Image();
                                        image1.Id = image.Id;
                                        image1.Path = image.Path;

                                        imagesForeward.Add(image1);
                                    }
                                }
                            }
                        }
                        tour1.Images = imagesForeward;
                        imagesForeward = new List<Image>();

                        Tours.Add(tour1);
                    }
                }
            }
            

            /*
            Tours = new List<Tour>
            { 
                new Tour(1,"Tour 1",new Location(),"descriptionaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa","lang",100,new List<KeyPoint>(),10,new List<Image>()),
                new Tour(2,"Tour 2",new Location(),"description","lang",100,new List<KeyPoint>(),10,new List<Image>()),
                new Tour(3,"Tour 3",new Location(),"description","lang",100,new List<KeyPoint>(),10,new List<Image>()),
                new Tour(4,"Tour 4",new Location(),"description","lang",5,new List<KeyPoint>(),10,new List<Image>())
            };
            */

            
        }

        private void SearchBoxClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Search tours...")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }

        private void SearchBoxNotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search tours...";
                textBox.Foreground= Brushes.Gray;
            }
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        private void ToursTabClick(object sender, RoutedEventArgs e)
        {
            Tab.SelectedIndex = 0;
        }
        private void ReservationTabClick(object sender, RoutedEventArgs e)
        {
            Tab.SelectedIndex = 1;
        }
        
        private void SuggestionTabClick(object sender, RoutedEventArgs e)
        {
            Tab.SelectedIndex = 2;
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
    }
}
