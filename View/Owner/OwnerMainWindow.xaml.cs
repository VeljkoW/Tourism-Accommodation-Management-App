using BookingApp.Domain.Model;
using BookingApp.Localization;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using GuestRatingPage = BookingApp.View.Owner.GuestRating;
using ForumModel = BookingApp.Domain.Model.Forum;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";

        public User user { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationRegistration AccommodationRegistration { get; set; }
        public Renovation Renovation { get; set; }
        public RenovationHistory RenovationHistory { get; set; }
        public Forum Forum {  get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public ObservableCollection<OwnerNotification> OwnerNotifications { get; set; }
        public OwnerMainWindowViewModel OwnerMainWindowViewModel { get; set; }
        public OwnerMainWindow(User user)
        {
            App.ChangeLanguage(ENG);
            InitializeComponent();
            OwnerMainWindowViewModel = new OwnerMainWindowViewModel(this, user);
            DataContext = OwnerMainWindowViewModel;

            this.user = user;
            NotificationListBox.Visibility = Visibility.Collapsed;
            Accommodation = new Accommodation();
            AccommodationRegistration = new AccommodationRegistration(user);
            ReservedAccommodations = new ObservableCollection<ReservedAccommodation>();
            ReservedAccommodationService.GetInstance().NotificationUpdate(user, ReservedAccommodations);

            OwnerNotifications = new ObservableCollection<OwnerNotification>();
            foreach (OwnerNotification ownerNotification in OwnerNotificationService.GetInstance().GetAll())
                OwnerNotifications.Add(ownerNotification);
            Renovation = new Renovation(this);
            RenovationHistory = new RenovationHistory(this);
            Forum = new Forum(this);
            mainFrame.Navigate(AccommodationRegistration);

            if (OwnerNotifications.Count == 0)
            {
                NotificationListBox.BorderBrush = Brushes.Gray;
                NotificationListBox.BorderThickness = new Thickness(1);
                NewNotificationImage.Visibility = Visibility.Collapsed;
            }
            NavigationButtonBarPressed("AccommodationManagementButton");
            ChangeLanguageENG();
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        private void AccommodationManagementClick(object sender, RoutedEventArgs e)
        { 
            mainFrame.Navigate(AccommodationRegistration);
            NavigationButtonBarPressed("AccommodationManagementButton");
        }

        private void GuestRatingClick(object sender, RoutedEventArgs e)
        {
            GuestRatingPage GuestRatingPage = new GuestRatingPage(this, user);
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            mainFrame.Navigate(GuestRatingPage);
            NavigationButtonBarPressed("GuestRatingButton");
        }

        private void GuestReviewsClick(object sender, RoutedEventArgs e)
        {
            GuestReviews GuestReviews = new GuestReviews(this);
            mainFrame.Navigate(GuestReviews);
            NavigationButtonBarPressed("GuestReviewsButton");
        }

        private void AccommodationStatisticsClick(object sender, RoutedEventArgs e)
        {
            AccommodationStatistics AccommodationStatistics = new AccommodationStatistics(this);
            mainFrame.Navigate(AccommodationStatistics);
            NavigationButtonBarPressed("AccommodationStatisticsButton");
        }

        private void ReservationReschedulingClick(object sender, RoutedEventArgs e)
        {
            ReservationRescheduling ReservationRescheduling = new ReservationRescheduling(user);
            mainFrame.Navigate(ReservationRescheduling);
            NavigationButtonBarPressed("ReservationReschedulingButton");
        }

        private void RenovationClick(object sender, RoutedEventArgs e)
        { 
            mainFrame.Navigate(Renovation);
            NavigationButtonBarPressed("RenovationButton");
        }

        private void ForumClick(object sender, RoutedEventArgs e)
        { 
            mainFrame.Navigate(Forum);
            NavigationButtonBarPressed("ForumButton");
        }

        private void NotificationButtonClick(object sender, RoutedEventArgs e)
        {
            if(NotificationListBox.Visibility == Visibility.Collapsed)
                NotificationListBox.Visibility = Visibility.Visible;
            else
                NotificationListBox.Visibility = Visibility.Collapsed;
            NewNotificationImage.Visibility = Visibility.Collapsed;
        }
        public void NavigationButtonBarPressed(string buttonName)
        {
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);

            AccommodationManagementButton.Background = buttonName == "AccommodationManagementButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            AccommodationStatisticsButton.Background = buttonName == "AccommodationStatisticsButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            ReservationReschedulingButton.Background = buttonName == "ReservationReschedulingButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            GuestRatingButton.Background = buttonName == "GuestRatingButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            GuestReviewsButton.Background = buttonName == "GuestReviewsButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            RenovationButton.Background = buttonName == "RenovationButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            ForumButton.Background = buttonName == "ForumButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
        }

        private void ChangeLanguageSRB(object sender, RoutedEventArgs e)
        {
            App.ChangeLanguage(SRB);
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            ENGButton.Background = basicBackgroundBrush;
            SRBButton.Background = backgroundButtonPressedBrush;
        }
        private void ChangeLanguageENG()
        {
            App.ChangeLanguage(ENG);
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            ENGButton.Background = backgroundButtonPressedBrush;
            SRBButton.Background = basicBackgroundBrush;
        }
        private void ChangeLanguageENG(object sender, RoutedEventArgs e)
        {
            ChangeLanguageENG();
        }

        private void ChangeThemeLight(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeThemeDark(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            if (OwnerMainWindowViewModel.SelectedOwnerNotification.Root == "Forum")
            {
                mainFrame.Navigate(Forum);

                ForumModel? forum = ForumService.GetInstance().GetById(OwnerMainWindowViewModel.SelectedOwnerNotification.ForumId);
                Location? location = LocationService.GetInstance().GetById(forum.LocationId);
                var SelectedState = Forum.OwnerForumViewModel.States.FirstOrDefault(t => t.Equals(location.State));
                Forum.OwnerForumViewModel.SelectedState = SelectedState;
                LocationService.GetInstance().GetCitiesForState(Forum.OwnerForumViewModel.Cities, SelectedState);

                for (int i = 0; i < Forum.OwnerForumViewModel.Cities.Count(); i++)
                {
                    if (Forum.OwnerForumViewModel.Cities[i].Id == location.Id)
                    {
                        Forum.OwnerForumViewModel.SelectedCity = Forum.OwnerForumViewModel.Cities[i];
                        Forum.CitiesComboBox.SelectedIndex = i;
                        break;
                    }
                }
                Forum.OwnerForumViewModel.SearchExecute();


                NavigationButtonBarPressed("ForumButton");
                OwnerNotificationService.GetInstance().Delete(OwnerMainWindowViewModel.SelectedOwnerNotification.Id);
            }
            else if (OwnerMainWindowViewModel.SelectedOwnerNotification.Root == "OwnerRating")
            {
                GuestRatingPage GuestRatingPage = new GuestRatingPage(this, user);
                mainFrame.Navigate(GuestRatingPage);
                NavigationButtonBarPressed("GuestRatingButton");
                OwnerNotificationService.GetInstance().Delete(OwnerMainWindowViewModel.SelectedOwnerNotification.Id);
            }
        }
    }
}
