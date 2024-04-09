using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
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
        private TourPersonRepository tourPersonRepository = TourPersonRepository.GetInstance();
        public TourPersonService() { }
        public TourPersonService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<TourPersonService>();
        }
        public TourPerson Add(TourPerson newTourPerson)
        {
            return tourPersonRepository.Add(newTourPerson);
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
    }
}
