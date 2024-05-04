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
    public class AcceptedReservationReschedulingRepository : IAcceptedReservationReschedulingRepository
    {
        public static AcceptedReservationReschedulingRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<AcceptedReservationReschedulingRepository>();
        }
        private const string FilePath = "../../../Resources/Data/acceptedReservationRescheduling.csv";

        private readonly Serializer<AcceptedReservationRescheduling> _serializer;

        private List<AcceptedReservationRescheduling> _acceptedReservationReschedulings;
        public AcceptedReservationReschedulingRepository()
        {
            _serializer = new Serializer<AcceptedReservationRescheduling>();
            _acceptedReservationReschedulings = _serializer.FromCSV(FilePath);
        }
        public void Add(AcceptedReservationRescheduling acceptedReservationRescheduling)
        {
            _acceptedReservationReschedulings.Add(acceptedReservationRescheduling);
            _serializer.ToCSV(FilePath, _acceptedReservationReschedulings);
        }
        public List<AcceptedReservationRescheduling> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
