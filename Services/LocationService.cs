using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class LocationService
    {
        public ILocationRepository LocationRepository { get; set; }
        public LocationService(ILocationRepository locationRepository) { LocationRepository = locationRepository; }
        public static LocationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<LocationService>();
        }
        public void Add(Location newLocation)
        {
            LocationRepository.Add(newLocation);
        }

        public List<Location> GetAll()
        {
            return LocationRepository.GetAll();
        }

        public Location? GetById(int Id)
        {
            return LocationRepository.GetById(Id);
        }
        public int GetIdByStateCity(string State, string City)
        {
            return LocationRepository.GetIdByStateCity(State, City);
        }
    }
}
