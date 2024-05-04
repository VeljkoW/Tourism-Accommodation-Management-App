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
    public class ReservationCancellationService
    {
        public IReservationCancellationRepository ReservationCancellationRepository { get; set; }
        public ReservationCancellationService(IReservationCancellationRepository reservationCancellationRepository) { ReservationCancellationRepository = reservationCancellationRepository; }
        public static ReservationCancellationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ReservationCancellationService>();
        }
        public void Add(ReservationCancellation reservationCancellation)
        {
            ReservationCancellationRepository.Add(reservationCancellation);
        }

        public List<ReservationCancellation> GetAll()
        {
            return ReservationCancellationRepository.GetAll();
        }
    }
}
