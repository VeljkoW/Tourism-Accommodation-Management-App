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
    public class ForumService
    {
        public IForumRepository ForumRepository { get; set; }
        public ForumService(IForumRepository forumRepository) { ForumRepository = forumRepository; }
        public static ForumService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ForumService>();
        }
        public void Add(Forum forum)
        {
            ForumRepository.Add(forum);
            OwnerNotification ownerNotification = new OwnerNotification();
            ownerNotification.ForumId = forum.Id;
            ownerNotification.Root = "Forum";
            OwnerNotificationService.GetInstance().Add(ownerNotification);
        }

        public List<Forum> GetAll()
        {
            return ForumRepository.GetAll();
        }

        public void Update(Forum forum)
        { 
            ForumRepository.Update(forum);
        }

        public Forum? GetById(int Id)
        {
            return ForumRepository.GetById(Id);
        }
        public bool isSpecial(int id)
        {
            Forum? forum = GetById(id);
            int ownerNumber = 0;
            int guestNumber = 0;
            ownerNumber = GetOwnerCommentNumber(forum);
            guestNumber = GetUserCommentNumber(forum);
            if(ownerNumber >= 1 && guestNumber >= 2)////////////////////////////////////////////////////////////////////////////////
                return true;
            else
                return false;
        }
        private int GetOwnerCommentNumber(Forum forum)
        {
            int ownerNumber = 0;
            foreach(GuestPost guestPost in forum.GuestPosts)
            {
                User? user = UserService.GetInstance().GetById(guestPost.UserId);
                if (user.UserType == UserType.Owner)
                    ownerNumber++;
            }
            return ownerNumber;
        }
        private int GetUserCommentNumber(Forum forum)
        {
            int guestNumber = 0;
            foreach (GuestPost guestPost in forum.GuestPosts)
            {
                User? user = UserService.GetInstance().GetById(guestPost.UserId);
                if (user.UserType == UserType.Guest)
                {
                    foreach(ReservedAccommodation reservedAccommodation in ReservedAccommodationService.GetInstance().GetAll())
                        if(forum.LocationId == reservedAccommodation.Accommodation.Location.Id && user.Id == reservedAccommodation.GuestId)
                            guestNumber++;
                }
            }
            return guestNumber;
        }
    }
}
