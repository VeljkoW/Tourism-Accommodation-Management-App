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
using System.Collections.ObjectModel;
using ForumModel = BookingApp.Domain.Model.Forum;

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
        public List<string> GetStates()
        {
            List<string> States = new List<string>();
            foreach(Location location in GetAll())
                if(States.Count == 0 || States.Where(t => t == location.State).Count() == 0)
                    States.Add(location.State);
            return States;
        }
        public void GetCitiesForState(ObservableCollection<Location> Cities, string state)
        {
            Cities.Clear();
            foreach (Location location in GetAll())
                if(location.State == state)
                    Cities.Add(location);
        }
        public List<string> GetStatesWithForums()
        {
            List<string> States = new List<string>();
            foreach (Location location in GetAll())
                foreach (ForumModel forumModel in ForumService.GetInstance().GetAll())
                    if (forumModel.LocationId == location.Id && 
                        (States.Count == 0 || States.Where(t => t == location.State).Count() == 0))
                        States.Add(location.State);
            return States;
        }
        public void GetCitiesForStateWithForums(ObservableCollection<Location> Cities, string state)
        {
            Cities.Clear();
            foreach (Location location in GetAll())
                foreach (ForumModel forumModel in ForumService.GetInstance().GetAll())
                    if (location.State == state && forumModel.LocationId == location.Id)
                    {
                        Cities.Add(location);
                        break;
                    }
        }
    }
}
