using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.View.Guest.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ScheduledRenovationService
    {
        public IScheduledRenovationRepository ScheduledRenovationRepository { get; set; }
        public ScheduledRenovationService(IScheduledRenovationRepository scheduledRenovationRepository)
        {
            ScheduledRenovationRepository = scheduledRenovationRepository;
        }
        public static ScheduledRenovationService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ScheduledRenovationService>();
        }
        public void Add(ScheduledRenovation newScheduledRenovation)
        {
            ScheduledRenovationRepository.Add(newScheduledRenovation);
        }

        public List<ScheduledRenovation> GetAll()
        {
            return ScheduledRenovationRepository.GetAll();
        }

        public ScheduledRenovation? GetById(int Id)
        {
            return ScheduledRenovationRepository.GetById(Id);
        }
        public void UpdateUpcomingRenovations(User user, ObservableCollection<ScheduledRenovation> ScheduledRenovations)
        {
            ScheduledRenovations.Clear();
            foreach (ScheduledRenovation scheduledRenovation in GetAllByUser(user))
                if (scheduledRenovation.EndDate >= DateTime.Now)
                    ScheduledRenovations.Add(scheduledRenovation);
        }
        public void UpdatePreviousRenovations(User user, ObservableCollection<ScheduledRenovation> ScheduledRenovations)
        {
            foreach (ScheduledRenovation scheduledRenovation in GetAllByUser(user))
                if (scheduledRenovation.EndDate < DateTime.Now)
                    ScheduledRenovations.Add(scheduledRenovation);
        }

        public ObservableCollection<ScheduledRenovation> GetAllByUser(User user)
        {
            ObservableCollection<ScheduledRenovation> ScheduledRenovations = new ObservableCollection<ScheduledRenovation>();
            foreach(ScheduledRenovation scheduledRenovation in GetAll())
            {
                Accommodation? accommodation = AccommodationService.GetInstance().GetById(scheduledRenovation.AccommodationId);
                User? RenovationOwner = UserService.GetInstance().GetById(accommodation.OwnerId);
                if(user.Id == RenovationOwner.Id)
                    ScheduledRenovations.Add(scheduledRenovation);
            }
            return ScheduledRenovations;
        }
        public ObservableCollection<Accommodation> GetUnrenovatedAccommodations(ObservableCollection<Accommodation> allAccommodations)
        {
            ObservableCollection<Accommodation> UnrenovatedAccommodations = new ObservableCollection<Accommodation>();
            foreach (Accommodation accommodation in allAccommodations)
            {
                if (GetAll().Count == 0)
                {
                    UnrenovatedAccommodations.Add(accommodation);
                    continue;
                }
                bool alreadyRenovated = false;
                foreach (ScheduledRenovation scheduledRenovation in GetAll())
                {
                    if (scheduledRenovation.AccommodationId == accommodation.Id)
                    {
                        alreadyRenovated = true;
                        break;
                    }
                }
                if (!alreadyRenovated) UnrenovatedAccommodations.Add(accommodation);
            }
            return UnrenovatedAccommodations;
        }
    }
}
