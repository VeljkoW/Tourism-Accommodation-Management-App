using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository;
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

using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using BookingApp.ViewModel.Owner;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for GuestRating.xaml
    /// </summary>
    public partial class GuestRating : Page
    {
        public int Cleanliness {  get; set; }
        public int FollowingGuidelines {  get; set; }
        public GuestRatingViewModel GuestRatingViewModel {  get; set; }
        public GuestRating(OwnerMainWindow ownerMainWindow, User user)
        {
            InitializeComponent();
            GuestRatingViewModel = new GuestRatingViewModel(this, ownerMainWindow, user);
            this.DataContext = GuestRatingViewModel;
            Cleanliness = 0;
            FollowingGuidelines = 0;
            App.ThemeChanged += OnThemeChanged;
            OnThemeChanged();
            if (GuestRatingViewModel.ReservedAccommodations.Count <= 0)
            {
                NoGuestsToRateMessage.Visibility = Visibility.Visible;
            }
        }

        private void CleanlinessChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "Cleanliness1") Cleanliness = 1;
                    else if (radioButton.Name == "Cleanliness2") Cleanliness = 2;
                    else if (radioButton.Name == "Cleanliness3") Cleanliness = 3;
                    else if (radioButton.Name == "Cleanliness4") Cleanliness = 4;
                    else if (radioButton.Name == "Cleanliness5") Cleanliness = 5;
                }
            }
        }
        public void OnThemeChanged()
        {
            Color OwnerTabLightColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush OwnerTabLightColorBrush = new SolidColorBrush(OwnerTabLightColor);
            Color OwnerTabDarkColor = (Color)FindResource("OwnerTabDarkColor");
            SolidColorBrush OwnerTabDarkColorBrush = new SolidColorBrush(OwnerTabDarkColor);
            if (App.currentTheme() == "Light")
            {
                RateGuestBorder.Background = OwnerTabLightColorBrush;
                NoGuestsToRateMessageBorder.Background = OwnerTabLightColorBrush;
            }
            else
            {
                Color OwnerDarkBackgroundColor = (Color)FindResource("OwnerDarkBackgroundColor");
                SolidColorBrush OwnerDarkBackgroundColorBrush = new SolidColorBrush(OwnerDarkBackgroundColor);
                RateGuestBorder.Background = OwnerTabDarkColorBrush;
                NoGuestsToRateMessageBorder.Background = OwnerDarkBackgroundColorBrush;
            }
        }
        private void FollowingGuidelinesChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "FollowingGuidelines1") FollowingGuidelines = 1;
                    else if (radioButton.Name == "FollowingGuidelines2") FollowingGuidelines = 2;
                    else if (radioButton.Name == "FollowingGuidelines3") FollowingGuidelines = 3;
                    else if (radioButton.Name == "FollowingGuidelines4") FollowingGuidelines = 4;
                    else if (radioButton.Name == "FollowingGuidelines5") FollowingGuidelines = 5;
                }
            }
        }
    }
}
