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
    /// Interaction logic for GuestCancelReservation.xaml
    /// </summary>
    public partial class GuestCancelReservation : Window
    {
        public GuestCancelReservationViewModel GuestCancelReservationViewModel { get; set; }
        public GuestCancelReservation(ReservedAccommodation reservedAccommodation, GuestReservationsViewModel guestReservationsViewModel)
        {
            InitializeComponent();
            GuestCancelReservationViewModel = new GuestCancelReservationViewModel(reservedAccommodation, this, guestReservationsViewModel);
            DataContext = GuestCancelReservationViewModel;
        }
    }
}
