using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RescheduleStatus.xaml
    /// </summary>
    public partial class RescheduleStatus : Window
    {

        public GuestRescheduleStatusViewModel GuestRescheduleStatusViewModel { get; set; }
        public User user { get; set; }
        public RescheduleStatus(User user)
        {
            InitializeComponent();
            this.user = user;
            GuestRescheduleStatusViewModel = new GuestRescheduleStatusViewModel(this, user);
            DataContext = GuestRescheduleStatusViewModel;
        }
    }
}
