using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.View.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuestGallery = BookingApp.View.Guest.ImageGallery;

namespace BookingApp.ViewModel.Guest
{
    public class GuestGalleryViewModel
    {
        public ObservableCollection<Accommodation> Accommodations { get; set; }

        public GuestGallery GuestGallery { get; set; }
        public GuestGalleryViewModel(GuestGallery GuestGallery, Accommodation selectedAccommodation) 
        {
            this.GuestGallery = GuestGallery;
            Accommodations = new ObservableCollection<Accommodation>();
            foreach (Accommodation accommodation in AccommodationService.GetInstance().GetAll())
            {
                if (accommodation.Id == selectedAccommodation.Id)
                {
                    Accommodations.Add(AccommodationService.GetInstance().GetById(accommodation.Id));
                    GuestGallery.Gallery.ItemsSource = Accommodations;
                }
            }
        }
    }
}
