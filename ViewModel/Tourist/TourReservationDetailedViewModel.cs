using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
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
    public class TourReservationDetailedViewModel
    {
        public TourReservationDetailed TourReservationDetailed { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public List<TourPerson> allTourists { get; set; }
        public RelayCommand ClickGoBack => new RelayCommand(execute => GoBackExecute());
        public TourReservationDetailedViewModel(TourReservationDetailed tourReservationDetailed,Tour selectedReservedTour,User user) 
        {
            this.TourReservationDetailed = tourReservationDetailed;
            Tour = selectedReservedTour;
            User = user;

            allTourists = new List<TourPerson>();

            TourReservationDetailed.NameTextBlock.Text = Tour.Name;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourReservationDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
            }
            if (Tour.Location != null)
            {
                TourReservationDetailed.StateTextBlock.Text = Tour.Location.State;
                if (!String.IsNullOrEmpty(Tour.Location.City))
                {
                    TourReservationDetailed.CityTextBlock.Text = ", " + Tour.Location.City;
                }
                else
                {
                    TourReservationDetailed.CityTextBlock.Text = Tour.Location.City;
                }
            }

            TourReservationDetailed.LanguageTextBlock.Text = Tour.Language;
            TourReservationDetailed.DateTextBlock.Text = Tour.DateTime.Date.ToString();
            TourReservationDetailed.DurationTextBlock.Text = Tour.Duration.ToString() + "h";
            TourReservationDetailed.MaxPeopleTextBlock.Text = Tour.MaxTourists.ToString();

            foreach (TourSchedule tourSchedule in TourScheduleService.GetInstance().GetAll())
            {
                if (tourSchedule.Date == Tour.DateTime && tourSchedule.TourId == Tour.Id)
                {
                    foreach (TourReservation tourReservation in TourReservationService.GetInstance().GetAll())
                    {
                        if (tourReservation.TourScheduleId == tourSchedule.Id && tourReservation.UserId == User.Id)
                        {
                            allTourists.AddRange(tourReservation.People);
                        }
                    }
                }
            }

            TourReservationDetailed.ReservedTourists.Text = allTourists.Count().ToString();
            if (allTourists.Count() != 1)
            {
                TourReservationDetailed.PeoplePersonTextBlock.Text = " people.";
            }
            else
            {
                TourReservationDetailed.PeoplePersonTextBlock.Text = " person.";
            }
        }

        public void GoBackExecute()
        {
            TourReservationDetailed.Close();
        }
    }
}
