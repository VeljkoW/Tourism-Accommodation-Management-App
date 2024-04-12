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
        private string _imgPath;
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
        public string ImgPath
        {
            get { return _imgPath; }
            set
            {
                if (_imgPath != value)
                {
                    _imgPath = value;
                    OnPropertyChanged(nameof(ImgPath));
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
            Load();
        }
        public void Load()
        {
            Reviews = TourReviewService.GetInstance().GetAll().Where(t => t.TourScheduleId == Schedule.Id).ToList();
            Tour tour = TourService.GetInstance().GetById(Schedule.TourId);
            Description = tour.Description;
            Language = tour.Language;
            TourName = tour.Name;
            Location location = LocationService.GetInstance().GetById(tour.LocationId);
            Location = location.State + " " + location.City;
            Date = Schedule.Date.ToString();
            List<TourImage> tourImage = TourImageService.GetInstance().GetAll().Where(t=>t.TourId ==tour.Id).ToList();
            ImgPath = ImageService.GetInstance().GetById(tourImage[0].ImageId).Path;
        }
        public TourSchedule Schedule { get; set; }
        public List<TourReview> Reviews { get; set; }
    }
}
