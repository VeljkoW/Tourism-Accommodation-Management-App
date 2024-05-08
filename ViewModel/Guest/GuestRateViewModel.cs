using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.View.Guest.Windows;
using Microsoft.Win32;
using System.IO;
using BookingApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace BookingApp.ViewModel.Guest
{
    public class GuestRateViewModel : INotifyPropertyChanged
    {
        private int currentImageIndex = 0;
        public ObservableCollection<string> ImagePaths { get; set; }
        public RelayCommand attachImage => new RelayCommand(execute => AddImageExecute());

        public RelayCommand rateIt => new RelayCommand(execute => RateIt(), canExecute => CanRateIt());
        public RelayCommand DeleteImageCommand => new RelayCommand(execute => DeleteImage(), canExecute => CanDeleteImage());
        public RelayCommand PreviousImageCommand => new RelayCommand(execute => PreviousImage(), canExecute => CanPreviousImage());
        public RelayCommand NextImageCommand => new RelayCommand(execute => NextImage(), canExecute => CanNextImage());
        public ObservableCollection<Image> Images { get; set; }
        public ObservableCollection<string> RelativeImagePaths { get; set; }

        public ObservableCollection<string> strings { get; set; }

        public GuestRate GuestRate { get; set; }
        public User User { get; set; }  
        public ReservedAccommodation ReservedAccommodation { get; set; }

        public string CurrentImagePath => ImagePaths.ElementAtOrDefault(CurrentImageIndex);
        public int TotalImages => ImagePaths.Count;
        public GuestRateViewModel(GuestRate guestRate, User user, ReservedAccommodation selectedAccommodation)
        { 
            GuestRate = guestRate;
            User = user;
            ReservedAccommodation = selectedAccommodation;
            strings = new ObservableCollection<string>();
            Images = new ObservableCollection<Image>();
            ImagePaths = new ObservableCollection<string>();
            RelativeImagePaths = new ObservableCollection<string>();
            foreach(RenovationRequest renovationRequest in RenovationRequestService.GetInstance().GetAll())
            {
                if(renovationRequest.AccommodationId == selectedAccommodation.Accommodation.Id && renovationRequest.GuestId == User.Id)
                {
                    GuestRate.RenovationButton.IsEnabled = false;
                    GuestRate.RenovationButton.Content = "Renovation sent";
                }    
            }
            GuestRate.AccommodationName.Content += selectedAccommodation.Accommodation.Name;
            GuestRate.Location.Content += selectedAccommodation.Accommodation.Location.State + ", " + selectedAccommodation.Accommodation.Location.City;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
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
            if (TotalImages == 0)
                return false;
            return true;
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
        public void RateIt()
        {
            Comment comment = new Comment();
            comment.Text = GuestRate.CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserService.GetInstance().GetById(User.Id);
            comment = CommentService.GetInstance().Save(comment);

            Accommodation? accommodation = AccommodationService.GetInstance().GetById(ReservedAccommodation.Accommodation.Id);

            OwnerRating ownerRating = new OwnerRating();
            ownerRating.OwnerId = accommodation.OwnerId;
            ownerRating.GuestId = User.Id;
            ownerRating.CommentId = comment.Id;
            ownerRating.Cleanliness = GuestRate.Cleanliness;
            ownerRating.OwnerIntegrity = GuestRate.Integrity;
            ownerRating.AccommodationId = accommodation.Id;

            foreach(Image image in Images) ownerRating.Images.Add(image);

            foreach(OwnerRating owner in OwnerRatingService.GetInstance().GetAll())
            {
                if(owner.OwnerId == accommodation.OwnerId && owner.GuestId == User.Id) 
                {
                    MessageBox.Show("Already rated!");
                    GuestRate.Close();
                    return;
                } 
            }
            OwnerRatingService.GetInstance().Add(ownerRating);

            GuestRate.Close();
        }

        public bool CanRateIt()
        { 
            if((GuestRate.Cleanliness1.IsChecked == false && GuestRate.Cleanliness2.IsChecked == false && GuestRate.Cleanliness3.IsChecked == false
                && GuestRate.Cleanliness4.IsChecked == false && GuestRate.Cleanliness5.IsChecked == false) || string.IsNullOrEmpty(GuestRate.CommentTextBox.Text.ToString()))
            {
                return false;
            }

            return true;
        }
        public void AddImageExecute()
        {
            RelativeImagePaths.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Accommodation";

                if (!Directory.Exists(targetFolderPath)) Directory.CreateDirectory(targetFolderPath);
                foreach (string filePath in dlg.FileNames)
                {
                    strings.Add(filePath);
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    fileName = SaveImageFile(filePath, destFilePath, fileName);

                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Accommodation/", fileName);
                    RelativeImagePaths.Add(relativePath);

                    ImagePaths.Add(destFilePath);
                }
                if (RelativeImagePaths.Count > 0) SaveImageIntoCSV(RelativeImagePaths);

                if (ImagePaths.Count > 0)
                {
                    CurrentImageIndex = 0;
                    OnPropertyChanged(nameof(CurrentImagePath));
                    OnPropertyChanged(nameof(CurrentImageIndex));
                }
            }
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
