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
    /// Interaction logic for AnywhereAnytimeWithoutDate.xaml
    /// </summary>
    public partial class AnywhereAnytimeWithoutDate : Window
    {
        public AnywhereAnytimeWithoutDateViewModel AnywhereAnytimeWithoutDateViewModel { get; set; }
        public Accommodation accommodation { get; set; }

        public AnywhereAnytimeViewModel AnywhereAnytimeViewModel { get; set; }
        public AccommodationForReservation AccommodationForReservation { get; set; }
        public User user { get; set; }
        public AnywhereAnytimeWithoutDate(AnywhereAnytimeViewModel anywhereAnytimeViewModel, AccommodationForReservation accommodationForReservation, User user)
        {
            InitializeComponent();
            AccommodationForReservation = accommodationForReservation;
            this.user = user;
            AnywhereAnytimeViewModel = anywhereAnytimeViewModel;
            AnywhereAnytimeWithoutDateViewModel = new AnywhereAnytimeWithoutDateViewModel(AnywhereAnytimeViewModel, this, accommodationForReservation);
            this.DataContext = AnywhereAnytimeWithoutDateViewModel;
        }

        private void PreviousImageButton_Click(object sender, RoutedEventArgs e)
        {
            AnywhereAnytimeWithoutDateViewModel.PreviousImage(sender, e);
            //((GuestReservationViewModel)DataContext).PreviousImage();
        }

        private void NextImageButton_Click(object sender, RoutedEventArgs e)
        {
            AnywhereAnytimeWithoutDateViewModel.NextImage(sender, e);
            //((GuestReservationViewModel)DataContext).NextImage();
        }
    }
}
