using BookingApp.Services;
using BookingApp.View.Guest;
using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using BookingApp.View;

namespace BookingApp.ViewModel.Guest
{
    public class GuestMainWindowViewModel
    {
        public GuestMainWindow GuestMainWindow { get; set; }
        public GuestRate GuestRate { get; set; }

        public GuestReservations GuestReservations { get; set; }
        public GuestForum GuestForum { get; set; }

        public OwnerReviews OwnerReviews { get; set; }
        public Accommodations Accommodations { get; set; }

        public RelayCommand Accommodation => new RelayCommand(execute => AccommodationsPage());
        public RelayCommand Reservations => new RelayCommand(execute => ReservationsPage());
        public RelayCommand Reviews => new RelayCommand(execute => ReviewsPage());
        public RelayCommand Forum => new RelayCommand(execute => ForumPage());
        public RelayCommand LogOut => new RelayCommand(execute => LogOutWindow());
        public RelayCommand Tutorial => new RelayCommand(execute => TutorialWindow());
        public RelayCommand Notifications => new RelayCommand(execute => NotificationsWindow());
        public GuestMainWindowViewModel(GuestMainWindow guestMainWindow, User user)
        {
            GuestMainWindow = guestMainWindow;
            GuestMainWindow.Username.Content = user.Username;
            GuestBonusService.GetInstance().UpdateAll();

            if (GuestBonusService.GetInstance().IsSuperGuest(user))
            {
                GuestMainWindow.SuperGuestImage.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                GuestMainWindow.SuperGuestImage.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public void AccommodationsPage()
        {
            GuestMainWindow.mainFrame.Navigate(GuestMainWindow.Accommodations);
            GuestMainWindow.NavigationButtonBarPressed("AccommodationButton");
        }
        public void ReservationsPage()
        {
            GuestReservations = new GuestReservations(GuestMainWindow.user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(GuestReservations);
            GuestMainWindow.NavigationButtonBarPressed("ReservationsButton");
        }
        public void ReviewsPage()
        {
            OwnerReviews = new OwnerReviews(GuestMainWindow.user);
            GuestMainWindow.mainFrame.Navigate(OwnerReviews);
            GuestMainWindow.NavigationButtonBarPressed("ReviewsButton");
        }
        public void ForumPage()
        {
            GuestForum = new GuestForum(GuestMainWindow.user);
            GuestMainWindow.mainFrame.Navigate(GuestForum);
            GuestMainWindow.NavigationButtonBarPressed("ForumButton");
        }
        public void LogOutWindow()
        {

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            GuestMainWindow.Close();
        }
        public void TutorialWindow()
        {
            GuestTutorial guestTutorial = new GuestTutorial();
            guestTutorial.Show();
        }
        public void NotificationsWindow()
        {
            GuestNotifications guestNotifications = new GuestNotifications(GuestMainWindow.user, GuestMainWindow);
            guestNotifications.Show();
        }
    }
}
