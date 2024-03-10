using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class KeyPoint : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public string Point {  get; set; }
        public KeyPoint() { }
        public KeyPoint(int id, string point) {  Id = id; Point = point; }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Point};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Point = values[1];
        }
    }
}
