using BookingApp.ViewModel.Guide;
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

namespace BookingApp.View.Guide.Pages
{
    /// <summary>
    /// Interaction logic for WizardPage.xaml
    /// </summary>
    public partial class WizardPage : Page
    {
        public Action Finished;
        public WizardPage()
        {
            InitializeComponent();
            WizardPageViewModel viewModel = new WizardPageViewModel();
            DataContext = viewModel;
            Finish();
        }
        private void Finish()
        {
            Finished?.Invoke();
        }
        private void FinishWizard(object sender, RoutedEventArgs e)
        {
            Finish();
        }
    }
}
