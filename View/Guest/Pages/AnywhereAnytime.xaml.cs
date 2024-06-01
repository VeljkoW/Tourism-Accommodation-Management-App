using BookingApp.Domain.Model;
using BookingApp.View.Guest.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Pages
{
    /// <summary>
    /// Interaction logic for AnywhereAnytime.xaml
    /// </summary>
    public partial class AnywhereAnytime : Page
    {
        public User User { get; set; }
        public GuestMainWindow GuestMainWindow { get; set; }

        public AnywhereAnytimeViewModel anywhereAnytimeView { get; set; }
        
        public bool openWindow { get; set; }
        public AnywhereAnytime(User user, GuestMainWindow guestMainWindow)
        {
            InitializeComponent();
            User = user;
            anywhereAnytimeView = new AnywhereAnytimeViewModel(this);
            this.DataContext = anywhereAnytimeView;
            Color backgroundButtonPressedColor = (Color)ColorConverter.ConvertFromString("#74877A");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            AnyButton.Background = backgroundButtonPressedBrush;
            Color backgroundButtonColor = (Color)ColorConverter.ConvertFromString("#56736F");
            SolidColorBrush backgroundButtonBrush = new SolidColorBrush(backgroundButtonColor);
            AccommodationButton.Background = backgroundButtonBrush;
            GuestMainWindow = guestMainWindow;
            openWindow = true;
        }


        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as Accommodation;
            if (openWindow)
            {
                AccommodationForReservation accommodationForReservation = anywhereAnytimeView.accommodationForReservations.Where(t => t.AccommodationId == selectedCard.Id).First();
                AnywhereAnytimeWithDate anywhereAnytimeWithDate = new AnywhereAnytimeWithDate(anywhereAnytimeView, accommodationForReservation, User);
                this.Focusable = false;
                anywhereAnytimeWithDate.Show();
                anywhereAnytimeWithDate.Focusable = true;

            }
            else
            {
                AccommodationForReservation accommodationForReservation = anywhereAnytimeView.accommodationForReservations.Where(t => t.AccommodationId == selectedCard.Id).First();
                AnywhereAnytimeWithoutDate anywhereAnytimeWithoutDate = new AnywhereAnytimeWithoutDate(anywhereAnytimeView, accommodationForReservation, User);
                this.Focusable = false;
                anywhereAnytimeWithoutDate.Show();
                anywhereAnytimeWithoutDate.Focusable = true;
            }
            //GuestAccommodationsViewModel.ClickedOnCard(sender, e);
        }
        private void AccommodationsClick(object sender, RoutedEventArgs e)
        {
            Accommodations accommodations = new Accommodations(User, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(accommodations);
        }

        private void AnyClick(object sender, RoutedEventArgs e)
        {
            AnywhereAnytime anywhereAnytime = new AnywhereAnytime(User, GuestMainWindow);
            GuestMainWindow.mainFrame.Navigate(anywhereAnytime);
        }

    }
}
