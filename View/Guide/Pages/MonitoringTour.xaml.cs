using BookingApp.Model;
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
        public List<KeyPoint> KeyPoints { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }

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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MonitoringTour(Tour tour,User user)
        {
            InitializeComponent();
            DataContext = this;
            KeyPoints = tour.KeyPoints;
            TourName = tour.Name;
            User = user;
            Tour = tour;
            Update();
        }
        private void Update()
        {
            foreach(KeyPoint keyPoint in KeyPoints)
            {
                UserControlKeyPoint userControlKeyPoint = new UserControlKeyPoint(keyPoint);
                userControlKeyPoint.Margin = new Thickness(0, 0, 0, 15);
                ListOfKeypoints.Children.Add(userControlKeyPoint);
            }
        }
    }
}
