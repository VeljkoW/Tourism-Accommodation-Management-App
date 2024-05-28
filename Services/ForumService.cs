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
    }
}
