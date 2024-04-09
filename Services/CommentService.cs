using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository.TourRepositories;
using BookingApp.Serializer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Services
{
    public class CommentService
    {
        private ICommentRepository commentRepository = CommentRepository.GetInstance();
        public CommentService() { }
        public static CommentService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<CommentService>();
        }
        public Comment Save(Comment newComment)
        {
           return commentRepository.Save(newComment);
        }

        public List<Comment> GetAll()
        {
            return commentRepository.GetAll();
        }

        public void Delete(Comment comment)
        {
            commentRepository.Delete(comment);
        }
        public Comment Update(Comment newComment)
        {
            return commentRepository.Update(newComment);
        }
        public List<Comment> GetByUser(User user)
        {
            return commentRepository.GetByUser(user);
        }
    }
}
