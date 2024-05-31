using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using BookingApp.View.Owner;
using BookingApp.ViewModel;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for GuestReservations.xaml
    /// </summary>
    public partial class GuestReservations : Page
    {
        public User user { get; set; }

        public GuestReservationsViewModel GuestReservationsViewModel { get; set; }

        public ReservedAccommodation reservedAccommodation;

        public ReservedAccommodation selectedAccommodation;
        public GuestMainWindow GuestMainWindow { get; set; }

        public GuestReservations(User user, GuestMainWindow guestMainWindow)
        {
            InitializeComponent();
            this.user = user;
            GuestReservationsViewModel = new GuestReservationsViewModel(this, user);
            DataContext = GuestReservationsViewModel;
            selectedAccommodation = new ReservedAccommodation();
            GuestMainWindow = guestMainWindow; 
            Color backgroundButtonPressedColor = (Color)ColorConverter.ConvertFromString("#74877A");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            ReservationsButton.Background = backgroundButtonPressedBrush;
            Color backgroundButtonColor = (Color)ColorConverter.ConvertFromString("#56736F");
            SolidColorBrush backgroundButtonBrush = new SolidColorBrush(backgroundButtonColor);
            StatusesButton.Background = backgroundButtonBrush;
            Color backgroundButton = (Color)ColorConverter.ConvertFromString("#56736F");
            SolidColorBrush backgroundButtons = new SolidColorBrush(backgroundButton);
            HistoryButton.Background = backgroundButtons;

        }

        private void ReschedulingClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            ReschedulingReservation reschedulingReservation = new ReschedulingReservation(user, selectedCard);
            reschedulingReservation.Show();
            reschedulingReservation.Focus();

        }

        private void clickOnCard(object sender, MouseButtonEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            reservedAccommodation = selectedCard;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ReservedAccommodation;
            ReservedAccommodation? reserved = new ReservedAccommodation();
            reserved = ReservedAccommodationService.GetInstance().GetById(selectedCard.Id);
            Accommodation? accommodation = new Accommodation();
            accommodation = AccommodationService.GetInstance().GetById(selectedCard.Accommodation.Id);
            DateTime checkIn = reserved.CheckInDate;
            if ((checkIn - DateTime.Now).Days > accommodation.CancelationDaysLimit)
            {
                GuestCancelReservation guestCancelReservation = new GuestCancelReservation(selectedCard, GuestReservationsViewModel);
                guestCancelReservation.Show();
                guestCancelReservation.Focus();
            }
            else MessageBox.Show("The cancellation deadline has expired!");
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

        private void PdfClick(object sender, RoutedEventArgs e)
        {
            GuestCreatePdf guestCreatePdf = new GuestCreatePdf();
            guestCreatePdf.Show();
        }
    }
}
