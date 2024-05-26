using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class GuestPostService
    {
        public IGuestPostRepository GuestPostRepository { get; set; }
        public GuestPostService(IGuestPostRepository guestPostRepository) { GuestPostRepository = guestPostRepository; }
        public static GuestPostService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestPostService>();
        }
        public void Add(GuestPost guestPost)
        {
            GuestPostRepository.Add(guestPost);
        }

        public List<GuestPost> GetAll()
        {
            return GuestPostRepository.GetAll();
        }

        public GuestPost? GetById(int Id)
        {
            return GuestPostRepository.GetById(Id);
        }
        public void Update(GuestPost guestPost)
        {
            GuestPostRepository.Update(guestPost);
        }
    }
}
