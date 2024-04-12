using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlTouristReviewViewModel
    {
        private string _knowledge;
        private string _language;
        private string _enjoyment;
        private string _description;
        private string _touristName;
        public RelayCommand ReportReview => new RelayCommand(execute => ReportReviewExecute(), canExecute => ReportReviewCanExecute());
        private void ReportReviewExecute()
        {
            Review.Status = ReviewStatus.Invalid;
            TourReviewService.GetInstance().Update(Review);
            Load();
        }

        private bool ReportReviewCanExecute()
        {
            return Review.Status == ReviewStatus.Valid? true : false;
        }

        public string Knowledge
        {
            get { return _knowledge; }
            set
            {
                if (_knowledge != value)
                {
                    _knowledge = value;
                    OnPropertyChanged(nameof(Knowledge));
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

        public string Enjoyment
        {
            get { return _enjoyment; }
            set
            {
                if (_enjoyment != value)
                {
                    _enjoyment = value;
                    OnPropertyChanged(nameof(Enjoyment));
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
        public string TouristName
        {
            get { return _touristName; }
            set
            {
                if (_touristName != value)
                {
                    _touristName = value;
                    OnPropertyChanged(nameof(TouristName));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UserControlTouristReviewViewModel(TourReview t)
        {
            Review = t;
            Load();
        }
        public void Load()
        {
            Description = Review.Comment;
            Enjoyment = Review.TourEnjoyment.ToString();
            Language = Review.GuideSpeech.ToString();
            Knowledge = Review.GuideKnowledge.ToString();
            TouristName = UserService.GetInstance().GetById(Review.UserId)?.Username ?? "Username";

        }
        public TourReview Review { get; }
    }
}
