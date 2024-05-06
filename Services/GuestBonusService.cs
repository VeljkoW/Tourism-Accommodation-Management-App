using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
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
        public void Update(GuestBonus guestBonus)
        {
            GuestBonusRepository.Update(guestBonus);
        }

        public void UpdateAll()
        {
            List<User> users = UserService.GetInstance().GetAll().Where(t => t.UserType == UserType.Guest).ToList();
            foreach(User user in users)
            {
                bool alreadyExist = false;
                foreach (GuestBonus guestBonus in GetAll())
                {
                    if(user.Id == guestBonus.GuestId)
                    {
                        alreadyExist = true;
                        Bonus(guestBonus, user);
                        Update(guestBonus);
                    }
                }
                if(!alreadyExist)
                {
                    GuestBonus guest = new GuestBonus();
                    guest.GuestId = user.Id;
                    guest.Bonus = 0;
                    guest.IsSuperGuest = false;
                    Add(guest);
                    if(IsSuperGuest(user))
                    {
                        guest.StartSuperGuest = DateTime.Now;
                        guest.IsSuperGuest = true;
                        guest.Bonus += 5;
                        Update(guest);
                    }
                }
            }
        }

        public void Bonus(GuestBonus guestBonus, User user)
        {
            if (IsSuperGuest(user))
                if (guestBonus.StartSuperGuest > DateTime.Now.AddYears(-1))
                    guestBonus.IsSuperGuest = true;
                else
                {
                    guestBonus.StartSuperGuest = DateTime.Now;
                    guestBonus.IsSuperGuest = true;
                    guestBonus.Bonus += 5;
                }
            else
            {
                guestBonus.StartSuperGuest = DateTime.MinValue;
                guestBonus.IsSuperGuest = false;
                guestBonus.Bonus = 0;
            }
        }

        public bool IsSuperGuest(User user)
        {
            int numberOfReservation = 0;
            foreach (ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
            {
                if (user.Id == reservedAccommodation.GuestId && DateTime.Now > reservedAccommodation.checkInDate && DateTime.Now.AddYears(-1) < reservedAccommodation.checkInDate)
                    numberOfReservation++;
            }

            if (numberOfReservation >= 10)
                return true;

            return false;
        }

        public int GetBonus(User user)
        {
            int numberOfBonus = 0;
            foreach (GuestBonus guestBonus in GuestBonusService.GetInstance().GetAll())
            {
                if (guestBonus.GuestId == user.Id)
                {
                    numberOfBonus = guestBonus.Bonus;
                }
            }
            return numberOfBonus;
        }
    }
}
