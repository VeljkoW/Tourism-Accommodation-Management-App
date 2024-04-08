using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourRepository : ITourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";
        public static TourRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourRepository>();
        }

        private readonly Serializer<Tour> _serializer;

        private List<Tour> _tours;

        public TourRepository()
        {
            _serializer = new Serializer<Tour>();
            _tours = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }
        public Tour Add(Tour newTour)
        {
            newTour.Id = NextId();
            _tours.Add(newTour);
            _serializer.ToCSV(FilePath, _tours);
            return newTour;
        }
        public List<Tour> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public Tour? GetById(int Id)
        {
            _tours = _serializer.FromCSV(FilePath);
            return _tours.Find(c => c.Id == Id);
        }
    }
}