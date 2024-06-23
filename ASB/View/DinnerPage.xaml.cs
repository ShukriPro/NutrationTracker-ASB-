using ASB.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ASB
{
    public partial class DinnerPage : ContentPage, INotifyPropertyChanged
    {
        private NutritionDatabase _database;
        private ObservableCollection<NutritionEntry> _dinnerEntries;
        public ObservableCollection<NutritionEntry> DinnerEntries
        {
            get => _dinnerEntries;
            set
            {
                _dinnerEntries = value;
                OnPropertyChanged();
            }
        }

        public DinnerPage()
        {
            InitializeComponent();
            _database = new NutritionDatabase();
            LoadDinnerEntries();
            BindingContext = this;
        }

        private void LoadDinnerEntries()
        {
        
            var entries = _database.GetNutritionEntries()
               .Where(e => e.MealType == "Dinner")
               .Select(e =>
               {
                   e.FoodName = char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower();
                   return e;
               });

            DinnerEntries = new ObservableCollection<NutritionEntry>(entries);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
