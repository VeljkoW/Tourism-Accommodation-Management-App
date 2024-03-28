using BookingApp.Model;
using BookingApp.Repository.TourRepositories;
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
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourOngoingDetailed.xaml
    /// </summary>
    public partial class TourOngoingDetailed : Window
    {
        public Tour Tour { get; set; }
        public User User { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public TourOngoingDetailed(Tour SelectedOngoingTour,User user)
        {
            InitializeComponent();
            DataContext = this;
            Tour = SelectedOngoingTour;
            User = user;

            NameTextBlock.Text = Tour.Name;

            if (Tour.Images != null && Tour.Images.Count > 0)
            {
                Image Image = Tour.Images[0];
                var converter = new ImageSourceConverter();
                ImageBox.Source = (ImageSource)converter.ConvertFromString(Image.Path);
            }
            if (Tour.Location != null)
            {
                StateTextBlock.Text = Tour.Location.State;
                if (!String.IsNullOrEmpty(Tour.Location.City))
                {
                    CityTextBlock.Text = ", " + Tour.Location.City;
                }
                else
                {
                    CityTextBlock.Text = Tour.Location.City;
                }
            }

            LanguageTextBlock.Text = Tour.Language;
            DateTextBlock.Text = Tour.DateTime.Date.ToString();

            DurationTextBlock.Text = Tour.Duration.ToString() + "h";

            MaxPeopleTextBlock.Text = Tour.MaxTourists.ToString();

            KeyPoints = Tour.KeyPoints;

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

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
