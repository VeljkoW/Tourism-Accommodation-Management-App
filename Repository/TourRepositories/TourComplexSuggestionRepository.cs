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
    public class TourComplexSuggestionRepository : ITourComplexSuggestionRepository
    {
        public static TourComplexSuggestionRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourComplexSuggestionRepository>();
        }
        private const string FilePath = "../../../Resources/Data/tourcomplexsuggestions.csv";

        private readonly Serializer<TourComplexSuggestion> _serializer;

        private List<TourComplexSuggestion> _tourComplexSuggestions;

        public TourComplexSuggestionRepository()
        {
            _serializer = new Serializer<TourComplexSuggestion>();
            _tourComplexSuggestions = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourComplexSuggestions = _serializer.FromCSV(FilePath);
            if (_tourComplexSuggestions.Count < 1)
            {
                return 1;
            }
            return _tourComplexSuggestions.Max(c => c.Id) + 1;
        }
        public void Add(TourComplexSuggestion newTourComplexSuggestion)
        {
            newTourComplexSuggestion.Id = NextId();
            _tourComplexSuggestions.Add(newTourComplexSuggestion);
            _serializer.ToCSV(FilePath, _tourComplexSuggestions);
        }
        public List<TourComplexSuggestion> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourComplexSuggestion? GetById(int Id)
        {
            _tourComplexSuggestions = _serializer.FromCSV(FilePath);
            return _tourComplexSuggestions.Find(c => c.Id == Id);
        }
    }
}
