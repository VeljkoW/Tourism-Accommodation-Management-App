using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
        private int Counter;
        public RelayCommand ClickLeftArrow => new RelayCommand(execute => ClickLeftArrowExecute(), canExecute => ClickLeftArrowCanExecute());
        public RelayCommand ClickRightArrow => new RelayCommand(execute => ClickRightArrowExecute(), canExecute => ClickRightArrowCanExecute());
        public RelayCommand ClickGoBack => new RelayCommand(execute => GoBackExecute());
        public RelayCommand ClickReview => new RelayCommand(execute => ReviewExecute());
        public TourFinishedDetailedViewModel(TourFinishedDetailed tourFinishedDetailed,Tour selectedTour,User user) 
        { 
            this.TourFinishedDetailed = tourFinishedDetailed;
            Tour = selectedTour;
            User = user;
            TourFinishedDetailed.NameTextBlock.Text = Tour.Name;
            TourFinishedDetailed.DescriptionTextBlock.Text = Tour.Description;
            Counter = 0;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                TourFinishedDetailed.ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
                TourFinishedDetailed.Image1.Source = (ImageSource)converter.ConvertFromString(Image.Path);
                if (Tour.Images.Count > 1)
                {
                    TourFinishedDetailed.Image2.Source = (ImageSource)converter.ConvertFromString(Tour.Images[1].Path);
                }
                if (Tour.Images.Count > 2)
                {
                    TourFinishedDetailed.Image3.Source = (ImageSource)converter.ConvertFromString(Tour.Images[2].Path);
                    TourFinishedDetailed.LeftArrowButton.Visibility = Visibility.Visible;
                    TourFinishedDetailed.RightArrowButton.Visibility = Visibility.Visible;
                }
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
        public void ReviewExecute()
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
        public void GoBackExecute()
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
        public void ClickLeftArrowExecute()
        {
            Counter--;
            var converter = new ImageSourceConverter();
            TourFinishedDetailed.Image1.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter].Path);
            TourFinishedDetailed.Image2.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 1].Path);
            TourFinishedDetailed.Image3.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 2].Path);
        }
        public void ClickRightArrowExecute()
        {
            Counter++;
            var converter = new ImageSourceConverter();
            TourFinishedDetailed.Image1.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter].Path);
            TourFinishedDetailed.Image2.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 1].Path);
            TourFinishedDetailed.Image3.Source = (ImageSource)converter.ConvertFromString(Tour.Images[Counter + 2].Path);
        }
        public bool ClickRightArrowCanExecute()
        {
            if (Tour.Images.Count > Counter + 3) { return true; }
            return false;
        }
        public bool ClickLeftArrowCanExecute()
        {
            var converter = new ImageSourceConverter();
            if (TourFinishedDetailed.Image1.Source.ToString() != Tour.Images[0].Path && Tour.Images.Count > 0) { return true; }
            return false;
        }
    }
}
