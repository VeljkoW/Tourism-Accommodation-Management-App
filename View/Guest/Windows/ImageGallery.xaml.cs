using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository;
using BookingApp.ViewModel.Guest;
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
using System.Windows.Shapes;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for ImageGallery.xaml
    /// </summary>
    public partial class ImageGallery : Window
    {
        public User user { get; set; }
        public Image Image { get; set; }
        public AccommodationRepository AccommodationRepository { get; set; }
        public Accommodation Accommodation { get; set; }
        public ImageRepository ImageRepository { get; set; }
        List<Accommodation> accommodations { get; set; }

        public GuestGalleryViewModel GuestGalleryViewModel { get; set; }
        public ImageGallery(Accommodation selectedAccommodation)
        {
            InitializeComponent();
            GuestGalleryViewModel = new GuestGalleryViewModel(this, selectedAccommodation);
            this.DataContext = GuestGalleryViewModel;
            /*this.user = user;
            Accommodation = new Accommodation();
            this.AccommodationRepository = new AccommodationRepository();
            ImageRepository = new ImageRepository();
            accommodations = new List<Accommodation>();
            PrintAccommodation.ItemsSource = AccommodationRepository.GetAll();*/
            /*foreach(Accommodation accommodation in AccommodationRepository.GetAll())
            {
                if(accommodation.Id == selectedAccommodation.Id)
                {
                    accommodations.Add(AccommodationRepository.GetById(accommodation.Id));
                    Gallery.ItemsSource = accommodations;
                }
            }*/
        }
    }
}
