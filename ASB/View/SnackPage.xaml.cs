
using ASB.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ASB
{
    public partial class SnackPage : ContentPage, INotifyPropertyChanged
    {
        private NutritionDatabase _database;
        private ObservableCollection<NutritionEntry> _snackEntries;
        public ObservableCollection<NutritionEntry> SnackEntries
        {
            get => _snackEntries;
            set
            {
                _snackEntries = value;
                OnPropertyChanged();
            }
        }

        public SnackPage()
        {
            InitializeComponent();
            _database = new NutritionDatabase();
            LoadSnackEntries();
            BindingContext = this;
        }

        private void LoadSnackEntries()
        {
            var entries = _database.GetNutritionEntries()
            .Where(e => e.MealType == "Snacks")
            .Select(e =>
            {
                e.FoodName = char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower();
                return e;
            });
            SnackEntries = new ObservableCollection<NutritionEntry>(entries);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
