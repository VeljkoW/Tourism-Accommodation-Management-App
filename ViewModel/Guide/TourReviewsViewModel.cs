using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualKeyboard.Wpf;

namespace BookingApp.ViewModel.Guide
{
    public class TourReviewsViewModel
    {
        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());

        private void ClickGoBackExecute()
        {
            TourReviews.NavigationService.GoBack();
        }

        public TourReviewsViewModel(TourReviews tourReviews, TourSchedule schedule, List<TourReview> reviews)
        {
            TourReviews = tourReviews;
            Schedule = schedule;
            //Reviews = reviews;
            Reviews = TourReviewService.GetInstance().GetAll().Where(t => t.TourScheduleId == schedule.Id).ToList();
            TourName = TourService.GetInstance().GetById(schedule.TourId).Name;
            Date = schedule.Date.ToString();
            Load();
        }

        private void Load()
        {
            TourReviewList = new ObservableCollection<UserControlTouristReview>();
            foreach (TourReview t in Reviews)
            {
                UserControlTouristReview u = new UserControlTouristReview(this,t);
                TourReviewList.Add(u);
            }
        }

        public TourReviews TourReviews { get; }
        public TourSchedule Schedule { get; }
        public List<TourReview> Reviews { get; }
        public ObservableCollection<UserControlTouristReview> TourReviewList { get; set; }
        private string _tourName;
        private string _date;

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
    }
}
