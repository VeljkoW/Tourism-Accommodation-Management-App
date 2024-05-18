using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest;
using BookingApp.View.Guest.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationHistoryViewModel
    {
        public User user { get; set; }

        public ReservationHistory ReservationHistory { get; set; }

        public ReservedAccommodation reservedAccommodation;

        public ReservedAccommodation selectedAccommodation;
        public ObservableCollection<ReservedAccommodation> reservedAccommodations { get; set; }

        public bool IsRateButtonVisible {  get; set; }
        public GuestMainWindow GuestMainWindow { get; set; }

       
        public GuestReservationHistoryViewModel(ReservationHistory reservationHistory, User user) 
        {
            ReservationHistory = reservationHistory;
            this.user = user; 
            reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().Update(user))
                if (reservedAccommodation.CheckOutDate <= DateTime.Now)
                    reservedAccommodations.Add(reservedAccommodation);
        }

    }
}
