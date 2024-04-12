using BookingApp.Domain.Model;
using BookingApp.View.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Owner
{
    public class OwnerMainWindowViewModel
    {
        public User User { get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public GuestReviewsViewModel GuestReviewsViewModel { get; set; }
        public OwnerMainWindowViewModel(OwnerMainWindow OwnerMainWindow, User User)
        {
            this.User = User;
            this.OwnerMainWindow = OwnerMainWindow;
            GuestReviewsViewModel = new GuestReviewsViewModel(User);
            if (GuestReviewsViewModel.MainWindowIsSuperOwner())
            {
                OwnerMainWindow.starImage.Visibility = Visibility.Visible;
            }
            else
            {
                OwnerMainWindow.starImage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
