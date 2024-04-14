using BookingApp.Domain.Model;
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
    /// Interaction logic for TourReservationFailed.xaml
    /// </summary>
    public partial class TourReservationFailed : Window
    {
        TourReservationFailedViewModel TourReservationFailedViewModel { get; set; }
        public TourReservationFailed(TourReservationWindow tourReservationWindow,int freeSlots,Tour selectedTour)
        {
            InitializeComponent();
            TourReservationFailedViewModel = new TourReservationFailedViewModel(this,tourReservationWindow,freeSlots,selectedTour);
            this.DataContext = TourReservationFailedViewModel;
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

        public void GoBack(object sender, RoutedEventArgs e)
        {
            TourReservationFailedViewModel.GoBack(sender, e);
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            TourReservationFailedViewModel.Exit(sender, e);
        }

        public void SearchSimilarTours(object sender, RoutedEventArgs e)
        {
            TourReservationFailedViewModel.SearchSimilarTours(sender, e);
        }

    }
}
