using BookingApp.Domain.Model;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for Forum.xaml
    /// </summary>
    public partial class Forum : Page
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public User User { get; set; }
        public OwnerForumViewModel OwnerForumViewModel {  get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public Forum(OwnerMainWindow ownerMainWindow)
        {
            User = ownerMainWindow.user;
            OwnerMainWindow = ownerMainWindow;
            InitializeComponent();
            OwnerForumViewModel = new OwnerForumViewModel(this);
            DataContext = OwnerForumViewModel;
            UsefulForumTextBlock.Visibility = Visibility.Hidden;
            BookmarkImage.Visibility = Visibility.Hidden;
            UsefulForumMessage.Visibility = Visibility.Collapsed;
            App.ThemeChanged += OnThemeChanged;
            OnThemeChanged();
        }

        private void statePicked(object sender, SelectionChangedEventArgs e)
        {
            OwnerForumViewModel.StatePicked();
        }

        private void cityPicked(object sender, SelectionChangedEventArgs e)
        {
            OwnerForumViewModel.CityPicked();
        }

        private void ReportClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as GuestPost;
            OwnerForumViewModel.ReportClick(selectedCard);

        }

        private void UsefulForumInfoClick(object sender, RoutedEventArgs e)
        {
            if (UsefulForumMessage.Visibility == Visibility.Collapsed)
                UsefulForumMessage.Visibility = Visibility.Visible;
            else
                UsefulForumMessage.Visibility = Visibility.Collapsed;
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
                BookmarkImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/bookmark.png"));
                OwnerMainPostImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/verified-user.png"));

                var newColor = (Color)Application.Current.Resources["BorderLightBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
            }
            else
            {
                BookmarkImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/bookmarkWhite.png"));
                OwnerMainPostImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Owner/user-verificationWhite.png"));

                var newColor = (Color)Application.Current.Resources["BorderDarkBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
            }
        }
    }
}
