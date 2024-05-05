using BookingApp.Services;
using BookingApp.View.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guest
{
    public class GuestMainWindowViewModel
    {
        public GuestMainWindow GuestMainWindow { get; set; }
        public GuestMainWindowViewModel(GuestMainWindow guestMainWindow)
        {
            GuestMainWindow = guestMainWindow;
            GuestBonusService.GetInstance().UpdateAll();
        }
    }
}
