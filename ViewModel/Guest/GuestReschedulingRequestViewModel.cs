using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
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
    public class GuestReschedulingRequestViewModel
    {
        public ReschedulingReservation reschedulingReservation { get; set; }

        public ReservedAccommodation reservedAccommodation { get; set; }

        public User User { get; set; }

        public Accommodation? accommodation { get; set; }

        public RelayCommand sendRequest => new RelayCommand(execute => SendRequest(), canExecute => AvailableSendRequest());

        public GuestReschedulingRequestViewModel(User user, ReschedulingReservation reschedulingReservation, ReservedAccommodation selectedReservedAccommodation)
        {
            this.reschedulingReservation = reschedulingReservation;
            this.User = user;
            reservedAccommodation = selectedReservedAccommodation;
            accommodation = AccommodationService.GetInstance().GetById(reservedAccommodation.accommodationId);
            reschedulingReservation.NameLabel.Content += reservedAccommodation.AccommodationName;
            reschedulingReservation.DatesLabel.Content += reservedAccommodation.checkInDate + " - " + reservedAccommodation.checkOutDate;
            reschedulingReservation.MinDaysLabel.Content += accommodation.MinReservationDays.ToString();
        }

        public void SendRequest()
        {
            DateTime checkIn = reschedulingReservation.checkInDatePicker.SelectedDate ?? DateTime.Now;
            DateTime checkOut = reschedulingReservation.checkOutDatePicker.SelectedDate ?? DateTime.Now;
            GuestReschedulingRequest request = new GuestReschedulingRequest();
            request.accommodationId = reservedAccommodation.accommodationId;
            request.guestId = User.Id;
            request.CheckInDate = checkIn;
            request.CheckOutDate = checkOut;

            GuestReschedulingRequestService.GetInstance().Add(request);
            reschedulingReservation.Close();
        }

        public bool AvailableSendRequest()
        {
            DateTime checkIn = reschedulingReservation.checkInDatePicker.SelectedDate ?? DateTime.Now;
            DateTime checkOut = reschedulingReservation.checkOutDatePicker.SelectedDate ?? DateTime.Now;
            checkOut = checkOut.AddHours(2);
            if (string.IsNullOrEmpty(reschedulingReservation.checkInDatePicker.ToString()) || string.IsNullOrEmpty(reschedulingReservation.checkOutDatePicker.ToString()))
            {
                return false;
            }
            else if((checkOut - checkIn).Days < accommodation.MinReservationDays)
            {
                return false;
            }
            return true;
        }
    }
}
