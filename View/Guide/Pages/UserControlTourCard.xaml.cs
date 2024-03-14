using BookingApp.Model;
using BookingApp.View.Owner;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for UserControlTourCard.xaml
    /// </summary>
    public partial class UserControlTourCard : UserControl
    {
        public event EventHandler TourSelected;
        public Tour Tour { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public Image Image {  get; set; }

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

        private string _date;
        public string DateString
        {
            get => _date;
            set
            {
                if (value != _date)
                {
                    _date = value;
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserControlTourCard(Tour t, User user)
        {
            InitializeComponent();
            DataContext = this;
            Tour = t;
            User = user;
            TourName =t.Name;
            DateString=t.DateTime.ToString();
            Description = t.Description;
            TourDate.Text = Date.ToString();
            Image =t.Images[0];
            var converter = new ImageSourceConverter();
            MainImage.Source = (ImageSource)converter.ConvertFromString(Image.Path);
        }
        private void Update()
        {
            TourSelected?.Invoke(this, EventArgs.Empty);
        }

        private void MonitoringSelectedTour(object sender, RoutedEventArgs e)
        {
            MonitoringTour monitoringTour = new MonitoringTour(Tour,User);
            NavigationService.GetNavigationService(this).Navigate(monitoringTour);
        }
    }
}
