using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for RateGuest.xaml
    /// </summary>
    public partial class RateGuest : Page
    {
        public OwnerMainWindow ownerMainWindow {  get; set; }
        public User user {  get; set; }
        public List<ReservedAccommodation> ReservedAccommodations { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public ReservedAccommodationRepository ReservedAccommodationRepository { get; set; }
        //public GuestRating GuestRating { get; set; }
        public CommentRepository CommentRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public GuestRatingRepository GuestRatingRepository { get; set; }
        public ReservedAccommodation SelectedReservedAccommodations { get; set; }

        public RateGuest(OwnerMainWindow ownerMainWindow, User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;
            this.ownerMainWindow = ownerMainWindow;
            UserRepository = new UserRepository();
            AccommodationRepository = new AccommodationRepository();
            CommentRepository = new CommentRepository();
            GuestRatingRepository = new GuestRatingRepository();
            ReservedAccommodations = new List<ReservedAccommodation>();
            ReservedAccommodationRepository = new ReservedAccommodationRepository();
            //GuestRating = new GuestRating();
            SelectErrorLabel.Visibility = Visibility.Collapsed;
            InvalidInputLabel.Visibility = Visibility.Collapsed;
            Update();
        }

        private void RateGuestClick(object sender, RoutedEventArgs e)
        {
            if (SelectedReservedAccommodations == null)
            {
                SelectErrorLabel.Visibility= Visibility.Visible;
                InvalidInputLabel.Visibility = Visibility.Collapsed;
                return;
            }
            if(CleanlinessComboBox.SelectedItem == null || FollowingGuidelinesComboBox.SelectedItem == null || CommentTextBox.Text.Equals(""))
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

            GuestRating guestRating = new GuestRating();
            guestRating.ownerId = user.Id;
            guestRating.guestId = SelectedReservedAccommodations.guestId;
            guestRating.CommentId = comment.Id;
            guestRating.Cleanliness = Convert.ToInt32(CleanlinessComboBox.SelectionBoxItem);
            guestRating.FollowingGuidelines = Convert.ToInt32(FollowingGuidelinesComboBox.SelectionBoxItem);
            GuestRatingRepository.Add(guestRating);

            SelectedReservedAccommodations = null;
            Update();
        }

        public List<ReservedAccommodation> Update()
        {
            ReservedAccommodations.Clear();
            foreach (ReservedAccommodation tempReservedAccommodation in ReservedAccommodationRepository.GetAll())
            {
                foreach (Accommodation accommodation in AccommodationRepository.GetAll())
                {
                    if(tempReservedAccommodation.accommodationId == accommodation.Id && user.Id == accommodation.OwnerId)
                    {
                        if (GuestRatingRepository.GetAll().Count == 0)
                        {
                            AvailableForRating(tempReservedAccommodation);
                        }
                        else
                        {
                            bool alreadyRated = false;
                            foreach (GuestRating guestRating in GuestRatingRepository.GetAll())
                            {
                                if (guestRating.guestId == tempReservedAccommodation.guestId && guestRating.ownerId == user.Id)
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
        }
    }
}
