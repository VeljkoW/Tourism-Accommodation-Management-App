using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.Repository.TourRepositories;
using BookingApp.ViewModel.Guide;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xaml.Schema;
using Image = BookingApp.Domain.Model.Image;

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for CreateTourForm.xaml
    /// </summary>
    public partial class CreateTourForm : Page
    {
        public User User { get; set; }
        public GuideMainPage GuideMainPage { get; }
        public CreateTourFormViewModel CreateTourFormViewModel { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateTourForm(User user)
        {
            InitializeComponent();
            CreateTourFormViewModel = new CreateTourFormViewModel(this,user);
            this.DataContext = CreateTourFormViewModel;
        }

        public CreateTourForm(GuideMainPage guideMainPage, User user)
        {
            CreateTourFormViewModel createTourFormViewModel = new CreateTourFormViewModel(this, user);
            this.DataContext = createTourFormViewModel;
            InitializeComponent();
            GuideMainPage = guideMainPage;
            User = user;
        }

        private void DatePicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        public event Action StateBoxEventHandler;
        private void StateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StateBoxEventHandler?.Invoke();
        }
    }
}
