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

namespace BookingApp.ViewModel.Guest
{
    public class GuestRateViewModel
    {
        public RelayCommand attachImage => new RelayCommand(execute => AddImageExecute());

        public RelayCommand rateIt => new RelayCommand(execute => RateIt(), canExecute => CanRateIt());
        public ObservableCollection<Image> Images { get; set; }
        public ObservableCollection<string> RelativeImagePaths { get; set; }

        public ObservableCollection<string> strings { get; set; }

        public GuestRate GuestRate { get; set; }
        public User User { get; set; }  
        public ReservedAccommodation ReservedAccommodation { get; set; }
        public GuestRateViewModel(GuestRate guestRate, User user, ReservedAccommodation selectedAccommodation)
        { 
            GuestRate = guestRate;
            User = user;
            ReservedAccommodation = selectedAccommodation;
            strings = new ObservableCollection<string>();
            Images = new ObservableCollection<Image>();
            RelativeImagePaths = new ObservableCollection<string>();
        }

        public void RateIt()
        {
            Comment comment = new Comment();
            comment.Text = GuestRate.CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserService.GetInstance().GetById(User.Id);
            comment = CommentService.GetInstance().Save(comment);
            Accommodation? accommodation = AccommodationService.GetInstance().GetById(ReservedAccommodation.AccommodationId);
            OwnerRating ownerRating = new OwnerRating();
            ownerRating.ownerId = accommodation.OwnerId;
            ownerRating.guestId = User.Id;
            ownerRating.CommentId = comment.Id;
            ownerRating.Cleanliness = Convert.ToInt32(GuestRate.CleanlinessComboBox.SelectionBoxItem);
            ownerRating.OwnerIntegrity = Convert.ToInt32(GuestRate.IntegrityComboBox.SelectionBoxItem);
            ownerRating.AccommodationId = accommodation.Id;
            foreach(Image image in Images)
            {
                ownerRating.Images.Add(image);
            }
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
            if(GuestRate.CleanlinessComboBox.SelectedItem == null || GuestRate.IntegrityComboBox.SelectedItem == null || string.IsNullOrEmpty(GuestRate.CommentTextBox.Text.ToString()))
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
                    strings.Add(filePath);
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    fileName = SaveImageFile(filePath, destFilePath, fileName);

                    // Forming relative path to the new Image
                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Accommodation/", fileName);
                    RelativeImagePaths.Add(relativePath);
                }
                if (RelativeImagePaths.Count > 0)
                {
                    SaveImageIntoCSV(RelativeImagePaths);
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
