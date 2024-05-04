﻿using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Repository;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using BookingApp.Services;
using System.Windows.Controls;
using System.Windows;
using Image = BookingApp.Domain.Model.Image;
using System.Collections.ObjectModel;
using BookingApp.View;

namespace BookingApp.ViewModel.Tourist
{
    public class TouristMainWindowViewModel : INotifyPropertyChanged
    {
        public TouristMainWindow TouristMainWindow { get; set; }
        public RelayCommand ClickToursTab => new RelayCommand(execute => ToursTabExecute(),canExecute => ToursTabCanExecute());
        public RelayCommand ClickOngoingToursTab => new RelayCommand(execute => OngoingToursTabExecute(),canExecute => OngoingToursTabCanExecute());
        public RelayCommand ClickFinishedToursTab => new RelayCommand(execute => FinishedToursTabExecute(),canExecute => FinishedToursTabCanExecute());
        public RelayCommand ClickReservationsTab => new RelayCommand(execute => ReservationsTabExecute(),canExecute => ReservationsTabCanExecute());
        public RelayCommand ClickSuggestionsTab => new RelayCommand(execute => SuggestionsTabExecute(),canExecute => SuggestionsTabCanExecute());
        public RelayCommand ClickCouponsTab => new RelayCommand(execute => CouponsTabExecute(),canExecute => CouponsTabCanExecute());
        public RelayCommand ClickCollapseSearchBar => new RelayCommand(execute => CollapseSearchBarExecute());
        public RelayCommand ClickSearchTours => new RelayCommand(execute => SearchToursExecute());
        public RelayCommand ClickNotificationButton => new RelayCommand(execute => NotificationButtonExecute());
        public RelayCommand ClickLogOut => new RelayCommand(execute => LogOutExecute());
        public RelayCommand ClickTourSuggestion => new RelayCommand(execute => TourSuggestionExecute());

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
        private ObservableCollection<Tour> finishedTours { get; set; }
        public ObservableCollection<Tour> FinishedTours
        {
            get
            {
                return finishedTours;
            }
            set
            {
                finishedTours = value;
                OnPropertyChanged(nameof(FinishedTours));
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
        public User User { get; set; }
        public string Username { get; set; }
        public TouristMainWindowViewModel(TouristMainWindow touristMainWindow,User user)
        {
            this.TouristMainWindow = touristMainWindow;
            User = user;
            Username = User.Username;

            if (Username.Length > 10)
            {
                Username = Username.Substring(0, 10) + "...";
            }
            List<Location> locations = new List<Location>();
            List<KeyPoint> keyPoints = new List<KeyPoint>();
            List<KeyPoint> keyPointsForward = new List<KeyPoint>();
            List<Image> images = new List<Image>();
            List<Image> imagesForward = new List<Image>();
            List<TourImage> tourImages = new List<TourImage>();

            locations = LocationService.GetInstance().GetAll();
            keyPoints = KeyPointService.GetInstance().GetAll();
            images = ImageService.GetInstance().GetAll();
            tourImages = TourImageService.GetInstance().GetAll();

            Tours = new List<Tour>();
            ToursAll = new List<Tour>();
            OngoingTours = new List<Tour>();
            ReservedTours = new List<Tour>();
            FinishedTours = new ObservableCollection<Tour>();


            IndividualTours = TourService.GetInstance().GetAll();
            Schedules = TourScheduleService.GetInstance().GetAll();

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
                            if (tour1.DateTime < DateTime.Now)
                            {
                                tourSchedule.ScheduleStatus = ScheduleStatus.Canceled;                  // Not sure if the tourist needs to get a coupon after the tour expires or not ????
                                TourScheduleService.GetInstance().Update(tourSchedule);
                            }
                            else
                            {
                                ToursAll.Add(tour1);
                            }
                        }
                        else if (tourSchedule.ScheduleStatus == ScheduleStatus.Ongoing)
                        {
                            bool TourAdded = false;

                            foreach (TourReservation tourReservation in TourReservationService.GetInstance().GetAll())
                            {
                                if (tourSchedule.Id == tourReservation.TourScheduleId && tourReservation.UserId == User.Id)      //Makes sure that the user only sees the ongoing tours which he reserved
                                {
                                    if (!TourAdded)
                                    {
                                        OngoingTours.Add(tour1);
                                        TourAdded = true;
                                    }
                                }
                            }
                        }
                        else if(tourSchedule.ScheduleStatus == ScheduleStatus.Finished)
                        {
                            foreach (TourReservation tourReservation in TourReservationService.GetInstance().GetAll())
                            {
                                if (tourReservation.TourScheduleId == tourSchedule.Id && tourReservation.UserId == User.Id)
                                {
                                        if (!FinishedTours.Contains(tour1)) //safe measure because it can get repeated if there are multiple reservations of the tour
                                        {
                                            FinishedTours.Add(tour1);
                                        }
                                }
                            }
                        }
                        //Inserting reserved Tours
                        List<int> tourScheduleIds = new List<int>();

                        foreach (TourReservation tr in TourReservationService.GetInstance().GetAll())
                        {
                            if (tr.TourScheduleId == tourSchedule.Id && tr.UserId == User.Id && tourSchedule.ScheduleStatus != ScheduleStatus.Finished && tourSchedule.ScheduleStatus != ScheduleStatus.Canceled) //made it so that it doesnt show finished tours, makes sense????? 
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

            AddStatesToComboBox(locations);

            Coupons = new List<TourCoupon>();
            UpdateCoupons();
        }
        public void AddStatesToComboBox(List<Location> locations)
        {
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
            TouristMainWindow.StateComboBox.Items.Clear();

            foreach (String s in States)
            {
                TouristMainWindow.StateComboBox.Items.Add(s);
            }
            TouristMainWindow.CityComboBox.IsEnabled = false;
        }
        public void UpdateCoupons()
        {
            List<TourCoupon> allTourCoupons = TourCouponService.GetInstance().GetAll();
            Coupons.Clear();
            foreach (TourCoupon t in allTourCoupons)
            {
                DateTime expiryDate = t.AcquiredDate.AddMonths(t.ExpirationMonths);
                t.ExpirationDate = expiryDate;
                if (DateTime.Now > expiryDate && t.Status != CouponStatus.Expired)   //Checks if the coupon has expired and changes it's status inside of the csv file if it is
                {
                    t.Status = CouponStatus.Expired;
                    TourCouponService.GetInstance().Update(t);
                }

                if (t.Status != CouponStatus.Expired && t.UserId == User.Id)
                {
                    Coupons.Add(t);                                                             // This needs to update when I switch to the coupons tab
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void StateComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            TouristMainWindow.CityComboBox.Items.Clear();

            if (TouristMainWindow.StateComboBox.SelectedIndex == -1)
            {
                TouristMainWindow.CityComboBox.IsEnabled = false;
            }
            else
            {
                TouristMainWindow.CityComboBox.IsEnabled = true;

                List<string> cities = new List<string>();
                string selectedState = (string)TouristMainWindow.StateComboBox.SelectedItem;

                cities = findCities(cities, selectedState);
                
                foreach (string city in cities)
                {
                    TouristMainWindow.CityComboBox.Items.Add(city);
                }

            }
        }
        public List<string> findCities(List<string> cities,string selectedState)
        {
            foreach (Location location in LocationService.GetInstance().GetAll())
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
            return cities;
        }
        public void SearchToursExecute()
        {

            
            string state = (string)TouristMainWindow.StateComboBox.SelectedItem;
            string city = (string)TouristMainWindow.CityComboBox.SelectedItem;
            int duration = 0;
            if (!string.IsNullOrEmpty(TouristMainWindow.DurationTextBox.Text))
            {
                duration = Convert.ToInt32(TouristMainWindow.DurationTextBox.Text);
            }
            string language = TouristMainWindow.LanguageTextBox.Text;
            int people = 0;
            if (!string.IsNullOrEmpty(TouristMainWindow.PeopleTextBox.Text))
            {
                people = Convert.ToInt32(TouristMainWindow.PeopleTextBox.Text);
            }

            List<Tour> searchResults = TouristMainWindow.searchTours(state, city, duration, language, people);
            Tours = searchResults;

            if (string.IsNullOrEmpty(state) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(TouristMainWindow.DurationTextBox.Text) && string.IsNullOrEmpty(language) && string.IsNullOrEmpty(TouristMainWindow.PeopleTextBox.Text))
            {
                TouristMainWindow.RefreshTours();
                TouristMainWindow.SearchBarTextBox.Text = "Search tours...";
            }
            else
            {
                TouristMainWindow.SearchBarTextBox.Text = "";

                if (!string.IsNullOrEmpty(state))
                {
                    TouristMainWindow.SearchBarTextBox.Text = state;
                }

                if (!string.IsNullOrEmpty(city))
                {
                    if (!string.IsNullOrEmpty(TouristMainWindow.SearchBarTextBox.Text))
                    {
                        TouristMainWindow.SearchBarTextBox.Text += ", " + city;
                    }
                    else
                    {
                        TouristMainWindow.SearchBarTextBox.Text = city;
                    }
                }

                if (!string.IsNullOrEmpty(TouristMainWindow.DurationTextBox.Text))
                {
                    if (!string.IsNullOrEmpty(TouristMainWindow.SearchBarTextBox.Text))
                    {
                        TouristMainWindow.SearchBarTextBox.Text += ", " + TouristMainWindow.DurationTextBox.Text + "h";
                    }
                    else
                    {
                        TouristMainWindow.SearchBarTextBox.Text = TouristMainWindow.DurationTextBox.Text + "h";
                    }
                }

                if (!string.IsNullOrEmpty(language))
                {
                    if (!string.IsNullOrEmpty(TouristMainWindow.SearchBarTextBox.Text))
                    {
                        TouristMainWindow.SearchBarTextBox.Text += ", " + language;
                    }
                    else
                    {
                        TouristMainWindow.SearchBarTextBox.Text = language;
                    }
                }

                if (!string.IsNullOrEmpty(TouristMainWindow.PeopleTextBox.Text))
                {
                    if (!string.IsNullOrEmpty(TouristMainWindow.SearchBarTextBox.Text))
                    {
                        TouristMainWindow.SearchBarTextBox.Text += ", " + TouristMainWindow.PeopleTextBox.Text;
                    }
                    else
                    {
                        TouristMainWindow.SearchBarTextBox.Text = TouristMainWindow.PeopleTextBox.Text;
                    }
                }
            }
            
        }
        public void RefreshTours()
        {
            Tours = ToursAll;
        }

        public List<Tour> searchTours(string state, string city, int duration, string language, int people)
        {
            return ToursAll.Where(tour =>
                (string.IsNullOrEmpty(state) || tour.Location.State.Contains(state)) &&
                (string.IsNullOrEmpty(city) || tour.Location.City.Contains(city)) &&
                (duration <= 0 || tour.Duration == duration) &&
                (string.IsNullOrEmpty(language) || tour.Language.Contains(language)) &&
                (people <= 0 || tour.MaxTourists >= people)
            ).ToList();
        }
        public void NotificationButtonExecute()
        {
            NotificationWindow notificationWindow = new NotificationWindow(User);
            notificationWindow.Owner = TouristMainWindow;
            notificationWindow.ShowDialog();
        }
        public void TourSuggestionExecute()
        {
            TourSuggestionWindow tourSuggestionWindow= new TourSuggestionWindow(User);
            tourSuggestionWindow.Owner = TouristMainWindow;
            tourSuggestionWindow.ShowDialog();
        }
        private void ToursTabExecute()
        {
            TouristMainWindow.ToursTabHeader.Visibility = Visibility.Visible;
            TouristMainWindow.OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.ReservationsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.CouponsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.Tab.SelectedIndex = 0;
        }
        private void OngoingToursTabExecute()
        {
            TouristMainWindow.ToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.OngoingToursTabHeader.Visibility = Visibility.Visible;
            TouristMainWindow.FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.ReservationsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.CouponsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.Tab.SelectedIndex = 1;
        }
        private void FinishedToursTabExecute()
        {
            TouristMainWindow.ToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.FinishedToursTabHeader.Visibility = Visibility.Visible;
            TouristMainWindow.ReservationsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.CouponsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.Tab.SelectedIndex = 2;
        }
        private void ReservationsTabExecute()
        {
            TouristMainWindow.ToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.ReservationsTabHeader.Visibility = Visibility.Visible;
            TouristMainWindow.SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.CouponsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.Tab.SelectedIndex = 3;
        }
        private void SuggestionsTabExecute()
        {
            TouristMainWindow.ToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.ReservationsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.SuggestionsTabHeader.Visibility = Visibility.Visible;
            TouristMainWindow.CouponsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.Tab.SelectedIndex = 4;
        }
        private void CouponsTabExecute()
        {
            TouristMainWindow.ToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.ReservationsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            TouristMainWindow.CouponsTabHeader.Visibility = Visibility.Visible;
            UpdateCoupons();
            TouristMainWindow.Tab.SelectedIndex = 5;
        }

        private void CollapseSearchBarExecute()     // NEEDS ANIMATION ??
        {
            TouristMainWindow.SearchBarGrid.Visibility = Visibility.Collapsed;
            TouristMainWindow.StateComboBox.SelectedIndex = -1;
            TouristMainWindow.CityComboBox.SelectedIndex = -1;
            TouristMainWindow.DurationTextBox.Text = string.Empty;
            TouristMainWindow.LanguageTextBox.Text = string.Empty;
            TouristMainWindow.PeopleTextBox.Text = string.Empty;
            TouristMainWindow.SearchBarTextBox.Text = "Search tours...";
            RefreshTours();                                 //refreshes searched tours after the search bar is collapsed, might get removed later??
        }
        private void LogOutExecute()
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            TouristMainWindow.Close();
        }
        public bool ToursTabCanExecute()
        {
            if(TouristMainWindow.Tab.SelectedIndex == 0) { return false; }
            return true;
        }
        public bool OngoingToursTabCanExecute()
        {
            if (TouristMainWindow.Tab.SelectedIndex == 1) { return false; }
            return true;
        }
        public bool FinishedToursTabCanExecute()
        {
            if (TouristMainWindow.Tab.SelectedIndex == 2) { return false; }
            return true;
        }
        public bool ReservationsTabCanExecute()
        {
            if (TouristMainWindow.Tab.SelectedIndex == 3) { return false; }
            return true;
        }
        public bool SuggestionsTabCanExecute()
        {
            if (TouristMainWindow.Tab.SelectedIndex == 4) { return false; }
            return true;
        }
        public bool CouponsTabCanExecute()
        {
            if (TouristMainWindow.Tab.SelectedIndex == 5) { return false; }
            return true;
        }
    }
}
