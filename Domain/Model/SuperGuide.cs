using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class SuperGuide : ISerializable
    {
        public int id;
        public string language;
        public SuperGuide() { }
        public SuperGuide(int id, string language)
        {
            this.id = id;
            this.language = language;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                id.ToString(),
                language
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            id = Convert.ToInt32(values[0]);
            language = values[1];
        }
    }
}
