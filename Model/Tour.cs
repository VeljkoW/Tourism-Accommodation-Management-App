using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxTourists { get; set; }
        public List<String> KeyPoints { get; set; }
        public int Duration { get; set; }
        public List<String> ImagePaths { get; set; }
        public Tour(int id, string name, Location location, string description, string language, int maxTourists, List<string> keyPoints, int duration, List<string> imagePaths)
        {
            Id = id;
            Name = name;
            Location = location;
            Description = description;
            Language = language;
            MaxTourists = maxTourists;
            KeyPoints = keyPoints;
            Duration = duration;
            ImagePaths = imagePaths;
        }
        public Tour() { }

        public string[] ToCSV()
        {
            throw new NotImplementedException();
        }

        public void FromCSV(string[] values)
        {
            throw new NotImplementedException();
        }
    }
}