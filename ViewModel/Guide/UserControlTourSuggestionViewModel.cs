using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.ViewModel.Guide
{
    public class UserControlTourSuggestionViewModel : INotifyPropertyChanged
    {
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _fromDate;
        public string FromDate
        {
            get => _fromDate;
            set
            {
                if (value != _fromDate)
                {
                    _fromDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _toDate;
        public string ToDate
        {
            get => _toDate;
            set
            {
                if (value != _toDate)
                {
                    _toDate = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _touristCount;
        public string TouristCount
        {
            get => _touristCount;
            set
            {
                if (value != _touristCount)
                {
                    _touristCount = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _location;
        public string Location
        {
            get => _location;
            set
            {
                if (value != _location)
                {
                    _location = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Dates
        {
            get => String.Format("{0} - {1}",TourSuggestion.FromDate.ToShortDateString(),TourSuggestion.ToDate.ToShortDateString());
        }

        public TourSuggestion TourSuggestion { get;set;}
        private int userId = GuideMainWindow.UserId;
        public TourRequestsPageViewModel TourRequestsPageViewModel { get; }
        public RelayCommand AcceptTourRequest => new RelayCommand(execute => AcceptTourRequestExecute());


        public void AcceptTourRequestExecute()
        {
            UserControlAcceptTourRequest popup = new UserControlAcceptTourRequest(TourRequestsPageViewModel,TourSuggestion);
            TourRequestsPageViewModel.Dimm();
            TourRequestsPageViewModel.AcceptTourCard = popup;
        }
        public UserControlTourSuggestionViewModel(TourRequestsPageViewModel tourRequestsPageViewModel, TourSuggestion tour) {
            TourSuggestion = tour;
            this.TourRequestsPageViewModel = tourRequestsPageViewModel;
            ToDate = tour.ToDate.ToShortTimeString();
            FromDate = tour.FromDate.ToShortTimeString();
            TouristCount = tour.NumberOfPeople.ToString();
            Description= tour.Description;
            Language = tour.Language;
            Location location= LocationService.GetInstance().GetById(tour.LocationId) ?? new Domain.Model.Location();
            Location = location.State + " " + location.City;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}