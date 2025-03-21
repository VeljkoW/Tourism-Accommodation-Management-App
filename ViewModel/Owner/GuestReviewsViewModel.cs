﻿using BookingApp.Domain.Model;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using BookingApp.View.Owner;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using Notification.Wpf;

namespace BookingApp.ViewModel.Owner
{
    public class GuestReviewsViewModel
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public INotificationManager notificationManager = App.GetNotificationManager();
        private int currentImageIndex = 0;
        public User User { get; set; }
        public int NumberOfReviews { get; set; }
        public double AverageGrade { get; set; }
        public OwnerRating SelectedOwnerRating {  get; set; }
        public ObservableCollection<OwnerRating> OwnerRatings {  get; set; }
        public GuestReviews GuestReviews {  get; set; }
        public GuestReviewsViewModel(GuestReviews GuestReviews, User User, OwnerRating SelectedOwnerRating)
        {
            this.User = User;
            this.SelectedOwnerRating = SelectedOwnerRating;
            this.GuestReviews = GuestReviews;
            OwnerRatings = new ObservableCollection<OwnerRating>();

            //OwnerRatings = Update();
            OwnerRatings = OwnerRatingService.GetInstance().GetOwnerRatings(User.Id);
            AverageGrade = OwnerService.GetInstance().GetAverageGrade(User.Id);
            NumberOfReviews = OwnerRatings.Count;
            if (OwnerService.GetInstance().isSuperOwner(User.Id))
            {
                GuestReviews.SuperOwnerInfoButton.Visibility = Visibility.Collapsed;
                GuestReviews.SuperownerLabel.Visibility = Visibility.Visible;
                GuestReviews.starImage.Visibility = Visibility.Visible;
            }
            else
            {
                GuestReviews.SuperOwnerInfoButton.Visibility = Visibility.Visible;
                GuestReviews.SuperownerLabel.Visibility = Visibility.Collapsed;
                GuestReviews.starImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
