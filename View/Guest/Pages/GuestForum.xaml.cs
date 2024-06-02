using BookingApp.Domain.Model;
using BookingApp.ViewModel.Guest;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.View.Guest.Pages
{
    /// <summary>
    /// Interaction logic for GuestForum.xaml
    /// </summary>
    public partial class GuestForum : Page
    {
        public GuestForumViewModel GuestForumViewModel { get; set; }
        public User User { get; set; }
        public GuestForum(User user)
        {
            InitializeComponent();
            MainGrid.Focus();
            User = user;
            GuestForumViewModel = new GuestForumViewModel(this, User);
            this.DataContext = GuestForumViewModel;
        }

        private void statePick(object sender, SelectionChangedEventArgs e)
        {
            GuestForumViewModel.statePick();
        }

        private void cityPick(object sender, SelectionChangedEventArgs e)
        {
            GuestForumViewModel.cityPick();
        }

        private void StateComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                // Ukloni fokus sa TextBox
                Keyboard.ClearFocus();

                // Postavi fokus na Window
                FocusManager.SetFocusedElement(MainGrid, MainGrid);

                // Spreči dalje procesiranje događaja
                e.Handled = true;
            }
        }

        private void CommentTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                // Ukloni fokus sa TextBox
                Keyboard.ClearFocus();

                // Postavi fokus na Window
                FocusManager.SetFocusedElement(MainGrid, MainGrid);

                // Spreči dalje procesiranje događaja
                e.Handled = true;
            }
        }
    }
}
