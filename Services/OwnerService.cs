using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.AccommodationRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class OwnerService
    {
        public IOwnerRepository OwnerRepository { get; set; }
        public OwnerService(IOwnerRepository ownerRepository) { OwnerRepository = ownerRepository; }
        public static OwnerService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<OwnerService>();
        }

        public void Add(Owner newOwner)
        {
            OwnerRepository.Add(newOwner);
        }

        public List<Owner> GetAll()
        {
            return OwnerRepository.GetAll();
        }

        public Owner? GetById(int Id)
        {
            return OwnerRepository.GetById(Id);
        }
        public void Update(int Id, bool IsSuperOwner)
        {
            OwnerRepository.Update(Id, IsSuperOwner);
        }
        public void UpdateAll()
        {
            List<User> owners = new List<User>();
            owners = UserService.GetInstance().GetAll().Where(t=>t.UserType == UserType.Owner).ToList();
            foreach (var tempOwner in owners)
            {
                List<OwnerRating> ratings = OwnerRatingService.GetInstance().GetOwnerRatings(tempOwner.Id).ToList();
                if (ratings.Count() >= 2) ////////////////////////////////////////////////IMPORTANT FOR TESTING///////////////////////////////////////////////
                {
                    double AverageGrade = GetAverageGrade(tempOwner.Id);
                    if (AverageGrade >= 4.5)
                    {
                        Update(tempOwner.Id, true);
                        continue;
                    }
                }
                Update(tempOwner.Id, false);
            }
        }
        public double GetAverageGrade(int ownerId)
        {
            List<OwnerRating> ratings = OwnerRatingService.GetInstance().GetOwnerRatings(ownerId).ToList();
            double AverageGrade = 0;
            foreach (OwnerRating ownerRating in ratings)
            {
                AverageGrade += (double)(ownerRating.Cleanliness + ownerRating.OwnerIntegrity) / 2;
            }
            AverageGrade /= ratings.Count();
            return AverageGrade;
        }
        public bool isSuperOwner(int Id)
        {
            return GetById(Id)?.IsSuperOwner ?? false;
        }
    }
}
