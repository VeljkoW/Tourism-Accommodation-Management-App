using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourRepository
    {
        List<Tour> GetAll();
        Tour? GetById(int Id);
        int NextId();
        Tour Add(Tour newTour);
    }
}