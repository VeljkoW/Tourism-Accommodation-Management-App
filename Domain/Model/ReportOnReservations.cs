using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BookingApp.Serializer;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Domain.Model
{
    public class ReportOnReservations : INotifyPropertyChanged, ISerializable
    {
        private int id {  get; set; }

        private int guestId { get; set; }
        private string typeReport { get; set; }

        private DateTime date { get; set; }

        private int accommodationId { get; set; }

        private int reservedId { get; set; }
        private DateTime checkInDate {  get; set; }
        private DateTime checkOutDate { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged(nameof(id));
                }
            }
        }
        public DateTime CheckInDate
        {
            get
            {
                return checkInDate;
            }
            set
            {
                if (value != checkInDate)
                {
                    checkInDate = value;
                    OnPropertyChanged(nameof(checkInDate));
                }
            }
        }
        public DateTime CheckOutDate
        {
            get
            {
                return checkOutDate;
            }
            set
            {
                if (value != checkOutDate)
                {
                    checkOutDate = value;
                    OnPropertyChanged(nameof(checkOutDate));
                }
            }
        }
        public int AccommodationId
        {
            get
            {
                return accommodationId;
            }
            set
            {
                if (value != accommodationId)
                {
                    accommodationId = value;
                    OnPropertyChanged(nameof(accommodationId));
                }
            }
        }
        public int ReservedId
        {
            get
            {
                return reservedId;
            }
            set
            {
                if (value != reservedId)
                {
                    reservedId = value;
                    OnPropertyChanged(nameof(reservedId));
                }
            }
        }
        public int GuestId
        {
            get
            {
                return guestId;
            }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged(nameof(guestId));
                }
            }
        }
        public string TypeReport
        {
            get
            {
                return typeReport;
            }
            set
            {
                if (value != typeReport)
                {
                    typeReport = value;
                    OnPropertyChanged(nameof(typeReport));
                }
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                if (value != date)
                {
                    date = value;
                    OnPropertyChanged(nameof(date));
                }
            }
        }
        
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                Date.ToString(),
                TypeReport.ToString(),
                ReservedId.ToString(),
                CheckInDate.ToString(),
                CheckOutDate.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            GuestId = Convert.ToInt32(values[2]);
            Date = Convert.ToDateTime(values[3]);
            TypeReport = values[4];
            ReservedId = Convert.ToInt32(values[5]);
            CheckInDate = Convert.ToDateTime(values[6]);
            CheckOutDate = Convert.ToDateTime(values[7]);
        }
    }
}
