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
            AccommodationManagementViewModel.CloseAccommodation();
        }
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
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                if(App.currentLanguage() == ENG)
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
            if (string.IsNullOrEmpty(MaxGuestNumberTextBox.NumTextBox.Text) || string.IsNullOrWhiteSpace(MaxGuestNumberTextBox.NumTextBox.Text))
            {
                if (App.currentLanguage() == ENG)
                    MaxNumOfGuestsValidation.Text = "Max number of guests is required!";
                else
                    MaxNumOfGuestsValidation.Text = "Unesite naziv smestaja!";
                MaxNumOfGuestsValidation.Visibility = Visibility.Visible;
                return;
            }
            Regex NumberRegex = new Regex("^[1-9]+[0-9]*$");
            if (!NumberRegex.Match(MaxGuestNumberTextBox.NumTextBox.Text).Success)
            {
                if (App.currentLanguage() == ENG)
                    MaxNumOfGuestsValidation.Text = "The field can only contain numbers!";
                else
                    MaxNumOfGuestsValidation.Text = "Polje moze da sadrzi samo brojeve!";
                MaxNumOfGuestsValidation.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                MaxNumOfGuestsValidation.Visibility = Visibility.Hidden;
            }
        }

        private void MinNumOfResDaysTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(MinResDaysTextBox.NumTextBox.Text) || string.IsNullOrWhiteSpace(MinResDaysTextBox.NumTextBox.Text))
            {
                if (App.currentLanguage() == ENG)
                    MinNumOfResDaysValidation.Text = "Min number of res. days is required!";
                else
                    MinNumOfResDaysValidation.Text = "Unesite minimalan broj rezervisanih dana!";
                MinNumOfResDaysValidation.Visibility = Visibility.Visible;
                return;
            }
            Regex NumberRegex = new Regex("^[1-9]+[0-9]*$");
            if (!NumberRegex.Match(MinResDaysTextBox.NumTextBox.Text).Success)
            {
                if (App.currentLanguage() == ENG)
                    MinNumOfResDaysValidation.Text = "The field can only contain numbers!";
                else
                    MinNumOfResDaysValidation.Text = "Polje moze da sadrzi samo brojeve!";
                MinNumOfResDaysValidation.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                MinNumOfResDaysValidation.Visibility = Visibility.Hidden;
            }
        }

        private void CancelationDaysLimitTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(CancelationDaysLimitTextBox.NumTextBox.Text) || string.IsNullOrWhiteSpace(CancelationDaysLimitTextBox.NumTextBox.Text))
            {
                if (App.currentLanguage() == ENG)
                    CancelationDaysLimitValdation.Text = "Cancelation days limit is required!";
                else
                    CancelationDaysLimitValdation.Text = "Unesite broj dana za otkazivanje!";
                CancelationDaysLimitValdation.Visibility = Visibility.Visible;
                return;
            }
            Regex NumberRegex = new Regex("^[1-9]+[0-9]*$");
            if (!NumberRegex.Match(CancelationDaysLimitTextBox.NumTextBox.Text).Success)
            {
                if (App.currentLanguage() == ENG)
                    CancelationDaysLimitValdation.Text = "The field can only contain numbers!";
                else
                    CancelationDaysLimitValdation.Text = "Polje moze da sadrzi samo brojeve!";
                CancelationDaysLimitValdation.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                CancelationDaysLimitValdation.Visibility = Visibility.Hidden;
            }
        }
    }
}
