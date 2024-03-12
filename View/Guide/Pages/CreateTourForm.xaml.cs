using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
        List<string> relativeImagePaths = new List<string>();
        private string _name;
        public string Name
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

        private void OnPropertyChanged()
        {
            throw new NotImplementedException();
        }

        public CreateTourForm()
        {
            DataContext = this;
            InitializeComponent();
        }


        private void BtnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                // Get path to .exe file
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                // Position to the right folder
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\Tour";

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
                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/Tour/", fileName);
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
            foreach(string filePath in relativeImagePaths)
            {
                Image image = new Image();
                image.Path = filePath;
                imageRepository.Add(image);
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

        private void ClickCreateButton(object sender, RoutedEventArgs e)
        {

            NavigationService.GoBack();
        }
        private void ClickCancelButton(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void MaxTouristTextbox_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
