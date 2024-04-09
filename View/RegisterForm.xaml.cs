using BookingApp.Domain.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for RegisterForm.xaml
    /// </summary>
    public partial class RegisterForm : Window
    {
        public RegisterForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }
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

        private void Register(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user != null)
            {
                MessageBox.Show("Username already exists!");
            }
            else
            {
                bool isOwnerChecked = OwnerRadioButton.IsChecked ?? false;
                bool isGuideChecked = GuideRadioButton.IsChecked ?? false;
                bool isGuestChecked = GuestRadioButton.IsChecked ?? false;
                bool isTouristChecked = TouristRadioButton.IsChecked ?? false;

                UserType userType = new UserType();

                if (isOwnerChecked)
                {
                    userType = UserType.Owner;
                }
                else if (isGuideChecked)
                {
                    userType = UserType.Guide;
                }
                else if (isGuestChecked)
                {
                    userType = UserType.Guest;
                }
                else if (isTouristChecked)
                {
                    userType = UserType.Tourist;
                }
                else
                {
                    MessageBox.Show("You need to check a user type button.");
                    return;
                }

                if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(confirmPassword.Password) || String.IsNullOrEmpty(txtPassword.Password))
                {
                    MessageBox.Show("You need to fill in each box");
                    return;
                }
                else
                {
                    if (txtPassword.Password == confirmPassword.Password)
                    {

                        User NewUser = new User();
                        NewUser.Username = Username;
                        NewUser.Password = confirmPassword.Password;
                        NewUser.UserType = userType;
                        _repository.Add(NewUser);

                        SignInForm signInForm = new SignInForm();
                        signInForm.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Passwords do not match!");
                    }
                }

            }

        }
        public void Cancel(object sender, EventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }

    }
}
