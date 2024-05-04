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
    public class ReservationCancellationRepository : IReservationCancellationRepository
    {
        public static ReservationCancellationRepository GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservationCancellationRepository>();
        }
        private const string FilePath = "../../../Resources/Data/reservationCancellation.csv";

        private readonly Serializer<ReservationCancellation> _serializer;

        private List<ReservationCancellation> _reservationCancellations;
        public ReservationCancellationRepository()
        {
            _serializer = new Serializer<ReservationCancellation>();
            _reservationCancellations = _serializer.FromCSV(FilePath);
        }
        public void Add(ReservationCancellation reservationCancellation)
        {
            _reservationCancellations.Add(reservationCancellation);
            _serializer.ToCSV(FilePath, _reservationCancellations);
        }
        public List<ReservationCancellation> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
    }
}
