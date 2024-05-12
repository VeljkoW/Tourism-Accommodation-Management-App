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
    public class TourReservationSuccessfulViewModel
    {
        public TourReservationSuccessful TourReservationSuccessful { get; set; }
        public Tour Tour { get; set; }
        public TourReservation TourReservation { get; set; }
        public RelayCommand ClickOk => new RelayCommand(execute => OkExecute());
        public TourReservationSuccessfulViewModel(TourReservationSuccessful tourReservationSuccessful,Tour tour, TourReservation tourReservation)
        {
            this.TourReservationSuccessful = tourReservationSuccessful;

            Tour = tour;
            TourReservation = tourReservation;
            TourReservationSuccessful.NumberTextBlock.Text = TourReservation.People.Count().ToString();
            TourReservationSuccessful.TourNameTextBlock.Text = "\"" + Tour.Name + "\"";

            TourReservationSuccessful.TourTextBlock.Text = " tour.";

            TourReservationSuccessful.TourDateTextBlock.Text = Tour.DateTime.ToString();
        }
        public void OkExecute()
        {
            TourReservationSuccessful.Close();
        }
    }
}
