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
using System.ComponentModel;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window, INotifyPropertyChanged
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";

        public User user { get; set; }
        public Accommodation Accommodation { get; set; }
        public AccommodationStatistics AccommodationStatistics { get; set; }
        public ReservationRescheduling ReservationRescheduling {  get; set; }
        public AccommodationRegistration AccommodationRegistration { get; set; }
        public Renovation Renovation { get; set; }
        public RenovationHistory RenovationHistory { get; set; }
        public Forum Forum {  get; set; }
        public ObservableCollection<ReservedAccommodation> ReservedAccommodations { get; set; }
        public ObservableCollection<OwnerNotification> OwnerNotifications { get; set; }
        public OwnerMainWindowViewModel OwnerMainWindowViewModel { get; set; }
        private string currentNavigationButton {  get; set; }
        public string CurrentNavigationButton
        {
            get { return currentNavigationButton; }
            set
            {
                if (currentNavigationButton != value)
                {
                    currentNavigationButton = value;
                    NavigationButtonBarPressed(currentNavigationButton);
                    OnPropertyChanged(nameof(currentNavigationButton));
                }
            }
        }
        public SolidColorBrush backgroundButtonPressedBrush { get; set; }
        public SolidColorBrush basicBackgroundBrush { get; set; }
        public SolidColorBrush basicDarkBackgroundBrush { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        //public AccommodationStatistics AccommodationStatistics {  get; set; }
        public OwnerMainWindow(User user)
        {
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            Color basicDarkBackgroundColor = (Color)FindResource("OwnerTabDarkColor");
            basicDarkBackgroundBrush = new SolidColorBrush(basicDarkBackgroundColor);
            App.ChangeLanguage(ENG);
            InitializeComponent();
            OwnerMainWindowViewModel = new OwnerMainWindowViewModel(this, user);
            DataContext = OwnerMainWindowViewModel;
            ChangeThemeLight();
            App.ThemeChanged += OnThemeChanged;

            this.user = user;
            NotificationListBox.Visibility = Visibility.Collapsed;
            Accommodation = new Accommodation();
            AccommodationRegistration = new AccommodationRegistration(user);
            AccommodationStatistics = new AccommodationStatistics(this);
            ReservationRescheduling = new ReservationRescheduling(user);
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
            CurrentNavigationButton = "AccommodationManagementButton";
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
            //NavigationButtonBarPressed("AccommodationManagementButton");
            CurrentNavigationButton = "AccommodationManagementButton";
        }

        private void GuestRatingClick(object sender, RoutedEventArgs e)
        {
            GuestRatingPage GuestRatingPage = new GuestRatingPage(this, user);
            NotificationListBox.BorderBrush = Brushes.Gray;
            NotificationListBox.BorderThickness = new Thickness(1);
            mainFrame.Navigate(GuestRatingPage);
            //NavigationButtonBarPressed("GuestRatingButton");
            CurrentNavigationButton = "GuestRatingButton";
        }

        private void GuestReviewsClick(object sender, RoutedEventArgs e)
        {
            GuestReviews GuestReviews = new GuestReviews(this);
            mainFrame.Navigate(GuestReviews);
            //NavigationButtonBarPressed("GuestReviewsButton");
            CurrentNavigationButton = "GuestReviewsButton";
        }

        private void AccommodationStatisticsClick(object sender, RoutedEventArgs e)
        {
            //AccommodationStatistics AccommodationStatistics = new AccommodationStatistics(this);
            mainFrame.Navigate(AccommodationStatistics);
            //NavigationButtonBarPressed("AccommodationStatisticsButton");
            CurrentNavigationButton = "AccommodationStatisticsButton";
        }

        private void ReservationReschedulingClick(object sender, RoutedEventArgs e)
        {
            //ReservationRescheduling ReservationRescheduling = new ReservationRescheduling(user);
            mainFrame.Navigate(ReservationRescheduling);
            //NavigationButtonBarPressed("ReservationReschedulingButton");
            CurrentNavigationButton = "ReservationReschedulingButton";
        }

        private void RenovationClick(object sender, RoutedEventArgs e)
        { 
            mainFrame.Navigate(Renovation);
            //NavigationButtonBarPressed("RenovationButton");
            CurrentNavigationButton = "RenovationButton";
        }

        private void ForumClick(object sender, RoutedEventArgs e)
        { 
            mainFrame.Navigate(Forum);
            //NavigationButtonBarPressed("ForumButton");
            CurrentNavigationButton = "ForumButton";
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
            if (App.currentTheme() == "Light")
            {
                AccommodationManagementButton.Background = buttonName == "AccommodationManagementButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
                AccommodationStatisticsButton.Background = buttonName == "AccommodationStatisticsButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
                ReservationReschedulingButton.Background = buttonName == "ReservationReschedulingButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
                GuestRatingButton.Background = buttonName == "GuestRatingButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
                GuestReviewsButton.Background = buttonName == "GuestReviewsButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
                RenovationButton.Background = buttonName == "RenovationButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
                ForumButton.Background = buttonName == "ForumButton" ? backgroundButtonPressedBrush : basicBackgroundBrush;
            }
            else
            {
                AccommodationManagementButton.Background = buttonName == "AccommodationManagementButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
                AccommodationStatisticsButton.Background = buttonName == "AccommodationStatisticsButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
                ReservationReschedulingButton.Background = buttonName == "ReservationReschedulingButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
                GuestRatingButton.Background = buttonName == "GuestRatingButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
                GuestReviewsButton.Background = buttonName == "GuestReviewsButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
                RenovationButton.Background = buttonName == "RenovationButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
                ForumButton.Background = buttonName == "ForumButton" ? backgroundButtonPressedBrush : basicDarkBackgroundBrush;
            }
        }

        private void ChangeLanguageSRB(object sender, RoutedEventArgs e)
        {
            App.ChangeLanguage(SRB);
            if (App.currentTheme() == "Light")
            {
                ENGButton.Background = basicBackgroundBrush;
                SRBButton.Background = backgroundButtonPressedBrush;
            }
            else
            {
                ENGButton.Background = basicDarkBackgroundBrush;
                SRBButton.Background = backgroundButtonPressedBrush;
            }
        }
        private void ChangeLanguageENG()
        {
            App.ChangeLanguage(ENG);
            if(App.currentTheme() == "Light")
            {
                ENGButton.Background = backgroundButtonPressedBrush;
                SRBButton.Background = basicBackgroundBrush;
            }
            else
            {
                ENGButton.Background = backgroundButtonPressedBrush;
                SRBButton.Background = basicDarkBackgroundBrush;
            }
        }
        private void ChangeLanguageENG(object sender, RoutedEventArgs e)
        {
            ChangeLanguageENG();
        }

        private void ChangeThemeLight(object sender, RoutedEventArgs e)
        {
            ChangeThemeLight();
        }
        public void ChangeThemeLight()
        {
            LightThemeButton.Background = backgroundButtonPressedBrush;
            DarkThemeButton.Background = basicBackgroundBrush;

            App.ChangeTheme("Light");


        }
        private void ChangeThemeDark(object sender, RoutedEventArgs e)
        {
            LightThemeButton.Background = basicDarkBackgroundBrush;
            DarkThemeButton.Background = backgroundButtonPressedBrush;

            App.ChangeTheme("Dark");
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
        private void OnThemeChanged()
        {
            NavigationButtonBarPressed(CurrentNavigationButton);
            if(App.currentTheme() == "Light")
            {
                AccommodationManagementImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/hotel.png"));
                AccommodationStatisticsImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/statistics.png"));
                ReservationReschedulingImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/rescheduling.png"));
                GuestRatingImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/guest-rating.png"));
                GuestReviewsImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/review.png"));
                RenovationImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/renovation.png"));
                ForumImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/forum.png"));
                NotificationBellImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/bell.png"));
                OwnerUserImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/user.png"));
                
                MainWindowGrid.Background = new SolidColorBrush(Colors.White);
                if(App.currentLanguage() == ENG)
                {
                    ENGButton.Background = backgroundButtonPressedBrush;
                    SRBButton.Background = basicBackgroundBrush;
                }
                else
                {
                    ENGButton.Background = basicBackgroundBrush;
                    SRBButton.Background = backgroundButtonPressedBrush;
                }
            }
            else
            {
                AccommodationManagementImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/hotelWhite.png"));
                AccommodationStatisticsImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/statisticsWhite.png"));
                ReservationReschedulingImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/reschedulingWhite.png"));
                GuestRatingImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/guestRatingWhite.png"));
                GuestReviewsImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/guestReviewWhite.png"));
                RenovationImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/renovationWhite.png"));
                ForumImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/forumWhite.png"));
                NotificationBellImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/bellWhite.png"));
                OwnerUserImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/userWhite.png"));

                Color OwnerDarkBackgroundColor = (Color)FindResource("OwnerDarkBackgroundColor");
                SolidColorBrush OwnerDarkBackgroundColorBrush = new SolidColorBrush(OwnerDarkBackgroundColor);
                MainWindowGrid.Background = OwnerDarkBackgroundColorBrush;
                if (App.currentLanguage() == ENG)
                {
                    ENGButton.Background = backgroundButtonPressedBrush;
                    SRBButton.Background = basicDarkBackgroundBrush;
                }
                else
                {
                    ENGButton.Background = basicDarkBackgroundBrush;
                    SRBButton.Background = backgroundButtonPressedBrush;
                }
            }
        }
    }
}
