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
        public ITourPersonRepository TourPersonRepository {get;set;}
        public TourPersonService(ITourPersonRepository tourPersonRepository) {
            TourPersonRepository=tourPersonRepository;
        }
        public static TourPersonService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourPersonService>();
        }
        public TourPerson Add(TourPerson newTourPerson)
        {
            return TourPersonRepository.Add(newTourPerson);
        }
        public int NextId()
        {
            return TourPersonRepository.NextId();
        }

        public TourPerson? GetById(int Id)
        {
            return TourPersonRepository.GetById(Id);
        }
        public List<TourPerson> GetAll()
        {
            return TourPersonRepository.GetAll();
        }
        public TourPerson? Update(TourPerson tourPerson)
        {
            return TourPersonRepository.Update(tourPerson);
        }
        public void DeleteById(int Id)
        {
            TourPersonRepository.DeleteById(Id);
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
