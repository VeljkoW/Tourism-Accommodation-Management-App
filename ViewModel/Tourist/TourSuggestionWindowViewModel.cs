using BookingApp.Domain.Model;
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
    public class TourSuggestionWindowViewModel
    {
        public TourSuggestionWindow TourSuggestionWindow { get; set; }
        public User User { get; set; }


        public TourSuggestionWindowViewModel(TourSuggestionWindow tourSuggestionWindow, User user)
        {
            TourSuggestionWindow = tourSuggestionWindow;
            User = user;
        }

        public void GenerateTextBoxes(object sender, TextChangedEventArgs e)
        {
            
            if (!TourSuggestionWindow.NumberOfPeopleTextBox.Text.Contains("Max") && (int.TryParse(TourSuggestionWindow.NumberOfPeopleTextBox.Text, out int numberOfPeople)))
            {
                
                if (numberOfPeople >= 50)
                {
                    numberOfPeople = 50;          // Caps off the text boxes generation,sets the limit to 50 to reduce lag | A TEMPORARY SOLUTION??
                    TourSuggestionWindow.NumberOfPeopleTextBox.Text = numberOfPeople.ToString();
                }
                

                TourSuggestionWindow.TextBoxesPanel.Children.Clear();

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

                    TourSuggestionWindow.TextBoxesPanel.Children.Add(stackPanel);
                }

                TourSuggestionWindow.TextBoxesPanel.UpdateLayout();
            }
            
        }

        public void Cancel(object sender, RoutedEventArgs e)
        {
            TourSuggestionWindow.Close();
        }
        public void Suggest(object sender, RoutedEventArgs e)
        {

        }
    }
}
