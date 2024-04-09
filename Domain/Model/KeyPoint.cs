using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class KeyPoint : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string Point { get; set; }
        public bool IsVisited { get; set; }
        public KeyPoint() { Id = -1; TourId = -1; Point = ""; IsVisited = false; }
        public KeyPoint(int id, string point) { TourId = id; Point = point; IsVisited = false; }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), TourId.ToString(), Point };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId = Convert.ToInt32(values[1]);
            Point = values[2];
            IsVisited = false;
        }
    }
}
