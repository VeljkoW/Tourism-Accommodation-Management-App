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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Pages
{
    public partial class Accommodations : Page
    {
        public GuestAccommodationsViewModel GuestAccommodationsViewModel { get; set; }
        public GuestMainWindow GuestMainWindow { get; set; }
        public User User { get; set; }
        public Accommodations(User user, GuestMainWindow guestMainWindow)
        {
            InitializeComponent();
            User = user;
            MainGrid.Focus();
            GuestAccommodationsViewModel = new GuestAccommodationsViewModel(this, user);
            this.DataContext = GuestAccommodationsViewModel;
            Color backgroundButtonPressedColor = (Color)ColorConverter.ConvertFromString("#56736F");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            AnyButton.Background = backgroundButtonPressedBrush;
            Color backgroundButtonColor = (Color)ColorConverter.ConvertFromString("#74877A");
            SolidColorBrush backgroundButtonBrush = new SolidColorBrush(backgroundButtonColor);
            AccommodationButton.Background = backgroundButtonBrush;
            GuestMainWindow = guestMainWindow;
        }

        public void ClickedOnCard(object sender, RoutedEventArgs e)
        {
            GuestAccommodationsViewModel.ClickedOnCard(sender, e);
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

        private void NameTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            //{
            //    // Ukloni fokus sa TextBox
            //    Keyboard.ClearFocus();

            //    // Postavi fokus na Window
            //    FocusManager.SetFocusedElement(MainGrid, MainGrid);

            //    // Spreči dalje procesiranje događaja
            //    e.Handled = true;
            //}
        }

        private void ClickEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ClickedOnCard(sender, e);
            }
        }
    }
}
