using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Guest.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class GuestBonusRepository : IGuestBonus
    {
        public static GuestBonusRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestBonusRepository>();
        }
        private const string FilePath = "../../../Resources/Data/bonus.csv";

        private readonly Serializer<GuestBonus> _serializer;

        private List<GuestBonus> _guestBonus;
        public GuestBonusRepository()
        {
            _serializer = new Serializer<GuestBonus>();
            _guestBonus = _serializer.FromCSV(FilePath);
        }
        public void Add(GuestBonus guestBonus)
        {
            guestBonus.Id = NextId();
            _guestBonus.Add(guestBonus);
            _serializer.ToCSV(FilePath, _guestBonus);
        }

        public List<GuestBonus> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public GuestBonus? GetById(int Id)
        {
            _guestBonus = _serializer.FromCSV(FilePath);
            return _guestBonus.Find(c => c.Id == Id);
        }

        public int NextId()
        {
            _guestBonus = _serializer.FromCSV(FilePath);
            if (_guestBonus.Count < 1)
            {
                return 1;
            }
            return _guestBonus.Max(c => c.Id) + 1;
        }
    }
}
