using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
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
        public TourComplexSuggestion? Update(TourComplexSuggestion tourComplexSuggestion)
        {
            return TourComplexSuggestionRepository.Update(tourComplexSuggestion);
        }
        public TourComplexSuggestion? GetById(int Id)
        {
            return TourComplexSuggestionRepository.GetById(Id);
        }
        public int GetNextId()
        {
            return TourComplexSuggestionRepository.NextId();
        }
        public void FlagExpired(int id)
        {
            List<TourComplexSuggestion> tourComplexSuggestions = TourComplexSuggestionRepository.GetAll().Where(u => u.UserId == id).ToList();
            foreach(TourComplexSuggestion tcs in tourComplexSuggestions.Where(t => t.Status == TourSuggestionStatus.Pending)) 
            {
                bool expired = false;
                foreach(TourSuggestion ts in tcs.TourSuggestions)
                {
                    if(ts.Status == TourSuggestionStatus.Rejected)
                    {
                        expired = true;
                        break;
                    }
                }
                if(expired)
                {
                    tcs.Status = TourSuggestionStatus.Rejected;
                    Update(tcs);
                }
            }
        }
        public void FlagAccepted(int id)
        {
            List<TourComplexSuggestion> tourComplexSuggestions = GetAll().Where(u => u.UserId == id).ToList();
            foreach (TourComplexSuggestion tcs in tourComplexSuggestions.Where(t => t.Status == TourSuggestionStatus.Pending))
            {
                bool accepted = true;
                foreach (TourSuggestion ts in tcs.TourSuggestions)
                {
                    if (ts.Status != TourSuggestionStatus.Accepted)
                    {
                        accepted = false;
                        break;
                    }
                }
                if (accepted)
                {
                    tcs.Status = TourSuggestionStatus.Accepted;
                    Update(tcs);
                }
            }
        }
        public void UpdateStatus(int id)
        {
            FlagAccepted(id);
            FlagExpired(id);
        }
    }
}
