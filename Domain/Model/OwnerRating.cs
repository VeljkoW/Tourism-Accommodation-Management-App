using System;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Repository;

namespace BookingApp.Domain.Model
{
    public class OwnerRating : INotifyPropertyChanged, ISerializable
    {
        public int ownerId { get; set; }
        public int guestId { get; set; }
        public int cleanliness { get; set; }
        public int ownerIntegrity { get; set; }
        public int commentId { get; set; }

        public List<Image> images { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }

        public OwnerRating()
        {
            images = new List<Image>();
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
        public int Cleanliness
        {
            get
            {
                return cleanliness;
            }
            set
            {
                if (value != cleanliness)
                {
                    cleanliness = value;
                    OnPropertyChanged(nameof(cleanliness));
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

        public int OwnerIntegrity
        {
            get
            {
                return ownerIntegrity;
            }
            set
            {
                if (value != ownerIntegrity)
                {
                    ownerIntegrity = value;
                    OnPropertyChanged(nameof(ownerIntegrity));
                }
            }
        }

        public List<Image> Images
        {
            get
            {
                return images;
            }
            set
            {
                if (value != images)
                {
                    images = value;
                    OnPropertyChanged(nameof(images));
                }
            }
        }
        public string ImagesIdToCSV()
        {
            string str = "";
            if (images.Count != 0)
            {
                foreach (Image i in Images)
                {
                    str += i.Id + ",";
                }
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                GuestId.ToString(),
                OwnerId.ToString(),
                Cleanliness.ToString(),
                OwnerIntegrity.ToString(),
                CommentId.ToString(),
                ImagesIdToCSV()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            GuestId = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Cleanliness = Convert.ToInt32(values[2]);
            OwnerIntegrity = Convert.ToInt32(values[3]);
            CommentId = Convert.ToInt32(values[4]);
            if (values[5].Length > 0)
            {
                string[] ImageIds = values[5].Split(',');
                for (int i = 0; i < ImageIds.Length; i++)
                {
                    Image image = new Image();
                    ImageRepository imageRepository = new ImageRepository();
                    image = imageRepository.GetById(Convert.ToInt32(ImageIds[i]));
                    Images.Add(image);
                }
            }
        }

    }
}
