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
    /// Interaction logic for RenovationHistory.xaml
    /// </summary>
    public partial class RenovationHistory : Page
    {
        public RenovationHistoryViewModel RenovationHistoryViewModel {  get; set; }
        public User User { get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public RenovationHistory(OwnerMainWindow OwnerMainWindow)
        {
            this.OwnerMainWindow = OwnerMainWindow;
            User = OwnerMainWindow.user;
            InitializeComponent();
            RenovationHistoryViewModel = new RenovationHistoryViewModel(this);
            DataContext = RenovationHistoryViewModel;
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            SchedulingButton.Background = basicBackgroundBrush;
            HistoryButton.Background = backgroundButtonPressedBrush;
            App.ThemeChanged += OnThemeChanged;
            OnThemeChanged();
        }

        private void SchedulingClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.mainFrame.Navigate(OwnerMainWindow.Renovation);
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
                SchedulingButton.Background = basicBackgroundBrush;
                HistoryButton.Background = backgroundButtonPressedBrush;
            }
            else
            {
                var newColor = (Color)Application.Current.Resources["BorderDarkBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
                SchedulingButton.Background = basicDarkBackgroundBrush;
                HistoryButton.Background = backgroundButtonPressedBrush;
            }
        }
    }
}
