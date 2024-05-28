using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class TourComplexSuggestionDetailedViewModel
    {
        public TourComplexSuggestionDetailed TourComplexSuggestionDetailed { get; set; }
        public RelayCommand ClickGoBack => new RelayCommand(execute => GoBackExecute());
        public User User { get; set; }
        public TourComplexSuggestion TourComplexSuggestion { get; set; }
        public ObservableCollection<TourSuggestion> TourSuggestions { get; set; } = new ObservableCollection<TourSuggestion>();
        public TourComplexSuggestionDetailedViewModel(TourComplexSuggestionDetailed tourComplexSuggestionDetailed, User user,TourComplexSuggestion tourComplexSuggestion) 
        {
            TourComplexSuggestionDetailed = tourComplexSuggestionDetailed;
            User = user;
            TourComplexSuggestion = tourComplexSuggestion;

            TourComplexSuggestionDetailed.ComplexTourName.Text = "Complex Tour #" + tourComplexSuggestion.Id.ToString();
            Update();
        }
        public void Update()
        {
            TourSuggestions.Clear();
            foreach (var item in TourSuggestionComplexService.GetInstance().GetAllByComplexId(TourComplexSuggestion.Id))
            {
                item.Location = LocationService.GetInstance().GetById(item.LocationId);
                TourSuggestions.Add(item);
            }
        }
        public void GoBackExecute()
        {
            TourComplexSuggestionDetailed.Close();
        }

    }
}
