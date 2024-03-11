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
    /// Interaction logic for GuideMainPage.xaml
    /// </summary>
    public partial class GuideMainPage : Page
    {
        public GuideMainPage()
        {

            InitializeComponent();
        }

        private void ClickCreateTour(object sender, RoutedEventArgs e)
        {
            CreateTourForm createTourForm = new CreateTourForm();
            NavigationService.Navigate(createTourForm);
        }
    }
}
