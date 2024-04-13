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

namespace BookingApp.ViewModel.Owner
{
    public class AccommodationManagementViewModel
    {
        public RelayCommand Accept => new RelayCommand(execute => AcceptExecute(), canExecute => AcceptCanExecute());
        public RelayCommand AddImage => new RelayCommand(execute => AddImageExecute());
        public AccommodationRegistration AccommodationRegistration {  get; set; }
        public User user { get; set; }
        public List<Location> Locations { get; set; }
        public string? SelectedLocation { get; set; }
        public List<Image> Images { get; set; }
        public ObservableCollection<string> RelativeImagePaths { get; set; }
        public AccommodationManagementViewModel(AccommodationRegistration AccommodationRegistration, User user)
        {
            this.user = user;
            this.AccommodationRegistration = AccommodationRegistration;
            RelativeImagePaths = new ObservableCollection<string>();
            Locations = new List<Location>();
            Images = new List<Image>();
            Locations = LocationService.GetInstance().GetAll();
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
            Accommodation Accommodation = new Accommodation();
            Accommodation.Name = AccommodationRegistration.NameTextBox.Text;
            Accommodation.AccommodationType = ReturnAccommodationType();
            Accommodation.MaxGuestNumber = Convert.ToInt32(AccommodationRegistration.MaxGuestNumberTextBox.Text);
            Accommodation.MinReservationDays = Convert.ToInt32(AccommodationRegistration.MinResDaysTextBox.Text);
            Accommodation.CancelationDaysLimit = Convert.ToInt32(AccommodationRegistration.CancelationDaysLimitTextBox.Text);
            Accommodation.Images = Images;

            string[] temporaryStrings = SelectedLocation.Split(':');
            Location? Location = LocationService.GetInstance().GetById(Convert.ToInt32(temporaryStrings[0]));
            Accommodation.Location.Id = Location.Id;
            Accommodation.Location.State = Location.State;
            Accommodation.Location.City = Location.City;
            Accommodation.OwnerId = user.Id;
            AccommodationService.GetInstance().Add(Accommodation);

            ResetInputs();
            AccommodationRegistration.ErrorsVisibility(Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed);
            AccommodationRegistration.SuccessLabel.Visibility = Visibility.Visible;
        }
        public bool AcceptCanExecute()
        {
            //all the fields must be entered
            if (string.IsNullOrEmpty(AccommodationRegistration.NameTextBox.Text) || AccommodationRegistration.LocationComboBox.SelectedItem == null ||
                AccommodationRegistration.AccommodationTypeComboBox.SelectedItem == null || !IsNumeric(AccommodationRegistration.MaxGuestNumberTextBox.Text) ||
                !IsNumeric(AccommodationRegistration.MinResDaysTextBox.Text) || !IsNumeric(AccommodationRegistration.CancelationDaysLimitTextBox.Text) ||
                AccommodationRegistration.MinResDaysTextBox.Text.Equals("0") || AccommodationRegistration.MaxGuestNumberTextBox.Text.Equals("0") ||
                AccommodationRegistration.CancelationDaysLimitTextBox.Text.Equals("0") || RelativeImagePaths.Count == 0)
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
                }
                if (RelativeImagePaths.Count > 0)
                    SaveImageIntoCSV(RelativeImagePaths);
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
            AccommodationRegistration.MaxGuestNumberTextBox.Text = "0";
            AccommodationRegistration.MinResDaysTextBox.Text = "0";
            AccommodationRegistration.CancelationDaysLimitTextBox.Text = "0";
            AccommodationRegistration.LocationComboBox.SelectedIndex = 0;
            AccommodationRegistration.AccommodationTypeComboBox.SelectedIndex = 0;
            RelativeImagePaths.Clear();
            Images.Clear();
            AccommodationRegistration.ImagesListBox.ItemsSource = null;
            AccommodationRegistration.ImagesListBox.Items.Refresh();
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
    }
}
