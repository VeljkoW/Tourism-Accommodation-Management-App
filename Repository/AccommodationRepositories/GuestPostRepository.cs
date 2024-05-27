using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Owner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class GuestPostRepository : IGuestPostRepository
    {
        public static GuestPostRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestPostRepository>();
        }
        private const string FilePath = "../../../Resources/Data/guestPosts.csv";

        private readonly Serializer<GuestPost> _serializer;

        private List<GuestPost> _guestPost;
        public GuestPostRepository()
        {
            _serializer = new Serializer<GuestPost>();
            _guestPost = _serializer.FromCSV(FilePath);
        }
        public void Add(GuestPost guestPost)
        {
            guestPost.Id = NextId();
            _guestPost.Add(guestPost);
            _serializer.ToCSV(FilePath, _guestPost);
        }

        public List<GuestPost> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestPost? GetById(int Id)
        {
            _guestPost = _serializer.FromCSV(FilePath);
            return _guestPost.Find(c => c.Id == Id);
        }
        public int NextId()
        {
            _guestPost = _serializer.FromCSV(FilePath);
            if (_guestPost.Count < 1)
            {
                return 1;
            }
            return _guestPost.Max(c => c.Id) + 1;
        }

        public void Update(GuestPost guestPost)
        {
            _guestPost = _serializer.FromCSV(FilePath);
            GuestPost? GuestPost = _guestPost.Find(c => c.Id == guestPost.Id);
            GuestPost.UserId = guestPost.UserId;
            GuestPost.Comment = guestPost.Comment;
            GuestPost.SpecialUser = guestPost.SpecialUser;
            _serializer.ToCSV(FilePath, _guestPost);
        }
    }
}
