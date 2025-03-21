﻿using BookingApp.Domain.Model;
using BookingApp.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using BookingApp.Services;
using System.Threading;

namespace BookingApp.ViewModel.Tourist
{
    public class TourSuggestionWindowViewModel
    {
        public TourSuggestionWindow TourSuggestionWindow { get; set; }
        public TouristMainWindowViewModel? TouristMainWindowViewModel { get; set; }
        public User User { get; set; }
        public bool IsComplex { get; set; }
        public bool Demo { get; set; }
        public int ComplexId { get; set; }
        public RelayCommand ClickCancel => new RelayCommand(execute => CancelExecute());
        public RelayCommand ClickSuggest => new RelayCommand(execute => SuggestExecute());
        public CancellationTokenSource CancellationTokenSource;

        public TourSuggestionWindowViewModel(TourSuggestionWindow tourSuggestionWindow, User user, bool isComplex, int complexId, bool demo, TouristMainWindowViewModel touristMainWindowViewModel)
        {
            TourSuggestionWindow = tourSuggestionWindow;
            User = user;
            IsComplex = isComplex;
            ComplexId = complexId;
            Demo = demo;
            this.TouristMainWindowViewModel = touristMainWindowViewModel;
            AddStatesToComboBox(LocationService.GetInstance().GetAll());
            if(Demo)
            {
                StartDemo();
            }
        }
        public void EndDemoMode()
        {
            TourSuggestionWindow.TourSuggestionWindowOverlay.Visibility = Visibility.Collapsed;
            CancellationTokenSource.Cancel();
            TouristMainWindowViewModel.EndDemoMode();
            CancelExecute();
        }
        public async void StartDemo()
        {
            TourSuggestionWindow.TourSuggestionWindowOverlay.Visibility = Visibility.Visible;
            CancellationTokenSource = new CancellationTokenSource();
            try
            {
                CancellationToken cancellationToken = CancellationTokenSource.Token;
                await Task.Delay(1000, cancellationToken);
                TourSuggestionWindow.NumberOfPeopleTextBox.Text = "1";
                await Task.Delay(1000, cancellationToken);
                int numberOfPeople;
                if (int.TryParse(TourSuggestionWindow.NumberOfPeopleTextBox.Text, out numberOfPeople))
                {
                    foreach (StackPanel stackpanel in TourSuggestionWindow.TextBoxesPanel.Children)
                    {
                        foreach (var child in stackpanel.Children)
                        {
                            if (child is TextBox textBox)
                            {
                                if (textBox.Name == "nameTextBox")
                                {
                                    textBox.Text = "J";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "Jo";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "Joh";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "John";
                                    await Task.Delay(500, cancellationToken);
                                }
                                else if (textBox.Name == "surnameTextBox")
                                {
                                    textBox.Text = "S";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "Sm";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "Smi";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "Smit";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "Smith";
                                    await Task.Delay(500, cancellationToken);
                                }
                                else if (textBox.Name == "ageTextBox")
                                {
                                    textBox.Text = "3";
                                    await Task.Delay(500, cancellationToken);
                                    textBox.Text = "31";
                                    await Task.Delay(500, cancellationToken);
                                }
                            }
                        }
                    }
                }
                await Task.Delay(1000, cancellationToken);
                TourSuggestionWindow.LanguageTextBox.Text = "D";
                await Task.Delay(500, cancellationToken);
                TourSuggestionWindow.LanguageTextBox.Text = "Du";
                await Task.Delay(500, cancellationToken);
                TourSuggestionWindow.LanguageTextBox.Text = "Dut";
                await Task.Delay(500, cancellationToken);
                TourSuggestionWindow.LanguageTextBox.Text = "Dutc";
                await Task.Delay(500, cancellationToken);
                TourSuggestionWindow.LanguageTextBox.Text = "Dutch";
                await Task.Delay(1000, cancellationToken);
                try
                {
                    TourSuggestionWindow.StateComboBox.SelectedIndex = 0;
                }
                catch
                {

                }
                await Task.Delay(1000, cancellationToken);
                try
                {
                    TourSuggestionWindow.CityComboBox.SelectedIndex = 0;
                }
                catch
                {

                }
                await Task.Delay(1000, cancellationToken);
                TourSuggestionWindow.StartDatePicker.Text = "26-Sep-24";
                await Task.Delay(1000, cancellationToken);
                TourSuggestionWindow.EndDatePicker.Text = "05-Dec-24";
                await Task.Delay(1000, cancellationToken);
                TourSuggestionWindow.DescriptionTextBox.Text = ":";
                await Task.Delay(1000, cancellationToken);
                TourSuggestionWindow.DescriptionTextBox.Text = ":D";
                await Task.Delay(1000, cancellationToken);
                CancelExecute();
            }
            catch
            {
                EndDemoMode();
            }

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
        public void AddStatesToComboBox(List<Location> locations)
        {
            List<String> States = new List<string>();
            foreach (Location location in locations)
            {
                bool Exists = false;

                foreach (String s in States)
                {
                    if (s == location.State)
                    {
                        Exists = true;
                    }
                }
                if (!Exists)
                {
                    States.Add(location.State);
                }
            }
            TourSuggestionWindow.StateComboBox.Items.Clear();

            foreach (String s in States)
            {
                TourSuggestionWindow.StateComboBox.Items.Add(s);
            }
            TourSuggestionWindow.CityComboBox.IsEnabled = false;
        }
        public void StateComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TourSuggestionWindow.CityComboBox.Items.Clear();

            if (TourSuggestionWindow.StateComboBox.SelectedIndex == -1)
            {
                TourSuggestionWindow.CityComboBox.IsEnabled = false;
            }
            else
            {
                TourSuggestionWindow.CityComboBox.IsEnabled = true;

                List<string> cities = new List<string>();
                string selectedState = (string)TourSuggestionWindow.StateComboBox.SelectedItem;

                cities = findCities(cities, selectedState);

                foreach (string city in cities)
                {
                    TourSuggestionWindow.CityComboBox.Items.Add(city);
                }

            }
        }
        public List<string> findCities(List<string> cities, string selectedState)
        {
            foreach (Location location in LocationService.GetInstance().GetAll())
            {
                if (location.State == selectedState)
                {
                    bool Exists = false;

                    foreach (string c in cities)
                    {
                        if (c == location.City)
                        {
                            Exists = true;
                        }
                    }

                    if (!Exists)
                    {
                        cities.Add(location.City);
                    }
                }
            }
            return cities;
        }
        public void CancelExecute()
        {
            TourSuggestionWindow.Close();
        }
        public void SuggestExecute()
        {
            string description = TourSuggestionWindow.DescriptionTextBox.Text;
            string language = TourSuggestionWindow.LanguageTextBox.Text;
            string state = (string)TourSuggestionWindow.StateComboBox.SelectedItem;
            string city = (string)TourSuggestionWindow.CityComboBox.SelectedItem;

            if (!string.IsNullOrEmpty(language) && !string.IsNullOrEmpty(description) && !string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(TourSuggestionWindow.StartDatePicker.Text) && !string.IsNullOrEmpty(TourSuggestionWindow.EndDatePicker.Text) && !string.IsNullOrEmpty(TourSuggestionWindow.NumberOfPeopleTextBox.Text))
            {
                int numberOfPeople = 0;
                try
                {
                    numberOfPeople = Convert.ToInt32(TourSuggestionWindow.NumberOfPeopleTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("The number of people textbox can have only numbers");
                    return;
                }
                if(numberOfPeople == 0)
                {
                    MessageBox.Show("The number of people cannot be 0");
                    return;
                }
                DateTime fromDate = DateTime.Parse(TourSuggestionWindow.StartDatePicker.Text);
                DateTime toDate = DateTime.Parse(TourSuggestionWindow.EndDatePicker.Text);
                if(fromDate >  toDate)
                {
                    MessageBox.Show("The starting date cannot be after the end date!","Wrong date input",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }
                List<TourPerson> tourists = new List<TourPerson>();

                foreach (StackPanel stackpanel in TourSuggestionWindow.TextBoxesPanel.Children)
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
                    int id = TourPersonService.GetInstance().NextId();
                    TourPerson tourist = new TourPerson(id, name, surname, age, -1);
                    tourists.Add(tourist);
                }

                int locationId = LocationService.GetInstance().GetIdByStateCity(state, city);
                if (locationId != -1)
                {
                    Location location = LocationService.GetInstance().GetById(locationId) ?? new Location();

                    foreach(TourPerson person in tourists)
                    {
                        TourPersonService.GetInstance().Add(person);
                    }

                    TourSuggestion tourSuggestion = new TourSuggestion(User.Id,location,description,language,numberOfPeople,tourists,fromDate,toDate,DateTime.MinValue,TourSuggestionStatus.Pending,ComplexId,-1);
                    if (!IsComplex)
                    {
                        TourSuggestionService.GetInstance().Add(tourSuggestion);
                    }
                    else
                    {
                        TourSuggestionComplexService.GetInstance().Add(tourSuggestion);
                    }
                    TourSuggestionWindow.Close();
                }
                else
                {
                    MessageBox.Show("Could not find that location!");
                    return;
                }

            }
            else
            {
                MessageBox.Show("All fields need to be filled!","Missing input",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
        }
    }
}
