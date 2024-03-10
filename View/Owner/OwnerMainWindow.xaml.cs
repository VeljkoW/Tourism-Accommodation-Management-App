using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        User user { get; set; }
        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            this.user = user;

            // Kreiranje instance Accommodation i prosleđivanje u konstruktor AccommodationRegistration stranice
            Accommodation accommodation = new Accommodation(); // Zamijenjajte sa stvarnim podacima
            AccommodationRegistration registrationPage = new AccommodationRegistration(accommodation);

            // Postavljanje stranice u glavni okvir (mainFrame)
            mainFrame.Navigate(registrationPage);

            //mainFrame.Navigate(new Uri("../../../View/Owner/AccommodationRegistration.xaml", UriKind.Relative));
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new Uri("../../../View/Owner/RateGuest.xaml", UriKind.Relative));
        }*/
    }
}
