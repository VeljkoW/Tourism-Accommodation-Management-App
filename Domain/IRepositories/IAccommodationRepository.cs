using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IAccommodationRepository
    {
        List<Accommodation> GetAll();
        Accommodation? GetById(int Id);
        int NextId();
        void Add(Accommodation newAccommodation);
        void DeleteById(int Id);
    }
}
