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

namespace BookingApp.ViewModel.Guide
{
    public class UserControlComplexTourSuggestionCardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string fromDate;
        private string toDate;
        public string DateRange
        {
            get => String.Format("{0} - {1}", fromDate, toDate);
        }
        private string location;
        public string Location
        {
            get => location;
        }
        private string language;
        public string Language
        {
            get => language;
        }
        private int people;
        public int People
        {
            get => people;
        }
        private string description;
        public string Description
        {
            get => description;
        }
        public RelayCommand AcceptTour => new RelayCommand(canExecute => AcceptTourExecute());

        private void AcceptTourExecute()
        {
            complexTourRequestToursPage.PopupPanel.Children.Clear();
            UserControlAcceptComplexTourSuggestion popup = new UserControlAcceptComplexTourSuggestion(complexTourRequestToursPage, suggestion);
            popup.requestRefresh += LoadComplextours;
            complexTourRequestToursPage.PopupPanel.Children.Add(popup);
        }

        private void LoadComplextours()
        {
            complexTourRequestToursPage.Load();
            userControlComplexTourSuggestionListing.LoadTours();
            complexTourRequestToursPage.NavigationService.GoBack();
        }

        private ComplexTourRequestToursPage complexTourRequestToursPage;
        private UserControlComplexTourSuggestionListing userControlComplexTourSuggestionListing;
        private TourSuggestion suggestion;
        public UserControlComplexTourSuggestionCardViewModel(ComplexTourRequestToursPage complexTourRequestToursPage, UserControlComplexTourSuggestionListing userControlComplexTourSuggestionListing, TourSuggestion suggestion)
        {
            this.suggestion = suggestion;
            this.userControlComplexTourSuggestionListing = userControlComplexTourSuggestionListing;
            this.complexTourRequestToursPage = complexTourRequestToursPage;
            fromDate = suggestion.FromDate.ToShortDateString();
            toDate = suggestion.ToDate.ToShortDateString();
            Location loc = LocationService.GetInstance().GetById(suggestion.LocationId);
            this.location = loc.State + " " + loc.City;
            people = suggestion.NumberOfPeople;
            language = suggestion.Language;
            description = suggestion.Description;
        }
    }
}
