using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Image:ISerializable
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public Image() { }
        public Image(int id, string path) {
        Id = id;
        Path = path;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {Id.ToString(), Path.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Path = Convert.ToString(values[1]);
        }
    }
}