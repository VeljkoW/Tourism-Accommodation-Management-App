using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window //, INotifyPropertyChanged
    {
        public TouristMainWindowViewModel TouristMainWindowViewModel { get; set; }
        public static User? User { get; set; }
        public TouristMainWindow(User user)
        {
            InitializeComponent();
            User = user;
            this.TouristMainWindowViewModel = new TouristMainWindowViewModel(this,user);
            this.DataContext = TouristMainWindowViewModel;
        }

        private void SearchBoxClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Search tours...")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
            SearchBarGrid.Visibility = Visibility.Visible;

        }

        private void SearchBoxNotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search tours...";
                textBox.Foreground = Brushes.Gray;
            }
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        private void ToursTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Visible;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 0;
        }
        private void OngoingToursTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Visible;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 1;
        }

        private void FinishedToursTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Visible;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 2;
        }
        private void ReservationsTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Visible;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 3;
        }
        private void SuggestionsTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Visible;
            CouponsTabHeader.Visibility = Visibility.Collapsed;
            Tab.SelectedIndex = 4;
        }
        private void CouponsTabClick(object sender, RoutedEventArgs e)
        {
            ToursTabHeader.Visibility = Visibility.Collapsed;
            OngoingToursTabHeader.Visibility = Visibility.Collapsed;
            FinishedToursTabHeader.Visibility = Visibility.Collapsed;
            ReservationsTabHeader.Visibility = Visibility.Collapsed;
            SuggestionsTabHeader.Visibility = Visibility.Collapsed;
            CouponsTabHeader.Visibility = Visibility.Visible;
            Tab.SelectedIndex = 5;
        }

        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double SWidth = SystemParameters.PrimaryScreenWidth;
            double SHeight = SystemParameters.PrimaryScreenHeight;
            double WWidth = this.Width;
            double WHeight = this.Height;

            this.Left = (SWidth - WWidth) / 2;
            this.Top = (SHeight - WHeight) / 2;
        }

        private void CollapseSearchBar(object sender, RoutedEventArgs e)     // NEEDS ANIMATION ??
        {
            SearchBarGrid.Visibility = Visibility.Collapsed;
            StateComboBox.SelectedIndex = -1;
            CityComboBox.SelectedIndex = -1;
            DurationTextBox.Text = string.Empty;
            LanguageTextBox.Text = string.Empty;
            PeopleTextBox.Text = string.Empty;
            SearchBarTextBox.Text = "Search tours...";
            RefreshTours();                                 //refreshes searched tours after the search bar is collapsed, might get removed later??
        }

        private void StateComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TouristMainWindowViewModel.StateComboBoxSelectionChanged(sender, e);
        }

        private void NumbersPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!double.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        public void SearchTours(object sender, RoutedEventArgs e)
        {
            TouristMainWindowViewModel.SearchTours(sender, e);
        }

        public List<Tour> searchTours(string state, string city, int duration, string language, int people)
        {
            return TouristMainWindowViewModel.searchTours(state, city, duration, language, people);
        }
        public void RefreshTours()
        {
            TouristMainWindowViewModel.RefreshTours();
        }
        private void NotificationButtonClick(object sender, RoutedEventArgs e)
        {
            TouristMainWindowViewModel.NotificationButtonClick(sender, e);
        }
    }
}
