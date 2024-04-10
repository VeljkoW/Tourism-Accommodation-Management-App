using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Repository;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BookingApp.Services;

namespace BookingApp.ViewModel.Tourist
{
    public class TourOngoingDetailedViewModel
    {
        public Tour Tour { get; set; }
        public User User { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public TourOngoingDetailed TourOngoingDetailed { get; set; }
        public TourOngoingDetailedViewModel(TourOngoingDetailed tourOngoingDetailed,Tour SelectedOngoingTour,User user) 
        {
            this.TourOngoingDetailed = tourOngoingDetailed;
            Tour = SelectedOngoingTour;
            User = user;

            KeyPoints = new List<KeyPoint>();

            TourOngoingDetailed.NameTextBlock.Text = Tour.Name;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourOngoingDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
            }
            if (Tour.Location != null)
            {
                TourOngoingDetailed.StateTextBlock.Text = Tour.Location.State;
                if (!String.IsNullOrEmpty(Tour.Location.City))
                {
                    TourOngoingDetailed.CityTextBlock.Text = ", " + Tour.Location.City;
                }
                else
                {
                    TourOngoingDetailed.CityTextBlock.Text = Tour.Location.City;
                }
            }

            TourOngoingDetailed.LanguageTextBlock.Text = Tour.Language;
            TourOngoingDetailed.DateTextBlock.Text = Tour.DateTime.Date.ToString();

            TourOngoingDetailed.DurationTextBlock.Text = Tour.Duration.ToString() + "h";

            TourOngoingDetailed.MaxPeopleTextBlock.Text = Tour.MaxTourists.ToString();

            foreach (TourSchedule tourSchedule in TourScheduleService.GetInstance().GetAll())
            {
                if (tourSchedule.TourId == Tour.Id && tourSchedule.Date == Tour.DateTime)
                {
                    int CurrentKeyPoint = tourSchedule.VisitedKeypoints;

                    foreach (KeyPoint keyPoint in Tour.KeyPoints)
                    {
                        if (keyPoint.Id <= CurrentKeyPoint)
                        {
                            keyPoint.IsVisited = true;
                        }
                    }
                }
            }


            KeyPoints = Tour.KeyPoints;

        }
    }
}
