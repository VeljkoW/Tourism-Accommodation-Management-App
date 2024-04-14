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
        private string _imgPath;
        private string _joinedOn;
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
        public string JoinedOn
        {
            get { 
                if(_joinedOn == null)
                {
                    return string.Format("Didn't join");
                }
                return string.Format("Joined on: {0}", _joinedOn); 
            }
            set
            {
                if (_joinedOn != value)
                {
                    _joinedOn = value;
                    OnPropertyChanged(nameof(JoinedOn));
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
            TourReviewImage tourReviewImage = TourReviewImageService.GetInstance().GetAll().Where(t => t.TourReviewId == Review.Id).FirstOrDefault();
            if(tourReviewImage != null)
            {
            ImgPath = ImageService.GetInstance().GetById(tourReviewImage.ImageId).Path;
            }
            else
            {
                ImgPath = "../../../Resources/Images/No-Image-Placeholder.png";
            }
            List<TourPerson> People = new List<TourPerson>();
            foreach (var item in TourReservationService.GetInstance().GetAll().Where(t => t.UserId == Review.UserId).Where(t => t.TourScheduleId == Review.TourScheduleId))
            {
                foreach (var item1 in item.People)
                {
                    People.Add(item1);
                }
            }
            People = People.Where(t => t.KeyPointId > -1).ToList();
            if (People.Count > 0)
            {
            int KeypointId = People.First().KeyPointId;
            JoinedOn = KeyPointService.GetInstance().GetById(KeypointId).Point;
            }
        }
        public TourReview Review { get; }
    }
}