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
        private ICommentRepository CommentRepository {get;set;}
        public CommentService(ICommentRepository commentRepository) { CommentRepository = commentRepository; }
        public static CommentService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<CommentService>();
        }
        public Comment Save(Comment newComment)
        {
           return CommentRepository.Save(newComment);
        }

        public List<Comment> GetAll()
        {
            return CommentRepository.GetAll();
        }

        public void Delete(Comment comment)
        {
            CommentRepository.Delete(comment);
        }
        public Comment Update(Comment newComment)
        {
            return CommentRepository.Update(newComment);
        }
        public List<Comment> GetByUser(User user)
        {
            return CommentRepository.GetByUser(user);
        }
        public Comment? GetById(int id)
        {
            return CommentRepository.GetById(id);
        }
    }
}
