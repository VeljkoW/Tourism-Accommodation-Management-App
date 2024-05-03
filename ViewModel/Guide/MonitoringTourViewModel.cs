using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using BookingApp.Services;

namespace BookingApp.ViewModel.Guide
{
    public class MonitoringTourViewModel
    {
        public List<KeyPoint> KeyPoints { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public event Action OnFinishedTour;

        private string _tourName;
        public string TourName
        {
            get => String.Format("Monitoring tour: {0}", _tourName);
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
                    OnPropertyChanged();
                }
            }
        }
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

        private int _scheduleId;
        public int ScheduleId
        {
            get => _scheduleId;
            set
            {
                if (value != _scheduleId)
                {
                    _scheduleId = value;
                    OnPropertyChanged();
                }
            }
        }
        private TourSchedule _schedule;
        public TourSchedule Schedule
        {
            get => _schedule;
            set
            {
                if (value != _schedule)
                {
                    _schedule = value;
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public RelayCommand ClickGoBack => new RelayCommand(execute => ClickGoBackExecute());
        public RelayCommand ClickFinishTour => new RelayCommand(execute => ClickFinishTourExecute());
        public MonitoringTour MonitoringTour { get; set; }
        public MonitoringTourViewModel() { }
        public MonitoringTourViewModel(MonitoringTour monitoringTour, Tour tour, User user, TourSchedule schedule)
        {
            MonitoringTour= monitoringTour;
            KeyPoints = tour.KeyPoints;
            TourName = tour.Name;
            User = user;
            Tour = tour;
            Description = tour.Description;
            ScheduleId = schedule.Id;
            Schedule = schedule;
            if (Schedule.VisitedKeypoints > 0)
            {
                CurrentKeypointId = Schedule.VisitedKeypoints;
            }
            else
            {
                CurrentKeypointId = Tour.KeyPoints[0].Id;
                Schedule.ScheduleStatus = ScheduleStatus.Ongoing;
            }
            Update();
        }
        private void Update()
        {
            MonitoringTour.ListOfKeypoints.Children.Clear();
            MonitoringTour.ListOfTourists.Children.Clear();
            foreach (KeyPoint keyPoint in KeyPoints)
            {
                UserControlKeyPoint userControlKeyPoint = new UserControlKeyPoint(keyPoint);
                userControlKeyPoint.Margin = new Thickness(0, 0, 0, 15);
                if (keyPoint.Id <= CurrentKeypointId)
                {
                    userControlKeyPoint.VisitKeypointButton.IsEnabled = false;
                }
                MonitoringTour.ListOfKeypoints.Children.Add(userControlKeyPoint);
                userControlKeyPoint.EventHandler += (s, e) => changeCurrentKeypointId(s, e);
                userControlKeyPoint.OnClickedVisitTour += (s, e) => ClickedOnLastKeypoint(s, e);
            }
            (MonitoringTour.ListOfKeypoints.Children[0] as UserControlKeyPoint).VisitKeypointButton.IsEnabled = false;
            if (Schedule.VisitedKeypoints == -1)
            {
                Schedule.VisitedKeypoints = Tour.KeyPoints[0].Id;
                TourScheduleService.GetInstance().Update(Schedule);
            }
            List<TourReservation> reservations = TourReservationService.GetInstance().GetAll();
            List<TourPerson> persons = TourPersonService.GetInstance().GetAll();
            CreateTouristCards(reservations);
        }

        public void CreateTouristCards(List<TourReservation> reservations)
        {
            foreach (TourReservation reservation in reservations)
            {
                if (reservation.TourScheduleId == Schedule.Id)
                {
                    foreach (TourPerson person in reservation.People)
                    {
                        if (person.KeyPointId == -1)
                        {
                            UserControlTourist userControlTourist = new UserControlTourist(person, CurrentKeypointId);
                            MonitoringTour.ListOfTourists.Children.Add(userControlTourist);
                            userControlTourist.touristVisitedKeypoint += touristVisited;
                        }
                    }
                }
            }
        }
        public EventHandler OnLastKeypoint { get; set; }
        private void ClickedOnLastKeypoint(object s, EventArgs e)
        {
            OnLastKeypoint?.Invoke(this, e);
        }

        private void touristVisited()
        {
            Update();
        }

        private void changeCurrentKeypointId(object s, KeyPoint e)
        {
            CurrentKeypointId = e.Id;
            Schedule.VisitedKeypoints = CurrentKeypointId;
            if (Schedule.VisitedKeypoints == KeyPoints[KeyPoints.Count() - 1].Id)
            {
                Schedule.ScheduleStatus = ScheduleStatus.Finished;
                TourScheduleService.GetInstance().Update(Schedule);
                OnFinishedTour?.Invoke();
                OnClickGoBack?.Invoke();
                FinishTour();
            }
            else
            {
                TourScheduleService.GetInstance().Update(Schedule);
                Update();
            }
        }
        private void ClickFinishTourExecute()
        {
            Schedule.ScheduleStatus = ScheduleStatus.Finished;
            TourScheduleService.GetInstance().Update(Schedule);
            FinishTour();
            OnFinishedTour?.Invoke();
            OnClickGoBack?.Invoke();
            RaiseEventHandler();
        }
        private void FinishTour()
        {
            Schedule.ScheduleStatus = ScheduleStatus.Finished;
            TourScheduleService.GetInstance().Update(Schedule);
            Update();
            MonitoringTour.NavigationService.GoBack();
        }
        private void RaiseEventHandler()
        {
            OnFinishedTour?.Invoke();
        }
        public Action OnClickGoBack { get; set; }
        private void ClickGoBackExecute()
        {
            OnClickGoBack?.Invoke();
            RaiseEventHandler();
            MonitoringTour.NavigationService.GoBack();
        }
    }
}
