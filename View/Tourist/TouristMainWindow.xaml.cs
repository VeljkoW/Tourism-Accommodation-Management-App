using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {
        public ObservableCollection<Tour> Tours {  get; set; }
        public TouristMainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Tours = new ObservableCollection<Tour>
            {
                new Tour(1,"Tour 1",new Location(),"descriptionaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa","lang",100,new List<string>(),10,new List<string>()),
                //int id, string name, Location location, string description, string language, int maxTourists, List<string> keyPoints, int duration, List<string> imagePaths
                new Tour(1,"Tour 2",new Location(),"description","lang",100,new List<string>(),10,new List<string>()),
                new Tour(1,"Tour 3",new Location(),"description","lang",100,new List<string>(),10,new List<string>()),
                new Tour(1,"Tour 4",new Location(),"description","lang",100,new List<string>(),10,new List<string>())
            };
            


        }

        private void SearchBox_Clicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Search tours...")
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }

        }

        private void SearchBox_NotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if(string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search tours...";
                textBox.Foreground= Brushes.Gray;
            }
        }

    }
}
