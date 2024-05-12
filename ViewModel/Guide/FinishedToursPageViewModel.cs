using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class FinishedToursPageViewModel : INotifyPropertyChanged
    {
        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());
        public FinishedToursPage FinishedToursPage { get; }
        public User User { get; }
        public ObservableCollection<UserControlTourCardForReview> Cards { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClickGoBackExecute()
        {
            Cards.Clear();
            FinishedToursPage.NavigationService.GoBack();
        }
        public FinishedToursPageViewModel(FinishedToursPage finishedToursPage, User user)
        {
            FinishedToursPage = finishedToursPage;
            User = user;
            Cards = new ObservableCollection<UserControlTourCardForReview>();
            Load();
        }
        public void Load()
        {
            Cards.Clear();
            Dictionary<TourSchedule, List<TourReview>> finishedTours = TourReviewService.LoadFinishedTours();
            foreach (var item in finishedTours)
            {
                Cards.Add(new UserControlTourCardForReview(this, item.Key, item.Value));
            }
        }
    }
}
