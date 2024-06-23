
using ASB.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ASB
{
    public partial class BreakfastPage : ContentPage, INotifyPropertyChanged
    {
        private NutritionDatabase _database;
        private ObservableCollection<NutritionEntry> _breakfastEntries;
        public ObservableCollection<NutritionEntry> BreakfastEntries
        {
            get => _breakfastEntries;
            set
            {
                _breakfastEntries = value;
                OnPropertyChanged();
            }
        }

        public BreakfastPage()
        {
            InitializeComponent();
            _database = new NutritionDatabase();
            LoadBreakfastEntries();
            BindingContext = this;
        }

        private void LoadBreakfastEntries()
        {
            var entries = _database.GetNutritionEntries()
                .Where(e => e.MealType == "Breakfast")
                .Select(e =>
                {
                    e.FoodName = char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower();
                    return e;
                });

            BreakfastEntries = new ObservableCollection<NutritionEntry>(entries);
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
