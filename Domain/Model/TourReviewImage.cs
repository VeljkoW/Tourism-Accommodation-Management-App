using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourReviewImage : ISerializable
    {
        public int TourReviewId { get; set; }
        public int ImageId { get; set; }
        public TourReviewImage() { }
        public TourReviewImage(int tourReviewId, int imageId)
        {
            TourReviewId = tourReviewId;
            ImageId = imageId;
        }

        public string[] ToCSV()
        {
            string[] ret = { TourReviewId.ToString(), ImageId.ToString() };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            TourReviewId = Convert.ToInt32(values[0]);
            ImageId = Convert.ToInt32(values[1]);
        }
    }
}
