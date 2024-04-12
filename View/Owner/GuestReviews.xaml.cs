using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
using BookingApp.ViewModel.Owner;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for GuestReviews.xaml
    /// </summary>
    public partial class GuestReviews : Page
    {
        public User User {  get; set; }
        public GuestReviewsViewModel GuestReviewsViewModel {  get; set; }
        public OwnerRating SelectedOwnerRating {  get; set; }
        public GuestReviews(User User)
        {
            InitializeComponent();
            this.User = User;
            GuestReviewsViewModel = new GuestReviewsViewModel(User, SelectedOwnerRating);
            DataContext = GuestReviewsViewModel;
        }
    }
}
