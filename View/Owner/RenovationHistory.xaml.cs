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
    /// Interaction logic for RenovationHistory.xaml
    /// </summary>
    public partial class RenovationHistory : Page
    {
        public RenovationHistoryViewModel RenovationHistoryViewModel {  get; set; }
        public User User { get; set; }
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public RenovationHistory(OwnerMainWindow OwnerMainWindow)
        {
            this.OwnerMainWindow = OwnerMainWindow;
            User = OwnerMainWindow.user;
            InitializeComponent();
            RenovationHistoryViewModel = new RenovationHistoryViewModel(this);
            DataContext = RenovationHistoryViewModel;
        }

        private void SchedulingClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindow.mainFrame.Navigate(OwnerMainWindow.Renovation);
        }
    }
}
