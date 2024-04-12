using System;
using System.Collections.Generic;
using System.ComponentModel;
using BookingApp.Serializer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class ProcessedReschedulingRequest : ISerializable, INotifyPropertyChanged
    {
        public int id { get; set; }
        public int accommodationId { get; set; }
        public int guestId { get; set; }
        public int commentId { get; set; }
        public bool isAccepted { get; set; }
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
        }
    }
}
