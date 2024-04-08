using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourPerson : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int KeyPointId { get; set; }

        public TourPerson(int id, string name, string surname, int age, int keyPointId)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Age = age;
            KeyPointId = keyPointId;
        }
        public TourPerson()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Age = 0;
            KeyPointId = -1;
        }

        public string[] ToCSV()
        {
            string[] ret = {
                Id.ToString(),
                Name,
                Surname,
                Age.ToString(),
                KeyPointId.ToString(),
            };
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Surname = values[2];
            Age = Convert.ToInt32(values[3]);
            KeyPointId = Convert.ToInt32(values[4]);
        }
    }
}
