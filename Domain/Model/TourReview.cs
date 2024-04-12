using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Domain.Model
{
    public class TourReview : ISerializable
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int TourScheduleId {  get; set; }
        public int GuideKnowledge {  get; set; }
        public int GuideSpeech {  get; set; }
        public int TourEnjoyment {  get; set; }
        public string Comment {  get; set; }
        public ReviewStatus Status { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();

        public TourReview()
        {
            Id = -1;
            UserId = -1;
            TourScheduleId = -1;
            GuideKnowledge = 0;
            GuideSpeech = 0;
            TourEnjoyment = 0;
            Comment = String.Empty;
            Status = ReviewStatus.Invalid;
        }
        public TourReview(int userId,int tourScheduleId,int guideKnowledge,int guideSpeech,int tourEnjoyment,string comment,ReviewStatus reviewStatus)
        {
            UserId = userId;
            GuideKnowledge = guideKnowledge;
            TourScheduleId = tourScheduleId;
            GuideSpeech = guideSpeech;
            TourEnjoyment= tourEnjoyment;
            Comment = comment;
            Status = reviewStatus;
        }
        public string[] ToCSV()
        {
            string[] ret = {Id.ToString(),
                            UserId.ToString(),
                            TourScheduleId.ToString(),
                            GuideKnowledge.ToString(),
                            GuideSpeech.ToString(),
                            TourEnjoyment.ToString(),
                            Comment,
                            Status.ToString()
                           };
            return ret;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            TourScheduleId = Convert.ToInt32(values[2]);
            GuideKnowledge = Convert.ToInt32(values[3]);
            GuideSpeech= Convert.ToInt32(values[4]);
            TourEnjoyment = Convert.ToInt32(values[5]);
            Comment = values[6];
            Status = (ReviewStatus)Enum.Parse(typeof(ReviewStatus), values[7]);
        }
    }
}
