using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReviewWindowViewModel
    {
        public TourReviewWindow TourReviewWindow { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public int GuideKnowledge { get; set; }
        public int GuideSpeech {  get; set; }
        public int TourEnjoyment {  get; set; }
        public BitmapImage StarFilled = new BitmapImage(new Uri("../../Resources/Images/Tourist/star.png", UriKind.Relative));
        public BitmapImage StarEmpty = new BitmapImage(new Uri("../../Resources/Images/Tourist/star_empty.png", UriKind.Relative));
        public List<string> RelativeImagePaths = new List<string>();
        private List<Image> Images = new List<Image>();
        public RelayCommand ClickCancel => new RelayCommand(execute => CancelExecute());
        public RelayCommand ClickSubmit => new RelayCommand(execute => SubmitExecute());
        public TourReviewWindowViewModel(TourReviewWindow tourReviewWindow, Tour tour, User user) 
        { 
            this.TourReviewWindow = tourReviewWindow;
            this.Tour = tour;
            this.User = user;
            Images.Clear();
            RelativeImagePaths.Clear();
            TourReviewWindow.NameTextBlock.Text = Tour.Name;
            GuideKnowledge = -1;
            GuideSpeech = -1;
            TourEnjoyment = -1;
        }
        public void BtnSelectFiles_ClickExecute(object sender,RoutedEventArgs e)
        {
            RelativeImagePaths.Clear();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                string binPath = AppDomain.CurrentDomain.BaseDirectory;
                string targetFolderPath = GetBaseFolder(binPath) + "\\Resources\\Images\\TourReview";
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
                foreach (string filePath in dlg.FileNames)
                {
                    string fileName = System.IO.Path.GetFileName(filePath);
                    string destFilePath = System.IO.Path.Combine(targetFolderPath, fileName);
                    Images.Clear();
                    fileName = SaveImageFile(filePath, destFilePath, fileName);
                    string relativePath = System.IO.Path.Combine("../../../Resources/Images/TourReview/", fileName);
                    RelativeImagePaths.Add(relativePath);
                }
            }
        }

        public string GetBaseFolder(string path)
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

        public string SaveImageFile(string filePath, string destFilePath, string fileName)
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
        public void SaveImageIntoCSV(List<string> relativeImagePaths)
        {
            foreach (string filePath in relativeImagePaths)
            {
                Image image = new Image(0, filePath);
                image = ImageService.GetInstance().Add(image);
                Images.Add(image);
            }
        }

        public void StarMouseEnter(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Image star = (System.Windows.Controls.Image)sender;
            star.Source = StarFilled;

            SetPreviousStarsFilled(star);
        }
        public void StarMouseLeave(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Image star = (System.Windows.Controls.Image)sender;
            if ((string)star.Tag == "selected")
            {
                star.Source = StarFilled;
                SetPreviousStarsFilled(star);
            }
            else
            {
                star.Source = StarEmpty;
            }
            SetUnselectedStarsEmpty();
        }
        public void SetUnselectedStarsEmpty()
        {   //Fixes a bug where unselected stars keep staying filled
            SetUnselectedStarsEmptyKnowledge();
            SetUnselectedStarsEmptySpeech();
            SetUnselectedStarsEmptyEnjoyment();
        }
        public void SetUnselectedStarsEmptyKnowledge()
        {   //Guide Knowledge
            if ((string)TourReviewWindow.GuideKnowledge5Star.Tag == "unselected")
            {
                TourReviewWindow.GuideKnowledge5Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideKnowledge4Star.Tag == "unselected")
            {
                TourReviewWindow.GuideKnowledge4Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideKnowledge3Star.Tag == "unselected")
            {
                TourReviewWindow.GuideKnowledge3Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideKnowledge2Star.Tag == "unselected")
            {
                TourReviewWindow.GuideKnowledge2Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideKnowledge1Star.Tag == "unselected")
            {
                TourReviewWindow.GuideKnowledge1Star.Source = StarEmpty;
            }
        }
        public void SetUnselectedStarsEmptySpeech()
        {   //Guide Speech
            if ((string)TourReviewWindow.GuideSpeech5Star.Tag == "unselected")
            {
                TourReviewWindow.GuideSpeech5Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideSpeech4Star.Tag == "unselected")
            {
                TourReviewWindow.GuideSpeech4Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideSpeech3Star.Tag == "unselected")
            {
                TourReviewWindow.GuideSpeech3Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideSpeech2Star.Tag == "unselected")
            {
                TourReviewWindow.GuideSpeech2Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.GuideSpeech1Star.Tag == "unselected")
            {
                TourReviewWindow.GuideSpeech1Star.Source = StarEmpty;
            }

        }
        public void SetUnselectedStarsEmptyEnjoyment()
        {   //Tour Enjoyment
            if ((string)TourReviewWindow.TourEnjoyment5Star.Tag == "unselected")
            {
                TourReviewWindow.TourEnjoyment5Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.TourEnjoyment4Star.Tag == "unselected")
            {
                TourReviewWindow.TourEnjoyment4Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.TourEnjoyment3Star.Tag == "unselected")
            {
                TourReviewWindow.TourEnjoyment3Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.TourEnjoyment2Star.Tag == "unselected")
            {
                TourReviewWindow.TourEnjoyment2Star.Source = StarEmpty;
            }
            if ((string)TourReviewWindow.TourEnjoyment1Star.Tag == "unselected")
            {
                TourReviewWindow.TourEnjoyment1Star.Source = StarEmpty;
            }
        }
        public void StarClick(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Image star = (System.Windows.Controls.Image)sender;
            star.Tag = "selected";

            SetGuideKnowledge(star);    //Sets the rating of the selected type
            SetGuideSpeech(star);
            SetTourEnjoyment(star);

            SetPreviousStarsFilled(star);   //Sets next and previous stars to empty and filled depending on the clicked star
            SetNextStarsEmpty(star);

            SetPreviousStarsSelected(star); //Sets the tag of the next and previous stars to unselected and selected depending on the clicked star
            SetNextStarsUnSelected(star);
        }
        public void SetGuideKnowledge(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //GuideKnowledge
                case "GuideKnowledge5Star":
                    GuideKnowledge = 5;
                    break;
                case "GuideKnowledge4Star":
                    GuideKnowledge = 4;
                    break;
                case "GuideKnowledge3Star":
                    GuideKnowledge = 3;
                    break;
                case "GuideKnowledge2Star":
                    GuideKnowledge = 2;
                    break;
                case "GuideKnowledge1Star":
                    GuideKnowledge = 1;
                    break;

            }
        }
        public void SetGuideSpeech(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Speech
                case "GuideSpeech5Star":
                    GuideSpeech = 5;
                    break;
                case "GuideSpeech4Star":
                    GuideSpeech = 4;
                    break;
                case "GuideSpeech3Star":
                    GuideSpeech = 3;
                    break;
                case "GuideSpeech2Star":
                    GuideSpeech = 2;
                    break;
                case "GuideSpeech1Star":
                    GuideSpeech = 1;
                    break;

            }
        }
        public void SetTourEnjoyment(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Tour Enjoyment
                case "TourEnjoyment5Star":
                    TourEnjoyment = 5;
                    break;
                case "TourEnjoyment4Star":
                    TourEnjoyment = 4;
                    break;
                case "TourEnjoyment3Star":
                    TourEnjoyment = 3;
                    break;
                case "TourEnjoyment2Star":
                    TourEnjoyment = 2;
                    break;
                case "TourEnjoyment1Star":
                    TourEnjoyment = 1;
                    break;
            }
        }
        private void SetPreviousStarsFilled(System.Windows.Controls.Image currentStar)
        {   //checks what star was clicked and sets the previous stars to be filled
            SetPreviousStarsFilledKnowledge(currentStar);
            SetPreviousStarsFilledSpeech(currentStar);
            SetPreviousStarsFilledEnjoyment(currentStar);
        }
        private void SetPreviousStarsFilledKnowledge(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Knowledge
                case "GuideKnowledge5Star":
                    TourReviewWindow.GuideKnowledge4Star.Source = StarFilled;
                    goto case "GuideKnowledge4Star";
                case "GuideKnowledge4Star":
                    TourReviewWindow.GuideKnowledge3Star.Source = StarFilled;
                    goto case "GuideKnowledge3Star";
                case "GuideKnowledge3Star":
                    TourReviewWindow.GuideKnowledge2Star.Source = StarFilled;
                    goto case "GuideKnowledge2Star";
                case "GuideKnowledge2Star":
                    TourReviewWindow.GuideKnowledge1Star.Source = StarFilled;
                    break;
            }
        }
        private void SetPreviousStarsFilledSpeech(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Speech
                case "GuideSpeech5Star":
                    TourReviewWindow.GuideSpeech4Star.Source = StarFilled;
                    goto case "GuideSpeech4Star";
                case "GuideSpeech4Star":
                    TourReviewWindow.GuideSpeech3Star.Source = StarFilled;
                    goto case "GuideSpeech3Star";
                case "GuideSpeech3Star":
                    TourReviewWindow.GuideSpeech2Star.Source = StarFilled;
                    goto case "GuideSpeech2Star";
                case "GuideSpeech2Star":
                    TourReviewWindow.GuideSpeech1Star.Source = StarFilled;
                    break;
            }
        }
        private void SetPreviousStarsFilledEnjoyment(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name) //Tour Enjoyment
            {
                case "TourEnjoyment5Star":
                    TourReviewWindow.TourEnjoyment4Star.Source = StarFilled;
                    goto case "TourEnjoyment4Star";
                case "TourEnjoyment4Star":
                    TourReviewWindow.TourEnjoyment3Star.Source = StarFilled;
                    goto case "TourEnjoyment3Star";
                case "TourEnjoyment3Star":
                    TourReviewWindow.TourEnjoyment2Star.Source = StarFilled;
                    goto case "TourEnjoyment2Star";
                case "TourEnjoyment2Star":
                    TourReviewWindow.TourEnjoyment1Star.Source = StarFilled;
                    break;
            }
        }
        private void SetNextStarsEmpty(System.Windows.Controls.Image currentStar)
        {   //checks what star was clicked and sets the mext stars to be empty (in case the user changes his rating) 
            SetNextStarsEmptyKnowledge(currentStar);
            SetNextStarsEmptySpeech(currentStar);
            SetNextStarsEmptyEnjoyment(currentStar);
        }

        private void SetNextStarsEmptyKnowledge(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide knowledge
                case "GuideKnowledge1Star":
                    TourReviewWindow.GuideKnowledge2Star.Source = StarEmpty;
                    goto case "GuideKnowledge2Star";
                case "GuideKnowledge2Star":
                    TourReviewWindow.GuideKnowledge3Star.Source = StarEmpty;
                    goto case "GuideKnowledge3Star";
                case "GuideKnowledge3Star":
                    TourReviewWindow.GuideKnowledge4Star.Source = StarEmpty;
                    goto case "GuideKnowledge4Star";
                case "GuideKnowledge4Star":
                    TourReviewWindow.GuideKnowledge5Star.Source = StarEmpty;
                    break;
            }
        }
        private void SetNextStarsEmptySpeech(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide speech
                case "GuideSpeech1Star":
                    TourReviewWindow.GuideSpeech2Star.Source = StarEmpty;
                    goto case "GuideSpeech2Star";
                case "GuideSpeech2Star":
                    TourReviewWindow.GuideSpeech3Star.Source = StarEmpty;
                    goto case "GuideSpeech3Star";
                case "GuideSpeech3Star":
                    TourReviewWindow.GuideSpeech4Star.Source = StarEmpty;
                    goto case "GuideSpeech4Star";
                case "GuideSpeech4Star":
                    TourReviewWindow.GuideSpeech5Star.Source = StarEmpty;
                    break;
            }
        }
        private void SetNextStarsEmptyEnjoyment(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Tour Enjoyment
                case "TourEnjoyment1Star":
                    TourReviewWindow.TourEnjoyment2Star.Source = StarEmpty;
                    goto case "TourEnjoyment2Star";
                case "TourEnjoyment2Star":
                    TourReviewWindow.TourEnjoyment3Star.Source = StarEmpty;
                    goto case "TourEnjoyment3Star";
                case "TourEnjoyment3Star":
                    TourReviewWindow.TourEnjoyment4Star.Source = StarEmpty;
                    goto case "TourEnjoyment4Star";
                case "TourEnjoyment4Star":
                    TourReviewWindow.TourEnjoyment5Star.Source = StarEmpty;
                    break;
                    
            }
        }
        private void SetPreviousStarsSelected(System.Windows.Controls.Image currentStar)
        {   //Sets all the previous stars to be selected, so they dont turn empty once the mouse has left the image
            SetPreviousStarsSelectedKnowledge(currentStar);
            SetPreviousStarsSelectedSpeech(currentStar);
            SetPreviousStarsSelectedEnjoyment(currentStar);
        }
        private void SetPreviousStarsSelectedKnowledge(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Knowledge
                case "GuideKnowledge5Star":
                    TourReviewWindow.GuideKnowledge4Star.Tag = "selected";
                    goto case "GuideKnowledge4Star";
                case "GuideKnowledge4Star":
                    TourReviewWindow.GuideKnowledge3Star.Tag = "selected";
                    goto case "GuideKnowledge3Star";
                case "GuideKnowledge3Star":
                    TourReviewWindow.GuideKnowledge2Star.Tag = "selected";
                    goto case "GuideKnowledge2Star";
                case "GuideKnowledge2Star":
                    TourReviewWindow.GuideKnowledge1Star.Tag = "selected";
                    break;
            }
        }
        private void SetPreviousStarsSelectedSpeech(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Speech
                case "GuideSpeech5Star":
                    TourReviewWindow.GuideSpeech4Star.Tag = "selected";
                    goto case "GuideSpeech4Star";
                case "GuideSpeech4Star":
                    TourReviewWindow.GuideSpeech3Star.Tag = "selected";
                    goto case "GuideSpeech3Star";
                case "GuideSpeech3Star":
                    TourReviewWindow.GuideSpeech2Star.Tag = "selected";
                    goto case "GuideSpeech2Star";
                case "GuideSpeech2Star":
                    TourReviewWindow.GuideSpeech1Star.Tag = "selected";
                    break;
            }
        }
        private void SetPreviousStarsSelectedEnjoyment(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Tour Enjoyment
                case "TourEnjoyment5Star":
                    TourReviewWindow.TourEnjoyment4Star.Tag = "selected";
                    goto case "TourEnjoyment4Star";
                case "TourEnjoyment4Star":
                    TourReviewWindow.TourEnjoyment3Star.Tag = "selected";
                    goto case "TourEnjoyment3Star";
                case "TourEnjoyment3Star":
                    TourReviewWindow.TourEnjoyment2Star.Tag = "selected";
                    goto case "TourEnjoyment2Star";
                case "TourEnjoyment2Star":
                    TourReviewWindow.TourEnjoyment1Star.Tag = "selected";
                    break;
            }
        }
        private void SetNextStarsUnSelected(System.Windows.Controls.Image currentStar)
        {   //checks what star was clicked and sets the mext stars to be unselected (in case the user changes his rating)
            SetNextStarsUnSelectedKnowledge(currentStar);
            SetNextStarsUnSelectedSpeech(currentStar);
            SetNextStarsUnSelectedEnjoyment(currentStar);
        }
        private void SetNextStarsUnSelectedKnowledge(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Knowledge
                case "GuideKnowledge1Star":
                    TourReviewWindow.GuideKnowledge2Star.Tag = "unselected";
                    goto case "GuideKnowledge2Star";
                case "GuideKnowledge2Star":
                    TourReviewWindow.GuideKnowledge3Star.Tag = "unselected";
                    goto case "GuideKnowledge3Star";
                case "GuideKnowledge3Star":
                    TourReviewWindow.GuideKnowledge4Star.Tag = "unselected";
                    goto case "GuideKnowledge4Star";
                case "GuideKnowledge4Star":
                    TourReviewWindow.GuideKnowledge5Star.Tag = "unselected";
                    break;
            }
        }
        private void SetNextStarsUnSelectedSpeech(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Guide Speech
                case "GuideSpeech1Star":
                    TourReviewWindow.GuideSpeech2Star.Tag = "unselected";
                    goto case "GuideSpeech2Star";
                case "GuideSpeech2Star":
                    TourReviewWindow.GuideSpeech3Star.Tag = "unselected";
                    goto case "GuideSpeech3Star";
                case "GuideSpeech3Star":
                    TourReviewWindow.GuideSpeech4Star.Tag = "unselected";
                    goto case "GuideSpeech4Star";
                case "GuideSpeech4Star":
                    TourReviewWindow.GuideSpeech5Star.Tag = "unselected";
                    break;
            }
        }
        private void SetNextStarsUnSelectedEnjoyment(System.Windows.Controls.Image currentStar)
        {
            switch (currentStar.Name)
            {   //Tour Enjoyment
                case "TourEnjoyment1Star":
                    TourReviewWindow.TourEnjoyment2Star.Tag = "unselected";
                    goto case "TourEnjoyment2Star";
                case "TourEnjoyment2Star":
                    TourReviewWindow.TourEnjoyment3Star.Tag = "unselected";
                    goto case "TourEnjoyment3Star";
                case "TourEnjoyment3Star":
                    TourReviewWindow.TourEnjoyment4Star.Tag = "unselected";
                    goto case "TourEnjoyment4Star";
                case "TourEnjoyment4Star":
                    TourReviewWindow.TourEnjoyment5Star.Tag = "unselected";
                    break;
                    
            }
        }
        public void SaveReviewImageIntoCSV(TourReview tourReview)
        {
            if (RelativeImagePaths != null && RelativeImagePaths.Count > 0)
            {
                SaveImageIntoCSV(RelativeImagePaths);
                foreach (Image image in Images)
                {
                    foreach (TourReview tourReview1 in TourReviewService.GetInstance().GetAll())
                    {
                        if (tourReview1.UserId == tourReview.UserId && tourReview1.TourScheduleId == tourReview.TourScheduleId)
                        {
                            TourReviewImage tourReviewImage = new TourReviewImage(tourReview1.Id, image.Id);
                            TourReviewImageService.GetInstance().Add(tourReviewImage);
                        }
                    }
                }
            }
        }
        public void CancelExecute()
        {
            foreach (string s in RelativeImagePaths)
            {
                File.Delete(s);
            }
            RelativeImagePaths.Clear();
            Images.Clear();
            TourReviewWindow.Close();
            TourFinishedDetailed tourFinishedDetailed = new TourFinishedDetailed(Tour, User);
            tourFinishedDetailed.ShowDialog();
        }
        public void SubmitExecute()
        {
            if(TourEnjoyment != -1 && GuideKnowledge != -1 && GuideSpeech != -1 && !String.IsNullOrEmpty(TourReviewWindow.DescriptionTextBox.Text))
            {
                int tourscheduleId = -1;
                foreach(TourSchedule tourSchedule in TourScheduleService.GetInstance().GetAll())
                {
                    if(tourSchedule.TourId == Tour.Id && tourSchedule.Date  == Tour.DateTime) 
                    {
                        tourscheduleId = tourSchedule.Id;
                    }
                }
                string comment = TourReviewWindow.DescriptionTextBox.Text;
                TourReview tourReview = new TourReview(User.Id, tourscheduleId, GuideKnowledge, GuideSpeech, TourEnjoyment, comment, ReviewStatus.Valid);
                TourReviewService.GetInstance().Add(tourReview);
                SaveReviewImageIntoCSV(tourReview);
                
                TourReviewWindow.Close();

            }
        }

    }
}
