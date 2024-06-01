using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class ReportOnReservationsRepository : IReportOnReservationsRepository
    {
        public static ReportOnReservationsRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReportOnReservationsRepository>();
        }
        private const string FilePath = "../../../Resources/Data/report.csv";

        private readonly Serializer<ReportOnReservations> _serializer;

        private List<ReportOnReservations> _report;
        public ReportOnReservationsRepository()
        {
            _serializer = new Serializer<ReportOnReservations>();
            _report = _serializer.FromCSV(FilePath);
        }
        public void Add(ReportOnReservations reportOnReservations)
        {
            reportOnReservations.Id = NextId();
            _report.Add(reportOnReservations);
            _serializer.ToCSV(FilePath, _report);
        }
        public void Update(ReportOnReservations reportOnReservations)
        {
            _report = _serializer.FromCSV(FilePath);
            ReportOnReservations? ReportOnReservations = _report.Find(c => c.Id == reportOnReservations.Id);
            ReportOnReservations.AccommodationId = reportOnReservations.AccommodationId;
            ReportOnReservations.TypeReport = reportOnReservations.TypeReport;
            ReportOnReservations.Date = reportOnReservations.Date;
            ReportOnReservations.GuestId = reportOnReservations.GuestId;
            ReportOnReservations.ReservedId = reportOnReservations.ReservedId;
            _serializer.ToCSV(FilePath, _report);
        }
        public List<ReportOnReservations> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public ReportOnReservations? GetById(int Id)
        {
            _report = _serializer.FromCSV(FilePath);
            return _report.Find(c => c.Id == Id);
        }
        public int NextId()
        {
            _report = _serializer.FromCSV(FilePath);
            if (_report.Count < 1)
            {
                return 1;
            }
            return _report.Max(c => c.Id) + 1;
        }
    }
}
