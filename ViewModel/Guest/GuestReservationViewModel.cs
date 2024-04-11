using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GuestReservations = BookingApp.View.Guest.Windows.GuestReservation;
using System.Collections.ObjectModel;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationViewModel
    {
        public User user { get; set; }
        public GuestReservations GuestReservations { get; set; }
        public ObservableCollection<AvailableDate> printDates { get; set; }

        public ReservedAccommodation reservedAccommodation { get; set; }

        public Accommodation Accommodation { get; set; }

        public RelayCommand ReservationSearchButton => new RelayCommand(execute => ReservationSearch(), canExecute => AvailableReservationSearch());
        public RelayCommand ReservationClickButton => new RelayCommand(execute => ReservationClick(), canExecute => AvailableReservationClick());


        public GuestReservationViewModel(GuestReservations GuestReservations, Accommodation selectedAccommodation, User user) 
        {
            this.GuestReservations = GuestReservations;
            reservedAccommodation = new ReservedAccommodation();
            Accommodation = selectedAccommodation;
            printDates = new ObservableCollection<AvailableDate>();
            GuestReservations.GuestNumberTextBox.Text = "Max guest number " + selectedAccommodation.MaxGuestNumber;
            GuestReservations.ReservationDaysTextBox.Text = "Min reservation days " + selectedAccommodation.MinReservationDays;
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
                foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                {
                    if (Accommodation.Id == reservedAccommodation.accommodationId)
                    {
                        if (date > reservedAccommodation.checkInDate && date < reservedAccommodation.checkOutDate)
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
        public bool IsNumeric(string text)
        {
            try
            {
                int number = int.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AvailableReservationSearch() 
        {
            if (!IsNumeric(GuestReservations.ReservationDaysTextBox.Text))
            {
                return false;
            }
            if (Convert.ToInt32(GuestReservations.ReservationDaysTextBox.Text) < GuestReservations.accommodation.MinReservationDays)
            {
                return false;
            }

            return true;
        }

        public void ReservationSearch()
        {
            printDates.Clear();
            GuestReservations.AvailableDates.ItemsSource = printDates;
            DateTime startDate = GuestReservations.CheckInDatePicker.SelectedDate ?? DateTime.Now;
            DateTime endDate = GuestReservations.CheckOutDatePicker.SelectedDate ?? DateTime.Now;

            int numberOfDays = (endDate - startDate).Days;
            startDate = startDate.AddHours(12);
            endDate = endDate.AddHours(10);
            int reservationDays = Convert.ToInt32(GuestReservations.ReservationDaysTextBox.Text);

            List<DateTime> availableDates = new List<DateTime>();
            availableDates = FindAvailableDates(startDate, endDate, numberOfDays, reservationDays);

            GuestReservations.ReservationButton.IsEnabled = true;
            GuestReservations.GuestNumberTextBox.IsEnabled = true;

            if (availableDates.Count != 0)
            {
                GuestReservations.AvailableDates.IsEnabled = true;
                GuestReservations.AvailableDates.ItemsSource = availableDates;
                foreach (DateTime availableDate in availableDates)
                {
                    AvailableDate dates = new AvailableDate();
                    dates.checkInDate = availableDate;
                    dates.checkOutDate = availableDate.AddDays(reservationDays - 1).AddHours(22);
                    printDates.Add(dates);
                }
                GuestReservations.AvailableDates.ItemsSource = printDates;
            }
            else
            {
                GuestReservations.AvailableDates.ItemsSource = "Nema datuma";
            }
        }


        public bool AvailableReservationClick()
        {
            if (GuestReservations.AvailableDates.SelectedValue == null)
            {
                return false;
            }
            else if (GuestReservations.GuestNumberTextBox.Text.Equals("") || GuestReservations.GuestNumberTextBox.Text.Equals("Max guest number " + Accommodation.MaxGuestNumber))
            {
                return false;
            }
            else if (!int.TryParse(GuestReservations.GuestNumberTextBox.Text, out int guestNumber) || guestNumber <= 0)
            {
                return false;
            }
            else if (Convert.ToInt32(GuestReservations.GuestNumberTextBox.Text) > Accommodation.MaxGuestNumber)
            {
                return false;
            }
            else 
            {
                return true;
            }
            
        }
        public void ReservationClick() 
        {
            string? selectedDate = GuestReservations.AvailableDates.SelectedValue.ToString();
            string[] dates = selectedDate.Split('-');
            reservedAccommodation.checkInDate = Convert.ToDateTime(dates[0].Trim());
            reservedAccommodation.checkOutDate = Convert.ToDateTime(dates[1].Trim());
            reservedAccommodation.accommodationId = Accommodation.Id;
            reservedAccommodation.guestId = user.Id;
            reservedAccommodation.accommodationName = Accommodation.Name;
            reservedAccommodation.location = Accommodation.Location;
            reservedAccommodation.image = Accommodation.Images[0];
            reservedAccommodation.accommodationType = Accommodation.AccommodationType;
            ReservedAccommodationService.GetInstance().Add(reservedAccommodation);
            GuestReservations.Close();
        }
       /* public void ReservationSearchButton(object sender, RoutedEventArgs e)
        {
            if (!IsNumeric(GuestReservations.ReservationDaysTextBox.Text))
            {
                GuestReservations.ErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            if (Convert.ToInt32(GuestReservations.ReservationDaysTextBox.Text) < GuestReservations.accommodation.MinReservationDays)
            {
                GuestReservations.ErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                GuestReservations.ErrorLabel.Visibility = Visibility.Collapsed;
                printDates.Clear();
                GuestReservations.AvailableDates.ItemsSource = printDates;
                DateTime startDate = GuestReservations.CheckInDatePicker.SelectedDate ?? DateTime.Now;
                DateTime endDate = GuestReservations.CheckOutDatePicker.SelectedDate ?? DateTime.Now;

                int numberOfDays = (endDate - startDate).Days;
                startDate = startDate.AddHours(12);
                endDate = endDate.AddHours(10);
                int reservationDays = Convert.ToInt32(GuestReservations.ReservationDaysTextBox.Text);

                List<DateTime> availableDates = new List<DateTime>();
                availableDates = FindAvailableDates(startDate, endDate, numberOfDays, reservationDays);

                GuestReservations.ReservationButton.IsEnabled = true;
                GuestReservations.GuestNumberTextBox.IsEnabled = true;

                if (availableDates.Count != 0)
                {
                    GuestReservations.AvailableDates.IsEnabled = true;
                    GuestReservations.AvailableDates.ItemsSource = availableDates;
                    foreach (DateTime availableDate in availableDates)
                    {
                        AvailableDate dates = new AvailableDate();
                        dates.checkInDate = availableDate;
                        dates.checkOutDate = availableDate.AddDays(reservationDays - 1).AddHours(22);
                        printDates.Add(dates);
                    }
                    GuestReservations.AvailableDates.ItemsSource = printDates;
                }
                else
                {
                    GuestReservations.AvailableDates.ItemsSource = "Nema datuma";
                }
            }
        }*/

        public void AvailableDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GuestReservations.AvailableDates.SelectedItem != null)
            {
                GuestReservations.selectedDates = GuestReservations.AvailableDates.SelectedItem.ToString();
            }
        }
       /* public void ReservationClickButton(object sender, RoutedEventArgs e)
        {
            if (GuestReservations.AvailableDates.SelectedValue == null)
            {
                GuestReservations.InvalidInput.Visibility = Visibility.Collapsed;
                GuestReservations.ErrorSelect.Visibility = Visibility.Visible;
                return;
            }
            else if (GuestReservations.GuestNumberTextBox.Text.Equals("") || GuestReservations.GuestNumberTextBox.Text.Equals("Max guest number " + Accommodation.MaxGuestNumber))
            {
                GuestReservations.ErrorSelect.Visibility = Visibility.Collapsed;
                GuestReservations.InvalidInput.Visibility = Visibility.Visible;
                return;
            }
            else if (!int.TryParse(GuestReservations.GuestNumberTextBox.Text, out int guestNumber) || guestNumber <= 0)
            {
                GuestReservations.ErrorSelect.Visibility = Visibility.Collapsed;
                GuestReservations.InvalidInput.Visibility = Visibility.Visible;
                return;
            }
            else if (Convert.ToInt32(GuestReservations.GuestNumberTextBox.Text) > Accommodation.MaxGuestNumber)
            {
                GuestReservations.ErrorSelect.Visibility = Visibility.Collapsed;
                GuestReservations.InvalidInput.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                GuestReservations.ErrorSelect.Visibility = Visibility.Collapsed;
                GuestReservations.InvalidInput.Visibility = Visibility.Collapsed;
                string? selectedDate = GuestReservations.AvailableDates.SelectedValue.ToString();
                string[] dates = selectedDate.Split('-');
                reservedAccommodation.checkInDate = Convert.ToDateTime(dates[0].Trim());
                reservedAccommodation.checkOutDate = Convert.ToDateTime(dates[1].Trim());
                reservedAccommodation.accommodationId = Accommodation.Id;
                reservedAccommodation.guestId = user.Id;
                ReservedAccommodationService.GetInstance().Add(reservedAccommodation);
                GuestReservations.Close();
            }
        }*/
    }
}
