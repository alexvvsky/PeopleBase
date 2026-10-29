using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PeopleBase
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PersonContext database = new();
        public ObservableCollection<Person> People { get; private set; } = new ObservableCollection<Person>()
        {
            new Person {FirstName = "John", LastName = "Doe", Age = 30},
            new Person {FirstName = "Jane", LastName = "Smith", Age = 25},
        };
        public MainWindow()
        {
            InitializeComponent();

            database.Database.EnsureCreated();
            database.People.Load();
            People = database.People.Local.ToObservableCollection();

            DataContext = this;
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            database.SaveChanges();
        }
    }
}