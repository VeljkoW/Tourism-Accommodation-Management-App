﻿using BookingApp.Repository;
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
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxTourists { get; set; }
        public List<KeyPoint> KeyPoints { get; set; }
        public int Duration { get; set; }
        public List<Image> Images { get; set; }
        public DateTime DateTime { get; set; }
         public Tour(int id, string name, Location location, string description, string language, int maxTourists, List<KeyPoint> keyPoints, int duration, List<Image> imagePaths,DateTime dateTime=new DateTime())
        {
            Id = id;
            Name = name;
            Location = location;
            LocationId = location.Id;
            Description = description;
            Language = language;
            MaxTourists = maxTourists;
            KeyPoints = keyPoints;
            Duration = duration;
            Images = imagePaths;
            DateTime = dateTime;
        }
        public Tour() { }
        public string[] ToCSV()
        {
            string[] ret = {Id.ToString(),Name,LocationId.ToString(),Description,Language,MaxTourists.ToString(),Duration.ToString()};
            return ret;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = Convert.ToString(values[1]);
            LocationId = Convert.ToInt32(values[2]);
            Description = Convert.ToString(values[3]);
            Language = Convert.ToString(values[4]);
            MaxTourists = Convert.ToInt32(values[5]);
            Duration = Convert.ToInt32(values[6]);
        }
    }
}