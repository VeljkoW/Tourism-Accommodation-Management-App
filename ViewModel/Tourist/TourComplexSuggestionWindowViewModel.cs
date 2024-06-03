using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Tourist
{
    public class TourComplexSuggestionWindowViewModel : INotifyPropertyChanged
    {
        public TourComplexSuggestionWindow TourComplexSuggestionWindow { get; set; }
        public ObservableCollection<TourSuggestion> TourSuggestions { get; set; } = new ObservableCollection<TourSuggestion>();
        public User User { get; set; }
        public int ComplexId {  get; set; }
        public RelayCommand ClickAdd => new RelayCommand(execute => AddExecute());
        public RelayCommand ClickCancel => new RelayCommand(execute => CancelExecute());
        public RelayCommand ClickSuggest => new RelayCommand(execute => SuggestExecute(),canExecute => SuggestCanExecute());
        public event PropertyChangedEventHandler? PropertyChanged;
        public TourComplexSuggestionWindowViewModel(TourComplexSuggestionWindow tourComplexSuggestionWindow,User user) 
        { 
            TourComplexSuggestionWindow = tourComplexSuggestionWindow;
            User = user;
            ComplexId = TourComplexSuggestionService.GetInstance().GetNextId();
            Update();
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Update()
        {
            TourSuggestions.Clear();
            foreach(var item in TourSuggestionComplexService.GetInstance().GetAllByComplexId(ComplexId))
            {
                item.Location = LocationService.GetInstance().GetById(item.LocationId);
                TourSuggestions.Add(item);
            }
        }
        public void AddExecute()
        {
            TourSuggestionWindow tourSuggestionWindow = new TourSuggestionWindow(User, true,ComplexId,false, null);
            tourSuggestionWindow.Owner = TourComplexSuggestionWindow;
            tourSuggestionWindow.Closed += (s, e) => Update();
            tourSuggestionWindow.ShowDialog();
        }
        public void CancelExecute()
        {
            if (TourComplexSuggestionWindow != null)
            {
                foreach(var item in TourSuggestions)
                {
                    foreach( var item2 in item.Tourists)
                    {
                        TourPersonService.GetInstance().DeleteById(item2.Id);
                    }
                    TourSuggestionComplexService.GetInstance().DeleteById(item.Id);
                }
                TourComplexSuggestionWindow.Close();
            }
        }
        public void SuggestExecute()
        {
            TourComplexSuggestion tourComplexSuggestion = new TourComplexSuggestion(User.Id,TourSuggestions.ToList(),TourSuggestionStatus.Pending);
            TourComplexSuggestionService.GetInstance().Add(tourComplexSuggestion);
            TourComplexSuggestionWindow.Close();
        }
        public bool SuggestCanExecute()
        {
            if(TourSuggestions.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
