using BookingApp.Domain.Model;
using BookingApp.View.Guest.Pages;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for Renovation.xaml
    /// </summary>
    public partial class Renovation : Page
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public OwnerMainWindow OwnerMainWindow {  get; set; }
        public User User { get; set; }
        public RenovationViewModel RenovationViewModel { get; set; }
        public Renovation(OwnerMainWindow OwnerMainWindow)
        {
            this.OwnerMainWindow = OwnerMainWindow;
            User = OwnerMainWindow.user;
            InitializeComponent();
            RenovationViewModel = new RenovationViewModel(this);
            DataContext = RenovationViewModel;
            ValidationErrors();
        }

        private void HistoryClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.mainFrame.Navigate(OwnerMainWindow.RenovationHistory);
        }

        private void DeleteRow(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as ScheduledRenovation;
            RenovationViewModel.SelectedScheduledRenovation = selectedCard;
            RenovationViewModel.DeleteRowExecute(selectedCard);
        }
        public void ValidationErrors()
        {
            if (App.currentLanguage() == ENG)
            {
                AccommodationValidation.Text = "Accommodation is required!";
                StartDateValidation.Text = "Start renovation date is required";
                EndDateValidation.Text = "End renovation date is required";
                DurationValidation.Text = "Duration of renovation is required!";
            }
            else
            {
                AccommodationValidation.Text = "Izaberite smeštaj!";
                StartDateValidation.Text = "Unesite početni datum renoviranja";
                EndDateValidation.Text = "Unesite završni datum renoviranja";
                DurationValidation.Text = "Unesite trajanje renoviranja!";
            }
        }
        private void AccommodationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationValidation.Visibility = Visibility.Hidden;
        }

        private void StartDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
