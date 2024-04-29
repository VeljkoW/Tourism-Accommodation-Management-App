using BookingApp.Domain.Model;
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
    /// Interaction logic for RenovationRequestPage.xaml
    /// </summary>
    public partial class RenovationRequestPage : Page
    {
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public User User { get; set; }
        public RenovationRequestPageViewModel RenovationRequestPageViewModel { get; set; }
        public RenovationRequestPage(OwnerMainWindow ownerMainWindow)
        {
            OwnerMainWindow = ownerMainWindow;
            User = ownerMainWindow.user;
            InitializeComponent();
            RenovationRequestPageViewModel = new RenovationRequestPageViewModel(this);
            DataContext = RenovationRequestPageViewModel;
        }

        private void ReviewsClick(object sender, RoutedEventArgs e)
        {
            GuestReviews GuestReviews = new GuestReviews(OwnerMainWindow);
            OwnerMainWindow.mainFrame.Navigate(GuestReviews);
        }
    }
}
