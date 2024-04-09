using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class LocationRepository : ILocationRepository
    {
        public static LocationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<LocationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> _serializer;

        private List<Location> _locations;
        public LocationRepository()
        {
            _serializer = new Serializer<Location>();
            _locations = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _locations = _serializer.FromCSV(FilePath);
            if (_locations.Count < 1)
            {
                return 1;
            }
            return _locations.Max(c => c.Id) + 1;
        }
        public void Add(Location newLocation)
        {
            newLocation.Id = NextId();
            _locations.Add(newLocation);
            _serializer.ToCSV(FilePath, _locations);
        }
        public List<Location> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public Location? GetById(int Id)
        {
            _locations = _serializer.FromCSV(FilePath);
            return _locations.Find(c => c.Id == Id);
        }
        public int GetIdByStateCity(string State,string City) {
            /*
            List<Location> locations = GetAll();
            foreach(Location location in locations)
            {
                if(location.City == City)
                {
                    if(location.State==State)
                    {
                        return location.Id;
                    }
                }
            }
            return -1;*/
            return GetAll().FirstOrDefault(location => location.City == City && location.State == State)?.Id ?? -1;
        }
    }
}