using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumPage = BookingApp.View.Owner.Forum;
using ForumModel = BookingApp.Domain.Model.Forum;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BookingApp.Services;
using System.Windows.Media;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Owner;
using Notification.Wpf;

namespace BookingApp.ViewModel.Owner
{
    public class OwnerForumViewModel : INotifyPropertyChanged
    {
        public INotificationManager notificationManager = App.GetNotificationManager();
        public const string SRB = "sr-RS";
        public const string ENG = "en-US";
        public ObservableCollection<GuestPost> Posts { get; set; }
        public ForumPage ForumPage { get; set; }
        public ForumModel ForumModel { get; set; }
        public User User { get; set; }
        public RelayCommand SearchClick => new RelayCommand(execute => SearchExecute(), canExecute => CanSearchExecute());
        public RelayCommand PostClick => new RelayCommand(execute => PostExecute(), canExecute => CanPostExecute());
        
        public bool HasAccommodations {  get; set; }
        public List<string> States { get; set; }
        public ObservableCollection<Location> Cities { get; set; }
        private string selectedState { get; set; }
        private Location selectedCity { get; set; }
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
        public Location SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged(nameof(selectedCity));
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
        public OwnerForumViewModel(ForumPage forumPage)
        {
            ForumPage = forumPage;
            User = forumPage.User;
            ForumPage.MainPostBorder.Visibility = System.Windows.Visibility.Collapsed;
            States = LocationService.GetInstance().GetStatesWithForums();
            Cities = new ObservableCollection<Location>();
            Posts = new ObservableCollection<GuestPost>();

            ForumPage.MainPostBorder.Visibility = System.Windows.Visibility.Collapsed;
            ForumPage.CommentsItemsControl.Visibility = System.Windows.Visibility.Collapsed;
            ForumPage.SelectedLocationTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            //LoadStates();
        }

        public void PostExecute()
        {
            GuestPost guestPost = new GuestPost();
            guestPost.ForumId = ForumModel.Id;
            guestPost.UserId = User.Id;
            guestPost.Comment = ForumPage.PostCommentTextBox.Text;
            guestPost.SpecialUser = false;
            GuestPostService.GetInstance().Add(guestPost);
            ForumModel.GuestPosts.Add(guestPost);
            Posts.Add(guestPost);
            ForumService.GetInstance().Update(ForumModel);
            if (App.currentLanguage() == ENG)
                notificationManager.Show("Success!", "Post successfully added!", NotificationType.Success);
            else
                notificationManager.Show("Uspeh!", "Uspešno postavljena objava!", NotificationType.Success);
        }
        public bool CanPostExecute()
        {
            if (string.IsNullOrEmpty(ForumPage.PostCommentTextBox.Text))
                return false;
            return true;
        }

        public void SearchExecute()
        {
            Posts.Clear();
            ForumPage.SelectedLocationTextBlock.Visibility = System.Windows.Visibility.Visible;
            ForumPage.SelectedStateRun.Text = SelectedState;
            ForumPage.SelectedCityRun.Text = SelectedCity.City;
            ForumPage.CommentsItemsControl.Visibility = System.Windows.Visibility.Visible;

            bool hasAccommodations = false;
            List<Accommodation> accommodations = AccommodationService.GetInstance().GetAllByUser(User).ToList();
            foreach (Accommodation accommodation in accommodations)
                if (accommodation.Location.Id == SelectedCity.Id)
                {
                    hasAccommodations = true; break;
                }
            HasAccommodations = hasAccommodations;

            foreach (ForumModel forumModel in ForumService.GetInstance().GetAll())
                if (forumModel.LocationId == SelectedCity.Id)
                {
                    ForumModel = forumModel;
                    foreach(GuestPost guestPost in ForumModel.GuestPosts)
                        Posts.Add(guestPost);
                    break;
                }
            

            if (hasAccommodations)
                ForumPage.MainPostBorder.Visibility = System.Windows.Visibility.Visible;
            else
                ForumPage.MainPostBorder.Visibility = System.Windows.Visibility.Collapsed;
            if (ForumService.GetInstance().isSpecial(ForumModel.Id))
            {
                ForumPage.BookmarkImage.Visibility = System.Windows.Visibility.Visible;
                ForumPage.UsefulForumTextBlock.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                ForumPage.BookmarkImage.Visibility = System.Windows.Visibility.Collapsed;
                ForumPage.UsefulForumTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            }
            ForumPage.LocationInfoTextBlock.Visibility = System.Windows.Visibility.Collapsed;

        }
        public bool CanSearchExecute()
        {
            if (string.IsNullOrEmpty(ForumPage.StatesComboBox.Text) ||
                string.IsNullOrEmpty(ForumPage.CitiesComboBox.Text))
                return false;
            return true;
        }
        public void StatePicked()
        {
            if (!string.IsNullOrEmpty(SelectedState))
                LocationService.GetInstance().GetCitiesForStateWithForums(Cities, SelectedState);
        }
        public void ReportClick(GuestPost guestPost)
        {
            List<Accommodation> accommodations = AccommodationService.GetInstance().GetAllByUser(User).ToList();
            if (!HasAccommodations)
            {
                if (App.currentLanguage() == ENG)
                    notificationManager.Show("Info", "You cannot report this user if you have no accommodations on this location!", NotificationType.Error);
                else
                    notificationManager.Show("Info", "Ne možeš da prijaviš ovog korisnika ako nemaš smeštaj na ovoj lokaciji!", NotificationType.Error);
                return;
            }
            foreach (OwnerReport report in OwnerReportService.GetInstance().GetAll())
            {
                if (report.OwnerId == User.Id && report.PostId == guestPost.Id)
                {
                    if (App.currentLanguage() == ENG)
                        notificationManager.Show("Info", "You have already reported this user!", NotificationType.Error);
                    else
                        notificationManager.Show("Info", "Već si prijavio ovog korisnika!", NotificationType.Error);
                    return;
                }
            }
            OwnerReport ownerReport = new OwnerReport();
            ownerReport.OwnerId = User.Id;
            ownerReport.PostId = guestPost.Id;
            OwnerReportService.GetInstance().Add(ownerReport);
            guestPost.Reports++;
            GuestPostService.GetInstance().Update(guestPost);
            Posts.Clear();
            foreach (GuestPost GuestPost in ForumModel.GuestPosts)
                Posts.Add(GuestPost);
        }
        public void CityPicked()
        {

        }
    }
}
