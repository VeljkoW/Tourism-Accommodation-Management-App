using System;
using System.Collections.Generic;
using System.ComponentModel;
using BookingApp.Serializer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Services;

namespace BookingApp.Domain.Model
{
    public class ProcessedReschedulingRequest : ISerializable, INotifyPropertyChanged
    {
        private int id { get; set; }
        private int accommodationId { get; set; }
        private int guestId { get; set; }
        private int commentId { get; set; }
        private bool isAccepted { get; set; }
        private DateTime checkInDate { get; set; }
        private DateTime checkOutDate { get; set; }
        public ProcessedReschedulingRequest() { }
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
        public int CommentId
        {
            get
            {
                return commentId;
            }
            set
            {
                if (value != commentId)
                {
                    commentId = value;
                    OnPropertyChanged(nameof(commentId));
                }
            }
        }
        public bool IsAccepted
        {
            get
            {
                return isAccepted;
            }
            set
            {
                if (value != isAccepted)
                {
                    isAccepted = value;
                    OnPropertyChanged(nameof(isAccepted));
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                CommentId.ToString(),
                isAccepted.ToString(),
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
            CommentId = Convert.ToInt32(values[3]);
            IsAccepted = Convert.ToBoolean(values[4]);
            CheckInDate = Convert.ToDateTime(values[5]);
            CheckOutDate = Convert.ToDateTime(values[6]);
        }

        public string PrintNotifications
        {
            get
            {
                if(isAccepted == true)
                {
                    string str = "Your request for a change of reservation\nhas been accepted.\n";
                    str += "Name: " + AccommodationService.GetInstance().GetById(accommodationId).Name + "\nCheck In: " + checkInDate.ToString() + "\nCheck Out: " + checkOutDate.ToString();
                    return str;
                }
                else
                {
                    string str = "The request to reschedule the reservation\nhas been declined. Please select new dates.\n";
                    str += "Name: " + AccommodationService.GetInstance().GetById(accommodationId).Name + "\nCheck In: " + checkInDate.ToString() + "\nCheck Out: " + checkOutDate.ToString();
                    return str;
                }
            }
            set
            {
                if (value != PrintNotifications)
                {
                    PrintNotifications = value;
                    OnPropertyChanged("PrintNotifications");
                }
            }
        }
    }
}
