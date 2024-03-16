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
        public string? selectedDates { get; set; }
        public List<AvailableDate> printDates { get; set; }
        public GuestReservation(Accommodation selectedAccommodation, User logUser)
        {
            InitializeComponent();
            this.DataContext = this;
            accommodation = selectedAccommodation;
            user = logUser;
            reservedAccommodationRepository = new ReservedAccommodationRepository();
            reservedAccommodation = new ReservedAccommodation();
            printDates = new List<AvailableDate>();
            GuestNumberTextBox.Text = "Max guest number " + accommodation.MaxGuestNumber;
            ReservationDaysTextBox.Text = "Min reservation days " + accommodation.MinReservationDays;
            ErrorLabel.Visibility = Visibility.Collapsed;
            InvalidInput.Visibility = Visibility.Collapsed;
            ErrorSelect.Visibility = Visibility.Collapsed;
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
            for (DateTime date = startDate; date <= startDate.AddDays(reservationDays); date = date.AddDays(1))
            {
                foreach(ReservedAccommodation reservedAccommodation in reservedAccommodationRepository.GetAll()) 
                {
                    if(accommodation.Id == reservedAccommodation.accommodationId)
                    {
                        if(date > reservedAccommodation.checkInDate && date < reservedAccommodation.checkOutDate) 
                        {
                            return false;
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
            while (currentStartDate <= currentEndDate.AddHours(2))
            {
                // Provera dostupnosti datuma
                if (AreDatesAvailable(currentStartDate, currentEndDate.AddHours(2), reservationDays))
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
                currentEndDate = endDate.AddHours(2);
                int counterDates = 0;

                // Pretraga slobodnih datuma van izabranog opsega
                while (true)
                {
                    // Tražimo sledeći slobodan datum nakon izabranog opsega
                    currentStartDate = currentStartDate.AddDays(1);
                    currentEndDate = currentStartDate.AddDays(reservationDays);

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
                ErrorLabel.Visibility = Visibility.Collapsed;
                printDates.Clear();
                AvailableDates.ItemsSource = printDates;
                DateTime startDate = CheckInDatePicker.SelectedDate ?? DateTime.Now;
                DateTime endDate = CheckOutDatePicker.SelectedDate ?? DateTime.Now;

                int numberOfDays = (endDate - startDate).Days;
                startDate = startDate.AddHours(12);
                endDate = endDate.AddHours(10);
                int reservationDays = Convert.ToInt32(ReservationDaysTextBox.Text);

                List<DateTime> availableDates = new List<DateTime>();
                availableDates = FindAvailableDates(startDate, endDate, numberOfDays, reservationDays);

                ReservationButton.IsEnabled = true;
                GuestNumberTextBox.IsEnabled = true;

                if (availableDates.Count != 0)
                {
                    AvailableDates.IsEnabled = true;
                    AvailableDates.ItemsSource = availableDates;
                    foreach (DateTime availableDate in availableDates)
                    {
                        AvailableDate dates = new AvailableDate();
                        dates.checkInDate = availableDate;
                        dates.checkOutDate = availableDate.AddDays(reservationDays - 1).AddHours(22);
                        printDates.Add(dates);
                    }
                    AvailableDates.ItemsSource = printDates;
                }
                else
                {
                    AvailableDates.ItemsSource = "Nema datuma";
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
        private void AvailableDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AvailableDates.SelectedItem != null)
            {
                // Postavite selectedDates na vrednost odabrane stavke
                selectedDates = AvailableDates.SelectedItem.ToString();
            }
        }
        private void ReservationClickButton(object sender, RoutedEventArgs e)
        {
            if (AvailableDates.SelectedValue == null)
            {
                InvalidInput.Visibility = Visibility.Collapsed;
                ErrorSelect.Visibility = Visibility.Visible;
                return;
            }
            else if (GuestNumberTextBox.Text.Equals("") || GuestNumberTextBox.Text.Equals("Max guest number " + accommodation.MaxGuestNumber))
            {
                ErrorSelect.Visibility = Visibility.Collapsed;
                InvalidInput.Visibility = Visibility.Visible;
                return;
            }
            else if(!int.TryParse(GuestNumberTextBox.Text, out int guestNumber) || guestNumber <= 0)
            {
                ErrorSelect.Visibility = Visibility.Collapsed;
                InvalidInput.Visibility = Visibility.Visible;
                return;
            }
            else if (Convert.ToInt32(GuestNumberTextBox.Text) > accommodation.MaxGuestNumber)
            {
                ErrorSelect.Visibility = Visibility.Collapsed;
                InvalidInput.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ErrorSelect.Visibility = Visibility.Collapsed;
                InvalidInput.Visibility = Visibility.Collapsed;
                string? selectedDate = AvailableDates.SelectedValue.ToString();
                string[] dates = selectedDate.Split('-');
                reservedAccommodation.checkInDate = Convert.ToDateTime(dates[0].Trim());
                reservedAccommodation.checkOutDate = Convert.ToDateTime(dates[1].Trim());
                reservedAccommodation.accommodationId = accommodation.Id;
                reservedAccommodation.guestId = user.Id;
                reservedAccommodationRepository.Add(reservedAccommodation);
                Close();
            }
        }
    }
}
