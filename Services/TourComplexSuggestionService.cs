using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourComplexSuggestionService
    {
        public ITourComplexSuggestionRepository TourComplexSuggestionRepository { get; set; }
        public TourComplexSuggestionService(ITourComplexSuggestionRepository tourComplexSuggestionRepository)
        {
            TourComplexSuggestionRepository = tourComplexSuggestionRepository;
        }
        public static TourComplexSuggestionService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourComplexSuggestionService>();
        }
        public void Add(TourComplexSuggestion newTourComplexSuggestion)
        {
            TourComplexSuggestionRepository.Add(newTourComplexSuggestion);
        }

        public List<TourComplexSuggestion> GetAll()
        {
            return TourComplexSuggestionRepository.GetAll();
        }

        public TourComplexSuggestion? GetById(int Id)
        {
            return TourComplexSuggestionRepository.GetById(Id);
        }
        public int GetNextId()
        {
            return TourComplexSuggestionRepository.NextId();
        }
    }
}
