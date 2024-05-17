using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
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
        public ReservedAccommodation reservedAccommodation { get; set; }

        public GuestCancelReservation GuestCancelReservation { get; set; }

        public GuestReservationsViewModel GuestReservationsViewModel { get; set; }

        public RelayCommand cancelReservation => new RelayCommand(execute => CancelReservation());

        public RelayCommand closeWindow => new RelayCommand(execute => CloseWindow());
        public GuestCancelReservationViewModel(ReservedAccommodation selectedReservedAccommodation, GuestCancelReservation guestCancelReservation, GuestReservationsViewModel guestReservationsViewModel) 
        { 
            reservedAccommodation = selectedReservedAccommodation;
            GuestCancelReservation = guestCancelReservation;
            GuestReservationsViewModel = guestReservationsViewModel;
            GuestCancelReservation.AccommodationInfo.Content += selectedReservedAccommodation.Accommodation.Name + ", Location: " + selectedReservedAccommodation.Accommodation.Location.State + " - " + selectedReservedAccommodation.Accommodation.Location.City;
        }

        public void CloseWindow()
        {
            GuestCancelReservation.Close();
        }

        public void CancelReservation()
        {
            ReservationCancellation reservationCancellation = new ReservationCancellation();
            reservationCancellation.AccommodationId = reservedAccommodation.Accommodation.Id;
            reservationCancellation.GuestId = reservedAccommodation.GuestId;
            reservationCancellation.CancelDate = DateTime.Now;
            ReservationCancellationService.GetInstance().Add(reservationCancellation);

            ReservedAccommodationService.GetInstance().Delete(reservedAccommodation);
            GuestReservationsViewModel.reservedAccommodations.Clear();
            foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().Update(GuestReservationsViewModel.user))
                GuestReservationsViewModel.reservedAccommodations.Add(reservedAccommodation);
            GuestCancelReservation.Close();
            
        }
    }
}
