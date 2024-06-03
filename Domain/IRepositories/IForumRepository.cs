using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IForumRepository
    {
        List<Forum> GetAll();
        void Update(Forum forum);
        Forum? GetById(int Id);

        int NextId();

        void Add(Forum forum);
    }
}
