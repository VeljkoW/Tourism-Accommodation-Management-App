using BookingApp.Domain.Model;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReservationFailedViewModel
    {
        TourReservationFailed TourReservationFailed { get; set; }
        public int FreeSlots { get; set; }
        private TourReservationWindow PreviousWindow;
        public Tour SelectedTour { get; set; }
        public TourReservationFailedViewModel(TourReservationFailed tourReservationFailed,TourReservationWindow tourReservationWindow,int freeSlots,Tour selectedTour)
        { 
            this.TourReservationFailed = tourReservationFailed;
            this.PreviousWindow = tourReservationWindow;

            FreeSlots = freeSlots;
            SelectedTour = selectedTour;
            TourReservationFailed.FreeSlotsTextBlock.Text = FreeSlots.ToString();


            if (FreeSlots > 0)
            {
                TourReservationFailed.ExceededTheAmoutTextBlock.Text = "It looks like you have exceeded the amount of free slots on this tour!";
                TourReservationFailed.GoBackButtonGrid.Visibility = Visibility.Visible;
            }
            else
            {
                TourReservationFailed.ExceededTheAmoutTextBlock.Text = "It looks like there aren't any more free slots on this tour!";
                TourReservationFailed.GoBackButtonGrid.Visibility = Visibility.Collapsed;
            }


        }
        public void GoBack(object sender, RoutedEventArgs e)
        {
            if(FreeSlots > 0)       //Temporary solution
            {
                TourReservationFailed.Close();
            }
        }

        public void Exit(object sender, RoutedEventArgs e)
        {
            
            if(PreviousWindow != null)
            {
                PreviousWindow.Close();
            }

            TourReservationFailed.Close();

        }
        public void SearchSimilarTours(object sender, RoutedEventArgs e)
        {
            if (PreviousWindow != null)
            {
                PreviousWindow.Close();
            }

            TourReservationFailed.Close();
            TourReservationSimilarTours tourReservationSimilarTours = new TourReservationSimilarTours(SelectedTour);
            tourReservationSimilarTours.ShowDialog();
        }
    }
}
