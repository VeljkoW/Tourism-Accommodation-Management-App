using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.ViewModel.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Color backgroundButtonPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush backgroundButtonPressedBrush = new SolidColorBrush(backgroundButtonPressedColor);
            Color basicBackgroundColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush basicBackgroundBrush = new SolidColorBrush(basicBackgroundColor);
            HistoryButton.Background = basicBackgroundBrush;
            SchedulingButton.Background = backgroundButtonPressedBrush;

            RenovationDetailsValidation.Visibility = Visibility.Hidden;
            AvailableDatesValidation.Visibility = Visibility.Hidden;
            CancelRenovationAccept.Visibility = Visibility.Hidden;
            App.LanguageChanged += OnLanguageChanged;
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
            Accommodation? accommodation = AccommodationService.GetInstance().GetById(selectedCard.AccommodationId);
            SelectedAccommodationNameRun.Text = accommodation.Name;
            SelectedAccommodationStateRun.Text = accommodation.Location.State;
            SelectedAccommodationCityRun.Text = accommodation.Location.City;
            CancelRenovationAccept.Visibility = Visibility.Visible;
        }
        private void CancelRenovationAcceptedClick(object sender, RoutedEventArgs e)
        {
            RenovationViewModel.DeleteRowExecute(RenovationViewModel.SelectedScheduledRenovation);
            CancelRenovationAccept.Visibility = Visibility.Hidden;
        }

        private void CancelRenovationCancelClick(object sender, RoutedEventArgs e)
        {

            CancelRenovationAccept.Visibility = Visibility.Hidden;
        }





        //VALIDATION
        public void ValidationErrors()
        {
            if (App.currentLanguage() == ENG)
            {
                AccommodationValidation.Text = "Accommodation is required!";
                StartDateValidation.Text = "Start renovation date is required!";
                EndDateValidation.Text = "End renovation date is required!";
                DurationValidation.Text = "Duration of renovation is required!";
            }
            else
            {
                AccommodationValidation.Text = "Izaberite smeštaj!";
                StartDateValidation.Text = "Unesite početni datum renoviranja!";
                EndDateValidation.Text = "Unesite završni datum renoviranja!";
                DurationValidation.Text = "Unesite trajanje renoviranja!";
            }
        }
        public void Validation2()
        {
            RenovationDetailsValidation.Visibility= Visibility.Visible;
            AvailableDatesValidation.Visibility = Visibility.Visible;
            if (App.currentLanguage() == ENG)
            {
                RenovationDetailsValidation.Text = "Renovation details are required!";
                AvailableDatesValidation.Text = "Select renovation date!";
            }
            else
            {
                RenovationDetailsValidation.Text = "Unesite detalje renoviranja!";
                AvailableDatesValidation.Text = "Izaberite datum za renoviranje!";
            }
        }
        private void AccommodationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationValidation.Visibility = Visibility.Hidden;
        }

        private void StartDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(StartDatePicker.SelectedDate != null)
            {
                StartDateValidation.Visibility = Visibility.Hidden;
            }
            else
            {
                if (App.currentLanguage() == ENG)
                    StartDateValidation.Text = "Start renovation date is required!";
                else
                    StartDateValidation.Text = "Unesite početni datum renoviranja!";
                StartDateValidation.Visibility = Visibility.Visible;
            }
        }

        private void EndDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EndDatePicker.SelectedDate != null)
            {
                EndDateValidation.Visibility = Visibility.Hidden;
            }
            else
            {
                if (App.currentLanguage() == ENG)
                    EndDateValidation.Text = "End renovation date is required!";
                else
                    EndDateValidation.Text = "Unesite završni datum renoviranja!";
                EndDateValidation.Visibility = Visibility.Visible;
            }
        }

        private void DurationTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateDuration();
        }
        public void ValidateDuration()
        {
            if (string.IsNullOrEmpty(DurationTextBox.NumTextBox.Text) || string.IsNullOrWhiteSpace(DurationTextBox.NumTextBox.Text))
            {
                if (App.currentLanguage() == ENG)
                    DurationValidation.Text = "Duration of renovation is required!";
                else
                    DurationValidation.Text = "Unesite trajanje renoviranja!";
                DurationValidation.Visibility = Visibility.Visible;
                return;
            }
            Regex NumberRegex = new Regex("^[1-9][0-9]*$");
            if (!NumberRegex.Match(DurationTextBox.NumTextBox.Text).Success)
            {
                if (App.currentLanguage() == ENG)
                    DurationValidation.Text = "The field can only contain letters!";
                else
                    DurationValidation.Text = "Polje moze da sadrzi samo slova!";
                DurationValidation.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                DurationValidation.Visibility = Visibility.Hidden;
            }
        }
        private void RenovationDetailsChanged(object sender, TextChangedEventArgs e)
        {
            ValidateRenovationDetails();
        }
        public void ValidateRenovationDetails()
        {
            if (string.IsNullOrEmpty(CommentTextBox.Text) || string.IsNullOrWhiteSpace(CommentTextBox.Text))
            {
                if (App.currentLanguage() == ENG)
                    RenovationDetailsValidation.Text = "Duration of renovation is required!";
                else
                    RenovationDetailsValidation.Text = "Unesite trajanje renoviranja!";
                RenovationDetailsValidation.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                RenovationDetailsValidation.Visibility = Visibility.Hidden;
            }
        }
        private void AvailableDatesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AvailableDatesListBox.SelectedItem != null)
            {
                AvailableDatesValidation.Visibility = Visibility.Hidden;
            }
            else
            {
                if (App.currentLanguage() == ENG)
                    AvailableDatesValidation.Text = "Select renovation date!";
                else
                    AvailableDatesValidation.Text = "Izaberite datum za renoviranje!";
                AvailableDatesValidation.Visibility = Visibility.Visible;
            }
        }
        private void OnLanguageChanged()
        {
            ValidateDuration();
            ValidateRenovationDetails();
            if (App.currentLanguage() == ENG)
            {
                AccommodationValidation.Text = "Accommodation is required!";
                AvailableDatesValidation.Text = "Select renovation date!";
                EndDateValidation.Text = "End renovation date is required!";
                StartDateValidation.Text = "Start renovation date is required!";
                RenovationDetailsValidation.Text = "Renovation details are required!";
                AvailableDatesValidation.Text = "Select renovation date!";
            }
            else
            {
                AccommodationValidation.Text = "Izaberite smeštaj!";
                AvailableDatesValidation.Text = "Izaberite datum za renoviranje!";
                EndDateValidation.Text = "Unesite završni datum renoviranja!";
                StartDateValidation.Text = "Unesite početni datum renoviranja!";
                RenovationDetailsValidation.Text = "Unesite detalje renoviranja!";
                AvailableDatesValidation.Text = "Izaberite datum za renoviranje!";
            }
        }
        ~Renovation()
        {
            App.LanguageChanged -= OnLanguageChanged;
        }

    }
}
