using BookingApp.Domain.Model;
using BookingApp.Domain.IRepositories;
using BookingApp.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    // IImageRepository za inverziju zavisnosti
    public class ImageService
    {
        public ImageService(IImageRepository imageRepository) { ImageRepository = imageRepository; }
        public IImageRepository ImageRepository { get;set; }
        public static ImageService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ImageService>();
        }

        public Image Add(Image image)
        {
            return ImageRepository.Add(image);
        }
        public List<Image> GetAll()
        {
            return ImageRepository.GetAll();
        }
        public Image? GetById(int id)
        {
            return ImageRepository.GetById(id);
        }
    }
}