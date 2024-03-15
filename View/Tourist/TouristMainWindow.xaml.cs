using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<Tour> IndividualTours { get; set; }
        private List<Tour> tours {  get; set; }
        public List<Tour> Tours
        {
            get 
            { 
                return tours; 
            }
            set
            {
                tours = value;
                OnPropertyChanged(nameof(Tours));
            }
        }
        public List<Tour> ToursAll {  get; set; }
        public List<TourSchedule> Schedules { get; set; }
        public TourRepository tourRepository { get; set; }
        public TourScheduleRepository tourScheduleRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public KeyPointRepository keyPointRepository { get; set; }
        public TourImageRepository tourImageRepository { get; set; }
        public ImageRepository imageRepository { get; set; }
        public static User? User { get; set; }
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
            List<KeyPoint> keyPointsForward = new List<KeyPoint>();
            List<Image> images = new List<Image>();
            List<Image> imagesForward = new List<Image>();
            List<TourImage> tourImages = new List<TourImage>();

            locations = locationRepository.GetAll();
            keyPoints = keyPointRepository.GetAll();
            images = imageRepository.GetAll();
            tourImages = tourImageRepository.GetAll();

            Tours = new List<Tour>();
            ToursAll = new List<Tour>();

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

                                keyPointsForward.Add(keyPoint1);
                            }
                        }

                        tour1.KeyPoints = keyPointsForward;
                        keyPointsForward = new List<KeyPoint>();

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

            Tours = ToursAll;

            List<String> States = new List<string>();
            foreach(Location location in locations)
            {
                bool Exists = false;

                foreach(String s in States)
                {
                    if(s == location.State)
                    {
                        Exists = true;
                    }
                }
                if(!Exists)
                {
                    States.Add(location.State);
                }
            }
            StateComboBox.Items.Clear();
            
            foreach(String s in States)
            {
                StateComboBox.Items.Add(s);
            }
            CityComboBox.IsEnabled = false;

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
            SearchBarGrid.Visibility = Visibility.Visible;

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

        private void CollapseSearchBar(object sender, RoutedEventArgs e)     // NEEDS ANIMATION ??
        {
            SearchBarGrid.Visibility = Visibility.Collapsed;
            StateComboBox.SelectedIndex = -1;
            CityComboBox.SelectedIndex = -1;
            DurationTextBox.Text = string.Empty;
            LanguageTextBox.Text = string.Empty;
            PeopleTextBox.Text = string.Empty;
            SearchBarTextBox.Text = "Search tours...";
            RefreshTours();                                 //refreshes searched tours after the search bar is collapsed, might get removed later??
        }

        private void StateComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityComboBox.Items.Clear();

            if(StateComboBox.SelectedIndex == -1)
            {
                CityComboBox.IsEnabled = false;
            }
            else
            {
                CityComboBox.IsEnabled = true;
                List<Location> locations = locationRepository.GetAll();

                List<string> cities = new List<string>();
                string selectedState = (string)StateComboBox.SelectedItem;

                foreach(Location location in locations)
                {
                    if(location.State == selectedState)
                    {
                        bool Exists = false;

                        foreach(string c in cities)
                        {
                            if(c == location.City)
                            {
                                Exists = true;
                            }
                        }

                        if(!Exists)
                        {
                            cities.Add(location.City);
                        }

                    }

                }
                foreach(string city in cities)
                {
                    CityComboBox.Items.Add(city);
                }

            }

        }

        private void NumbersPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!double.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        public void SearchTours(object sender, RoutedEventArgs e)
        {
            string state = (string)StateComboBox.SelectedItem;
            string city = (string)CityComboBox.SelectedItem;
            int duration = 0;
            if(!string.IsNullOrEmpty(DurationTextBox.Text))
            {
                duration = Convert.ToInt32(DurationTextBox.Text);
            }
            string language = LanguageTextBox.Text;
            int people = 0;
            if(!string.IsNullOrEmpty(PeopleTextBox.Text)) 
            {
                people = Convert.ToInt32(PeopleTextBox.Text);
            }

            List<Tour> searchResults = searchTours(state, city, duration, language, people);
            Tours = searchResults;

            if (string.IsNullOrEmpty(state) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(DurationTextBox.Text) && string.IsNullOrEmpty(language) && string.IsNullOrEmpty(PeopleTextBox.Text))
            {
                RefreshTours();
                SearchBarTextBox.Text = "Search tours...";
            }
            else
            {
                SearchBarTextBox.Text = "";

                if(!string.IsNullOrEmpty(state))
                {
                    SearchBarTextBox.Text = state;
                }

                if(!string.IsNullOrEmpty(city))
                {
                    if(!string.IsNullOrEmpty(SearchBarTextBox.Text))
                    {
                        SearchBarTextBox.Text += ", " + city;
                    }
                    else
                    {
                        SearchBarTextBox.Text = city;
                    }
                }

                if (!string.IsNullOrEmpty(DurationTextBox.Text))
                {
                    if (!string.IsNullOrEmpty(SearchBarTextBox.Text))
                    {
                        SearchBarTextBox.Text += ", " + DurationTextBox.Text + "h";
                    }
                    else
                    {
                        SearchBarTextBox.Text = DurationTextBox.Text + "h";
                    }
                }

                if (!string.IsNullOrEmpty(language))
                {
                    if (!string.IsNullOrEmpty(SearchBarTextBox.Text))
                    {
                        SearchBarTextBox.Text += ", " + language;
                    }
                    else
                    {
                        SearchBarTextBox.Text = language;
                    }
                }

                if (!string.IsNullOrEmpty(PeopleTextBox.Text))
                {
                    if (!string.IsNullOrEmpty(SearchBarTextBox.Text))
                    {
                        SearchBarTextBox.Text += ", " + PeopleTextBox.Text;
                    }
                    else
                    {
                        SearchBarTextBox.Text = PeopleTextBox.Text;
                    }
                }
            }
        }

        private List<Tour> searchTours(string state, string city, int duration, string language, int people)
        {
            return ToursAll.Where( tour =>
                (string.IsNullOrEmpty(state) || tour.Location.State.Contains(state)) &&
                (string.IsNullOrEmpty(city)  || tour.Location.City.Contains(city)) &&
                (duration <= 0 || tour.Duration == duration) &&
                (string.IsNullOrEmpty(language) || tour.Language.Contains(language)) &&
                (people <= 0 || tour.MaxTourists >= people)
            ).ToList();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshTours()
        {
            Tours = ToursAll;
        }

    }
}
