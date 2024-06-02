using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using BookingApp.ViewModel;
using BookingApp.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

        public void SearchBoxClicked(Object sender, RoutedEventArgs e)
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
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }
        public void EndDemoMode(object sender, MouseButtonEventArgs e)
        {
            TouristMainWindowViewModel.EndDemoMode();
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

        public List<Tour> searchTours(string state, string city, int duration, string language, int people)
        {
            return TouristMainWindowViewModel.searchTours(state, city, duration, language, people);
        }
        public void RefreshTours()
        {
            TouristMainWindowViewModel.RefreshTours();
        }
    }
}
