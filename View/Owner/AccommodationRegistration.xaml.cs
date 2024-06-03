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
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Page
    {
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        private bool calledFromStatistics = false;
        private double lastVerticalOffset = 0;
        public AccommodationManagementViewModel AccommodationManagementViewModel { get; set; }
        public AccommodationRegistration(User user)
        {
            InitializeComponent();
            AccommodationManagementViewModel = new AccommodationManagementViewModel(this, user);
            this.DataContext = AccommodationManagementViewModel;
            App.LanguageChanged += OnLanguageChanged;
            App.ThemeChanged += OnThemeChanged;
            OnThemeChanged();
            ValidationErrors();
        }
        public AccommodationRegistration(User user, bool calledFromStatistics)
        {
            InitializeComponent();
            AccommodationManagementViewModel = new AccommodationManagementViewModel(this, user);
            this.DataContext = AccommodationManagementViewModel;
            this.calledFromStatistics = calledFromStatistics;
        }
        private void StatePickedSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationManagementViewModel.StatePicked();
            StateValidation.Visibility = Visibility.Hidden;
            if (App.currentLanguage() == ENG)
                CityValidation.Text = "City is required!";
            else
                CityValidation.Text = "Unesite grad!";

        }
        private void CityPickedSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CityValidation.Visibility = Visibility.Hidden;
        }
        private void StateChosenSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calledFromStatistics)
            {
                calledFromStatistics = false;
                return;
            }
            AccommodationManagementViewModel.StateChosen();
        }
        private void CityChosenSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationManagementViewModel.CityChosen();
        }
        private void CloseAccommodationClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as Accommodation;
            AccommodationManagementViewModel.SelectedAccommodation = selectedCard;
            CloseAccommodationAccept.Visibility = Visibility.Visible;
            SelectedAccommodationNameRun.Text = selectedCard.Name + ",";
            SelectedAccommodationStateRun.Text = selectedCard.Location.State;
            SelectedAccommodationCityRun.Text = selectedCard.Location.City;
        }
        private void CloseAccommodationAcceptedClick(object sender, RoutedEventArgs e)
        {
            CloseAccommodationAccept.Visibility = Visibility.Collapsed;
            AccommodationManagementViewModel.CloseAccommodation();
        }

        private void CloseAccommodationCancelClick(object sender, RoutedEventArgs e)
        {
            CloseAccommodationAccept.Visibility = Visibility.Collapsed;
        }

        public void OnThemeChanged()
        {
            Color OwnerTabLightColor = (Color)FindResource("OwnerTabLightColor");
            SolidColorBrush OwnerTabLightColorBrush = new SolidColorBrush(OwnerTabLightColor);
            Color OwnerTabDarkColor = (Color)FindResource("OwnerTabDarkColor");
            SolidColorBrush OwnerTabDarkColorBrush = new SolidColorBrush(OwnerTabDarkColor);
            Color OwnerTabPressedColor = (Color)FindResource("OwnerTabPressedColor");
            SolidColorBrush OwnerTabPressedColorBrush = new SolidColorBrush(OwnerTabPressedColor); 

            if (App.currentTheme() == "Light")
            {
                //AccommodationRegistrationBorder.Background = OwnerTabLightColorBrush;
                //ChooseLocationBorder.Background = OwnerTabLightColorBrush;
                //CloseAccommodationAcceptBorder.Background = OwnerTabLightColorBrush;
                var newColor = (Color)Application.Current.Resources["BorderLightBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
            }
            else
            {
                Color OwnerDarkBackgroundColor = (Color)FindResource("OwnerDarkBackgroundColor");
                SolidColorBrush OwnerDarkBackgroundColorBrush = new SolidColorBrush(OwnerDarkBackgroundColor);
                //AccommodationRegistrationBorder.Background = OwnerTabDarkColorBrush;
                //ChooseLocationBorder.Background = OwnerTabDarkColorBrush;
                //CloseAccommodationAcceptBorder.Background = OwnerDarkBackgroundColorBrush;
                var newColor = (Color)Application.Current.Resources["BorderDarkBackgroundColor"];
                Application.Current.Resources["BorderBackgroundBrush"] = new SolidColorBrush(newColor);
            }
        }
        //VALIDATION
        public void ValidationErrors()
        {
            if (App.currentLanguage() == ENG)
            {
                AccommodationNameValidation.Text = "Accommodation Name is required!";
                AccommodationTypeValidation.Text = "Accommodation Type is required!";
                StateValidation.Text = "State is required!";
                CityValidation.Text = "First select a state!";
                MaxNumOfGuestsValidation.Text = "Max number of guests is required!";
                MinNumOfResDaysValidation.Text = "Min number of res. days is required!";
                CancelationDaysLimitValdation.Text = "Cancelation days limit is reqired!";
                ImageValidation.Text = "At least one image has to be added!";
            }
            else
            {
                AccommodationNameValidation.Text = "Unesite naziv smeštaja!";
                AccommodationTypeValidation.Text = "Unesite tip smeštaja!";
                StateValidation.Text = "Unesite državu!";
                CityValidation.Text = "Prvo unesite državu!";
                MaxNumOfGuestsValidation.Text = "Unesite maksimalan broj gostiju!";
                MinNumOfResDaysValidation.Text = "Unesite minimalan broj rezervisanih dana!";
                CancelationDaysLimitValdation.Text = "Unesite broj dana za otkazivanje!";
                ImageValidation.Text = "Barem jedna slika mora da se doda!";
            }
        }
        private void AccommodationNameTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateAccommodationName();
        }
        public void ValidateAccommodationName()
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                if (App.currentLanguage() == ENG)
                    AccommodationNameValidation.Text = "Accommodation Name is required!";
                else
                    AccommodationNameValidation.Text = "Unesite naziv smestaja!";
                AccommodationNameValidation.Visibility = Visibility.Visible;
                return;
            }
            Regex TextNumberRegex = new Regex("^[A-Za-zČĆŠĐŽčćšđž ]+[0-9]*$");
            if (!TextNumberRegex.Match(NameTextBox.Text).Success)
            {
                if (App.currentLanguage() == ENG)
                    AccommodationNameValidation.Text = "The field can only contain letters!";
                else
                    AccommodationNameValidation.Text = "Polje moze da sadrzi samo slova!";
                AccommodationNameValidation.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                AccommodationNameValidation.Visibility = Visibility.Hidden;
            }

        }
        private void AccommodationTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationTypeValidation.Visibility = Visibility.Hidden;
        }

        private void MaxNumOfGuestsTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateMaxNumOfGuests();
        }
        public void ValidateMaxNumOfGuests()
        {
            string requiredMessage = App.currentLanguage() == ENG
                ? "Max number of guests is required!"
                : "Unesite naziv smestaja!";

            string invalidNumberMessage = App.currentLanguage() == ENG
                ? "The field can only contain numbers!"
                : "Polje moze da sadrzi samo brojeve!";

            ValidateNumberField(MaxGuestNumberTextBox.NumTextBox.Text, MaxNumOfGuestsValidation, requiredMessage, invalidNumberMessage);

        }
        private void MinNumOfResDaysTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateMinNumOfResDays();
        }
        public void ValidateMinNumOfResDays()
        {
            string requiredMessage = App.currentLanguage() == ENG
                ? "Min number of res. days is required!"
                : "Unesite minimalan broj rezervisanih dana!";

            string invalidNumberMessage = App.currentLanguage() == ENG
                ? "The field can only contain numbers!"
                : "Polje moze da sadrzi samo brojeve!";

            ValidateNumberField(MinResDaysTextBox.NumTextBox.Text, MinNumOfResDaysValidation, requiredMessage, invalidNumberMessage);

        }
        private void CancelationDaysLimitTextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateCancelationDaysLimit();
        }
        public void ValidateCancelationDaysLimit()
        {
            string requiredMessage = App.currentLanguage() == ENG 
                ? "Cancelation days limit is required!"
                : "Unesite broj dana za otkazivanje!";

            string invalidNumberMessage = App.currentLanguage() == ENG 
                ? "The field can only contain numbers!"
                : "Polje moze da sadrzi samo brojeve!";

            ValidateNumberField(CancelationDaysLimitTextBox.NumTextBox.Text, CancelationDaysLimitValdation, requiredMessage, invalidNumberMessage);

        }
        private void ValidateNumberField(string text, TextBlock validationBlock, string requiredMessage, string invalidNumberMessage)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                validationBlock.Text = requiredMessage;
                validationBlock.Visibility = Visibility.Visible;
                return;
            }

            Regex NumberRegex = new Regex("^[1-9]+[0-9]*$");
            if (!NumberRegex.Match(text).Success)
            {
                validationBlock.Text = invalidNumberMessage;
                validationBlock.Visibility = Visibility.Visible;
                return;
            }

            validationBlock.Visibility = Visibility.Hidden;
        }
        private void OnLanguageChanged()
        {
            ValidateAccommodationName();
            ValidateMaxNumOfGuests();
            ValidateMinNumOfResDays();
            ValidateCancelationDaysLimit();
            if (App.currentLanguage() == ENG)
            {
                AccommodationTypeValidation.Text = "Accommodation Type is required!";
                StateValidation.Text = "State is required!";
                CityValidation.Text = "City is required!";
                ImageValidation.Text = "At least one image has to be added!";
            }
            else
            {
                AccommodationTypeValidation.Text = "Unesite tip smeštaja!";
                StateValidation.Text = "Unesite državu!";
                CityValidation.Text = "Unesite grad!";
                ImageValidation.Text = "Barem jedna slika mora da se doda!";
            }
        }
        ~AccommodationRegistration()
        {
            App.LanguageChanged -= OnLanguageChanged;
        }
    }
}
