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
        public User user {  get; set; }
        public List<ReservedAccommodation> ReservedAccommodations { get; set; }
        public ReservedAccommodationRepository ReservedAccommodationRepository { get; set; }
        public GuestRating GuestRating { get; set; }
        public CommentRepository CommentRepository { get; set; }
        public UserRepository UserRepository { get; set; }
        public GuestRatingRepository GuestRatingRepository { get; set; }
        public ReservedAccommodation SelectedReservedAccommodations { get; set; }

        public RateGuest(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            this.user = user;
            UserRepository = new UserRepository();
            CommentRepository = new CommentRepository();
            GuestRatingRepository = new GuestRatingRepository();
            ReservedAccommodations = new List<ReservedAccommodation>();
            ReservedAccommodationRepository = new ReservedAccommodationRepository();
            GuestRating = new GuestRating();
            foreach (ReservedAccommodation tempReservedAccommodation in ReservedAccommodationRepository.GetAll())
            {
                if (GuestRatingRepository.GetAll().Count == 0)
                {
                    if ((DateTime.Now > tempReservedAccommodation.checkOutDate) &&
                           (DateTime.Now - tempReservedAccommodation.checkOutDate).Days <= 5)
                    {
                        ReservedAccommodations.Add(tempReservedAccommodation);
                    }
                }
                else
                {
                    foreach (GuestRating guestRating in GuestRatingRepository.GetAll())
                    {
                        if (guestRating.ownerId == user.Id && guestRating.guestId == tempReservedAccommodation.guestId)
                        {
                            continue;
                        }
                        else
                        {
                            if ((DateTime.Now > tempReservedAccommodation.checkOutDate) &&
                               (DateTime.Now - tempReservedAccommodation.checkOutDate).Days <= 5)
                            {
                                ReservedAccommodations.Add(tempReservedAccommodation);
                            }
                        }
                    }
                }
            }
            SelectErrorLabel.Visibility = Visibility.Collapsed;
            InvalidInputLabel.Visibility = Visibility.Collapsed;
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

            User CommentUser = new User();
            comment.User = UserRepository.GetById(user.Id);
            comment = CommentRepository.Save(comment);
            GuestRating.ownerId = user.Id;
            GuestRating.guestId = SelectedReservedAccommodations.guestId;
            GuestRating.CommentId = comment.Id;
            GuestRatingRepository.Add(GuestRating);
        }
    }
}
