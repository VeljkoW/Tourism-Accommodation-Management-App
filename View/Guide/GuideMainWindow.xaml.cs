using BookingApp.Model;
using BookingApp.View.Guide.Pages;
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

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for GuideMainWindow.xaml
    /// </summary>
    public partial class GuideMainWindow : Window
    {
        public User User {  get; set; }
        public string UserName { get; set; }
        public GuideMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            User = user;
            UserName = user.Username;
            GuideMainPage guideMainPage = new GuideMainPage();
            MainFrame.Navigate(guideMainPage);
        }
    }
}