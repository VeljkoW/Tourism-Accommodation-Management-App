using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
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

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for GuestRenovation.xaml
    /// </summary>
    public partial class GuestRenovation : Window
    {
        public int Level { get; set; }
        public GuestRenovationViewModel guestRenovationViewModel { get; set; }

        public GuestRate GuestRate { get; set; }
        public GuestRenovation(GuestRate guestRate, ReservedAccommodation reservedAccommodation)
        {
            InitializeComponent();
            GuestRate = guestRate;
            guestRenovationViewModel = new GuestRenovationViewModel(this, reservedAccommodation);
            DataContext = guestRenovationViewModel;
            Level = 0;
        }

        private void LevelChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "Level1")
                    {
                        Level = 1;
                    }
                    else if (radioButton.Name == "Level2")
                    {
                        Level = 2;
                    }
                    else if (radioButton.Name == "Level3")
                    {
                        Level = 3;
                    }
                    else if (radioButton.Name == "Level4")
                    {
                        Level = 4;
                    }
                    else if (radioButton.Name == "Level5")
                    {
                        Level = 5;
                    }
                }
            }
        }
    }
}
