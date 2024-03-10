using BookingApp.Model;
using BookingApp.Repository.AccommodationRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Page
    {
        public Accommodation Accommodation { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public AccommodationRegistration(Accommodation accommodation)
        {
            InitializeComponent();
            DataContext = this;
            Accommodation = new Accommodation();
            AccommodationRepository = new AccommodationRepository();
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Accommodation.MaxGuestNumber);
            AccommodationRepository.Add(Accommodation);
        }
    }
}
