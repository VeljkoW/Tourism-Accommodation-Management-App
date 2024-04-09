using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ILocationRepository
    {
        List<Location> GetAll();
        Location? GetById(int Id);
        int NextId();
        void Add(Location newImage);
        int GetIdByStateCity(string State, string City);
    }
}
