using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository.AccommodationRepositories;

namespace BookingApp.Services
{
    public class UserService
    {
        public IUserRepository UserRepository {get;set;}
        public UserService(IUserRepository userRepository) {
            UserRepository = userRepository;
            }
        public static UserService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserService>();
        }
        public void Add(User newUser)
        {
            UserRepository.Add(newUser);
        }

        public User? GetById(int Id)
        {
            return UserRepository.GetById(Id);
        }
        public User GetByUsername(string username)
        {
            return UserRepository.GetByUsername(username);
        }
        public List<User> GetAll()
        {
            return UserRepository.GetAll();
        }
    }
}
