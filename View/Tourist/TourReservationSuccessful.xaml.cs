using BookingApp.Domain.Model;
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
using System.Windows.Shapes;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReservationSuccessful.xaml
    /// </summary>
    public partial class TourReservationSuccessful : Window
    {
        public Tour Tour {  get; set; }
        public TourReservation TourReservation { get; set; }
        public TourReservationSuccessful(Tour tour,TourReservation tourReservation)
        {
            InitializeComponent();
            Tour = tour;
            TourReservation = tourReservation;
            NumberTextBlock.Text = TourReservation.People.Count().ToString();
            TourNameTextBlock.Text = "\"" + Tour.Name + "\"";
            
            //if(!Tour.Name.Contains("Tour") && !Tour.Name.Contains("tour"))
            //{
                TourTextBlock.Text = " tour.";
            //}
            //else
            //{
            //    TourTextBlock.Text = ".";
            //}

            TourDateTextBlock.Text = Tour.DateTime.ToString();

        }
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double SWidth = SystemParameters.PrimaryScreenWidth;
            double SHeight = SystemParameters.PrimaryScreenHeight;
            double WWidth = this.Width;
            double WHeight = this.Height;

            this.Left = (SWidth - WWidth) / 2;
            this.Top = (SHeight - WHeight) / 2;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
