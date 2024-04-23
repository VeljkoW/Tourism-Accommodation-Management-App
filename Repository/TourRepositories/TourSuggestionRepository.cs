using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourSuggestionRepository : ITourSuggestionRepository
    {
        public static TourSuggestionRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourSuggestionRepository>();
        }
        private const string FilePath = "../../../Resources/Data/toursuggestions.csv";

        private readonly Serializer<TourSuggestion> _serializer;

        private List<TourSuggestion> _tourSuggestions;

        public TourSuggestionRepository()
        {
            _serializer = new Serializer<TourSuggestion>();
            _tourSuggestions = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourSuggestions = _serializer.FromCSV(FilePath);
            if (_tourSuggestions.Count < 1)
            {
                return 1;
            }
            return _tourSuggestions.Max(c => c.Id) + 1;
        }
        public void Add(TourSuggestion newTourSuggestion)
        {
            newTourSuggestion.Id = NextId();
            _tourSuggestions.Add(newTourSuggestion);
            _serializer.ToCSV(FilePath, _tourSuggestions);
        }
        public List<TourSuggestion> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourSuggestion? GetById(int Id)
        {
            _tourSuggestions = _serializer.FromCSV(FilePath);
            return _tourSuggestions.Find(c => c.Id == Id);
        }
        public TourSuggestion? Update(TourSuggestion tourSuggestion)
        {
            TourSuggestion? oldTourSuggestion = GetById(tourSuggestion.Id);
            if (oldTourSuggestion is null) return null;
            oldTourSuggestion.UserId = tourSuggestion.UserId;
            oldTourSuggestion.Location = tourSuggestion.Location;
            oldTourSuggestion.LocationId = tourSuggestion.LocationId;
            oldTourSuggestion.Description = tourSuggestion.Description;
            oldTourSuggestion.Language = tourSuggestion.Language;
            oldTourSuggestion.NumberOfPeople = tourSuggestion.NumberOfPeople;
            oldTourSuggestion.Tourists = tourSuggestion.Tourists;
            oldTourSuggestion.FromDate = tourSuggestion.FromDate;
            oldTourSuggestion.ToDate = tourSuggestion.ToDate;
            oldTourSuggestion.Date = tourSuggestion.Date;
            _serializer.ToCSV(FilePath, _tourSuggestions);
            return oldTourSuggestion;
        }
    }
}
