using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace BookingApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for ComboBox.xaml
    /// </summary>
    public partial class ComboBox : UserControl, INotifyPropertyChanged
    {
        public ComboBox()
        {
            InitializeComponent();
            DataContext = this;
            InputComboBox.Loaded += InputComboBox_Loaded;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string str)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(str));
            }
        }
        private string placeholder;
        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                OnPropertyChanged(nameof(Placeholder));
            }
        }

        private void InputComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            PlaceholderTextBlock.Visibility = Visibility.Visible;
            UpdatePlaceholderVisibility();
        }

        private void InputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            if (InputComboBox.SelectedItem == null || (InputComboBox.SelectedItem is ComboBoxItem selectedItem && string.IsNullOrEmpty(selectedItem.Content.ToString())))
            {
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                PlaceholderTextBlock.Visibility = Visibility.Hidden;
            }
        }
    }
}
