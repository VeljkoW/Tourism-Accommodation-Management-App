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
    public class TourSuggestionService
    {
        public ITourSuggestionRepository TourSuggestionRepository { get; set; }
        public TourSuggestionService(ITourSuggestionRepository tourSuggestionRepository)
        {
            TourSuggestionRepository = tourSuggestionRepository;
        }
        public static TourSuggestionService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourSuggestionService>();
        }
        public void Add(TourSuggestion newTourSuggestion)
        {
            TourSuggestionRepository.Add(newTourSuggestion);
        }

        public List<TourSuggestion> GetAll()
        {
            return TourSuggestionRepository.GetAll();
        }

        public TourSuggestion? GetById(int Id)
        {
            return TourSuggestionRepository.GetById(Id);
        }
        public TourSuggestion? Update(TourSuggestion tourSuggestion)
        {
            return TourSuggestionRepository.Update(tourSuggestion);
        }
    }
}
