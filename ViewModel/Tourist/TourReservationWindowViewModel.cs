using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository.TourRepositories;
using BookingApp.Services;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace BookingApp.ViewModel.Tourist
{
    public class TourReservationWindowViewModel
    {
        public TourReservationWindow TourReservationWindow { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public TourReservationWindowViewModel(TourReservationWindow tourReservationWindow,Tour tour,User user) 
        { 
            this.TourReservationWindow = tourReservationWindow;
            Tour = tour;
            User = user;
            TourReservationWindow.NameTextBlock.Text = Tour.Name;
            TourReservationWindow.DateTextBlock.Text = Tour.DateTime.ToString();
            //TourReservationWindow.NumberOfPeopleTextBox.Text = "Max " + Tour.MaxTourists.ToString();  //mrzim ovu liniju koda

            List<string> CouponNames = new List<string>();
            CouponNames.Add("");
            foreach (TourCoupon tc in TourCouponService.GetInstance().GetAll())
            {
                DateTime expiryDate = tc.AcquiredDate.AddMonths(tc.ExpirationMonths);
                if (DateTime.Now > expiryDate && tc.Status != CouponStatus.Expired)   //Checks if the coupon has expired and changes it's status inside of the csv file if it is
                {
                    tc.Status = CouponStatus.Expired;
                    TourCouponService.GetInstance().Update(tc);
                }

                if (tc.Status != CouponStatus.Expired)
                {
                    CouponNames.Add(tc.Name);
                }
            }
            foreach (string coupon in CouponNames)
            {
                TourReservationWindow.CouponsComboBox.Items.Add(coupon);
            }
        }
        public void NumberOfPeopleTextBoxClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Contains("Max"))
            {
                textBox.Text = string.Empty;
                textBox.Foreground = new SolidColorBrush(Color.FromRgb(64, 48, 34));
            }

        }

        public void NumberOfPeopleTextBoxNotClicked(Object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Max " + Tour.MaxTourists.ToString(); //Needs to change to the number of available people if the user presses reserve, and it doesnt work  !!!!!!!!!!!!!!!!!!!!
                textBox.Foreground = Brushes.Gray;
            }
        }
        public void GenerateTextBoxes(object sender, TextChangedEventArgs e)
        {
            
            if(!TourReservationWindow.NumberOfPeopleTextBox.Text.Contains("Max") && (int.TryParse(TourReservationWindow.NumberOfPeopleTextBox.Text, out int numberOfPeople)))
            {

                if(numberOfPeople > Tour.MaxTourists)
                {
                    numberOfPeople = Tour.MaxTourists;          // Caps off the text boxes generation,sets the limit to the max people on that tour to reduce lag | A TEMPORARY SOLUTION??
                    TourReservationWindow.NumberOfPeopleTextBox.Text = numberOfPeople.ToString();
                }

                TourReservationWindow.TextBoxesPanel.Children.Clear();

                for (int i = 0; i < numberOfPeople; i++)
                {
                    //Name text box
                    TextBox nameTextBox = new TextBox();
                    nameTextBox.VerticalAlignment = VerticalAlignment.Center;
                    nameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    nameTextBox.Width = 60;
                    nameTextBox.Margin = new Thickness(5);
                    nameTextBox.FontSize = 15;
                    nameTextBox.Background = Brushes.White;
                    nameTextBox.Name = "nameTextBox";

                    //Surname textbox
                    TextBox surnameTextBox = new TextBox();
                    surnameTextBox.VerticalAlignment = VerticalAlignment.Center;
                    surnameTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    surnameTextBox.Width = 60;
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

                    TourReservationWindow.TextBoxesPanel.Children.Add(stackPanel);
                }

                TourReservationWindow.TextBoxesPanel.UpdateLayout();
            }
            
        }
        public void Cancel(object sender, RoutedEventArgs e)
        {
            TourReservationWindow.Close();
            TourDetailed tourDetailed = new TourDetailed(Tour, User);
            tourDetailed.ShowDialog();
        }
        public void Reserve(object sender, RoutedEventArgs e)
        {

            List<TourSchedule> tourSchedules = TourScheduleService.GetInstance().GetAll();
            TourSchedule tourSchedule = new TourSchedule();
            tourSchedule.Id = -1;
            //tourSchedule.Guests = 4;  // temporary line

            //finds the right schedule for the tour
            foreach (TourSchedule tourScheduleI in tourSchedules)
            {
                if (tourScheduleI.TourId == Tour.Id && tourScheduleI.Date == Tour.DateTime)
                {
                    tourSchedule = tourScheduleI;
                }
            }

            if (!TourReservationWindow.NumberOfPeopleTextBox.Text.Contains("Max") && !string.IsNullOrEmpty(TourReservationWindow.NumberOfPeopleTextBox.Text) && tourSchedule.Guests + Convert.ToInt32(TourReservationWindow.NumberOfPeopleTextBox.Text) <= Tour.MaxTourists)
            {
                List<TourPerson> tourPeople = new List<TourPerson>();

                foreach (StackPanel stackpanel in TourReservationWindow.TextBoxesPanel.Children)
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
                                else if (!int.TryParse(textBox.Text, out int result))
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
                    int id = TourPersonService.GetInstance().NextId(); //mozda ne valja
                    TourPerson person = new TourPerson(id, name, surname, age, -1);
                    tourPeople.Add(person);
                }

                string couponName = (string)TourReservationWindow.CouponsComboBox.SelectedItem;
                int couponId = -1;
                if (couponName == "")
                {
                    couponId = -1;
                }
                else
                {
                    foreach (TourCoupon tc in TourCouponService.GetInstance().GetAll())
                    {
                        if (tc.Name.Equals(couponName))
                        {
                            couponId = tc.Id;
                            tc.Status = CouponStatus.Expired;
                            TourCouponService.GetInstance().Update(tc);
                            break;
                        }

                    }
                }


                foreach (TourPerson person in tourPeople)
                {
                    TourPersonService.GetInstance().Add(person);
                }

                if (tourSchedule.Id == -1)
                {
                    MessageBox.Show("Couldn't find the tour schedlue!");
                    return;
                }
                tourSchedule.Guests += Convert.ToInt32(TourReservationWindow.NumberOfPeopleTextBox.Text);
                TourScheduleService.GetInstance().Update(tourSchedule);

                TourReservation tourReservation = new TourReservation(0, User.Id, tourSchedule.Id, couponId, tourPeople);
                TourReservationService.GetInstance().Add(tourReservation);
                TourReservationWindow.Close();
                TourReservationSuccessful tourReservationSuccessful = new TourReservationSuccessful(Tour, tourReservation);
                tourReservationSuccessful.ShowDialog();

            }
            else if (TourReservationWindow.NumberOfPeopleTextBox.Text.Contains("Max") || string.IsNullOrEmpty(TourReservationWindow.NumberOfPeopleTextBox.Text))
            {
                MessageBox.Show("You need to insert the amount of people going on the tour!");
                return;
            }
            else// Opens the "There aren't enough free slots on the tour" window
            {
                int FreeSlots = Tour.MaxTourists - tourSchedule.Guests;
                TourReservationFailed tourReservationFailed = new TourReservationFailed(TourReservationWindow, FreeSlots, Tour);
                tourReservationFailed.Owner = TourReservationWindow;
                tourReservationFailed.ShowDialog();
            }

        }


    }
}
