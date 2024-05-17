using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BookingApp.Services
{
    public class AccommodationService
    {
        public IAccommodationRepository AccommodationRepository {get;set;}
        public AccommodationService(IAccommodationRepository accommodationRepository) { AccommodationRepository = accommodationRepository; }
        public static AccommodationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<AccommodationService>();
        }
        public void Add(Accommodation newAccommodation)
        {
            AccommodationRepository.Add(newAccommodation);
        }

        public List<Accommodation> GetAll()
        {
            return AccommodationRepository.GetAll();
        }

        public Accommodation? GetById(int Id)
        {
            return AccommodationRepository.GetById(Id);
        }
        public ObservableCollection<Accommodation> GetAllByUser(User user)
        {
            ObservableCollection<Accommodation> Accommodations = new ObservableCollection<Accommodation>();
            foreach(Accommodation accommodation in GetAll())
            {
                if(user.Id == accommodation.OwnerId)
                {
                    Accommodations.Add(accommodation);
                }
            }
            return Accommodations;
        }
        public void AccommodationsByLocation(User user, ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations)
        {
            foreach (Accommodation accommodation in GetAll().Where(t => t.OwnerId == user.Id))
            {
                if (AccommodationsStatisticsByLocations.Count == 0)
                    AddAccommodationsStatisticsByLocation(accommodation, AccommodationsStatisticsByLocations);
                else
                {
                    bool alreadyExists = false;
                    foreach (AccommodationsStatisticsByLocation accommodationsStatisticsByLocation in AccommodationsStatisticsByLocations)
                        if (accommodationsStatisticsByLocation.LocationId == accommodation.Location.Id)
                        {
                            accommodationsStatisticsByLocation.Accommodations.Add(accommodation);
                            alreadyExists = true;
                            break;
                        }
                    if (!alreadyExists)
                        AddAccommodationsStatisticsByLocation(accommodation, AccommodationsStatisticsByLocations);
                }
            }
        }
        private void AddAccommodationsStatisticsByLocation(Accommodation accommodation,
            ObservableCollection<AccommodationsStatisticsByLocation> AccommodationsStatisticsByLocations)
        {
            AccommodationsStatisticsByLocation AccommodationsStatisticsByLocation = new AccommodationsStatisticsByLocation();
            AccommodationsStatisticsByLocation.LocationId = accommodation.Location.Id;
            AccommodationsStatisticsByLocation.Accommodations.Add(accommodation);
            AccommodationsStatisticsByLocations.Add(AccommodationsStatisticsByLocation);
        }
    }
}
