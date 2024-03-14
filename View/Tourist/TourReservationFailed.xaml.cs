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
        public int FreeSlots {  get; set; }
        private TourReservationWindow PreviousWindow;
        public TourReservationFailed(TourReservationWindow tourReservationWindow,int freeSlots)
        {
            InitializeComponent();
            this.PreviousWindow = tourReservationWindow;
            FreeSlots = freeSlots;
            FreeSlotsTextBlock.Text = FreeSlots.ToString();

            if(FreeSlots > 0)
            {
                ExceededTheAmoutTextBlock.Text = "It looks like you have exceeded the amount of free slots on this tour!";
            }
            else
            {
                ExceededTheAmoutTextBlock.Text = "It looks like there aren't any more free slots on this tour!";
            }

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
            if(FreeSlots > 0)       //Temporary solution
            {
                Close();
            }
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            if(PreviousWindow != null)
            {
                PreviousWindow.Close();
            }

            this.Close();
        }

        public void SearchSimilarTours(object sender, RoutedEventArgs e)
        {

        }

    }
}
