using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
using BookingApp.ViewModel.Owner;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for GuestReviews.xaml
    /// </summary>
    public partial class GuestReviews : Page
    {
        public User User {  get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public GuestReviewsViewModel GuestReviewsViewModel {  get; set; }
        public OwnerRating? SelectedOwnerRating {  get; set; }
        public GuestReviews(OwnerMainWindow OwnerMainWindow)
        {
            InitializeComponent();
            User = OwnerMainWindow.user;
            this.OwnerMainWindow = OwnerMainWindow;
            GuestReviewsViewModel = new GuestReviewsViewModel(this, User, SelectedOwnerRating);
            DataContext = GuestReviewsViewModel;
            SuperOwnerInfoLabel.Visibility = Visibility.Collapsed;
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            RenovationRequestsButton.Background = basicBackgroundBrush;
            ReviewsButton.Background = backgroundButtonPressedBrush;
        }

        private void RenovationRequestsClick(object sender, RoutedEventArgs e)
        {
            RenovationRequestPage RenovationRequestPage = new RenovationRequestPage(OwnerMainWindow);
            OwnerMainWindow.mainFrame.Navigate(RenovationRequestPage);
        }

        private void SuperOwnerInfoClick(object sender, RoutedEventArgs e)
        {
            if(SuperOwnerInfoLabel.Visibility == Visibility.Collapsed)
                SuperOwnerInfoLabel.Visibility = Visibility.Visible;
            else
                SuperOwnerInfoLabel.Visibility = Visibility.Collapsed;
        }
    }
}
