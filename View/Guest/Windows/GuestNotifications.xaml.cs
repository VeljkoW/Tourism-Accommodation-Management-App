using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestNotifications.xaml
    /// </summary>
    public partial class GuestNotifications : Window
    {
        public GuestNotificationsViewModel GuestNotificationsViewModel { get; set; }

        public GuestNotifications(User user)
        {
            InitializeComponent();
            GuestNotificationsViewModel = new GuestNotificationsViewModel(user);
            DataContext = GuestNotificationsViewModel;
        }
    }
}
