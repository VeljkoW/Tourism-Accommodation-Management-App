using System;
using System.Collections.Generic;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Location : ISerializable
    {
        public int Id {  get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public Location() {
            this.City = string.Empty;
            this.State = string.Empty;
        }
        public Location(string city, string state)
        {
            this.City = city;
            this.State = state;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), State, City };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            State = values[1];
            City = values[2];
        }
    }
}
