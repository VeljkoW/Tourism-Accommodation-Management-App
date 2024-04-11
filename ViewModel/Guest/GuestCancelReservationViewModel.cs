using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestCancelReservationViewModel
    {
        public User User { get; set; }
        public ReservedAccommodation reservedAccommodation { get; set; }

        public GuestCancelReservation GuestCancelReservation { get; set; }

        public RelayCommand cancelReservation => new RelayCommand(execute => CancelReservation());

        public RelayCommand closeWindow => new RelayCommand(execute => CloseWindow());
        public GuestCancelReservationViewModel(User user, ReservedAccommodation selectedReservedAccommodation, GuestCancelReservation guestCancelReservation) 
        { 
            User = user;
            reservedAccommodation = selectedReservedAccommodation;
            GuestCancelReservation = guestCancelReservation;
        }

        public void CloseWindow()
        {
            GuestCancelReservation.Close();
        }

        public void CancelReservation()
        {
            ReservedAccommodationService.GetInstance().Delete(reservedAccommodation);
            GuestCancelReservation.Close();
        }
    }
}
