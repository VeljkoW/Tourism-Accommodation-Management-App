using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.ViewModel.Tourist;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window
    {
        TourReservationWindowViewModel TourReservationWindowViewModel { get; set; }
        public TourReservationWindow(Tour tour, User user)
        {
            InitializeComponent();
            TourReservationWindowViewModel = new TourReservationWindowViewModel(this,tour,user);
            NumberOfPeopleTextBox.Text = "Max " + TourReservationWindowViewModel.Tour.MaxTourists.ToString();
            this.DataContext = TourReservationWindowViewModel;
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

        private void NumberOfPeoplePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!double.TryParse(e.Text,out _))
            {
                e.Handled = true;
            }
        }
        private void NumberOfPeopleTextBoxClicked(Object sender, RoutedEventArgs e)
        {
            TourReservationWindowViewModel.NumberOfPeopleTextBoxClicked(sender, e);
        }

        private void NumberOfPeopleTextBoxNotClicked(Object sender, RoutedEventArgs e)
        {
            TourReservationWindowViewModel.NumberOfPeopleTextBoxNotClicked(sender, e);
        }

        private void GenerateTextBoxes(object sender, TextChangedEventArgs e)
        {
            TourReservationWindowViewModel.GenerateTextBoxes(sender, e);
        }
    }
}
