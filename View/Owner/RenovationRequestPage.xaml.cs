using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for RenovationRequestPage.xaml
    /// </summary>
    public partial class RenovationRequestPage : Page
    {
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public User User { get; set; }
        public RenovationRequestPageViewModel RenovationRequestPageViewModel { get; set; }
        public RenovationRequestPage(OwnerMainWindow ownerMainWindow)
        {
            OwnerMainWindow = ownerMainWindow;
            User = ownerMainWindow.user;
            InitializeComponent();
            RenovationRequestPageViewModel = new RenovationRequestPageViewModel(this);
            DataContext = RenovationRequestPageViewModel;
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            RenovationRequestsButton.Background = backgroundButtonPressedBrush;
            ReviewsButton.Background = basicBackgroundBrush;
            App.ThemeChanged += OnThemeChanged;
            OnThemeChanged();
        }

        private void ReviewsClick(object sender, RoutedEventArgs e)
        {
            GuestReviews GuestReviews = new GuestReviews(OwnerMainWindow);
            OwnerMainWindow.mainFrame.Navigate(GuestReviews);
        }

        private void RenovationClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as RenovationRequest;
            Renovation renovation = new Renovation(OwnerMainWindow);
            Accommodation accommodationForRenovation = AccommodationService.GetInstance().GetById(selectedCard.AccommodationId);

            Accommodation selectedAccommodation = renovation.RenovationViewModel.Accommodations.FirstOrDefault(accommodation => accommodation.Id == accommodationForRenovation.Id);
            if (selectedAccommodation != null)
                renovation.RenovationViewModel.SelectedAccommodation = selectedAccommodation;

            OwnerMainWindow.mainFrame.Navigate(renovation);
            OwnerMainWindow.NavigationButtonBarPressed("RenovationButton");
        }

        private void clickOnCard(object sender, MouseButtonEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as RenovationRequest;
            RenovationRequestPageViewModel.SelectedRenovationRequest = selectedCard;
        }

        private void CloseRequestClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as RenovationRequest;
            RenovationRequestPageViewModel.SelectedRenovationRequest = selectedCard;
            Accommodation? accommodation = AccommodationService.GetInstance().GetById(selectedCard.AccommodationId);
            SelectedAccommodationNameRun.Text = accommodation?.Name + ",";
            SelectedAccommodationStateRun.Text = accommodation?.Location.State;
            SelectedAccommodationCityRun.Text = accommodation?.Location.City;
            SelectedLevelOfRequest.Text = selectedCard.Level.ToString();
            CloseRenovationRequestAccept.Visibility = Visibility.Visible;
        }
        private void CloseRenovationRequestAcceptClick(object sender, RoutedEventArgs e)
        {
            RenovationRequestPageViewModel.CloseRequest();
            CloseRenovationRequestAccept.Visibility = Visibility.Collapsed;
        }
        private void CloseRenovationRequestCancelClick(object sender, RoutedEventArgs e)
        {
            CloseRenovationRequestAccept.Visibility = Visibility.Collapsed;
        }
        private void OnThemeChanged()
        {
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            Color basicDarkBackgroundColor = (Color)FindResource("OwnerTabDarkColor");
            SolidColorBrush basicDarkBackgroundBrush = new SolidColorBrush(basicDarkBackgroundColor);

            if (App.currentTheme() == "Light")
            {
                var newColor = (Color)Application.Current.Resources["BorderLightBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
                ReviewsButton.Background = basicBackgroundBrush;
                RenovationRequestsButton.Background = backgroundButtonPressedBrush;
                CloseRenovationRequestAcceptBorder.Background = basicBackgroundBrush;
            }
            else
            {
                Color OwnerDarkBackgroundColor = (Color)FindResource("OwnerDarkBackgroundColor");
                SolidColorBrush OwnerDarkBackgroundColorBrush = new SolidColorBrush(OwnerDarkBackgroundColor);

                var newColor = (Color)Application.Current.Resources["BorderDarkBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
                ReviewsButton.Background = basicDarkBackgroundBrush;
                RenovationRequestsButton.Background = backgroundButtonPressedBrush;
                CloseRenovationRequestAcceptBorder.Background = OwnerDarkBackgroundColorBrush;
            }
        }
    }
}
