using System;
using System.Collections.Generic;
using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
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

namespace BookingApp.View.Guest.Pages
{
    /// <summary>
    /// Interaction logic for OwnerReviews.xaml
    /// </summary>
    public partial class OwnerReviews : Page
    {
        public User user { get; set; }

        public OwnerReviewsViewModel OwnerReviewsViewModel { get; set; }
        public OwnerReviews(User user)
        {
            InitializeComponent();
            this.user = user;
            OwnerReviewsViewModel = new OwnerReviewsViewModel(this, user);
            DataContext = OwnerReviewsViewModel;
            MainGrid.Focus();
        }
    }
}
