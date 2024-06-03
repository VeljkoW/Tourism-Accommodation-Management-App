using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Guest.Windows;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReschedulingRequestViewModel
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public ReschedulingReservation reschedulingReservation { get; set; }

        public ReservedAccommodation reservedAccommodation { get; set; }
        public RelayCommand FocusStartDatePicker => new RelayCommand(execute => FocusStartDate());
        public RelayCommand FocusEndDatePicker => new RelayCommand(execute => FocusEndDate());
        public RelayCommand Exit => new RelayCommand(execute => CloseWindow());
        public User User { get; set; }

        public Accommodation? accommodation { get; set; }

        public RelayCommand sendRequest => new RelayCommand(execute => SendRequest(), canExecute => AvailableSendRequest());

        public GuestReschedulingRequestViewModel(User user, ReschedulingReservation reschedulingReservation, ReservedAccommodation selectedReservedAccommodation)
        {
            this.reschedulingReservation = reschedulingReservation;
            this.User = user;
            reservedAccommodation = selectedReservedAccommodation;
            accommodation = AccommodationService.GetInstance().GetById(reservedAccommodation.Accommodation.Id);
            reschedulingReservation.NameLabel.Content += reservedAccommodation.Accommodation.Name + ", " + reservedAccommodation.Accommodation.Location.State;
            reschedulingReservation.DatesLabel.Content += reservedAccommodation.CheckInDate.ToString("dd/MM/yyyy HHtt") + " - " + reservedAccommodation.CheckOutDate.ToString("dd/MM/yyyy HHtt");
            reschedulingReservation.MinDaysLabel.Content += accommodation.MinReservationDays.ToString();
        }
        public void CloseWindow()
        {
            reschedulingReservation.Close();
        }
        public void FocusStartDate()
        {
            reschedulingReservation.checkInDatePicker.IsDropDownOpen = true;
        }
        public void FocusEndDate()
        {
            reschedulingReservation.checkOutDatePicker.IsDropDownOpen = true;
        }
        public void SendRequest()
        {
            DateTime checkIn = reschedulingReservation.checkInDatePicker.SelectedDate ?? DateTime.Now;
            DateTime checkOut = reschedulingReservation.checkOutDatePicker.SelectedDate ?? DateTime.Now;
            GuestReschedulingRequest request = new GuestReschedulingRequest();
            request.AccommodationId = reservedAccommodation.Accommodation.Id;
            request.GuestId = User.Id;
            request.CheckInDate = checkIn.AddHours(12);
            request.CheckOutDate = checkOut.AddHours(10);
            request.ReservedAccommodationId = reservedAccommodation.Id;

            GuestReschedulingRequestService.GetInstance().Add(request);
            reschedulingReservation.Close();
            notificationManager.Show("Success", "Rescheduling Request Successfully sent!", NotificationType.Success);
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

            reschedulingReservation.checkInDatePicker.IsDropDownOpen = false;
            reschedulingReservation.checkOutDatePicker.IsDropDownOpen = false;
            return true;
        }
    }
}
