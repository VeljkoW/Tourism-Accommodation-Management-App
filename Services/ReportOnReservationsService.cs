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
    public class ReportOnReservationsService
    {
        public IReportOnReservationsRepository ReportOnReservationsRepository { get; set; }
        public ReportOnReservationsService(IReportOnReservationsRepository reportOnReservationsRepository) { ReportOnReservationsRepository = reportOnReservationsRepository; }
        public static ReportOnReservationsService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReportOnReservationsService>();
        }
        public void Add(ReportOnReservations reportOnReservations)
        {
            ReportOnReservationsRepository.Add(reportOnReservations);
        }

        public List<ReportOnReservations> GetAll()
        {
            return ReportOnReservationsRepository.GetAll();
        }

        public void Update(ReportOnReservations reportOnReservations)
        {
            ReportOnReservationsRepository.Update(reportOnReservations);
        }

    }
}
