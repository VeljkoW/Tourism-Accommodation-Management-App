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
    public class OwnerReportRepository : IOwnerReportRepository
    {
        public static OwnerReportRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerReportRepository>();
        }
        private const string FilePath = "../../../Resources/Data/ownerReports.csv";

        private readonly Serializer<OwnerReport> _serializer;

        private List<OwnerReport> _ownerReports;
        public OwnerReportRepository()
        {
            _serializer = new Serializer<OwnerReport>();
            _ownerReports = _serializer.FromCSV(FilePath);
        }
        public void Add(OwnerReport OwnerReport)
        {
            OwnerReport.Id = NextId();
            _ownerReports.Add(OwnerReport);
            _serializer.ToCSV(FilePath, _ownerReports);
        }
        public List<OwnerReport> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public OwnerReport? GetById(int Id)
        {
            _ownerReports = _serializer.FromCSV(FilePath);
            return _ownerReports.Find(c => c.Id == Id);
        }
        public int NextId()
        {
            _ownerReports = _serializer.FromCSV(FilePath);
            if (_ownerReports.Count < 1)
            {
                return 1;
            }
            return _ownerReports.Max(c => c.Id) + 1;
        }
    }
}
