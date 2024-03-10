using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ImageRepository
    {
        private const string FilePath = "../../../Resources/Data/images.csv";

        private readonly Serializer<Image> _serializer;

        private List<Image> _images;
        public ImageRepository()
        {
            _serializer = new Serializer<Image>();
            _images = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _images = _serializer.FromCSV(FilePath);
            if (_images.Count < 1)
            {
                return 1;
            }
            return _images.Max(c => c.Id) + 1;
        }
        internal void Add(Image newImage)
        {
            newImage.Id = NextId();
            _images.Add(newImage);
            _serializer.ToCSV(FilePath, _images);
        }
        public List<Image> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public Image? GetById(int Id)
        {
            _images = _serializer.FromCSV(FilePath);
            return _images.Find(c => c.Id == Id);
        }
    }
}
