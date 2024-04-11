using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest.Pages;
using System.Collections.ObjectModel;
using BookingApp.Services;

namespace BookingApp.ViewModel.Guest
{
    public class GuestReservationsViewModel
    {
        public GuestReservations GuestReservations { get; set; }

        public ObservableCollection<ReservedAccommodation> reservedAccommodations { get; set; }
        public User user { get; set; }
        public GuestReservationsViewModel(User user)
        {
            this.user = user;
            reservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            foreach (ReservedAccommodation reserved in ReservedAccommodationService.GetInstance().GetAll())
            {
                reservedAccommodations.Add(reserved);
            }
        }

    }
}
