using BookingApp.Model;
using BookingApp.Repository.TourRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {
        public List<Tour> Tours {  get; set; }
        public TourRepository tourRepository { get; set; }
        public User User { get; set; }
        public string Username {  get; set; }
        public TouristMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
            Username = User.Username;

            if(Username.Length > 10)
            {
                Username = Username.Substring(0,10) + "...";    
            }

            tourRepository = new TourRepository();

            //Tours = tourRepository.GetAll();
            Tours = new List<Tour>
            { 
                new Tour(1,"Tour 1",new Location(),"descriptionaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa","lang",100,new List<KeyPoint>(),10,new List<Image>()),
                new Tour(2,"Tour 2",new Location(),"description","lang",100,new List<KeyPoint>(),10,new List<Image>()),
                new Tour(3,"Tour 3",new Location(),"description","lang",100,new List<KeyPoint>(),10,new List<Image>()),
                new Tour(4,"Tour 4",new Location(),"description","lang",100,new List<KeyPoint>(),10,new List<Image>())
            };


            
        }

        private void SearchBoxClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Search tours...")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }

        private void SearchBoxNotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search tours...";
                textBox.Foreground= Brushes.Gray;
            }
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
        private void ToursTabClick(object sender, RoutedEventArgs e)
        {
            Tab.SelectedIndex = 0;
        }
        private void ReservationTabClick(object sender, RoutedEventArgs e)
        {
            Tab.SelectedIndex = 1;
        }
        
        private void SuggestionTabClick(object sender, RoutedEventArgs e)
        {
            Tab.SelectedIndex = 2;
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

    }
}
