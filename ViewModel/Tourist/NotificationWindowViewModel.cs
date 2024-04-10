using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class NotificationWindowViewModel
    {
        public NotificationWindow NotificationWindow { get; set; }
        public NotificationWindowViewModel(NotificationWindow notificationWindow) 
        { 
            this.NotificationWindow = notificationWindow;
        }
        public void Close(object sender, RoutedEventArgs e)
        {
            NotificationWindow.Close();
        }
    }
}
