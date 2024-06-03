using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourComplexSuggestionRepository
    {
        List<TourComplexSuggestion> GetAll();
        TourComplexSuggestion? GetById(int Id);
        int NextId();
        void Add(TourComplexSuggestion newTourComplexSuggestion);
        TourComplexSuggestion? Update(TourComplexSuggestion tourComplexSuggestion);
    }
}
