
using ASB.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ASB
{
    public partial class LunchPage : ContentPage, INotifyPropertyChanged
    {
        private NutritionDatabase _database;
        private ObservableCollection<NutritionEntry> _lunchEntries;
        public ObservableCollection<NutritionEntry> LunchEntries
        {
            get => _lunchEntries;
            set
            {
                _lunchEntries = value;
                OnPropertyChanged();
            }
        }

        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public LunchPage()
        {
            InitializeComponent();
            _database = new NutritionDatabase();
            LoadLunchEntries();
            //DeleteCommand = new Command<NutritionEntry>(OnDelete);
            EditCommand = new Command<NutritionEntry>(OnEdit);
            BindingContext = this;
        }

        private void LoadLunchEntries()
        {
            var entries = _database.GetNutritionEntries()
               .Where(e => e.MealType == "Lunch")
               .Select(e =>
               {
                   e.FoodName = char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower();
                   return e;
               });

            LunchEntries = new ObservableCollection<NutritionEntry>(entries);

        }

        //private void OnDelete(NutritionEntry entry)
        //{
        //    _database.DeleteNutritionEntry(entry.Id);
        //    LunchEntries.Remove(entry);
        //}

        private async void OnEdit(NutritionEntry entry)
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
