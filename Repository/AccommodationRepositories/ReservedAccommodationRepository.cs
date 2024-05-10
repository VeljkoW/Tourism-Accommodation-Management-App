using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using BookingApp.View.Guest.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository.AccommodationRepositories
{
    public class ReservedAccommodationRepository : IReservedAccommodationRepository
    {
        public static ReservedAccommodationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservedAccommodationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/reservedAccommodation.csv";

        private readonly Serializer<ReservedAccommodation> _serializer;

        private List<ReservedAccommodation> _reservedAccommodations;

        public ReservedAccommodationRepository()
        {
            _serializer = new Serializer<ReservedAccommodation>();
            _reservedAccommodations = _serializer.FromCSV(FilePath);
        }

        public int NextId()
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            if (_reservedAccommodations.Count < 1)
            {
                return 1;
            }
            return _reservedAccommodations.Max(c => c.Id) + 1;
        }
        public void Add(ReservedAccommodation newReservedAccommodation)
        {
            newReservedAccommodation.Id = NextId();
            _reservedAccommodations.Add(newReservedAccommodation);
            _serializer.ToCSV(FilePath, _reservedAccommodations);
        }
        public List<ReservedAccommodation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public List<ReservedAccommodation>? GetByAccommodationId(int Id)
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            return _reservedAccommodations.Where(c => c.Accommodation.Id == Id).ToList() ?? null;
        }
        public ReservedAccommodation? GetById(int Id)
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            return _reservedAccommodations.Find(c => c.Id == Id);
        }

        public List<ReservedAccommodation> GetByGuestId(int Id)
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            return _reservedAccommodations.FindAll(c => c.GuestId == Id);
        }

        public void Delete(ReservedAccommodation reservedAccommodation)
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            _reservedAccommodations.RemoveAll(c => c.Id == reservedAccommodation.Id);
            _serializer.ToCSV(FilePath, _reservedAccommodations);
        }

        public void UpdateDatesByReschedulingRequest(GuestReschedulingRequest GuestReschedulingRequest)
        {
            _reservedAccommodations = _serializer.FromCSV(FilePath);
            ReservedAccommodation? reservedAccommodation = _reservedAccommodations.Find(c => c.Id == GuestReschedulingRequest.ReservedAccommodationId);
            reservedAccommodation.CheckInDate = GuestReschedulingRequest.CheckInDate;
            reservedAccommodation.CheckOutDate = GuestReschedulingRequest.CheckOutDate;
            _serializer.ToCSV(FilePath, _reservedAccommodations);
        }
    }
}
