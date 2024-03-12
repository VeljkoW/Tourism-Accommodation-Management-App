using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourPerson : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age {  get; set; }

        public TourPerson(int id, string name, string surname, int age)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
        }
        public TourPerson() 
        {
            Name = string.Empty;
            Surname = string.Empty;
            Age = 0;
        }

        public string[] ToCSV()
        {
            string[] ret = {
                Id.ToString(),
                Name,
                Surname,
                Age.ToString()
            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Surname = values[2];
            Age = Convert.ToInt32(values[3]);
        }
    }
}
