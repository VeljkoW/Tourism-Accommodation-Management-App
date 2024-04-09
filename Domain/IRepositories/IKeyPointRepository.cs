using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IKeyPointRepository
    {
        List<KeyPoint> GetAll();
        KeyPoint? GetById(int Id);
        int NextId();
        KeyPoint Add(KeyPoint newKeyPoint);
    }
}
