using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Owner;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
using static System.Net.Mime.MediaTypeNames;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Page
    {
        public AccommodationManagementViewModel AccommodationManagementViewModel { get; set; }
        public AccommodationRegistration(Accommodation accommodation, User user)
        {
            InitializeComponent();
            AccommodationManagementViewModel = new AccommodationManagementViewModel(this, user);
            this.DataContext = AccommodationManagementViewModel;
            ErrorsVisibility(Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed);
            SuccessLabel.Visibility = Visibility.Collapsed;
        }
        public void ErrorsVisibility(System.Windows.Visibility invalidInputVisibility,
                                     System.Windows.Visibility imageErrorVisibility,
                                     System.Windows.Visibility numberErrorVisibility)
        {
            InvalidInputLabel.Visibility = invalidInputVisibility;
            ImageErrorLabel.Visibility = imageErrorVisibility;
            NumberErrorLabel.Visibility = numberErrorVisibility;
        }
        
        private void PreviewMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            if(SuccessLabel.Visibility == Visibility.Visible)
            {
                SuccessLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
