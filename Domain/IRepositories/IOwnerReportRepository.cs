using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IOwnerReportRepository
    {
        List<OwnerReport> GetAll();
        OwnerReport? GetById(int Id);
        int NextId();
        void Add(OwnerReport OwnerReport);
    }
}
