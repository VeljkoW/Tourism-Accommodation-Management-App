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
    public class TourReviewWindowViewModel
    {
        TourReviewWindow TourReviewWindow { get; set; }
        Tour Tour { get; set; }
        User User { get; set; }
        public TourReviewWindowViewModel(TourReviewWindow tourReviewWindow, Tour tour, User user) 
        { 
            this.TourReviewWindow = tourReviewWindow;
            this.Tour = tour;
            this.User = user;
            TourReviewWindow.NameTextBlock.Text = Tour.Name;
        }

        public void Cancel(object sender, RoutedEventArgs e)
        {
            TourReviewWindow.Close();
            TourFinishedDetailed tourFinishedDetailed = new TourFinishedDetailed(Tour, User);
            tourFinishedDetailed.ShowDialog();
        }
        public void Submit(object sender, RoutedEventArgs e)
        {
            TourReviewWindow.Close();
        }
    }
}
