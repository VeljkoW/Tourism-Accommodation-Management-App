using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.TourRepositories
{
    public class TourPersonRepository
    {
        private const string FilePath = "../../../Resources/Data/tourpeople.csv";

        private readonly Serializer<TourPerson> _serializer;

        private List<TourPerson> _tourPeople;

        public TourPersonRepository()
        {
            _serializer = new Serializer<TourPerson>();
            _tourPeople = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _tourPeople = _serializer.FromCSV(FilePath);
            if (_tourPeople.Count < 1)
            {
                return 1;
            }
            return _tourPeople.Max(c => c.Id) + 1;
        }
        public TourPerson Add(TourPerson newTourPerson)
        {
            newTourPerson.Id = NextId();
            _tourPeople.Add(newTourPerson);
            _serializer.ToCSV(FilePath, _tourPeople);
            return newTourPerson;
        }
        public List<TourPerson> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public TourPerson? GetById(int Id)
        {
            _tourPeople = _serializer.FromCSV(FilePath);
            return _tourPeople.Find(c => c.Id == Id);
        }
    }
}
