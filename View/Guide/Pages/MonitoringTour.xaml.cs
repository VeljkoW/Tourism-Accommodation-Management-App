using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MonitoringTour(Tour tour,User user)
        {
            InitializeComponent();
            KeyPoints = tour.KeyPoints;
            User = user;
            Tour = tour;
            Update();
        }
        private void Update()
        {
            foreach(var keyPoint in KeyPoints)
            {

               // ListOfKeypoints.Add(keyPoint);
            }
        }
    }
}
