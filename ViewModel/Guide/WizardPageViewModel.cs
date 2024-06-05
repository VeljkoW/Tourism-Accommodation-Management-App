using BookingApp.View.Guide.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.ViewModel.Guide
{
    public class WizardPageViewModel:INotifyPropertyChanged
    {
        //GoNext
        //GoBack
        public RelayCommand GoNext => new RelayCommand(execute => GoNextExecute(), canExecute => GoNextCanExecute());
        private void GoNextExecute()
        {
            index++;
            CurrentUserControl = Parts[index];
        }

        private bool GoNextCanExecute()
        {
            if (index < Parts.Count -1)
            {
                return true;
            }
            return false;
        }

        public RelayCommand GoBack => new RelayCommand(execute => GoBackExecute(), canExecute => GoBackCanExecute());
        int index = 0;
        private bool GoBackCanExecute()
        {
            if(index >0)
            {
                return true;
            }
            return false;
        }
        private void GoBackExecute()
        {
            index--;
            CurrentUserControl = Parts[index];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        //CurrentUserControl
        private UserControlWizardPart _userControl;
        public UserControlWizardPart CurrentUserControl 
        {
            get => _userControl;
            set
            {
                if (value != _userControl)
                {
                    _userControl = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<UserControlWizardPart> Parts { get; set; } = new ObservableCollection<UserControlWizardPart>();
        public WizardPageViewModel() {
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/BurgerCollapsed.png","Prvi prozor koji se pojavi kada se ulogujete u aplikaciju izgleda ovako. Na njemu vidimo ture koje nisu zavrsene. Moze da se logout-uje , otkaze tura i prati neka tura. Pritiskom na tri linije otvara se burger menu."));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/BurgerVisible.png","Kada se pritisne na tri crtice gore levo , otvara se burger menu koji nam daje pristup novim funkcijama"));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/TourCreation.png","Kada pritisnemo Create Tour otvara se forma koja ima sve potrebne informacije za kreiranje nove ture, uz provere unosa da ne napravi neku gresku."));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/TourCreation2.png", "Create dugme je ugaseno sve dok korisnik ne ispuni sva polja!!!"));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/MonitoringTour.png", "Pritiskom na \"oko\" korisnik ulazi u tour monitoring , oznacava da je tura pocela i moze da bira na koju kljucnu tacku je  dosao i ko je dosao na trenutnu kljucnu tacku od tursita.Pritiskom na dugme Finish tour vodic zavrsava turu na toj tacki, a go back se vraca na pocetnu stranicu gde moze da obavlja ostale funkcionalnosti."));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/TourStatistics.png","Most visited tour je tura koja je najposecenija ikada sa prikazom broja turista,moze da se filtrira po godinama."));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/FinishedTours.png","Na ovom prozoru se vide sve zavrsene ture. Kada imaju neku ocenu od turista, kada pritisnemo na dugme review otvara se stranica sa ocenama korisnika"));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/TourReview.png","Na ovom prozoru mozemo da vidimo sve ocene od turista za odabranu zavrsenu turu."));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/ComplexTourRequests.png", "Ovde su nam prikazani svi slozeni zahtevi na kojima nismo prihvatili ni jedan deo slozenog zahteva."));
            Parts.Add(new UserControlWizardPart("../../../Resources/Images/GuideWizard/ComplexTourRequestSuggestions.png", "Nakon biranja nekog od slozenih zahteva izadju nam svi zahtevi koje mozemo da prihvatimo i kada acceptujemo zahtev biramo jedan datum iz izlistanih kada bi vodili tu turu."));



            CurrentUserControl = Parts[0];
        }
    }
}
