using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourImage : ISerializable
    {
        public int TourId { get; set; }
        public int ImageId { get; set; }
        public TourImage() { }
        public TourImage(int tourId, int imageId)
        {
            TourId = tourId;
            ImageId = imageId;
        }

        public string[] ToCSV()
        {
            string[] ret = { TourId.ToString(), ImageId.ToString() };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            TourId = Convert.ToInt32(values[0]);
            ImageId = Convert.ToInt32(values[1]);
        }
    }
}
