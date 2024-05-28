using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class ForumRepository : IForumRepository
    {
        public static ForumRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ForumRepository>();
        }
        private const string FilePath = "../../../Resources/Data/forum.csv";

        private readonly Serializer<Forum> _serializer;

        private List<Forum> _forum;
        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _forum = _serializer.FromCSV(FilePath);
        }
        public void Add(Forum forum)
        {
            forum.Id = NextId();
            _forum.Add(forum);
            _serializer.ToCSV(FilePath, _forum);
        }
        public void Update(Forum forum) 
        {
            _forum = _serializer.FromCSV(FilePath);
            Forum? Forum = _forum.Find(c => c.LocationId == forum.LocationId);
            Forum.LocationId = forum.LocationId;
            Forum.GuestPosts = forum.GuestPosts;
            Forum.IsValid = forum.IsValid;
            _serializer.ToCSV(FilePath, _forum);
        }
        public List<Forum> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Forum? GetById(int Id)
        {
            _forum = _serializer.FromCSV(FilePath);
            return _forum.Find(c => c.Id == Id);
        }
        public int NextId()
        {
            _forum = _serializer.FromCSV(FilePath);
            if (_forum.Count < 1)
            {
                return 1;
            }
            return _forum.Max(c => c.Id) + 1;
        }

    }
}
