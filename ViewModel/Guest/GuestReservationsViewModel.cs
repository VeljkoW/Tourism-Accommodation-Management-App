using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest.Pages;
using System.Collections.ObjectModel;
using BookingApp.Services;
using Guests = BookingApp.View.Guest.Pages.GuestReservations;
using System.Reflection.Metadata.Ecma335;
using BookingApp.View.Guest.Windows;
using System.Windows;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationsViewModel
    {
        public Guests guestReservations { get; set; }

        public ObservableCollection<ReservedAccommodation> reservedAccommodations { get; set; }
        public User user { get; set; }
        public GuestReservationsViewModel(Guests guestReservations, User user)
        {
            this.user = user;
            reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            this.guestReservations = guestReservations;
            reservedAccommodations = ReservedAccommodationService.GetInstance().Update(user);
        }

    }
}
