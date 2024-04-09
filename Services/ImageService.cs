using BookingApp.Domain.Model;
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
        public ImageService(){}
        private ImageRepository imageRepository = ImageRepository.GetInstance();
        public static ImageService GetInstance()
        {
            return App._serviceProvider.GetRequiredService<ImageService>();
        }

        public Image Add(Image image)
        {
            return imageRepository.Add(image);
        }
        public List<Image> GetAll()
        {
            return imageRepository.GetAll();
        }
        public Image? GetById(int id)
        {
            return imageRepository.GetById(id);
        }
    }
}