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
    public class OwnerReportService
    {
        public IOwnerReportRepository OwnerReportRepository { get; set; }
        public OwnerReportService(IOwnerReportRepository ownerReportRepository) { OwnerReportRepository = ownerReportRepository; }
        public static OwnerReportService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerReportService>();
        }
        public void Add(OwnerReport OwnerReport)
        {
            OwnerReportRepository.Add(OwnerReport);
        }
        public List<OwnerReport> GetAll()
        {
            return OwnerReportRepository.GetAll();
        }
        public OwnerReport? GetById(int Id)
        {
            return OwnerReportRepository.GetById(Id);
        }
    }
}
