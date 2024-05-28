using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class OwnerReport : ISerializable, INotifyPropertyChanged
    {
        private int id {  get; set; }
        private int ownerId { get; set; }
        private int postId {  get; set; }
        public OwnerReport() { }
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
        public int OwnerId
        {
            get
            {
                return ownerId;
            }
            set
            {
                if (value != ownerId)
                {
                    ownerId = value;
                    OnPropertyChanged(nameof(ownerId));
                }
            }
        }
        public int PostId
        {
            get
            {
                return postId;
            }
            set
            {
                if (value != postId)
                {
                    postId = value;
                    OnPropertyChanged(nameof(postId));
                }
            }
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                    Id.ToString(),
                    OwnerId.ToString(),
                    PostId.ToString()
                };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            PostId = Convert.ToInt32(values[2]);
        }
    }
}
