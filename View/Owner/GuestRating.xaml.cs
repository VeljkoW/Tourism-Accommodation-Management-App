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
        //public OwnerMainWindow ownerMainWindow { get; set; }
        //public User user { get; set; }
        //public List<ReservedAccommodation> ReservedAccommodations { get; set; }
        //public ReservedAccommodation SelectedReservedAccommodations { get; set; }
        public GuestRating(OwnerMainWindow ownerMainWindow, User user)
        {
            InitializeComponent();
            GuestRatingViewModel = new GuestRatingViewModel(this, ownerMainWindow, user);
            this.DataContext = GuestRatingViewModel;
            //this.user = user;
            //this.ownerMainWindow = ownerMainWindow;
            //ReservedAccommodations = new List<ReservedAccommodation>();
            SelectErrorLabel.Visibility = Visibility.Collapsed;
            InvalidInputLabel.Visibility = Visibility.Collapsed;
        }
    }
}
