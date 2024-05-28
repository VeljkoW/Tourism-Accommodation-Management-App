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
    /// Interaction logic for Forum.xaml
    /// </summary>
    public partial class Forum : Page
    {
        public User User { get; set; }
        public OwnerForumViewModel OwnerForumViewModel {  get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public Forum(OwnerMainWindow ownerMainWindow)
        {
            User = ownerMainWindow.user;
            OwnerMainWindow = ownerMainWindow;
            InitializeComponent();
            OwnerForumViewModel = new OwnerForumViewModel(this);
            DataContext = OwnerForumViewModel;
        }

        private void statePicked(object sender, SelectionChangedEventArgs e)
        {
            OwnerForumViewModel.StatePicked();
        }

        private void cityPicked(object sender, SelectionChangedEventArgs e)
        {
            OwnerForumViewModel.CityPicked();
        }

        private void ReportClick(object sender, RoutedEventArgs e)
        {
            var selectedCard = ((FrameworkElement)sender).DataContext as GuestPost;
            OwnerForumViewModel.ReportClick(selectedCard);

        }
    }
}
