using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourStatistics
    {
        public int Underage { get; set; }
        public int Adult { get; set; }
        public int Elderly { get; set; }
        public int Visitors { get; set; }

        public TourStatistics(){}
        public TourStatistics(int underage, int adult, int elderly,int visitors=0)
        {
            Underage = underage;
            Adult = adult;
            Elderly = elderly;
            Visitors = Underage + Adult + Elderly;
        }
        public static TourStatistics operator +(TourStatistics a, TourStatistics b)
        {
            return new TourStatistics(
                a.Underage + b.Underage,
                a.Adult + b.Adult,
                a.Elderly + b.Elderly,
                a.Visitors + b.Visitors
            );
        }
    }
}
