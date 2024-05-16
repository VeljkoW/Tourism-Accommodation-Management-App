using BookingApp.Services;
using BookingApp.View.Guest;
using BookingApp.Domain.Model;
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
        public GuestMainWindowViewModel(GuestMainWindow guestMainWindow, User user)
        {
            GuestMainWindow = guestMainWindow;
            GuestMainWindow.Username.Content = user.Username;
            GuestBonusService.GetInstance().UpdateAll();
        }
    }
}
