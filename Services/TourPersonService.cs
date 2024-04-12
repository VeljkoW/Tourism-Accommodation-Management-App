using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourPersonService
    {
        private ITourPersonRepository tourPersonRepository = TourPersonRepository.GetInstance();
        public TourPersonService() { }
        public static TourPersonService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourPersonService>();
        }
        public TourPerson Add(TourPerson newTourPerson)
        {
            return tourPersonRepository.Add(newTourPerson);
        }
        public int NextId()
        {
            return tourPersonRepository.NextId();
        }

        public TourPerson? GetById(int Id)
        {
            return tourPersonRepository.GetById(Id);
        }
        public List<TourPerson> GetAll()
        {
            return tourPersonRepository.GetAll();
        }
        public TourPerson? Update(TourPerson tourPerson)
        {
            return tourPersonRepository.Update(tourPerson);
        }
        public int GetUnderageCount(List<TourPerson> tourPersons)
        {
            return tourPersons.Where(person => person.Age < 18).Count();
        }
        public int GetAdultCount(List<TourPerson> tourPersons)
        {
            return tourPersons.Where(person => person.Age > 18 && person.Age < 50).Count();
        }
        public int GetElderlyCount(List<TourPerson> tourPersons)
        {
            return tourPersons.Where(person => person.Age > 50).Count();
        }
    }
}
