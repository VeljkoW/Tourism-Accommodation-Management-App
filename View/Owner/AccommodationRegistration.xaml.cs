using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
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
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Page
    {
        public User user { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public LocationRepository LocationRepository {  get; set; }
        public List<Location> Locations { get; set; }
        public string? SelectedLocation {  get; set; }
        List<Image> Images {  get; set; }
        public ImageRepository imageRepository {  get; set; }
        List<string> relativeImagePaths {  get; set; }
        public AccommodationRegistration(Accommodation accommodation, User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;
            AccommodationRepository = new AccommodationRepository();
            LocationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            relativeImagePaths = new List<string>();
            Locations = new List<Location>();
            Images = new List<Image>();
            Locations = LocationRepository.GetAll();
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

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameTextBox.Text) || LocationComboBox.SelectedItem == null ||
                AccommodationTypeComboBox.SelectedItem == null || !IsNumeric(MaxGuestNumberTextBox.Text) ||
                !IsNumeric(MinResDaysTextBox.Text) || !IsNumeric(CancelationDaysLimitTextBox.Text))
            {
                ErrorsVisibility(Visibility.Visible, Visibility.Collapsed, Visibility.Collapsed);
                return;
            }

            if (MinResDaysTextBox.Text.Equals("0") ||
                MaxGuestNumberTextBox.Text.Equals("0") ||
                CancelationDaysLimitTextBox.Text.Equals("0"))
            {
                ErrorsVisibility(Visibility.Collapsed, Visibility.Collapsed, Visibility.Visible);
                return;
            }

            if(relativeImagePaths.Count == 0)
            {
                ErrorsVisibility(Visibility.Collapsed, Visibility.Visible, Visibility.Collapsed);
                return;
            }

            Accommodation Accommodation = new Accommodation();
            Accommodation.Name = NameTextBox.Text;
            if (AccommodationTypeComboBox.SelectionBoxItem.Equals("Apartment"))
                Accommodation.AccommodationType = AccommodationType.Apartment;
            else if (AccommodationTypeComboBox.SelectionBoxItem.Equals("House"))
                Accommodation.AccommodationType = AccommodationType.House;
            else if (AccommodationTypeComboBox.SelectionBoxItem.Equals("Hut"))
                Accommodation.AccommodationType = AccommodationType.Hut;

            Accommodation.MaxGuestNumber = Convert.ToInt32(MaxGuestNumberTextBox.Text);
            Accommodation.MinReservationDays = Convert.ToInt32(MinResDaysTextBox.Text);
            Accommodation.CancelationDaysLimit = Convert.ToInt32(CancelationDaysLimitTextBox.Text);
            Accommodation.Images = Images;

            string[] temporaryStrings = SelectedLocation.Split(':');
            Location Location = LocationRepository.GetById(Convert.ToInt32(temporaryStrings[0]));
            Accommodation.Location.Id = Location.Id;
            Accommodation.Location.State = Location.State;
            Accommodation.Location.City = Location.City;
            Accommodation.OwnerId = user.Id;
            AccommodationRepository.Add(Accommodation);

            ResetInputs();

            ErrorsVisibility(Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed);
            SuccessLabel.Visibility = Visibility.Visible;
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
            NameTextBox.Text = string.Empty;
            MaxGuestNumberTextBox.Text = "0";
            MinResDaysTextBox.Text = "0";
            CancelationDaysLimitTextBox.Text = "0";
            LocationComboBox.SelectedIndex = 0;
            AccommodationTypeComboBox.SelectedIndex = 0;
            relativeImagePaths.Clear();
            Images.Clear();
            ImagesListBox.ItemsSource = null;
            ImagesListBox.Items.Refresh();
        }
        private void AddImagesClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                // Get path to .exe file
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                // Position to the right folder
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Accommodation";

                // Ensure the target folder exists
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
                foreach (string filePath in dlg.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    SaveImageFile(filePath, destFilePath);

                    // Forming relative path to the new Image
                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Accommodation/", fileName);
                    relativeImagePaths.Add(relativePath);
                }
                if (relativeImagePaths.Count > 0)
                {
                    SaveImageIntoCSV(relativeImagePaths);
                }
                ImagesListBox.ItemsSource = relativeImagePaths;
                ImagesListBox.Items.Refresh();
            }
        }
        private void SaveImageIntoCSV(List<string> relativeImagePaths)
        {
            foreach (string filePath in relativeImagePaths)
            {
                Image? image = new Image();
                image.Path = filePath;
                image = imageRepository.Add(image);
                Images.Add(image);
            }
        }
        private void SaveImageFile(string filePath, string destFilePath)
        {
            if (!File.Exists(destFilePath))
            {
                File.Copy(filePath, destFilePath, false);
            }
            else
            {
                while (File.Exists(destFilePath))
                {
                    string[] name = destFilePath.Split(".");
                    name[0] = name[0] + "A";
                    destFilePath = name[0] + "." + name[1];
                }
                File.Copy(filePath, destFilePath, false);
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
        private void PreviewMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            if(SuccessLabel.Visibility == Visibility.Visible)
            {
                SuccessLabel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
