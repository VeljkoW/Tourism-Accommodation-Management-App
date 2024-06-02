using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.View.Guest.Pages;
using BookingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Windows
{
    /// <summary>
    /// Interaction logic for OwnerCommentForRescheduling.xaml
    /// </summary>
    public partial class OwnerCommentForRescheduling : Window
    {
        public RelayCommand Exit => new RelayCommand(execute => CloseWindow());
        public ProcessedReschedulingRequest ProcessedReschedulingRequest { get; set; }
        public OwnerCommentForRescheduling(ProcessedReschedulingRequest processedReschedulingRequest, User user)
        {
            InitializeComponent();
            DataContext = this;
            MainGrid.Focus();
            ProcessedReschedulingRequest = processedReschedulingRequest;
            Name.Text += AccommodationService.GetInstance().GetById(ProcessedReschedulingRequest.AccommodationId).Name + ", " + AccommodationService.GetInstance().GetById(ProcessedReschedulingRequest.AccommodationId).Location.State + " - " + AccommodationService.GetInstance().GetById(ProcessedReschedulingRequest.AccommodationId).Location.City;
            Date.Text += ProcessedReschedulingRequest.CheckInDate.ToString("dd/MM/yyyy HHtt - ") + processedReschedulingRequest.CheckOutDate.ToString("dd/MM/yyyy HHtt");
            if (ProcessedReschedulingRequest.CommentId == 0)
            {
                Comment.Text += "Comment has not been added!";
            }
            else
            {
                Comment.Text += CommentService.GetInstance().GetById(ProcessedReschedulingRequest.CommentId).Text;
            }
            if (ProcessedReschedulingRequest.IsAccepted == true)
                Accept.Text += "Accepted";
            else
                Accept.Text += "Declined";
        }
        public void CloseWindow()
        {
            Close();
        }
    }
}
