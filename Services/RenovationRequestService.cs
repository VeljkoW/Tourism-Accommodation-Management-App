using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class RenovationRequestService
    {
        public IRenovationRequestRepository RenovationRequestRepository { get; set; }
        public RenovationRequestService(IRenovationRequestRepository renovationRequestRepository) { RenovationRequestRepository = renovationRequestRepository; }
        public static RenovationRequestService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<RenovationRequestService>();
        }
        public void Add(RenovationRequest renovationRequest)
        {
            RenovationRequestRepository.Add(renovationRequest);
        }

        public List<RenovationRequest> GetAll()
        {
            return RenovationRequestRepository.GetAll();
        }

        public RenovationRequest? GetById(int Id)
        {
            return RenovationRequestRepository.GetById(Id);
        }
        public void RenovationRequestCountByYear(int accommodationId, ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            List<RenovationRequest> RenovationRequests = new List<RenovationRequest>();
            RenovationRequests = GetAll().Where(t => t.AccommodationId == accommodationId).ToList();
            foreach (RenovationRequest renovationRequest in RenovationRequests)
            {
                if (AccommodationStatisticsByYears.Count == 0)
                    AddAccommodationStatisticsByYear(renovationRequest, AccommodationStatisticsByYears);
                else
                {
                    bool alreadyExists = false;
                    foreach (AccommodationStatisticsByYear AccommodationStatisticsByYear in AccommodationStatisticsByYears)
                        if (AccommodationStatisticsByYear.Year == renovationRequest.RequestDate.Year)
                        {
                            AccommodationStatisticsByYear.RecommendedRenovations++;
                            alreadyExists = true;
                            break;
                        }
                    if (!alreadyExists)
                        AddAccommodationStatisticsByYear(renovationRequest, AccommodationStatisticsByYears);
                }
            }
        }
        private void AddAccommodationStatisticsByYear(RenovationRequest renovationRequest,
                                                      ObservableCollection<AccommodationStatisticsByYear> AccommodationStatisticsByYears)
        {
            AccommodationStatisticsByYear AccommodationStatisticsByYear = new AccommodationStatisticsByYear();
            AccommodationStatisticsByYear.AccommodationId = renovationRequest.AccommodationId;
            AccommodationStatisticsByYear.Year = renovationRequest.RequestDate.Year;
            AccommodationStatisticsByYear.RecommendedRenovations++;
            AccommodationStatisticsByYears.Add(AccommodationStatisticsByYear);
        }
        public void RenovationRequestCountByMonth(int year, int accommodationId, ObservableCollection<AccommodationStatisticsByMonth> AccommodationStatisticsByMonths)
        {
            List<RenovationRequest> RenovationRequests = new List<RenovationRequest>();
            RenovationRequests = GetAll().Where(t => t.AccommodationId == accommodationId && t.RequestDate.Year == year).ToList();
            foreach (RenovationRequest renovationRequest in RenovationRequests)
                foreach (AccommodationStatisticsByMonth AccommodationStatisticsByMonth in AccommodationStatisticsByMonths)
                    if (AccommodationStatisticsByMonth.Month == renovationRequest.RequestDate.Month)
                        AccommodationStatisticsByMonth.RecommendedRenovations++;
        }
    }
}
