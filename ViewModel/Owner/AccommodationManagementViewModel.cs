using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using BookingApp.View.Owner;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using Notification.Wpf;
using Notification.Wpf.Classes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Notification.Wpf.Constants;

namespace BookingApp.ViewModel.Owner
{
    public class AccommodationManagementViewModel : INotifyPropertyChanged
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        private int currentImageIndex = 0;
        public ObservableCollection<string> ImagePaths { get; set; }
        public string CurrentImagePath => ImagePaths.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths.Count;
        public RelayCommand Accept => new RelayCommand(execute => AcceptExecute(), canExecute => AcceptCanExecute());
        public RelayCommand AddImage => new RelayCommand(execute => AddImageExecute()); 
        public RelayCommand DeleteImageCommand => new RelayCommand(execute => DeleteImage(), canExecute => CanDeleteImage());
        public RelayCommand PreviousImageCommand => new RelayCommand(execute => PreviousImage(), canExecute => CanPreviousImage());
        public RelayCommand NextImageCommand => new RelayCommand(execute => NextImage(), canExecute => CanNextImage());
        public AccommodationRegistration AccommodationRegistration {  get; set; }
        public User user { get; set; }
        public Accommodation Accommodation {  get; set; }
        public List<string> States { get; set; }
        private string selectedState {  get; set; }
        public ObservableCollection<Location> Cities { get; set; }
        private Location selectedLocation { get; set; }
        public List<Image> Images { get; set; }
        public ObservableCollection<string> RelativeImagePaths { get; set; }
        public ObservableCollection<Accommodation> AccommodationsDisplay {  get; set; }
        public Accommodation SelectedAccommodation { get; set; }

        public List<string> StatesForChoosing { get; set; }
        public ObservableCollection<Location> CitiesForChoosing { get; set; }
        private string selectedChosenState { get; set; }
        private Location selectedChosenCity { get; set; }

