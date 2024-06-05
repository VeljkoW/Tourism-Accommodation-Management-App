using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Guide;
using Notification.Wpf;
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
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for GuideMainPage.xaml
    /// </summary>
    public partial class GuideMainPage : Page
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public EventHandler? ListUpdater {  get; set; }
        public GuideMainPageViewModel GuideMainPageViewModel { get;set; }
        public User User { get; set; }
        public string UserName { get; set; }
        private UpcomingTours upcomingTours;
        private TourStatistics tourStatistics;
        private FinishedToursPage tourReviewsPage;
        private TourRequestsPage tourRequestsPage;
        private TourRequestStatisticsPage tourRequestStatisticsPage;
        private HelpPage helpPage;
        private ComplexTourRequestsPage complexTourRequestsPage;
        public static int UserId;
        public GuideMainPage(User user)
        {
            InitializeComponent();
            User = user;
            UserId = user.Id;
            UserName = User.Username;
            GuideMainPageViewModel = new GuideMainPageViewModel(user);
            GuideMainPageViewModel.Resigned += Logout;
            DataContext = GuideMainPageViewModel;
            BurgerMenu burgerMenu = new BurgerMenu(this);
            BurgerMenu.Children.Add(burgerMenu);
            upcomingTours = new UpcomingTours(user);
            tourStatistics = new TourStatistics(User);
            tourReviewsPage = new FinishedToursPage(this, User);
            tourRequestsPage = new TourRequestsPage();
            complexTourRequestsPage = new ComplexTourRequestsPage();
            tourRequestStatisticsPage = new TourRequestStatisticsPage();
            helpPage = new HelpPage();
            Frame.Navigate(upcomingTours);
        }
        public void ClickCreateTour(object sender, RoutedEventArgs e)
        {
            CreateTourForm createTourForm = new CreateTourForm(User);
            NavigationService.Navigate(createTourForm);
                createTourForm.Unloaded += AddCreatedTourNotification;
        }

        private void AddCreatedTourNotification(object sender, RoutedEventArgs e)
        {
            if(CreateTourFormViewModel.IsCreated)
            {
                NotificationArea.HorizontalAlignment= HorizontalAlignment.Center;
                NotificationArea.VerticalAlignment= VerticalAlignment.Center;
                notificationManager.Show("Success", "You have successfully created a tour!", NotificationType.Success, "MainNotificationArea");
            }
        }
        public void Logout()
        {
            ClickLogout(1,new RoutedEventArgs());
        }
        public void ClickLogout(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            OnLogoutHandler?.Invoke(sender, e);
            signInForm.Show();
        }
        public EventHandler OnLogoutHandler { get; set; }
        public void ClickUpcommingTour(object sender, RoutedEventArgs e)
        {
            upcomingTours.ViewModelLoad();
            Frame.Navigate(upcomingTours);
        }
        public void ClickTourStatistics(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(tourStatistics);
        }

        public void ClickTourReviews(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(tourReviewsPage);
        }

        public void ClickTourSuggestions(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(tourRequestsPage);
        }
        public void ClickComplexTourSuggestions(object sender, RoutedEventArgs e)
        {
            complexTourRequestsPage.Load();
            Frame.Navigate(complexTourRequestsPage);
        }
        public void ClickHelp(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(helpPage);
        }

        public void ClickTourSuggestionsStatistics(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(tourRequestStatisticsPage);
        }
        public void ShowBurger(object sender, RoutedEventArgs e)
        {
            this.BurgerMenu.Visibility = Visibility.Visible;
            defaultBurger.Visibility=Visibility.Collapsed;
        }
        public void HideBurger(object sender, RoutedEventArgs e)
        {
            defaultBurger.Visibility = Visibility.Visible;
            this.BurgerMenu.Visibility = Visibility.Collapsed;
        }
        public void Navigate(Page page)
        {
            NavigationService.Navigate(page);
        }
    }
}