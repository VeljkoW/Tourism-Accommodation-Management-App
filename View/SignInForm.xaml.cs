using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Guide;
using BookingApp.View.Owner;
using BookingApp.View.Tourist;
using BookingApp.View.Guest;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password && user.UserType == UserType.Guide)
                {
                    GuideMainWindow guideMainWindow = new GuideMainWindow(user);
                    guideMainWindow.Show();
                    Close();
                }
                else if(user.Password == txtPassword.Password && user.UserType == UserType.Owner)
                {
                    OwnerMainWindow ownerMainWindow = new OwnerMainWindow(user);
                    ownerMainWindow.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password && user.UserType == UserType.Guest)
                {
                    GuestMainWindow guestMainWindow = new GuestMainWindow(user);
                    guestMainWindow.Show();
                    Close();
                }
                else if (user.Password == txtPassword.Password)
                {
                    if(user.UserType == UserType.Tourist)
                    {
                        TouristMainWindow touristMainWindow = new TouristMainWindow();
                        touristMainWindow.Show();
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
        private void RegisterView(object sender,RoutedEventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            Close();
        }
    }
}
