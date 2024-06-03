using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.View.Owner;
using System;
using System.Collections.ObjectModel;
using BookingApp.ViewModel.Owner;
using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using GuestRatingPage = BookingApp.View.Owner.GuestRating;
using System.ComponentModel;
using Notification.Wpf;
using System.Windows;

namespace BookingApp.ViewModel.Owner
{
    public class GuestRatingViewModel
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public INotificationManager notificationManager = App.GetNotificationManager();
        public RelayCommand RateGuest => new RelayCommand(execute => RateGuestExecute(), canExecute => RateGuestCanExecute());
        public User user { get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public GuestRatingPage GuestRatingPage {  get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public ReservedAccommodation SelectedReservedAccommodations { get; set; }
        public GuestRatingViewModel(GuestRatingPage GuestRatingPage, OwnerMainWindow OwnerMainWindow, User user) 
        {
            this.user = user;
            this.OwnerMainWindow = OwnerMainWindow;
            this.GuestRatingPage = GuestRatingPage;
            ReservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            ReservedAccommodationService.GetInstance().NotificationUpdate(user, ReservedAccommodations);
            if(ReservedAccommodations.Count == 0)
            {
                if (App.currentLanguage() == ENG)
                    notificationManager.Show("Info", "You have no guests to rate!", NotificationType.Information);
                else
                    notificationManager.Show("Info", "Nema gostiju za ocenjivanje!", NotificationType.Information);
            }
        }
        public void RateGuestExecute()
        {
            Comment comment = new Comment();
            comment.Text = GuestRatingPage.CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserService.GetInstance().GetById(user.Id);
            comment = CommentService.GetInstance().Save(comment);

            GuestRatingModel GuestRatingModel = new GuestRatingModel();
            GuestRatingModel.OwnerId = user.Id;
            GuestRatingModel.GuestId = SelectedReservedAccommodations.GuestId;
            GuestRatingModel.CommentId = comment.Id;
            GuestRatingModel.Cleanliness = GuestRatingPage.Cleanliness;
            GuestRatingModel.FollowingGuidelines = GuestRatingPage.FollowingGuidelines;
            GuestRatingService.GetInstance().Add(GuestRatingModel);

            ReservedAccommodationService.GetInstance().NotificationUpdate(user, ReservedAccommodations);
            ReservedAccommodationService.GetInstance().NotificationUpdate(user, OwnerMainWindow.ReservedAccommodations);
            OwnerMainWindow.NotificationListBox.Items.Refresh();

            GuestRatingPage.CommentTextBox.Text = string.Empty;

            if (ReservedAccommodations.Count <= 0)
            {
                GuestRatingPage.NoGuestsToRateMessage.Visibility = Visibility.Visible;
            }
            if (App.currentLanguage() == ENG)
                notificationManager.Show("Success!", "Guest successfully rated!", NotificationType.Success);
            else
                notificationManager.Show("Uspeh!", "Gost uspešno ocenjen!", NotificationType.Success);
        }
        public bool RateGuestCanExecute()
        {
            if (SelectedReservedAccommodations == null ||
                !IsCleanlinessChecked() ||
                !IsFollowingGuidelinesChecked() ||
                GuestRatingPage.CommentTextBox.Text.Equals(""))
            {
                GuestRatingPage.GuestRatingValidation.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            else
            {
                GuestRatingPage.GuestRatingValidation.Visibility = System.Windows.Visibility.Hidden;
                return true;
            }
        }
        public bool IsCleanlinessChecked()
        {
            if(GuestRatingPage.Cleanliness1.IsChecked == false && GuestRatingPage.Cleanliness2.IsChecked == false &&
                GuestRatingPage.Cleanliness3.IsChecked == false && GuestRatingPage.Cleanliness4.IsChecked == false &&
                GuestRatingPage.Cleanliness5.IsChecked == false)
            {
                return false;
            }
            return true;
        }
        public bool IsFollowingGuidelinesChecked()
        {
            if (GuestRatingPage.FollowingGuidelines1.IsChecked == false && GuestRatingPage.FollowingGuidelines2.IsChecked == false &&
                GuestRatingPage.FollowingGuidelines3.IsChecked == false && GuestRatingPage.FollowingGuidelines4.IsChecked == false &&
                GuestRatingPage.FollowingGuidelines5.IsChecked == false)
            {
                return false;
            }
            return true;
        }
    }
}
