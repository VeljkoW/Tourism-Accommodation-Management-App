using BookingApp.Domain.Model;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BookingApp.ViewModel.Tourist
{
    public class TourDetailedViewModel
    {
        public Tour Tour { get; set; }
        public User User { get; set; }
        TourDetailed TourDetailed { get; set; }
        public TourDetailedViewModel(TourDetailed tourDetailed,Tour selectedTour,User user) 
        {
            Tour = selectedTour;
            User = user;
            TourDetailed = tourDetailed;
            TourDetailed.NameTextBlock.Text = Tour.Name;
            TourDetailed.DescriptionTextBlock.Text = Tour.Description;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
            }
            if (Tour.Location != null)
            {
                TourDetailed.StateTextBlock.Text = Tour.Location.State;
                if (!String.IsNullOrEmpty(Tour.Location.City))
                {
                    TourDetailed.CityTextBlock.Text = ", " + Tour.Location.City;
                }
                else
                {
                    TourDetailed.CityTextBlock.Text = Tour.Location.City;
                }
            }

            TourDetailed.LanguageTextBlock.Text = Tour.Language;
            TourDetailed.DateTextBlock.Text = Tour.DateTime.Date.ToString();

            TourDetailed.DurationTextBlock.Text = Tour.Duration.ToString() + "h";

            TourDetailed.MaxPeopleTextBlock.Text = Tour.MaxTourists.ToString();

        }

        public void OpenReservationWindow(object sender, RoutedEventArgs e)
        {
            TourDetailed.Close();
            TourReservationWindow tourReservation = new TourReservationWindow(Tour, User);
            tourReservation.ShowDialog();
            
        }
        public void GoBack(object sender, RoutedEventArgs e)
        {
            TourDetailed.Close();
        }
    }
}
