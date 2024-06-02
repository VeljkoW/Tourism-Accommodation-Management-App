using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.View.Owner;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ForumView = BookingApp.Domain.Model.Forum;

namespace BookingApp.ViewModel.Guest
{
    public class GuestForumViewModel : INotifyPropertyChanged
    {
        public GuestForum GuestForum { get; set; }
        public User user { get; set; }
        public List<string> StatesForChoosing { get; set; }
        public ObservableCollection<Location> CitiesForChoosing { get; set; }
        private string selectedChosenState { get; set; }
        private Location selectedChosenCity { get; set; }
        public INotificationManager notificationManager = App.GetNotificationManager();
        public ObservableCollection<GuestPost> postItems { get; set; }
        public RelayCommand OpenButtonClick => new RelayCommand(execute => OpenForum(), canExecute => CanOpenForum());
        public RelayCommand CloseButtonClick => new RelayCommand(execute => CloseForum());
        public RelayCommand FirstComboBox => new RelayCommand(execute => FirstComboBoxSelect());
        public RelayCommand SecondComboBox => new RelayCommand(execute => SecondComboBoxSelect());
        public RelayCommand CommentTextBox => new RelayCommand(execute => CommentTextBoxFocus());
        public RelayCommand PostButtonClick => new RelayCommand(execute => PostComment(), canExecute => CanPostComment());
        public GuestForumViewModel(GuestForum guestForum, User user)
        {
            GuestForum = guestForum;
            this.user = user;
            StatesForChoosing = LocationService.GetInstance().GetStates();
            CitiesForChoosing = new ObservableCollection<Location>();
            postItems = new ObservableCollection<GuestPost>();
            GuestForum.PostCommentBox.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.Comments.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.CloseButton.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.usefulForum.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.UsernameLabel.Content += user.Username.ToString();
            GuestForum.WarningLabel.Visibility = System.Windows.Visibility.Visible;
        }
        public void FirstComboBoxSelect()
        {
            GuestForum.ComboBoxState.IsDropDownOpen = true;
        }
        public void SecondComboBoxSelect()
        {
            GuestForum.ComboBoxCity.Focusable = true;
            GuestForum.ComboBoxCity.IsDropDownOpen = true;
        }
        public void CommentTextBoxFocus()
        {
            GuestForum.CommentTextBox.Focus();
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
        public bool CanOpenForum()
        {
            if (string.IsNullOrEmpty(GuestForum.ComboBoxState.Text) || string.IsNullOrEmpty(GuestForum.ComboBoxCity.Text)) return false;
            return true;
        }

        public void statePick()
        {
            if (!string.IsNullOrEmpty(SelectedChosenState))
                LocationService.GetInstance().GetCitiesForState(CitiesForChoosing, SelectedChosenState);
        }

        public void cityPick()
        {

        }

        public bool CanPostComment() 
        {
            if (string.IsNullOrEmpty(GuestForum.CommentTextBox.Text)) return false;
            return true;
        }
        public bool IsSpecialUser(GuestPost guestPost, ForumView forum)
        {
            foreach(ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
            {
                if (reservedAccommodation.GuestId == guestPost.UserId && reservedAccommodation.Accommodation.Location.Id == forum.LocationId)
                    return true;
            }
            return false;
        }
        public void PostComment()
        {
            bool findForum = false;
            foreach (ForumView forum in ForumService.GetInstance().GetAll())
            {
                if (forum.LocationId == selectedChosenCity.Id)
                {
                    GuestPost guestPost = new GuestPost();
                    guestPost.ForumId = forum.Id;
                    guestPost.UserId = user.Id;
                    guestPost.Comment = GuestForum.CommentTextBox.Text;
                    GuestForum.CommentTextBox.Clear();
                    guestPost.SpecialUser = IsSpecialUser(guestPost, forum);
                    guestPost.Reports = 0;
                    GuestPostService.GetInstance().Add(guestPost);
                    forum.GuestPosts.Add(guestPost);
                    postItems.Add(guestPost);
                    ForumService.GetInstance().Update(forum);
                    findForum = true;
                    GuestForum.CommentTextBox.Focusable = false;
                    notificationManager.Show("Success!", "New post added", NotificationType.Success);
                    break;
                }
            }
            if (findForum == false)
            {
                ForumView forum = new ForumView();
                forum.LocationId = selectedChosenCity.Id;
                GuestPost guestPost = new GuestPost();
                guestPost.UserId = user.Id;
                guestPost.Comment = GuestForum.CommentTextBox.Text;
                GuestForum.CommentTextBox.Clear();
                guestPost.SpecialUser = IsSpecialUser(guestPost, forum);
                guestPost.Reports = 0;
                GuestPostService.GetInstance().Add(guestPost);
                forum.GuestPosts.Add(guestPost);
                forum.IsValid = false;
                postItems.Add(guestPost);
                ForumService.GetInstance().Add(forum);
                guestPost.ForumId = forum.Id;
                GuestPostService.GetInstance().Update(guestPost);
                notificationManager.Show("Success!", "New post added", NotificationType.Success);
                GuestForum.CommentTextBox.Focusable = false;
            }

        }
        public void CloseForum()
        {
            GuestForum.PostCommentBox.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.Comments.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.CloseButton.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.OpenButton.IsEnabled = true;
            GuestForum.ComboBoxState.IsEnabled = true;
            GuestForum.ComboBoxCity.IsEnabled = true;
            GuestForum.usefulForum.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.WarningLabel.Visibility = System.Windows.Visibility.Visible;
            GuestForum.CommentTextBox.Clear();
            postItems.Clear();
        }
        public void OpenForum() 
        {

            GuestForum.WarningLabel.Visibility = System.Windows.Visibility.Collapsed;
            GuestForum.OpenButton.IsEnabled = false;
            foreach (ForumView forum in ForumService.GetInstance().GetAll())
            {
                if (forum.LocationId == SelectedChosenCity.Id)
                {
                    if (ForumService.GetInstance().isSpecial(forum.Id))
                    {
                        GuestForum.usefulForum.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            GuestForum.ComboBoxState.IsEnabled = false;
            GuestForum.ComboBoxCity.IsEnabled = false;
            if (ForumService.GetInstance().GetAll().Count == 0)
            {
                postItems.Clear();
                GuestForum.PostCommentBox.Visibility = System.Windows.Visibility.Visible;
                GuestForum.Comments.Visibility = System.Windows.Visibility.Visible;
                GuestForum.CloseButton.Visibility = System.Windows.Visibility.Visible;
                
            }
            else
            {
                GuestForum.PostCommentBox.Visibility = System.Windows.Visibility.Visible;
                GuestForum.Comments.Visibility = System.Windows.Visibility.Visible;
                GuestForum.CloseButton.Visibility = System.Windows.Visibility.Visible;
                foreach (ForumView forum in ForumService.GetInstance().GetAll())
                {
                    if (forum.LocationId == selectedChosenCity.Id)
                    {
                        foreach(GuestPost guestPost in forum.GuestPosts)
                        {
                            guestPost.SpecialUser = IsSpecialUser(guestPost, forum);
                            GuestPostService.GetInstance().Update(guestPost);
                            postItems.Add(guestPost);
                        }
                    }
                }
            }
        }
    }
}
