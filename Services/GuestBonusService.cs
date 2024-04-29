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
    public class GuestBonusService
    {
        public IGuestBonus GuestBonusRepository { get; set; }
        public GuestBonusService(IGuestBonus guestBonusRepository) { GuestBonusRepository = guestBonusRepository; }
        public static GuestBonusService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<GuestBonusService>();
        }
        public void Add(GuestBonus guestBonus)
        {
            GuestBonusRepository.Add(guestBonus);
        }

        public List<GuestBonus> GetAll()
        {
            return GuestBonusRepository.GetAll();
        }

        public GuestBonus? GetById(int Id)
        {
            return GuestBonusRepository.GetById(Id);
        }
    }
}
