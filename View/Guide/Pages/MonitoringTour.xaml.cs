using BookingApp.Model;
using BookingApp.Repository.TourRepositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for MonitoringTour.xaml
    /// </summary>
    public partial class MonitoringTour : Page
    {
        TourScheduleRepository tourScheduleRepository  { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public event EventHandler OnFinishedTour;

        private string _tourName;
        public string TourName
        {
            get => _tourName;
            set
            {
                if (value != _tourName)
                {
                    _tourName = value;
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
        public MonitoringTour()
        {
            InitializeComponent();
        }
        public MonitoringTour(Tour tour, User user, TourSchedule schedule)
        {
            InitializeComponent();
            DataContext = this;
            KeyPoints = tour.KeyPoints;
            TourName = tour.Name;
            User = user;
            Tour = tour;
            ScheduleId = schedule.Id;
            Schedule = schedule;
            if (Schedule.VisitedKeypoints > 0)
            {
                CurrentKeypointId = Schedule.VisitedKeypoints;
            }
            else
            {
                CurrentKeypointId = Tour.KeyPoints[0].Id;
            }
            tourScheduleRepository = new TourScheduleRepository();
            Update();
        }
        private void Update()
        {
            ListOfKeypoints.Children.Clear();
            ListOfTourists.Children.Clear();
            foreach (KeyPoint keyPoint in KeyPoints)
            {
                UserControlKeyPoint userControlKeyPoint = new UserControlKeyPoint(keyPoint);
                userControlKeyPoint.Margin = new Thickness(0, 0, 0, 15);
                if (keyPoint.Id <= CurrentKeypointId)
                {
                    userControlKeyPoint.VisitKeypointButton.IsEnabled = false;
                }
                ListOfKeypoints.Children.Add(userControlKeyPoint);
                userControlKeyPoint.EventHandler += (s, e) => changeCurrentKeypointId(s, e);
                userControlKeyPoint.OnClickedVisitTour += (s, e) => ClickedOnLastKeypoint(s, e);
            }
            (ListOfKeypoints.Children[0] as UserControlKeyPoint).VisitKeypointButton.IsEnabled = false;
            if (Schedule.VisitedKeypoints == -1)
            {
                Schedule.VisitedKeypoints = Tour.KeyPoints[0].Id;
                tourScheduleRepository.Update(Schedule);
            }
            TourReservationRepository reservationRepository = new TourReservationRepository();
            TourPersonRepository tourPersonRepository = new TourPersonRepository();
            List<TourReservation> reservations = reservationRepository.GetAll();
            foreach (TourReservation reservation in reservations)
            {
                if (reservation.TourScheduleId == Schedule.Id)
                {
                    List<TourPerson> persons = tourPersonRepository.GetAll();
                    foreach (TourPerson tourist in persons)
                    {
                        if (tourist.KeyPointId == -1)
                        {
                            UserControlTourist userControlTourist = new UserControlTourist(tourist, CurrentKeypointId);
                            ListOfTourists.Children.Add(userControlTourist);
                            userControlTourist.touristVisitedKeypoint += (s, e) => touristVisited();
                        }
                    }
                }
            }
        }
        public EventHandler OnLastKeypoint {  get; set; }
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
            if (Schedule.VisitedKeypoints == KeyPoints[KeyPoints.Count()-1].Id)
            {
                Schedule.ScheduleStatus = ScheduleStatus.Finished;
                tourScheduleRepository.Update(Schedule);
                FinishTour();
            }
            else
            {
            tourScheduleRepository.Update(Schedule);
            Update();
            }
        }
        private void ClickFinishTour(object sender, RoutedEventArgs e)
        {
            Schedule.ScheduleStatus = ScheduleStatus.Finished;
            tourScheduleRepository.Update(Schedule);
            RaiseEventHandler(sender, e);
            FinishTour();
        }
        private void FinishTour()
        {
            Schedule.ScheduleStatus = ScheduleStatus.Finished;
            tourScheduleRepository.Update(Schedule);
            Update();
            NavigationService.GoBack();
        }
        private void RaiseEventHandler(object sender, RoutedEventArgs e)
        {
            OnFinishedTour?.Invoke(sender, e);
        }
    }
}
