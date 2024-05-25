using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
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
    /// Interaction logic for ReservationHistory.xaml
    /// </summary>
    public partial class ReservationHistory : Page
    {
        public User user {  get; set; }
        public GuestMainWindow GuestMainWindow { get; set; }
        public GuestReservationHistoryViewModel GuestReservationHistoryViewModel { get; set; }
        public ReservationHistory(User user, GuestMainWindow guestMainWindow)
        {
            InitializeComponent();
            this.user = user;
            GuestMainWindow = guestMainWindow;
            GuestReservationHistoryViewModel = new GuestReservationHistoryViewModel(this, user);
            DataContext = GuestReservationHistoryViewModel;
        }

        private void RescheduleClick(object sender, RoutedEventArgs e)
        {
            //RescheduleStatus rescheduleStatus = new RescheduleStatus(user);
            //rescheduleStatus.Show();
            //rescheduleStatus.Focus();
            ReschedulingStatuses reschedulingStatuses = new ReschedulingStatuses(user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(reschedulingStatuses);
        }

        private void ReservationsClick(object sender, RoutedEventArgs e)
        {
            GuestReservations guestReservations = new GuestReservations(user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(guestReservations);
        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            ReservationHistory reservationHistory = new ReservationHistory(user, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(reservationHistory);
        }

        private void RateItClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            ReservedAccommodation? reserved = new ReservedAccommodation();
            reserved = ReservedAccommodationService.GetInstance().GetById(selectedCard.Id);
            if ((DateTime.Now - reserved.CheckOutDate).Days <= 5 && reserved.CheckOutDate < DateTime.Now)
            {
            GuestRate guestRate = new GuestRate(user, selectedCard);
            guestRate.Show();
            guestRate.Focus();
            }
            else
            {
                MessageBox.Show("You can not rate the owner!!");
            }
        }
    }
}
