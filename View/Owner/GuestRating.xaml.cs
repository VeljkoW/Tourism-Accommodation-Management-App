using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
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

using GuestRatingModel = BookingApp.Domain.Model.GuestRating;
using BookingApp.ViewModel.Owner;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for GuestRating.xaml
    /// </summary>
    public partial class GuestRating : Page
    {
        ///
        public GuestRatingViewModel GuestRatingViewModel {  get; set; }
        /*public OwnerMainWindow ownerMainWindow { get; set; }
        public User user { get; set; }
        public List<ReservedAccommodation> ReservedAccommodations { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public ReservedAccommodationRepository ReservedAccommodationRepository { get; set; }
        //public GuestRating GuestRating { get; set; }
        public CommentRepository CommentRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public GuestRatingRepository GuestRatingRepository { get; set; }
        
        public ReservedAccommodation SelectedReservedAccommodations { get; set; }*/
        public GuestRating(OwnerMainWindow ownerMainWindow, User user)
        {
            InitializeComponent();
            ///////
            GuestRatingViewModel = new GuestRatingViewModel(this, ownerMainWindow, user);
            this.DataContext = GuestRatingViewModel;
            /*this.user = user;
            this.ownerMainWindow = ownerMainWindow;
            UserRepository = new UserRepository();
            AccommodationRepository = new AccommodationRepository();
            CommentRepository = new CommentRepository();
            GuestRatingRepository = new GuestRatingRepository();
            ReservedAccommodations = new List<ReservedAccommodation>();
            ReservedAccommodationRepository = new ReservedAccommodationRepository();
            Update();
            //GuestRating = new GuestRating();*/
            SelectErrorLabel.Visibility = Visibility.Collapsed;
            InvalidInputLabel.Visibility = Visibility.Collapsed;
            
        }

        private void RateGuestClick(object sender, RoutedEventArgs e)
        {
            GuestRatingViewModel.RateGuestClick(sender, e);
            /*if (SelectedReservedAccommodations == null)
            {
                SelectErrorLabel.Visibility = Visibility.Visible;
                InvalidInputLabel.Visibility = Visibility.Collapsed;
                return;
            }
            if (CleanlinessComboBox.SelectedItem == null || FollowingGuidelinesComboBox.SelectedItem == null || CommentTextBox.Text.Equals(""))
            {
                SelectErrorLabel.Visibility = Visibility.Collapsed;
                InvalidInputLabel.Visibility = Visibility.Visible;
                return;
            }
            SelectErrorLabel.Visibility = Visibility.Collapsed;
            InvalidInputLabel.Visibility = Visibility.Collapsed;
            Comment comment = new Comment();
            comment.Text = CommentTextBox.Text;
            comment.CreationTime = DateTime.Now;
            comment.User = UserRepository.GetById(user.Id);
            comment = CommentRepository.Save(comment);

            GuestRatingModel GuestRatingModel = new GuestRatingModel();
            GuestRatingModel.ownerId = user.Id;
            GuestRatingModel.guestId = SelectedReservedAccommodations.guestId;
            GuestRatingModel.CommentId = comment.Id;
            GuestRatingModel.Cleanliness = Convert.ToInt32(CleanlinessComboBox.SelectionBoxItem);
            GuestRatingModel.FollowingGuidelines = Convert.ToInt32(FollowingGuidelinesComboBox.SelectionBoxItem);
            GuestRatingRepository.Add(GuestRatingModel);

            SelectedReservedAccommodations = null;
            Update();*/
        }
        /*
        public List<ReservedAccommodation> Update()
        {
            ReservedAccommodations.Clear();
            foreach (ReservedAccommodation tempReservedAccommodation in ReservedAccommodationRepository.GetAll())
            {
                foreach (Accommodation accommodation in AccommodationRepository.GetAll())
                {
                    if (tempReservedAccommodation.accommodationId == accommodation.Id && user.Id == accommodation.OwnerId)
                    {
                        if (GuestRatingRepository.GetAll().Count == 0)
                        {
                            AvailableForRating(tempReservedAccommodation);
                        }
                        else
                        {
                            bool alreadyRated = false;
                            foreach (GuestRatingModel GuestRatingModel in GuestRatingRepository.GetAll())
                            {
                                if (GuestRatingModel.guestId == tempReservedAccommodation.guestId && GuestRatingModel.ownerId == user.Id)
                                {
                                    alreadyRated = true;
                                    break;
                                }
                            }
                            if (!alreadyRated)
                            {
                                AvailableForRating(tempReservedAccommodation);
                            }
                        }
                    }
                }
            }
            ownerMainWindow.NotificationListBox.ItemsSource = ReservedAccommodations;
            ownerMainWindow.NotificationListBox.Items.Refresh();
            RatingGuestsTable.Items.Refresh();
            return ReservedAccommodations;
        }
        public void AvailableForRating(ReservedAccommodation ReservedAccommodation)
        {
            if ((DateTime.Now > ReservedAccommodation.checkOutDate) &&
                (DateTime.Now - ReservedAccommodation.checkOutDate).Days <= 5)
            {
                ReservedAccommodations.Add(ReservedAccommodation);
            }
        }*/
    }
}
