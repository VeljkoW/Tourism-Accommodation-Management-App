using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourSuggestionComplexRepository
    {
        List<TourSuggestion> GetAll();
        TourSuggestion? GetById(int Id);
        int NextId();
        void Add(TourSuggestion newTourSuggestion);
        TourSuggestion? Update(TourSuggestion TourSuggestion);
        void DeleteById(int Id);
    }
}
