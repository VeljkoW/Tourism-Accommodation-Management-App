using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourPersonRepository
    {
        List<TourPerson> GetAll();
        TourPerson? GetById(int Id);
        int NextId();
        TourPerson Add(TourPerson newTourPerson);
        TourPerson? Update(TourPerson tourPerson);
        void DeleteById(int Id);
    }
}
