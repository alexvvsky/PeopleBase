using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PeopleBase
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly PersonContext database = new();
        private Person? _selectedPerson;
        private string _searchText = "";
        private ObservableCollection<Person> _people = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        public Person? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (_selectedPerson != value)
                {
                    _selectedPerson = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    Search();
                }
            }
        }
        public ObservableCollection<Person> People
        {
            get => _people;
            private set
            {
                if (_people != value)
                {
                    _people = value;
                    OnPropertyChanged();
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();

            database.Database.EnsureCreated();
            database.People.Load();
            
            People = database.People.Local.ToObservableCollection();

            DataContext = this;

            var results = People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
            People = new ObservableCollection<Person>(results);
            PeopleGrid.ItemsSource = People;
        }
        private void Search()
        {
            string search = SearchText.Trim();
            var selected = SelectedPerson;

            var results = database.People.Local.Where(p => p.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) || p.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            People = new ObservableCollection<Person>(results);
            PeopleGrid.ItemsSource = People;

            if (selected is not null && People.Contains(selected))
            {
                SelectedPerson = selected;
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            database.SaveChanges();
        }

        private void AddPerson(object sender, RoutedEventArgs e)
        {
            Person newPerson = new Person { FirstName = "New", LastName = "Person", Age = 18 };
            People.Add(newPerson);
            database.Add(newPerson);
            SelectedPerson = newPerson;
            FirstNameTextBox.Focus();

            PeopleGrid.ItemsSource = People;
        }

        private void DeletePerson(object sender, RoutedEventArgs e)
        {
            if (PeopleGrid.SelectedItem is Person selectedPerson)
            {
                People.Remove(selectedPerson);
                database.Remove(selectedPerson);
                SelectedPerson = People.FirstOrDefault();
                PeopleGrid.ItemsSource = People;
            }
        }

        private void SortAgeAscending(object sender, RoutedEventArgs e)
        {
            var results = People.OrderBy(p => p.Age).ToList();
            People = new ObservableCollection<Person>(results);
            PeopleGrid.ItemsSource = People;
        }

        private void SortNameAscending(object sender, RoutedEventArgs e)
        {
            var results = People.OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
            People = new ObservableCollection<Person>(results);
            PeopleGrid.ItemsSource = People;
        }
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}