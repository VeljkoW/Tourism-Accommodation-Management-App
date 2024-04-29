using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IGuestBonus
    {
        List<GuestBonus> GetAll();

        GuestBonus? GetById(int Id);

        int NextId();

        void Add(GuestBonus guestBonus);
    }
}
