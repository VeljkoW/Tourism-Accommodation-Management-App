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
using Image = BookingApp.Model.Image;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Page
    {
        public Accommodation Accommodation { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public ImageRepository imageRepository = new ImageRepository();
        List<string> relativeImagePaths = new List<string>();
        public AccommodationRegistration(Accommodation accommodation)
        {
            InitializeComponent();
            this.DataContext = this;
            Accommodation = new Accommodation();
            AccommodationRepository = new AccommodationRepository();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Accommodation.MaxGuestNumber);
            AccommodationRepository.Add(Accommodation);
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
                    if (relativeImagePaths.Count > 0)
                    {
                        SaveImageIntoCSV(relativeImagePaths);
                    }
                }
            }
        }
        private void SaveImageIntoCSV(List<string> relativeImagePaths)
        {
            foreach (string filePath in relativeImagePaths)
            {
                Image? image = new Image();
                image.Path = filePath;
                image = imageRepository.Add(image);
                Accommodation.Images.Add(image);
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

    }
}
