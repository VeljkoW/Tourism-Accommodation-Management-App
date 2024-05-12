using BookingApp.Domain.Model;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BookingApp.ViewModel.Tourist
{
    public class TourDetailedViewModel
    {
        public RelayCommand ClickLeftArrow => new RelayCommand(execute => ClickLeftArrowExecute(),canExecute => ClickLeftArrowCanExecute());
        public RelayCommand ClickRightArrow => new RelayCommand(execute => ClickRightArrowExecute(),canExecute => ClickRightArrowCanExecute());
        public RelayCommand ClickGoBack => new RelayCommand(execute => GoBackExecute());
        public RelayCommand ClickReserve => new RelayCommand(execute => OpenReservationWindowExecute());
        public Tour Tour { get; set; }
        public User User { get; set; }
        public TourDetailed TourDetailed { get; set; }
        private int Counter;
        public TourDetailedViewModel(TourDetailed tourDetailed,Tour selectedTour,User user) 
        {
            Tour = selectedTour;
            User = user;
            TourDetailed = tourDetailed;
            TourDetailed.NameTextBlock.Text = Tour.Name;
            TourDetailed.DescriptionTextBlock.Text = Tour.Description;
            Counter = 0;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
                TourDetailed.Image1.Source = (ImageSource)converter.ConvertFromString(Image.Path);
                if(Tour.Images.Count > 1)
                {
                    TourDetailed.Image2.Source = (ImageSource)converter.ConvertFromString(Tour.Images[1].Path);
                }
                if(Tour.Images.Count > 2)
                {
                    TourDetailed.Image3.Source = (ImageSource)converter.ConvertFromString(Tour.Images[2].Path);
                    TourDetailed.LeftArrowButton.Visibility = Visibility.Visible;
                    TourDetailed.RightArrowButton.Visibility = Visibility.Visible;
                }
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

        public void OpenReservationWindowExecute()
        {
            TourDetailed.Close();
            TourReservationWindow tourReservation = new TourReservationWindow(Tour, User);
            tourReservation.ShowDialog();
            
        }
        public void GoBackExecute()
        {
            TourDetailed.Close();
        }
        public void ClickLeftArrowExecute()
        {
            Counter--;
            var converter = new ImageSourceConverter();
            TourDetailed.Image1.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter].Path);
            TourDetailed.Image2.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 1].Path);
            TourDetailed.Image3.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 2].Path);
        }
        public void ClickRightArrowExecute()
        {
            Counter++;
            var converter = new ImageSourceConverter();
            TourDetailed.Image1.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter].Path);
            TourDetailed.Image2.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 1].Path);
            TourDetailed.Image3.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 2].Path);
        }
        public bool ClickRightArrowCanExecute()
        {
            if (Tour.Images.Count > Counter+3) { return true; }
            return false;
        }
        public bool ClickLeftArrowCanExecute()
        {
            var converter = new ImageSourceConverter();
            if (TourDetailed.Image1.Source.ToString() != Tour.Images[0].Path && Tour.Images.Count > 0) { return true; }
            return false;
        }
    }
}