        public string SelectedState
        {
            get { return selectedState; }
            set
            {
                if (selectedState != value)
                {
                    selectedState = value;
                    OnPropertyChanged(nameof(selectedState));
                }
            }
        }
        public Location SelectedLocation
        {
            get { return selectedLocation; }
            set
            {
                if (selectedLocation != value)
                {
                    selectedLocation = value;
                    OnPropertyChanged(nameof(selectedLocation));
                }
            }
        }
        public string SelectedChosenState
        {
            get { return selectedChosenState; }
            set
            {
                if (selectedChosenState != value)
                {
                    selectedChosenState = value;
                    OnPropertyChanged(nameof(selectedChosenState));
                }
            }
        }
        public Location SelectedChosenCity
        {
            get { return selectedChosenCity; }
            set
            {
                if (selectedChosenCity != value)
                {
                    selectedChosenCity = value;
                    OnPropertyChanged(nameof(selectedChosenCity));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public AccommodationManagementViewModel(AccommodationRegistration AccommodationRegistration, User user)
        {
            this.user = user;
            this.AccommodationRegistration = AccommodationRegistration;
            RelativeImagePaths = new ObservableCollection<string>();
            ImagePaths = new ObservableCollection<string>();
            Cities = new ObservableCollection<Location>();
            CitiesForChoosing = new ObservableCollection<Location>();
            AccommodationsDisplay = new ObservableCollection<Accommodation>();
            Images = new List<Image>();
            States = LocationService.GetInstance().GetStates();
            StatesForChoosing = LocationService.GetInstance().GetStates();
            AccommodationsDisplay = AccommodationService.GetInstance().GetAllByUser(user);
            SelectedChosenCity = new Location();
            Accommodation = new Accommodation();
            InitializeChooseStateComboBox();
        }
        private void InitializeChooseStateComboBox()
        {
            AccommodationRegistration.ChooseStateComboBox.Items.Clear();
            AccommodationRegistration.ChooseStateComboBox.Items.Add("");
            foreach (string state in LocationService.GetInstance().GetStates())
                AccommodationRegistration.ChooseStateComboBox.Items.Add(state);
        }
        
        public void StatePicked()
        {
            if (!string.IsNullOrEmpty(SelectedState))
                LocationService.GetInstance().GetCitiesForState(Cities, SelectedState);
        }
        public void StateChosen()
        {
            if (SelectedChosenState.Equals(""))
            {
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAllByUser(user))
                    AccommodationsDisplay.Add(accommodation);
                return;
            }
            UpdateChooseCityComboBox();
            UpdateAccommodationsDisplay("state");
            if (AccommodationsDisplay.Count == 0)
                notificationManager.Show("Info", "You have no accommodations in this state!", NotificationType.Information);
        }
        public void CityChosen()
        {
            if (SelectedChosenCity == null || AccommodationRegistration.ChooseCityComboBox.SelectedItem == null) return;
            if (AccommodationRegistration.ChooseCityComboBox.SelectedItem.ToString().Equals(""))
            {
                UpdateAccommodationsDisplay("state");
                return;
            }
            UpdateAccommodationsDisplay("city");
            if (AccommodationsDisplay.Count == 0)
                notificationManager.Show("Info", "You have no accommodations in this city!", NotificationType.Information);
        }
        private void UpdateChooseCityComboBox()
        {
            if (SelectedChosenState == null) return;
            AccommodationRegistration.ChooseCityComboBox.Items.Clear();
            AccommodationRegistration.ChooseCityComboBox.Items.Add("");
            CitiesForChoosing.Clear();
            LocationService.GetInstance().GetCitiesForState(CitiesForChoosing, SelectedChosenState);
            foreach (Location location in CitiesForChoosing)
                AccommodationRegistration.ChooseCityComboBox.Items.Add(location);
        }
        private void UpdateAccommodationsDisplay(string StateOrCity)
        {
            AccommodationsDisplay.Clear();
            if (StateOrCity.Equals("state"))
            {
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAllByUser(user))
                    if (accommodation.Location.State == SelectedChosenState)
                        AccommodationsDisplay.Add(accommodation);
            }else if (StateOrCity.Equals("city"))
            {
                foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAllByUser(user))
                    if (accommodation.Location.City == SelectedChosenCity.City)
                        AccommodationsDisplay.Add(accommodation);
            }
        }
        public int CurrentImageIndex
        {
            get { return currentImageIndex; }
            set
            {
                if (value >= 0 && value < ImagePaths.Count)
                {
                    currentImageIndex = value;
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }
        public void NextImage()
        {
            if (CurrentImageIndex < TotalImages - 1)
            {
                CurrentImageIndex++;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanNextImage()
        {
            if (CurrentImageIndex == TotalImages - 1 || TotalImages == 0)
                return false;
            return true;
        }
        public void PreviousImage()
        {
            if (CurrentImageIndex > 0)
            {
                CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
        }
        public bool CanPreviousImage()
        {
            if (CurrentImageIndex == 0 || TotalImages == 0)
                return false;
            return true;
        }
        public void DeleteImage()
        {
            try
            {
                ImagePaths.RemoveAt(CurrentImageIndex);
                if (CurrentImageIndex == TotalImages && TotalImages >= 1)
                    CurrentImageIndex--;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                CurrentImageIndex = 0;
                OnPropertyChanged(nameof(CurrentImageIndex));
                OnPropertyChanged(nameof(CurrentImagePath));
                Console.WriteLine("Error: Indeks not valid - " + ex.Message);
            }
        }
        public bool CanDeleteImage()
        {
            if(TotalImages == 0)
                return false;
            return true;
        }
        public AccommodationType ReturnAccommodationType()
        {
            if (AccommodationRegistration.AccommodationTypeComboBox.SelectionBoxItem.Equals("Apartment"))
                return AccommodationType.Apartment;
            else if (AccommodationRegistration.AccommodationTypeComboBox.SelectionBoxItem.Equals("House"))
                return AccommodationType.House;
            else
                return AccommodationType.Hut;
        }
        public void AcceptExecute()
        {
            //Accommodation Accommodation = new Accommodation();
            //Accommodation.Name = AccommodationRegistration.NameTextBox.Text;
            Accommodation.AccommodationType = ReturnAccommodationType();
            Accommodation.MaxGuestNumber = Convert.ToInt32(AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text);
            Accommodation.MinReservationDays = Convert.ToInt32(AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text);
            Accommodation.CancelationDaysLimit = Convert.ToInt32(AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text);
            Accommodation.Images = Images;
            Accommodation.Location = SelectedLocation;
            Accommodation.OwnerId = user.Id;
            AccommodationService.GetInstance().Add(Accommodation);

            ResetInputs();
            notificationManager.Show("Success", "Accommodation Successfully registered!", NotificationType.Success);
        }
        public bool AcceptCanExecute()
        {
            //all the fields must be entered
            if (string.IsNullOrEmpty(AccommodationRegistration.NameTextBox.Text) || AccommodationRegistration.StateComboBox.SelectedItem == null ||
                AccommodationRegistration.CityComboBox.SelectedItem == null ||
                AccommodationRegistration.AccommodationTypeComboBox.SelectedItem == null || !IsNumeric(AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text) ||
                !IsNumeric(AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text) || !IsNumeric(AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text) ||
                AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text.Equals("0") || AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text.Equals("0") ||
                AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text.Equals("0") || RelativeImagePaths.Count == 0)
            {
                return false;
            }
            return true;
        }

        public void AddImageExecute()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Accommodation";

                if (!Directory.Exists(targetFolderPath))
                    Directory.CreateDirectory(targetFolderPath);
                foreach (string filePath in dlg.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    fileName = SaveImageFile(filePath, destFilePath, fileName);

                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Accommodation/", fileName);
                    RelativeImagePaths.Add(relativePath);
                    ImagePaths.Add(destFilePath);
                }
                if (RelativeImagePaths.Count > 0)
                    SaveImageIntoCSV(RelativeImagePaths);
                if (ImagePaths.Count > 0)
                {
                    CurrentImageIndex = 0;
                    OnPropertyChanged(nameof(CurrentImagePath));
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
        }

        public bool IsNumeric(string text)
        {
            try
            {
                int number = int.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void ResetInputs()
        {
            AccommodationRegistration.NameTextBox.Text = string.Empty;
            AccommodationRegistration.MaxGuestNumberTextBox.NumTextBox.Text = string.Empty;
            AccommodationRegistration.MinResDaysTextBox.NumTextBox.Text = string.Empty;
            AccommodationRegistration.CancelationDaysLimitTextBox.NumTextBox.Text = string.Empty;
            AccommodationRegistration.StateComboBox.SelectedIndex = -1;
            AccommodationRegistration.CityComboBox.SelectedIndex = -1;
            AccommodationRegistration.AccommodationTypeComboBox.SelectedIndex = -1;
            Cities.Clear();
            RelativeImagePaths.Clear();
            Images.Clear();
            ImagePaths.Clear();
            currentImageIndex = 0;

            AccommodationsDisplay.Clear();
            foreach(Accommodation accommodation in AccommodationService.GetInstance().GetAllByUser(user))
                AccommodationsDisplay.Add(accommodation);

            OnPropertyChanged(nameof(CurrentImagePath));
            OnPropertyChanged(nameof(CurrentImageIndex));
        }
        private void SaveImageIntoCSV(ObservableCollection<string> relativeImagePaths)
        {
            foreach (string filePath in relativeImagePaths)
            {
                Image? image = new Image();
                image.Path = filePath;
                image = ImageService.GetInstance().Add(image);
                Images.Add(image);
            }
        }
        private string SaveImageFile(string filePath, string destFilePath, string fileName)
        {
            if (!File.Exists(destFilePath))
            {
                File.Copy(filePath, destFilePath, false);
                return fileName;
            }
            else
            {
                string[] fileNameParts = fileName.Split(".");
                while (File.Exists(destFilePath))
                {
                    string[] name = destFilePath.Split(".");
                    name[0] = name[0] + "A";
                    destFilePath = name[0] + "." + name[1];
                    fileNameParts[0] = fileNameParts[0] + "A";
                }
                File.Copy(filePath, destFilePath, false);
                return fileNameParts[0] + "." + fileNameParts[1];
            }
        }
        private string GetBaseFolder(string path)
        {
            for (int i = 0; i < 4; i++)
            {
                DirectoryInfo? parentDirectory = Directory.GetParent(path);
                if (parentDirectory != null)
                {
                    path = parentDirectory.FullName;
                }
                else
                {
                    throw new InvalidOperationException("Cannot navigate up three directories from base.");
                }
            }
            return path;
        }
        public void CloseAccommodation()
        {
            AccommodationsDisplay.Remove(SelectedAccommodation);
            AccommodationService.GetInstance().DeleteById(SelectedAccommodation.Id);
        }
    }
}
