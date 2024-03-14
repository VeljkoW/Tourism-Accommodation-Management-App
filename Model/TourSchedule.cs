﻿using BookingApp.Model;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Model
{
    public class TourSchedule : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime Date { get; set; }
        public int Guests {  get; set; }
        // status ture ready started finished
        public TourSchedule() { }
        public TourSchedule(Tour tour, DateTime dateTime)
        {
            Id = 999;
            TourId = tour.Id;
            Date=dateTime;
        }

        public string[] ToCSV()
        {
            string[] ret = { Id.ToString(), TourId.ToString(), Date.ToString(),Guests.ToString()};
            return ret;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId= Convert.ToInt32(values[1]);
            Date = Convert.ToDateTime(values[2]);
            Guests = Convert.ToInt32(values[3]);
        }
    }
}