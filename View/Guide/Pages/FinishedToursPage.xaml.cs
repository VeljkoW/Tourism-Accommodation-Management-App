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
    /// Interaction logic for TourReviewsPage.xaml
    /// </summary>
    public partial class FinishedToursPage : Page
    {
        public GuideMainPage GuideMainPage { get; }
        public User User { get; }
        public FinishedToursPage(GuideMainPage guideMainPage, User user)
        {
            InitializeComponent();
            GuideMainPage = guideMainPage;
            User = user;
            FinishedToursPageViewModel viewModel = new FinishedToursPageViewModel(this,User);
            DataContext = viewModel;
        }
    }
}
