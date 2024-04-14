using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAll();
        Owner? GetById(int Id);
        void Add(Owner newOwner);
        void Update(int Id, bool IsSuperOwner);
    }
}
