using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
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
    public class UserControlTouristViewModel
    {
        public KeyPoint KeyPoint { get; set; }
        TourPersonRepository tourPersonRepository { get; set; }

        private string _touristName;
        public string TouristName
        {
            get => _touristName;
            set
            {
                if (value != _touristName)
                {
                    _touristName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _touristSurname;
        public string TouristSurname
        {
            get => _touristSurname;
            set
            {
                if (value != _touristSurname)
                {
                    _touristSurname = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _touristAge;
        public int TouristAge
        {
            get => _touristAge;
            set
            {
                if (value != _touristAge)
                {
                    _touristAge = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _currentKeypointId;
        public int CurrentKeypointId
        {
            get => _currentKeypointId;
            set
            {
                if (value != _currentKeypointId)
                {
                    _currentKeypointId = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourPerson Tourist { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public RelayCommand ClickHasVisitedKeypoint => new RelayCommand(execute => ClickHasVisitedKeypointExecute());
        public UserControlTouristViewModel(TourPerson tourist, int currentKeypointId)
        {
            TouristName = tourist.Name;
            TouristSurname = tourist.Surname;
            TouristAge = tourist.Age;
            Tourist = new TourPerson();
            Tourist = tourist;
            CurrentKeypointId = currentKeypointId;
            tourPersonRepository = new TourPersonRepository();
        }
        private void ClickHasVisitedKeypointExecute()
        {
            Tourist.KeyPointId = CurrentKeypointId;
            tourPersonRepository.Update(Tourist);
            int userId = -1;
            foreach (TourReservation tourReservation in TourReservationService.GetInstance().GetAll())
            {
                foreach (TourPerson tourPerson1 in tourReservation.People)
                {
                    if (tourPerson1.Id == Tourist.Id)
                    {
                        userId = tourReservation.UserId;
                        break;
                    }
                }
            }
            if (userId != -1)
            {
                TourAttendenceNotification tourAttendenceNotification = new TourAttendenceNotification(userId, Tourist.Id, DateTime.Now, false);
                TourAttendenceNotificationService.GetInstance().Add(tourAttendenceNotification);
            }
            touristVisiting();
        }
        public Action touristVisitedKeypoint { get; set; }
        private void touristVisiting()
        {
            touristVisitedKeypoint?.Invoke();
        }

    }
}
