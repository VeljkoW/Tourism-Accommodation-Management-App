using BookingApp.Domain.Model;
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
    public class TourFinishedDetailedViewModel
    {
        public TourFinishedDetailed TourFinishedDetailed { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public bool AttendenceConfirmed;
        public bool Attended;
        public TourFinishedDetailedViewModel(TourFinishedDetailed tourFinishedDetailed,Tour selectedTour,User user) 
        { 
            this.TourFinishedDetailed = tourFinishedDetailed;
            Tour = selectedTour;
            User = user;
            TourFinishedDetailed.NameTextBlock.Text = Tour.Name;
            TourFinishedDetailed.DescriptionTextBlock.Text = Tour.Description;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourFinishedDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
            }
            if (Tour.Location != null)
            {
                TourFinishedDetailed.StateTextBlock.Text = Tour.Location.State;
                if (!String.IsNullOrEmpty(Tour.Location.City))
                {
                    TourFinishedDetailed.CityTextBlock.Text = ", " + Tour.Location.City;
                }
                else
                {
                    TourFinishedDetailed.CityTextBlock.Text = Tour.Location.City;
                }
            }

            TourFinishedDetailed.LanguageTextBlock.Text = Tour.Language;
            TourFinishedDetailed.DateTextBlock.Text = Tour.DateTime.Date.ToString();

            TourFinishedDetailed.DurationTextBlock.Text = Tour.Duration.ToString() + "h";

            TourFinishedDetailed.MaxPeopleTextBlock.Text = Tour.MaxTourists.ToString();
        }
        public void OpenReviewWindow(object sender, RoutedEventArgs e)
        {
            AttendenceConfirmed = true;
            Attended = false;
            GetAttendence();

            if (AttendenceConfirmed && Attended)
            {
                bool rated = false;
                foreach (TourReview tourReview in TourReviewService.GetInstance().GetAll())
                {
                    TourSchedule tourschedule = TourScheduleService.GetInstance().GetById(tourReview.TourScheduleId);
                    if (tourschedule.TourId == Tour.Id && tourschedule.Date == Tour.DateTime)
                    {
                        rated = true;
                    }
                }
                if (!rated)
                {
                    TourFinishedDetailed.Close();
                    TourReviewWindow tourReviewWindow = new TourReviewWindow(Tour, User);
                    tourReviewWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Tour already rated");
                }
            }
            else if(!AttendenceConfirmed && Attended)
            {
                MessageBox.Show("You need to confirm the attendence in the notifications menu");
            }
            else
            {
                MessageBox.Show("No one from the reservation attended this tour!");
            }
        }
        public void GoBack(object sender, RoutedEventArgs e)
        {
            TourFinishedDetailed.Close();
        }
        public void GetAttendence()
        {
            foreach (TourSchedule tourSchedule in TourScheduleService.GetInstance().GetAll())
            {
                foreach (TourReservation tourReservation in TourReservationService.GetInstance().GetAll())
                {
                    if (tourReservation.TourScheduleId == tourSchedule.Id && tourSchedule.TourId == Tour.Id && tourSchedule.Date == Tour.DateTime)

                    {
                        foreach (TourPerson tourPerson in tourReservation.People)
                        {
                            foreach (TourAttendenceNotification tourAttendenceNotification in TourAttendenceNotificationService.GetInstance().GetAll())
                            {
                                if (tourPerson.Id == tourAttendenceNotification.TourPersonId)
                                {
                                    if (tourAttendenceNotification.ConfirmedAttendence == false)
                                    {
                                        AttendenceConfirmed = false;
                                    }
                                    Attended = true;
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
