using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
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
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestReservation.xaml
    /// </summary>
    public partial class GuestReservation : Window
    {
        public GuestReservationViewModel GuestReservationViewModel { get; set; }
        public User user { get; set; }
        public Accommodation accommodation { get; set; }
        public ReservedAccommodationRepository reservedAccommodationRepository { get; set; }
        public ReservedAccommodation reservedAccommodation { get; set; }
        public string? selectedDates { get; set; }
        public List<AvailableDate> printDates { get; set; }
        public GuestReservation(Accommodation selectedAccommodation, User logUser)
        {
            InitializeComponent();
            GuestReservationViewModel = new GuestReservationViewModel(this, selectedAccommodation, logUser);
            this.DataContext = GuestReservationViewModel;
            accommodation = selectedAccommodation;
            
        }
        private void GuestNumber_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Max guest number " + accommodation.MaxGuestNumber)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void GuestNumber_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Max guest number " + accommodation.MaxGuestNumber;
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void ReservationDays_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Min reservation days " + accommodation.MinReservationDays)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }
        private void ReservationDays_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Min reservation days " + accommodation.MinReservationDays;
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void AvailableDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GuestReservationViewModel.AvailableDates_SelectionChanged(sender, e);
        }
        private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            GuestReservationViewModel.PreviousImage(sender, e);
            //((GuestReservationViewModel)DataContext).PreviousImage();
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            GuestReservationViewModel.NextImage(sender, e);
            //((GuestReservationViewModel)DataContext).NextImage();
        }
    }
}
