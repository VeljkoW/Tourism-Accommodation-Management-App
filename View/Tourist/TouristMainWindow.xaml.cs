using BookingApp.Domain.Model;
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
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public List<Tour> IndividualTours { get; set; }
        private List<Tour> tours { get; set; }
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
        private List<Tour> ongoingTours { get; set; }
        public List<Tour> OngoingTours
        {
            get
            {
                return ongoingTours;
            }
            set
            {
                ongoingTours = value;
                OnPropertyChanged(nameof(OngoingTours));
            }
        }
        private List<Tour> reservedTours { get; set; }
        public List<Tour> ReservedTours
        {
            get
            {
                return reservedTours;
            }
            set
            {
                reservedTours = value;
                OnPropertyChanged(nameof(ReservedTours));
            }
        }
        public List<Tour> ToursAll { get; set; }
        private List<TourCoupon> coupons { get; set; }
        public List<TourCoupon> Coupons
        {
            get
            {
                return coupons;
            }
            set
            {
                coupons = value;
                OnPropertyChanged(nameof(Coupons));
            }
        }
        public List<TourSchedule> Schedules { get; set; }
        public TourRepository tourRepository { get; set; }
        public TourCouponRepository tourCouponRepository { get; set; }
        public TourScheduleRepository tourScheduleRepository { get; set; }
        public TourReservationRepository tourReservationRepository { get; set; }
        public LocationRepository locationRepository { get; set; }
        public KeyPointRepository keyPointRepository { get; set; }
        public TourImageRepository tourImageRepository { get; set; }
        public ImageRepository imageRepository { get; set; }
        public static User? User { get; set; }
        public string Username { get; set; }
        public TouristMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
            Username = User.Username;

            if (Username.Length > 10)
            {
                Username = Username.Substring(0, 10) + "...";
            }

            tourRepository = new TourRepository();
            tourScheduleRepository = new TourScheduleRepository();
            locationRepository = new LocationRepository();
            keyPointRepository = new KeyPointRepository();
            tourImageRepository = new TourImageRepository();
            imageRepository = new ImageRepository();
            tourCouponRepository = new TourCouponRepository();
            tourReservationRepository = new TourReservationRepository();

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
            OngoingTours = new List<Tour>();
            ReservedTours = new List<Tour>();

            IndividualTours = tourRepository.GetAll();
            Schedules = tourScheduleRepository.GetAll();

            foreach (Tour tour in IndividualTours)
            {
                foreach (TourSchedule tourSchedule in Schedules)
                {
                    if (tour.Id == tourSchedule.TourId)
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
                        if (tourSchedule.ScheduleStatus == ScheduleStatus.Ready)    // checks if the tour is not finished yet
                        {
                            ToursAll.Add(tour1);
                        }
                        else if(tourSchedule.ScheduleStatus == ScheduleStatus.Ongoing)
                        {
                            bool TourAdded = false;

                            foreach(TourReservation tourReservation in tourReservationRepository.GetAll())
                            {
                                if(tourSchedule.Id == tourReservation.TourScheduleId && tourReservation.UserId == User.Id)      //Makes sure that the user only sees the ongoing tours which he reserved
                                { 
                                    if (!TourAdded)
                                    {
                                        OngoingTours.Add(tour1);
                                        TourAdded = true;
                                    }
                                }
                            }
                        }

                        //Inserting reserved Tours
                        List<int> tourScheduleIds = new List<int>();

                        foreach (TourReservation tr in tourReservationRepository.GetAll())
                        {
                            if (tr.TourScheduleId == tourSchedule.Id && tr.UserId == User.Id)
                            {
                                if (!tourScheduleIds.Contains(tr.TourScheduleId))
                                {
                                    ReservedTours.Add(tour1);                                           //shows only 1 of each tour schedule
                                    tourScheduleIds.Add(tr.TourScheduleId);
                                }
                            }
                        }
                    }
                }
            }

            Tours = ToursAll;

            List<String> States = new List<string>();
            foreach (Location location in locations)
            {
                bool Exists = false;

                foreach (String s in States)
                {
                    if (s == location.State)
                    {
                        Exists = true;
                    }
                }
                if (!Exists)
                {
                    States.Add(location.State);
                }
            }
            StateComboBox.Items.Clear();

            foreach (String s in States)
            {
                StateComboBox.Items.Add(s);
            }
            CityComboBox.IsEnabled = false;

            List<TourCoupon> allTourCoupons = tourCouponRepository.GetAll();
            Coupons = new List<TourCoupon>();

            
            foreach (TourCoupon t in allTourCoupons)
            {
                DateTime expiryDate = t.AcquiredDate.AddMonths(t.ExpirationMonths);
                t.ExpirationDate = expiryDate;
                if (DateTime.Now > expiryDate && t.Status != CouponStatus.Expired)   //Checks if the coupon has expired and changes it's status inside of the csv file if it is
                {
                    t.Status = CouponStatus.Expired;
                    tourCouponRepository.Update(t);
                }

                if (t.Status != CouponStatus.Expired && t.UserId == User.Id)
                {
                    Coupons.Add(t);                                                             // This needs to update when I switch to the coupons tab
                }

            }

            /*
            Coupons = new List<TourCoupon>
            { 
                new TourCoupon(1,"Coupon #1","Reason",new DateTime(),5,CouponStatus.Valid),
                new TourCoupon(2,"Coupon #2","Reason",new DateTime(),5,CouponStatus.Valid),
                new TourCoupon(3,"Coupon #2","Reason",new DateTime(),5,CouponStatus.Valid)
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
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search tours...";
                textBox.Foreground = Brushes.Gray;
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
            ToursTabHeader.Visibility = Visibility.Visible;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 0;
        }
        private void OngoingToursTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Visible;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 1;
        }

        private void FinishedToursTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Visible;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 2;
        }
        private void ReservationsTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Visible;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 3;
        }
        private void SuggestionsTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Visible;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 4;
        }
        private void CouponsTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Visible;
            Tab.SelectedIndex = 5;
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

            if (StateComboBox.SelectedIndex == -1)
            {
                CityComboBox.IsEnabled = false;
            }
            else
            {
                CityComboBox.IsEnabled = true;
                List<Location> locations = locationRepository.GetAll();

                List<string> cities = new List<string>();
                string selectedState = (string)StateComboBox.SelectedItem;

                foreach (Location location in locations)
                {
                    if (location.State == selectedState)
                    {
                        bool Exists = false;

                        foreach (string c in cities)
                        {
                            if (c == location.City)
                            {
                                Exists = true;
                            }
                        }

                        if (!Exists)
                        {
                            cities.Add(location.City);
                        }

                    }

                }
                foreach (string city in cities)
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
            if (!string.IsNullOrEmpty(DurationTextBox.Text))
            {
                duration = Convert.ToInt32(DurationTextBox.Text);
            }
            string language = LanguageTextBox.Text;
            int people = 0;
            if (!string.IsNullOrEmpty(PeopleTextBox.Text))
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

                if (!string.IsNullOrEmpty(state))
                {
                    SearchBarTextBox.Text = state;
                }

                if (!string.IsNullOrEmpty(city))
                {
                    if (!string.IsNullOrEmpty(SearchBarTextBox.Text))
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
            return ToursAll.Where(tour =>
                (string.IsNullOrEmpty(state) || tour.Location.State.Contains(state)) &&
                (string.IsNullOrEmpty(city) || tour.Location.City.Contains(city)) &&
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
        private void NotificationButtonClick(object sender, RoutedEventArgs e)
        {
            NotificationWindow notificationWindow = new NotificationWindow();
            notificationWindow.Owner = this;
            notificationWindow.ShowDialog();
        }
    }
}
