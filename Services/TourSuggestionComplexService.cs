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
    internal class TourSuggestionComplexService
    {
        public ITourSuggestionComplexRepository TourSuggestionComplexRepository { get; set; }
        public TourSuggestionComplexService(ITourSuggestionComplexRepository tourSuggestionComplexRepository)
        {
            TourSuggestionComplexRepository = tourSuggestionComplexRepository;
        }
        public static TourSuggestionComplexService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourSuggestionComplexService>();
        }
        public void Add(TourSuggestion newTourSuggestion)
        {
            TourSuggestionComplexRepository.Add(newTourSuggestion);
        }

        public List<TourSuggestion> GetAll()
        {
            return TourSuggestionComplexRepository.GetAll();
        }

        public TourSuggestion? GetById(int Id)
        {
            return TourSuggestionComplexRepository.GetById(Id);
        }
        public TourSuggestion? Update(TourSuggestion tourSuggestion)
        {
            return TourSuggestionComplexRepository.Update(tourSuggestion);
        }
        public void DeleteById(int Id) 
        {
            TourSuggestionComplexRepository.DeleteById(Id);
        }
        public List<TourSuggestion> GetAllByComplexId(int complexId)
        {
            return GetAll().Where(t => t.ComplexTourId == complexId).ToList();
        }
        public void CheckForExpiryDate(int id)
        {
            List<TourSuggestion> tourSuggestions = GetAll().Where(u => u.UserId == id).ToList();
            foreach(var tourSuggestion in tourSuggestions)
            {
                if(tourSuggestion.Status == TourSuggestionStatus.Pending)
                {
                    TimeSpan timeDifference = tourSuggestion.FromDate - DateTime.Now;

                    if (timeDifference.TotalHours < 48)
                    {
                        tourSuggestion.Status = TourSuggestionStatus.Rejected;
                        Update(tourSuggestion);
                    }
                }
            }

        }
    }
}
