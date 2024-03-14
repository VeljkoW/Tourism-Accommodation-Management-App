using BookingApp.Model;
using BookingApp.Repository.TourRepositories;
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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window
    {
        public Tour Tour {  get; set; }
        public User User { get; set; }
        public TourPersonRepository tourPersonRepository { get; set; }
        public TourReservationRepository tourReservationRepository { get; set; }
        public TourScheduleRepository tourScheduleRepository { get; set; }
        public TourReservationWindow(Tour tour, User user)
        {
            InitializeComponent();
            Tour = tour;
            User = user;
            NameTextBlock.Text = Tour.Name;
            DateTextBlock.Text = Tour.DateTime.ToString();
            NumberOfPeopleTextBox.Text = "Max " + Tour.MaxTourists.ToString();

            tourPersonRepository = new TourPersonRepository();
            tourReservationRepository = new TourReservationRepository();
            tourScheduleRepository = new TourScheduleRepository();
            User = user;
        }
        private void LoadedFunctions(object sender, RoutedEventArgs e)
        {
            CenterWindow();
        }

        private void CenterWindow()
        {
            double SWidth = SystemParameters.PrimaryScreenWidth;
            double SHeight = SystemParameters.PrimaryScreenHeight;
            double WWidth = this.Width;
            double WHeight = this.Height;

            this.Left = (SWidth - WWidth) / 2;
            this.Top = (SHeight - WHeight) / 2;
        }

        private void NumberOfPeoplePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!double.TryParse(e.Text,out _))
            {
                e.Handled = true;
            }
        }
        private void NumberOfPeopleTextBoxClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Contains("Max"))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Color.FromRgb(64, 48, 34));
            }

        }

        private void NumberOfPeopleTextBoxNotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Max " + Tour.MaxTourists.ToString(); //Needs to change to the number of available people if the user presses reserve, and it doesnt work  !!!!!!!!!!!!!!!!!!!!
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void GenerateTextBoxes(object sender, TextChangedEventArgs e)
        {
            if(!NumberOfPeopleTextBox.Text.Contains("Max") && (int.TryParse(NumberOfPeopleTextBox.Text, out int numberOfPeople)))
            {
                TextBoxesPanel.Children.Clear();

                if(numberOfPeople > Tour.MaxTourists)
                {
                    numberOfPeople = Tour.MaxTourists;          // Caps off the text boxes generation,sets the limit to the max people on that tour to reduce lag | A TEMPORARY SOLUTION??
                }

                for (int i = 0; i < numberOfPeople; i++)
                {
                    //Name text box
                    TextBox nameTextBox = new TextBox();
                    nameTextBox.VerticalAlignment = VerticalAlignment.Center;
                    nameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    nameTextBox.Width = 70;
                    nameTextBox.Margin = new Thickness(5);
                    nameTextBox.FontSize = 15;
                    nameTextBox.Background = Brushes.White;
                    nameTextBox.Name = "nameTextBox";

                    //Surname textbox
                    TextBox surnameTextBox = new TextBox();
                    surnameTextBox.VerticalAlignment = VerticalAlignment.Center;
                    surnameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    surnameTextBox.Width = 70;
                    surnameTextBox.Margin = new Thickness(5);
                    surnameTextBox.FontSize = 15;
                    surnameTextBox.Background = Brushes.White;
                    surnameTextBox.Name = "surnameTextBox";

                    //Age textbox
                    TextBox ageTextBox = new TextBox();
                    ageTextBox.VerticalAlignment = VerticalAlignment.Center;
                    ageTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ageTextBox.Width = 30;
                    ageTextBox.Margin = new Thickness(5);
                    ageTextBox.FontSize = 15;
                    ageTextBox.Background = Brushes.White;
                    ageTextBox.Name = "ageTextBox";

                    //Name textblock
                    TextBlock nameTextBlock = new TextBlock();
                    nameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    nameTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                    nameTextBlock.Text = "  Name: ";
                    nameTextBlock.FontSize = 15;
                    nameTextBlock.Foreground = Brushes.Black;


                    //Surname textblock
                    TextBlock surnameTextBlock = new TextBlock();
                    surnameTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    surnameTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                    surnameTextBlock.Text = " Surname: ";
                    surnameTextBlock.FontSize = 15;
                    surnameTextBlock.Foreground = Brushes.Black;

                    //Age textblock
                    TextBlock ageTextBlock = new TextBlock();
                    ageTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    ageTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                    ageTextBlock.Text = " Age: ";
                    ageTextBlock.FontSize = 15;
                    ageTextBlock.Foreground = Brushes.Black;

                    
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Children.Add(nameTextBlock);
                    stackPanel.Children.Add(nameTextBox);
                    stackPanel.Children.Add(surnameTextBlock);
                    stackPanel.Children.Add(surnameTextBox);
                    stackPanel.Children.Add(ageTextBlock);
                    stackPanel.Children.Add(ageTextBox);
                    stackPanel.Orientation = Orientation.Horizontal;
                    
                    TextBoxesPanel.Children.Add(stackPanel);
                }
            }
        }

        public void Cancel(object sender, RoutedEventArgs e)
        {
            Close();
            TourDetailed tourDetailed = new TourDetailed(Tour,User);
            tourDetailed.ShowDialog();
        }
        public void Reserve(object sender, RoutedEventArgs e)
        {

            List<TourSchedule> tourSchedules = tourScheduleRepository.GetAll();
            TourSchedule tourSchedule = new TourSchedule();
            tourSchedule.Id = -1;
            tourSchedule.Guests = 4;  // temporary line
            //finds the right schedule for the tour
            foreach (TourSchedule tourScheduleI in tourSchedules)
            {
                if (tourScheduleI.TourId == Tour.Id && tourScheduleI.Date == Tour.DateTime)
                {
                    tourSchedule = tourScheduleI;
                }
            }

            if (!NumberOfPeopleTextBox.Text.Contains("Max") && !string.IsNullOrEmpty(NumberOfPeopleTextBox.Text) && tourSchedule.Guests+Convert.ToInt32(NumberOfPeopleTextBox.Text) <= Tour.MaxTourists)
            {
                List<TourPerson> tourPeople = new List<TourPerson>();

                foreach (StackPanel stackpanel in TextBoxesPanel.Children)
                {
                    string name = "";
                    string surname = "";
                    int age = -1;

                    foreach (var child in stackpanel.Children)
                    {
                        if (child is TextBox)
                        {
                            TextBox textBox = (TextBox)child;

                            if (textBox.Name == "nameTextBox")
                            {
                                if (string.IsNullOrEmpty(textBox.Text))
                                {
                                    MessageBox.Show("You haven't filled in the Name textbox");
                                    return;
                                }
                                else
                                {
                                    name = textBox.Text;
                                }
                            }
                            else if (textBox.Name == "surnameTextBox")
                            {
                                if (string.IsNullOrEmpty(textBox.Text))
                                {
                                    MessageBox.Show("You haven't filled in the Surname textbox");
                                    return;
                                }
                                else
                                {
                                    surname = textBox.Text;
                                }
                            }
                            else if (textBox.Name == "ageTextBox")
                            {
                                if (string.IsNullOrEmpty(textBox.Text))
                                {
                                    MessageBox.Show("You haven't filled in the Age textbox");
                                    return;
                                }
                                else if(!int.TryParse(textBox.Text, out int result))
                                {
                                    MessageBox.Show("Only numbers go into the Age textbox");
                                    return;
                                }
                                else
                                {
                                    age = Convert.ToInt32(textBox.Text);
                                }
                            }
                        }
                    }
                    int id = tourPersonRepository.NextId();
                    TourPerson person = new TourPerson(id, name, surname, age);
                    tourPeople.Add(person);
                }

                foreach (TourPerson person in tourPeople)
                {
                    tourPersonRepository.Add(person);
                }
                
                if(tourSchedule.Id == -1)
                {
                    MessageBox.Show("Couldn't find the tour schedlue!");
                    return;
                }

                TourReservation tourReservation = new TourReservation(0, User.Id, tourSchedule.Id, tourPeople);
                tourReservationRepository.Add(tourReservation);
                Close();
                TourReservationSuccessful tourReservationSuccessful = new TourReservationSuccessful(Tour, tourReservation);
                tourReservationSuccessful.ShowDialog();

            }
            else if(NumberOfPeopleTextBox.Text.Contains("Max") && string.IsNullOrEmpty(NumberOfPeopleTextBox.Text))
            {
                MessageBox.Show("You need to insert the amount of people going on the tour!");
                return;
            }
            else// Opens the "There aren't enough free slots on the tour" window
            {

                int FreeSlots = Tour.MaxTourists - tourSchedule.Guests;
                TourReservationFailed tourReservationFailed = new TourReservationFailed(this,FreeSlots);
                tourReservationFailed.Owner = this;
                tourReservationFailed.ShowDialog();
            }

        }

    }
}
