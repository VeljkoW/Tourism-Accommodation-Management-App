using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guide;
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

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for UpcomingTours.xaml
    /// </summary>
    public partial class UpcomingTours : Page
    {
        UpcomingTourViewModel vm;
        public UpcomingTours(User user)
        {
            InitializeComponent();
            vm = new UpcomingTourViewModel(user);
            DataContext = vm;
        }
        public void ViewModelLoad()
        {
            vm.Load();
        }
    }
}
