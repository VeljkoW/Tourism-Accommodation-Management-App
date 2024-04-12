using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlTourCardForReviewViewModel
    {
        private string _tourName;
        private string _location;
        private string _language;
        private string _description;
        private string _date;
        public RelayCommand CheckReviews => new RelayCommand(execute => CheckReviewsExecute());

        private void CheckReviewsExecute()
        {
            TourReviews tourReviews = new TourReviews(this,Schedule,Reviews);
            UserControlTourCardForReview.FinishedToursPageViewModel.FinishedToursPage.NavigationService.Navigate(tourReviews);
        }
        public string TourName
        {
            get { return _tourName; }
            set
            {
                if (_tourName != value)
                {
                    _tourName = value;
                    OnPropertyChanged(nameof(TourName));
                }
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        public string Language
        {
            get { return _language; }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged(nameof(Language));
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
        public string Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserControlTourCardForReview UserControlTourCardForReview { get; set; }
        public UserControlTourCardForReviewViewModel(UserControlTourCardForReview userControlTourCardForReview, TourSchedule schedule, List<TourReview> reviews)
        {
            UserControlTourCardForReview= userControlTourCardForReview;
            Schedule = schedule;
            Reviews = TourReviewService.GetInstance().GetAll().Where(t => t.TourScheduleId == schedule.Id).ToList();
            Tour tour = TourService.GetInstance().GetById(schedule.TourId);
            Description = tour.Description;
            Language = tour.Language;
            TourName = tour.Name;
            Location location = LocationService.GetInstance().GetById(tour.LocationId);
            Location = location.State+ " "+ location.City;
            Date = schedule.Date.ToString();
        }
        public TourSchedule Schedule { get; }
        public List<TourReview> Reviews { get; }
    }
}
