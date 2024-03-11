using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public enum AccommodationType { Apartment, House, Hut }
namespace BookingApp.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location? Location { get; set; }
        public AccommodationType AccommodationType {  get; set; }
        
        public int MaxGuestNumber {  get; set; }
        //public int GuestNumber { get; set; }
        public int MinReservationDays {  get; set; }
        //public int ReservationDays { get; set; }
        public int CancelationDaysLimit {  get; set; }
        public List<Image> Images { get; set; }

        public Accommodation()
        {
            this.Name = string.Empty;
            this.Location = new Location();
            this.AccommodationType = AccommodationType.Apartment;
            this.MaxGuestNumber = 0;
            //this.GuestNumber = 0;
            this.MinReservationDays = 0;
            //this.ReservationDays = 0;
            this.CancelationDaysLimit = 0;
            this.Images = new List<Image>();
        }
        public Accommodation(string Name, Location Location, AccommodationType AccommodationType,
                            int MaxGuestNumber, int MinReservationDays, int CancelationDaysLimit, List<Image> Images)
        {
            this.Name = Name;
            this.Location = Location;
            this.AccommodationType = AccommodationType;
            this.MaxGuestNumber = MaxGuestNumber;
            this.MinReservationDays = MinReservationDays;
            this.CancelationDaysLimit = CancelationDaysLimit;
            this.Images = Images;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { 
                Id.ToString(),
                Name, 
                Location.Id.ToString(), 
                AccommodationType.ToString(), 
                MaxGuestNumber.ToString(), 
                MinReservationDays.ToString(), 
                CancelationDaysLimit.ToString(), 
                ImagesIdToCSV() 
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LocationRepository LocationRepository = new LocationRepository();
            Location = LocationRepository.GetById(Convert.ToInt32(values[2]));
            AccommodationType = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[3]);
            MaxGuestNumber = Convert.ToInt32(values[4]);
            MinReservationDays = Convert.ToInt32(values[5]);
            CancelationDaysLimit = Convert.ToInt32(values[6]);
            if (values[7].Length > 0)
            {
                string[] ImageIds = values[6].Split(',');
                for (int i = 0; i < ImageIds.Length; i++)
                {
                    Image image = new Image();
                    ImageRepository imageRepository = new ImageRepository();
                    image = imageRepository.GetById(Convert.ToInt32(ImageIds[i]));
                    Images.Add(image);
                }
            }
        }
        public string ImagesIdToCSV()
        {
            string str = "";
            foreach (Image i in Images)
            {
                str += i.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
    }
}
