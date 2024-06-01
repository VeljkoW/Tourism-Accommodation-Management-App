using BookingApp.Serializer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourComplexSuggestion : ISerializable
    {
        public int Id {  get; set; }
        public int UserId { get; set; }
        public List<TourSuggestion> TourSuggestions { get; set; }
        public TourSuggestionStatus Status { get; set; }
        public TourComplexSuggestion() 
        {
            Id = -1;
            UserId = -1;
            TourSuggestions = new List<TourSuggestion>();
            Status = TourSuggestionStatus.Pending;
        }
        public TourComplexSuggestion(int userId, List<TourSuggestion> tourSuggestions,TourSuggestionStatus suggestionStatus)
        {
            UserId = userId;
            TourSuggestions = tourSuggestions;
            Status = suggestionStatus;
        }

        public string[] ToCSV()
        {
            string[] ret = { Id.ToString(),
                             UserId.ToString(),
                             SuggestionIdToCSV(),
                             Status.ToString(),
                            };
            return ret;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            if (values[2].Length > 0)
            {
                string[] TourSuggestionIds = values[2].Split(',');
                for (int i = 0; i < TourSuggestionIds.Length; i++)
                {
                    TourSuggestion? suggestion = new TourSuggestion();
                    suggestion = TourSuggestionComplexService.GetInstance().GetById(Convert.ToInt32(TourSuggestionIds[i]));
                    if (suggestion != null)
                    {
                        TourSuggestions.Add(suggestion);
                    }
                }
            }
            Status = (TourSuggestionStatus)Enum.Parse(typeof(TourSuggestionStatus), values[3]);
        }
        public string SuggestionIdToCSV()
        {
            string str = "";
            foreach (TourSuggestion ts in TourSuggestions)
            {
                str += ts.Id + ",";
            }
            if (str.Length > 0)
                str = str.Remove(str.Length - 1);
            return str;
        }
    }
}
