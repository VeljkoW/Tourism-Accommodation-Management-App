using BookingApp.Domain.Model;
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
        public ReschedulingStatuses(User user, GuestMainWindow guestMainWindow)
        {
            this.user = user;
            GuestMainWindow = guestMainWindow;
            InitializeComponent();
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
