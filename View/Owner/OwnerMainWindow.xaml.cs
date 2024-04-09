﻿using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using GuestRatingPage = BookingApp.View.Owner.GuestRating;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public User user { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationRegistration AccommodationRegistration { get; set; }
        public AccommodationStatistics AccommodationStatistics {  get; set; }
        public ReservationRescheduling ReservationRescheduling { get; set; }
        public GuestRatingPage GuestRatingPage { get; set; }
        public GuestReviews GuestReviews { get; set; }
        public Renovation Renovation { get; set; }
        public Forum Forum {  get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;

            Accommodation = new Accommodation();
            AccommodationRegistration = new AccommodationRegistration(Accommodation, user);
            AccommodationStatistics = new AccommodationStatistics();
            ReservationRescheduling = new ReservationRescheduling();
            //ReservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            GuestRatingPage = new GuestRatingPage(this, user);
            ReservedAccommodations = GuestRatingPage.GuestRatingViewModel.Update();
            GuestReviews = new GuestReviews();
            Renovation = new Renovation();
            Forum = new Forum();
            mainFrame.Navigate(AccommodationRegistration);

            if (ReservedAccommodations.Count == 0)
            {
                NotificationListBox.BorderBrush = Brushes.Gray;
                NotificationListBox.BorderThickness = new Thickness(1);
            }
        }

        private void AccommodationReservationClick(object sender, RoutedEventArgs e)
        { mainFrame.Navigate(AccommodationRegistration); }

        private void GuestRatingClick(object sender, RoutedEventArgs e)
        {
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            mainFrame.Navigate(GuestRatingPage);
        }
        private void ListBoxItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            mainFrame.Navigate(GuestRatingPage);
        }

        private void GuestReviewsClick(object sender, RoutedEventArgs e)
        { mainFrame.Navigate(GuestReviews); }

        private void AccommodationStatisticsClick(object sender, RoutedEventArgs e)
        { mainFrame.Navigate(AccommodationStatistics); }

        private void ReservationReschedulingClick(object sender, RoutedEventArgs e)
        { mainFrame.Navigate(ReservationRescheduling); }

        private void RenovationClick(object sender, RoutedEventArgs e)
        { mainFrame.Navigate(Renovation); }

        private void ForumClick(object sender, RoutedEventArgs e)
        { mainFrame.Navigate(Forum); }
    }
}
