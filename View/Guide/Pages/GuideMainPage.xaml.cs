using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for GuideMainPage.xaml
    /// </summary>
    public partial class GuideMainPage : Page
    {
        public EventHandler? ListUpdater {  get; set; }
        public GuideMainPageViewModel guideMainPageViewModel { get;set; }
        public User User { get; set; }
        public string UserName { get; set; }
        public GuideMainPage(User user)
        {
            InitializeComponent();
            User = user;
            UserName = User.Username;
            guideMainPageViewModel = new GuideMainPageViewModel(user);
            this.DataContext = guideMainPageViewModel;
        }
        private void ClickCreateTour(object sender, RoutedEventArgs e)
        {
            CreateTourForm createTourForm = new CreateTourForm(User);
            NavigationService.Navigate(createTourForm);
        }
        private void ClickLogout(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            OnLogoutHandler?.Invoke(sender, e);
            signInForm.Show();
        }
        public EventHandler OnLogoutHandler { get; set; }
    }
}
