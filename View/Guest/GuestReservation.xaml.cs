using BookingApp.Model;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestReservation.xaml
    /// </summary>
    public partial class GuestReservation : Window
    {
        public User user { get; set; }
        public Accommodation accommodation { get; set; }
        public ReservedAccommodationRepository reservedAccommodationRepository { get; set; }

        public ReservedAccommodation reservedAccommodation { get; set; }

        public GuestReservation(Accommodation selectedAccommodation, User logUser)
        {
            InitializeComponent();
            accommodation = selectedAccommodation;
            user = logUser;
            reservedAccommodationRepository = new ReservedAccommodationRepository();
            reservedAccommodation = new ReservedAccommodation();
            GuestNumberTextBox.Text = "Max guest number " + accommodation.MaxGuestNumber;
            ReservationDaysTextBox.Text = "Min reservation days " + accommodation.MinReservationDays;
            ErrorLabel.Visibility = Visibility.Collapsed;
            ReservationButton.IsEnabled = false;
            GuestNumberTextBox.IsEnabled = false;
            AvailableDates.IsEnabled = false;
            this.user = user;
        }
        private bool AreDatesAvailable(DateTime startDate, DateTime endDate, int reservationDays)
        {
            // Provera da li je endDate nakon startDate
            if (endDate <= startDate)
            {
                return false;
            }

            // Provera da li je broj dana veći od razlike između startDate i endDate
            if ((endDate - startDate).Days < reservationDays)
            {
                return false;
            }

            // Provera dostupnosti datuma
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                foreach(ReservedAccommodation reservedAccommodation in reservedAccommodationRepository.GetAll()) 
                {
                    if(accommodation.Id == reservedAccommodation.accommodationId)
                    {
                        if(date >= reservedAccommodation.checkInDate && date <= reservedAccommodation.checkOutDate) 
                        {
                            continue;
                        }
                    }
                }
            }

            // Ako prođemo sve prethodne provere, smatramo da su datumi dostupni
            return true;
        }
        private List<DateTime> FindAvailableDates(DateTime startDate, DateTime endDate, int numberOfDays, int reservationDays)
        {
            List<DateTime> availableDates = new List<DateTime>();

            DateTime currentStartDate = startDate;
            DateTime currentEndDate = endDate;

            // Pretraga slobodnih datuma unutar izabranog opsega
            while (currentStartDate <= currentEndDate)
            {
                // Provera dostupnosti datuma
                if (AreDatesAvailable(currentStartDate, currentEndDate, reservationDays))
                {
                    availableDates.Add(currentStartDate);
                }

                currentStartDate = currentStartDate.AddDays(1);
            }

            // Ako nije pronađen nijedan slobodan datum unutar izabranog opsega
            // Tražimo ostale dostupne datume van opsega
            if (availableDates.Count == 0)
            {
                currentStartDate = startDate;
                currentEndDate = endDate;
                int counterDates = 0;

                // Pretraga slobodnih datuma van izabranog opsega
                while (true)
                {
                    // Tražimo sledeći slobodan datum nakon izabranog opsega
                    currentStartDate = currentEndDate.AddDays(1);
                    currentEndDate = currentStartDate.AddDays(numberOfDays - 1);

                    if (counterDates == 5)
                    {
                        break; // Prekidamo petlju ako smo dostigli maksimalnu vrednost datuma
                    }

                    // Provera dostupnosti datuma
                    if (AreDatesAvailable(currentStartDate, currentEndDate, reservationDays))
                    {
                        availableDates.Add(currentStartDate);
                        counterDates++;
                    }
                }
            }

            return availableDates;
        }

        private void ReservationSearchButton(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(ReservationDaysTextBox.Text) < accommodation.MinReservationDays) 
            {
                ErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                DateTime startDate = CheckInDatePicker.SelectedDate ?? DateTime.Now;
                DateTime endDate = CheckOutDatePicker.SelectedDate ?? DateTime.Now;

                int numberOfDays = (endDate - startDate).Days;
                int reservationDays = Convert.ToInt32(ReservationDaysTextBox.Text);

                List<DateTime> availableDates = FindAvailableDates(startDate, endDate, numberOfDays, reservationDays);

                if (availableDates.Count == 1)
                {
                    AvailableDates.IsEnabled = true;
                    ReservationButton.IsEnabled = true;
                    ReservationDaysTextBox.IsEnabled = true;
                }
                else if(availableDates.Count > 1)
                {
                    AvailableDates.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Nema datuma");
                }
            }
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
        private void ReservationClickButton(object sender, RoutedEventArgs e)
        {
            ErrorLabel.Visibility = Visibility.Collapsed;

            reservedAccommodation.checkInDate = CheckInDatePicker.SelectedDate ?? DateTime.Now;
            reservedAccommodation.checkOutDate = CheckOutDatePicker.SelectedDate ?? DateTime.Now;
            reservedAccommodation.accommodationId = accommodation.Id;
            reservedAccommodation.guestId = user.Id;
            reservedAccommodationRepository.Add(reservedAccommodation);
        }
    }
}
