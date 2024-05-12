using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
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
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guest.Windows
{
    public partial class GuestRate : Window
    {
        public int Cleanliness {  get; set; }
        public int Integrity { get; set; }
        public ReservedAccommodation ReservedAccommodation { get; set; }
        public  GuestRateViewModel guestRateViewModel { get; set; }
        public GuestRate(User user, ReservedAccommodation reservedAccommodation)
        {
            InitializeComponent();
            guestRateViewModel = new GuestRateViewModel(this, user, reservedAccommodation);
            DataContext = guestRateViewModel;
            ReservedAccommodation = reservedAccommodation;
            Cleanliness = 0;
            Integrity = 0;
        }

        private void RenovationClick(object sender, RoutedEventArgs e)
        {
            GuestRenovation guestRenovation = new GuestRenovation(this, ReservedAccommodation);
            guestRenovation.Show();
        }
        private void CleanLinessChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "Cleanliness1")
                        Cleanliness = 1;
                    else if (radioButton.Name == "Cleanliness2")
                        Cleanliness = 2;
                    else if (radioButton.Name == "Cleanliness3")
                        Cleanliness = 3;
                    else if (radioButton.Name == "Cleanliness4")
                        Cleanliness = 4;
                    else if (radioButton.Name == "Cleanliness5")
                        Cleanliness = 5;
                }
            }
        }
        private void IntegrityChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                if (radioButton.IsChecked == true)
                {
                    if (radioButton.Name == "Integrity1")
                        Integrity = 1;
                    else if (radioButton.Name == "Integrity2")
                        Integrity = 2;
                    else if (radioButton.Name == "Integrity3")
                        Integrity = 3;
                    else if (radioButton.Name == "Integrity4")
                        Integrity = 4;
                    else if (radioButton.Name == "Integrity5")
                        Integrity = 5;
                }
            }
        }

    }
}
