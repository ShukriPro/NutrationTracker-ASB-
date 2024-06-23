using ASB.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace ASB
{
    public class FoodEntryViewModel : INotifyPropertyChanged
    {
        private NutritionDatabase _database;
        public NutritionEntry Entry { get; set; }

        public ICommand SaveCommand { get; }

        public FoodEntryViewModel(NutritionEntry entry, NutritionDatabase database)
        {
            Entry = entry;
            _database = database;
            SaveCommand = new Command(OnSave);
        }

        private void OnSave()
        {
            _database.SaveNutritionEntry(Entry);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
