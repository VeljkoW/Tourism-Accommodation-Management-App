using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IReportOnReservationsRepository
    {
        List<ReportOnReservations> GetAll();
        void Update(ReportOnReservations reportOnReservations);
        ReportOnReservations? GetById(int Id);

        int NextId();

        void Add(ReportOnReservations reportOnReservations);
    }
}
