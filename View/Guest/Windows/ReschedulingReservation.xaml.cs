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
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for ReschedulingReservation.xaml
    /// </summary>
    public partial class ReschedulingReservation : Window
    {

        public GuestReschedulingRequestViewModel GuestReschedulingRequestViewModel { get; set; }
        public ReservedAccommodation selectedReservedAccommodation { get; set; }
        public ReschedulingReservation(User user, ReservedAccommodation reservedAccommodation)
        {
            InitializeComponent();
            selectedReservedAccommodation = reservedAccommodation;
            GuestReschedulingRequestViewModel = new GuestReschedulingRequestViewModel(user, this, selectedReservedAccommodation);
            DataContext = GuestReschedulingRequestViewModel;
            ValidateStartDate.Text = "*Select date!";
            ValidateStartDate.Visibility = Visibility.Visible;
            ValidateEndDate.Text = "*Select date!";
            ValidateEndDate.Visibility = Visibility.Visible;
        }

        private void SelectInDate(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(checkInDatePicker.Text) || string.IsNullOrWhiteSpace(checkInDatePicker.Text))
            {
                ValidateStartDate.Text = "*Select date!";
                ValidateStartDate.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ValidateStartDate.Visibility = Visibility.Hidden;
            }
        }

        private void SelectOutDate(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(checkOutDatePicker.Text) || string.IsNullOrWhiteSpace(checkOutDatePicker.Text))
            {
                ValidateEndDate.Text = "*Select date!";
                ValidateEndDate.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ValidateEndDate.Visibility = Visibility.Hidden;
            }
        }
    }
}
