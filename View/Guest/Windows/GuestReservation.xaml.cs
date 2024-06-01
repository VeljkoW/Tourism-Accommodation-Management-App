using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            ValidateStartDate.Text = "*Select date!";
            ValidateStartDate.Visibility = Visibility.Visible;
            ValidateEndDate.Text = "*Select date!";
            ValidateEndDate.Visibility = Visibility.Visible;
            ValidateTextBoxDays.Text = "*Input days!";
            ValidateTextBoxDays.Visibility = Visibility.Visible;
            ValidateTextBoxGuest.Visibility = Visibility.Hidden;
            GuestNumberTextBox.IsEnabled = false;

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

        private void InputGuestNumber(object sender, TextChangedEventArgs e)
        {

            Regex TextNumberRegex = new Regex("^[0-9]*$");
            if (string.IsNullOrEmpty(GuestNumberTextBox.Text) || string.IsNullOrWhiteSpace(GuestNumberTextBox.Text))
            {
                ValidateTextBoxGuest.Text = "*Input guest number!";
                ValidateTextBoxGuest.Visibility = Visibility.Visible;
            }
            else if (!TextNumberRegex.Match(GuestNumberTextBox.Text).Success && !string.IsNullOrEmpty(GuestNumberTextBox.Text) && !string.IsNullOrWhiteSpace(GuestNumberTextBox.Text))
            {
                ValidateTextBoxGuest.Text = "*Only numbers!";
                ValidateTextBoxGuest.Visibility = Visibility.Visible;
            }
            else
            {
                ValidateTextBoxGuest.Visibility = Visibility.Hidden;
            }
        }

        private void InputDays(object sender, TextChangedEventArgs e)
        {
            Regex TextNumberRegex = new Regex("^[0-9]*$");
            if (string.IsNullOrEmpty(ReservationDaysTextBox.Text) || string.IsNullOrWhiteSpace(ReservationDaysTextBox.Text))
            {
                ValidateTextBoxDays.Text = "*Input days!";
                ValidateTextBoxDays.Visibility = Visibility.Visible;
            }
            else if (!TextNumberRegex.Match(ReservationDaysTextBox.Text).Success)
            {
                ValidateTextBoxDays.Text = "*Only numbers!";
                ValidateTextBoxDays.Visibility = Visibility.Visible;
            }
            else
            {
                ValidateTextBoxDays.Visibility = Visibility.Hidden;
            }
        }

        private void ChangedStartDate(object sender, SelectionChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(CheckInDatePicker.Text) || string.IsNullOrWhiteSpace(CheckInDatePicker.Text))
            {
                ValidateStartDate.Text = "*Select date!";
                ValidateStartDate.Visibility = Visibility.Visible;
            }
            else
            {
                ValidateStartDate.Visibility = Visibility.Hidden;
            }
        }

        private void ChangedEndDate(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CheckOutDatePicker.Text) || string.IsNullOrWhiteSpace(CheckOutDatePicker.Text))
            {
                ValidateEndDate.Text = "*Select date!";
                ValidateEndDate.Visibility = Visibility.Visible;
            }
            else
            {
                ValidateEndDate.Visibility = Visibility.Hidden;
            }
        }

        private void GuestCommentTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                // Ukloni fokus sa TextBox
                Keyboard.ClearFocus();

                // Postavi fokus na Window
                FocusManager.SetFocusedElement(this, this);

                // Spreči dalje procesiranje događaja
                e.Handled = true;
            }
        }

        private void DaysTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                // Ukloni fokus sa TextBox
                Keyboard.ClearFocus();

                // Postavi fokus na Window
                FocusManager.SetFocusedElement(this, this);

                // Spreči dalje procesiranje događaja
                e.Handled = true;
            }
        }
    }
}
