using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.AccommodationRepositories;
using BookingApp.Services;
using BookingApp.ViewModel.Owner;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationRegistration.xaml
    /// </summary>
    public partial class AccommodationRegistration : Page
    {
        private bool calledFromStatistics = false;
        private double lastVerticalOffset = 0;
        public AccommodationManagementViewModel AccommodationManagementViewModel { get; set; }
        public AccommodationRegistration(User user)
        {
            InitializeComponent();
            AccommodationManagementViewModel = new AccommodationManagementViewModel(this, user);
            this.DataContext = AccommodationManagementViewModel;
        }
        public AccommodationRegistration(User user, bool calledFromStatistics)
        {
            InitializeComponent();
            AccommodationManagementViewModel = new AccommodationManagementViewModel(this, user);
            this.DataContext = AccommodationManagementViewModel;
            this.calledFromStatistics = calledFromStatistics;
        }
        private void StatePickedSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationManagementViewModel.StatePicked();
        }
        private void StateChosenSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calledFromStatistics)
            {
                calledFromStatistics = false;
                return;
            }
            AccommodationManagementViewModel.StateChosen();
        }
        private void CityChosenSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AccommodationManagementViewModel.CityChosen();
        }

        private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double borderTopPosition = GetBorderTopPosition(ChooseLocationBorder);
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer.VerticalOffset > lastVerticalOffset)
            {
                // Korisnik skrolira nadole
                // Implementirajte željenu logiku ovde
                /*Point topRightPoint = new Point(ScrollViewerName.ActualWidth, ScrollViewerName.TranslatePoint(new Point(0, 0), Application.Current.MainWindow).Y);
                if (borderTopPosition >= topRightPoint.Y)
                {
                    ChooseLocationBorder.Margin = new Thickness(ChooseLocationBorder.Margin.Left, ChooseLocationBorder.Margin.Top + 50, ChooseLocationBorder.Margin.Right, ChooseLocationBorder.Margin.Bottom);
                }*/
            }
            // Provera da li je korisnik skrolirao nagore
            else if (scrollViewer.VerticalOffset < lastVerticalOffset)
            {
                // Korisnik skrolira nagore
                // Implementirajte željenu logiku ovde
                /*if (borderTopPosition <= 400)
                {
                    ChooseLocationBorder.Margin = new Thickness(ChooseLocationBorder.Margin.Left, ChooseLocationBorder.Margin.Top - 50, ChooseLocationBorder.Margin.Right, ChooseLocationBorder.Margin.Bottom);
                }*/
            }
            lastVerticalOffset = scrollViewer.VerticalOffset;
        }
        private double GetBorderTopPosition(Border border)
        {
            // Pronalaženje koordinata gornjeg levog ćoška Border elementa unutar Page
            Point topLeftPoint = border.TranslatePoint(new Point(0, 0), Application.Current.MainWindow);

            // Ako Border nema roditeljski element koji je MainWindow, možete koristiti TranslatePoint relativno na Page:
            // Point topLeftPoint = border.TranslatePoint(new Point(0, 0), this);

            // Visina na kojoj se nalazi gornji deo Border elementa
            double borderTopPosition = topLeftPoint.Y;

            return borderTopPosition;
        }
    }
}
