using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class UserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _users = _serializer.FromCSV(FilePath);
            if (_users.Count < 1)
            {
                return 1;
            }
            return _users.Max(c => c.Id) + 1;
        }

        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public User GetById(int Id)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Id == Id);
        }

        public void Add(User newUser)
        {
            newUser.Id = NextId();
            _users.Add(newUser);
            _serializer.ToCSV(FilePath,_users);
        }
    }
}
