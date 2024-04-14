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
        private ILocationRepository locationRepository = LocationRepository.GetInstance();
        public LocationService() { }
        public static LocationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<LocationService>();
        }
        public void Add(Location newLocation)
        {
            locationRepository.Add(newLocation);
        }

        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }

        public Location? GetById(int Id)
        {
            return locationRepository.GetById(Id);
        }
        public int GetIdByStateCity(string State, string City)
        {
            return locationRepository.GetIdByStateCity(State, City);
        }
    }
}
