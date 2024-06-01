using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
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

namespace BookingApp.View.Guest.Pages
{
    /// <summary>
    /// Interaction logic for ReschedulingStatuses.xaml
    /// </summary>
    public partial class ReschedulingStatuses : Page
    {
        public User user { get; set; }

        public GuestMainWindow GuestMainWindow { get; set; }

        public ReschedulingStatusesViewModel reschedulingStatusesViewModel { get; set; }

        public ReschedulingStatuses(User user, GuestMainWindow guestMainWindow)
        {
            this.user = user;
            GuestMainWindow = guestMainWindow;
            reschedulingStatusesViewModel = new ReschedulingStatusesViewModel(this, user);
            InitializeComponent();
            DataContext = reschedulingStatusesViewModel;
            Color backgroundButtonPressedColor = (Color)ColorConverter.ConvertFromString("#56736F");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            ReservationsButton.Background = backgroundButtonPressedBrush;
            Color backgroundButtonColor = (Color)ColorConverter.ConvertFromString("#74877A");
            SolidColorBrush backgroundButtonBrush = new SolidColorBrush(backgroundButtonColor);
            StatusesButton.Background = backgroundButtonBrush;
            Color backgroundButton = (Color)ColorConverter.ConvertFromString("#56736F");
            SolidColorBrush backgroundButtons = new SolidColorBrush(backgroundButton);
            HistoryButton.Background = backgroundButtons;
            MainGrid.Focus();
        }

        private void ReservationsClick(object sender, RoutedEventArgs e)
        {
            GuestReservations guestReservations = new GuestReservations(user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(guestReservations);
        }
        private void RescheduleClick(object sender, RoutedEventArgs e)
        {
            //RescheduleStatus rescheduleStatus = new RescheduleStatus(user);
            //rescheduleStatus.Show();
            //rescheduleStatus.Focus();
            ReschedulingStatuses reschedulingStatuses = new ReschedulingStatuses(user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(reschedulingStatuses);
        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            ReservationHistory reservationHistory = new ReservationHistory(user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(reservationHistory);
        }
    }
}
