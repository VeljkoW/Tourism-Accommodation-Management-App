using BookingApp.Domain.Model;
using System.Collections.Generic;

namespace BookingApp.Domain.IRepositories
{
    public interface IImageRepository
    {
        List<Image> GetAll();
        Image? GetById(int Id);
        int NextId();
        Image? Add(Image newImage);
    }
}