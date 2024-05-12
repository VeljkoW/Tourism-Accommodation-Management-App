using BookingApp.ViewModel.Owner;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookingApp.Domain.Model;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for ReservationRescheduling.xaml
    /// </summary>
    public partial class ReservationRescheduling : Page
    {
        public User User { get; set; }
        public ReservationReschedulingViewModel ReservationReschedulingViewModel { get; set; }
        public ReservationRescheduling(User User)
        {
            InitializeComponent();
            this.User = User;
            ReservationReschedulingViewModel = new ReservationReschedulingViewModel(this, User);
            DataContext = ReservationReschedulingViewModel;
        }

        private void AccommodationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReservationReschedulingViewModel.AccommodationSelectionChanged();
        }
    }
}
