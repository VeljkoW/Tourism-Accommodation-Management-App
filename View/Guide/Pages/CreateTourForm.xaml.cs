using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for CreateTourForm.xaml
    /// </summary>
    public partial class CreateTourForm : Page
    {
        public ImageRepository imageRepository = new ImageRepository();
        public KeyPointRepository keyPointRepository = new KeyPointRepository();
        public TourRepository tourRepository = new TourRepository();
        public TourImageRepository tourImageRepository = new TourImageRepository();
        public TourScheduleRepository tourScheduleRepository = new TourScheduleRepository();
        public LocationRepository locationRepository = new LocationRepository();

        public List<string> relativeImagePaths = new List<string>();
        private List<Image> images = new List<Image>();
        private List<TourSchedule> schedules = new List<TourSchedule>();
        private List<DateTime> dates = new List<DateTime>();
        public List<KeyPoint> keyPoints = new List<KeyPoint>();
        public List<Location> Locations = new List<Location>();
        public List<string> keyPointStrings = new List<string>();
        public List<int> HoursList { get; set; }
        public List<int> MinutesList { get; set; }
        public List<string> States { get; set; }
        public List<string> Cities { get; set; }
        private int _hours;
        public int Hours
        {
            get => _hours;
            set
            {
                if (value != _hours)
                {
                    _hours = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _minutes;
        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value != _minutes)
                {
                    _minutes= value;
                    OnPropertyChanged();
                }
            }
        }
        private string _name;
        public new string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _maxTourists;
        public int MaxTourists
        {
            get => _maxTourists;
            set
            {
                if (value != _maxTourists)
                {
                    _maxTourists = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _state;
        public string State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public new string Language
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _language;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateTourForm()
        {
            InitializeComponent();
            DataContext = this;
            images.Clear();
            HoursList = Enumerable.Range(0, 24).ToList();
            Locations = locationRepository.GetAll();
            States = new List<string>();
            Cities = new List<string>();
            datePicker.DisplayDateStart= DateTime.Now;
            foreach (Location location in Locations)
            {
                if (!States.Contains(location.State))
                {
                    States.Add(location.State);
                }
            }
        }


        private void BtnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            relativeImagePaths.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                // Get path to .exe file
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                // Position to the right folder
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Tour";

                // Make sure that the target folder exists
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
                foreach (string filePath in dlg.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    images.Clear();
                    fileName=SaveImageFile(filePath, destFilePath,fileName);
                    // Forming relative path to the new Image
                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Tour/", fileName);
                    relativeImagePaths.Add(relativePath);
                }
            }
        }
        private void SaveImageIntoCSV(List<string> relativeImagePaths)
        {
            foreach(string filePath in relativeImagePaths)
            {
                Image image = new Image(0,filePath);
                image = imageRepository.Add(image);
                images.Add(image);
            }
        }
        private string SaveImageFile(string filePath, string destFilePath,string fileName)
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
                return fileNameParts[0]+"."+fileNameParts[1];
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

        private void ClickCreateButton(object sender, RoutedEventArgs e)
        {

            // Create a new Tour object
            State = StateBox.Text.Trim();
            City = CityBox.Text.Trim();
            Location location = new Location(State,City);
            location.Id = locationRepository.GetIdByStateCity(State,City);
            Tour newTour = new Tour
            {
                Name = TourNameTextbox.Text.Trim(),
                Description = DescriptionTextbox.Text.Trim(),
                Language = LanguageTextbox.Text.Trim(),
                Location = location,
                MaxTourists = Convert.ToInt32(MaxTouristTextbox.Text.Trim()),
                Duration = Convert.ToInt32(DurationTextbox.Text.Trim()),
                DateTime = DateTime.Now,
                Images =images,
                KeyPoints = this.keyPoints
            };
            newTour.LocationId = location.Id;
            try
            {
                if (relativeImagePaths.Count > 0)
                {
                    SaveImageIntoCSV(relativeImagePaths);
                }
                newTour = tourRepository.Add(newTour);
                int tourId = newTour.Id;
                foreach(string keyPointString in keyPointStrings)
                {
                    KeyPoint keyPoint = new KeyPoint(tourId, keyPointString);
                    keyPointRepository.Add(keyPoint);
                }
                foreach (Image image in images)
                {
                    TourImage tourImage = new TourImage(tourId, image.Id);
                    tourImageRepository.Add(tourImage);
                }
                foreach(TourSchedule schedule in schedules)
                {
                    schedule.TourId = tourId;
                    tourScheduleRepository.Add(schedule);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors, perhaps logging them and showing a message to the user
                MessageBox.Show($"Failed to create the tour. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            NavigationService.GoBack();
        }
        private void ClickCancelButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void MaxTouristTextbox_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
        private void ClickAddKeyPoint(object sender, RoutedEventArgs e)
        {
            keyPointStrings.Add(KeyPointTextbox.Text);
            KeyPointTextbox.Clear();
        }

        public void ClickAddDate(object sender, RoutedEventArgs e)
        {
            DateTime date;
            if(DateTime.TryParse(datePicker.SelectedDate.Value.Date.ToShortDateString() +" "+Hours.ToString()+":"+Minutes.ToString(), out date))
            {
                TourSchedule schedule = new TourSchedule();
                schedule.Guests = 0;
                if (DateExists(date))
                {
                    return;
                }
                schedule.Date = date;
                schedule.Guests = 0;
                schedules.Add(schedule);
                dates.Add(date);
            }
        }
        private bool DateExists(DateTime date)
        {
            foreach(DateTime listDate in dates)
            {
                if (date.Equals(listDate))
                {
                    return true;
                }
            }
            return false;
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void StateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cities.Clear();
            string selectedState = StateBox.SelectedItem as string;
            foreach (Location location in Locations)
            {
                if (selectedState.Equals(location.State) && !Cities.Contains(location.City))
                {
                    Cities.Add(location.City);
                }
            }
            CityBox.ItemsSource = null;
            CityBox.ItemsSource = Cities;
            CityBox.IsEnabled = StateBox.SelectedItem != null;
        }
    }
}
