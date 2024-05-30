﻿using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using Image = BookingApp.Domain.Model.Image;
using BookingApp.ViewModel.Guest;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        public User user { get; set; }
        public GuestRate GuestRate { get; set; }

        public GuestReservations GuestReservations { get; set; }

        public GuestMainWindowViewModel GuestMainWindowViewModel { get; set; }

        public GuestForum GuestForum { get; set; }

        public OwnerReviews OwnerReviews { get; set; }
        public Accommodations Accommodations { get; set; }
        public GuestMainWindow(User user)
        {
            InitializeComponent();
            GuestMainWindowViewModel = new GuestMainWindowViewModel(this, user);
            this.DataContext = GuestMainWindowViewModel;
            this.user = user;
            //GuestRate = new GuestRate(user, reservedAccommodation);
            Accommodations = new Accommodations(user, this);
            mainFrame.Navigate(Accommodations);
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
        }
        private void AccommodationsClick(object sender, RoutedEventArgs e)
        { 
            mainFrame.Navigate(Accommodations);
        }

        private void ReservationClick(object sender, RoutedEventArgs e)
        {
            GuestReservations = new GuestReservations(user, this);
            mainFrame.Navigate(GuestReservations);
        }

        private void ReviewsClick(object sender, RoutedEventArgs e)
        {
            OwnerReviews = new OwnerReviews(user);
            mainFrame.Navigate(OwnerReviews);
        }

        private void ForumClick(object sender, RoutedEventArgs e)
        {
            GuestForum = new GuestForum(user);
            mainFrame.Navigate(GuestForum);
        }

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

        private void TutorialClick(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationsClick(object sender, RoutedEventArgs e)
        {
            GuestNotifications guestNotifications = new GuestNotifications(user, this);
            guestNotifications.Show();
        }
    }
}
