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

namespace BookingApp.Services
{
    public class UserService
    {
        private IUserRepository userRepository = UserRepository.GetInstance();
        public UserService() { }
        public static UserService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<UserService>();
        }
        public void Add(User newUser)
        {
            userRepository.Add(newUser);
        }

        public User? GetById(int Id)
        {
            return userRepository.GetById(Id);
        }
        public User GetByUsername(string username)
        {
            return userRepository.GetByUsername(username);
        }
    }
}
