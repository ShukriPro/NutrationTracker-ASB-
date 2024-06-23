using ASB.Models;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ASB
{
    public partial class FoodEntryPage : ContentPage, INotifyPropertyChanged
    {
        private NutritionDatabase _database;
        private ObservableCollection<NutritionEntry> _entries;

        // Property to hold the list of nutrition entries
        public ObservableCollection<NutritionEntry> Entries
        {
            get => _entries;
            set
            {
                _entries = value;
                OnPropertyChanged();
            }
        }

        // Property to hold the list of foods from the JSON file
        public ObservableCollection<NutritionEntry> FoodList { get; private set; }

        // Constructor to initialize the page with a specified meal type
        public FoodEntryPage(string mealType)
        {
            InitializeComponent();
            _database = new NutritionDatabase();
            LoadEntries();
            BindingContext = this; // Set the BindingContext
            LoadFoodData();

            // Set the MealTypePicker based on the passed parameter
            MealTypePicker.SelectedItem = mealType;
        }

        // Default constructor
        public FoodEntryPage() : this(string.Empty)
        {
        }

        // Method to load nutrition entries from the database
        private void LoadEntries()
        {
            var entries = _database.GetNutritionEntries();
            Entries = new ObservableCollection<NutritionEntry>(entries);
        }

        // Event handler for text change in macronutrient entries to recalculate calories
        private void OnMacrosTextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateCalories();
        }

        // Method to calculate calories based on carbs, protein, and fat
        private void CalculateCalories()
        {
            if (double.TryParse(CarbsEntry.Text, out double carbs) &&
                double.TryParse(ProteinEntry.Text, out double protein) &&
                double.TryParse(FatEntry.Text, out double fat))
            {
                double calories = (carbs * 4) + (protein * 4) + (fat * 9);
                CaloriesEntry.Text = calories.ToString();
            }
            else
            {
                CaloriesEntry.Text = string.Empty;
            }
        }

        private async void LoadFoodData()
        {
            try
            {
                var assembly = typeof(FoodEntryPage).Assembly;
                var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(name => name.EndsWith("foods.json"));

                if (resourceName == null)
                {
                    await DisplayAlert("Error", "Resource not found: foods.json", "OK");
                    return;
                }

                using Stream stream = assembly.GetManifestResourceStream(resourceName);
                using StreamReader reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    var foodList = JsonConvert.DeserializeObject<List<NutritionEntry>>(json);
                    FoodList = new ObservableCollection<NutritionEntry>(foodList);

                    // Populate the Picker with FoodNames and ServingSizes
                    foreach (var food in FoodList)
                    {
                        FoodPicker.Items.Add($"{food.FoodName} : {food.ServingSize}");
                    }

                    await DisplayAlert("Success", "Data loaded successfully", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error loading food data: {ex.Message}", "OK");
            }
        }



        // Event handler for selecting a food item from the picker
        private void OnFoodPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (FoodPicker.SelectedItem != null)
            {
                string selectedItem = FoodPicker.SelectedItem.ToString();
                string selectedFoodName = selectedItem.Split(':')[0].Trim(); // Extract the food name
                var selectedFood = FoodList.FirstOrDefault(food => food.FoodName == selectedFoodName);

                if (selectedFood != null)
                {
                    FoodNameEntry.Text = selectedFood.FoodName;
                    CarbsEntry.Text = selectedFood.Carbs.ToString();
                    ProteinEntry.Text = selectedFood.Protein.ToString();
                    FatEntry.Text = selectedFood.Fat.ToString();
                    CaloriesEntry.Text = selectedFood.Calories.ToString();
                    //ServingSizeEntry.Text = selectedFood.ServingSize;
                }
            }
        }


        // Event handler for adding a new nutrition entry
        private async void OnAddEntryClicked(object sender, EventArgs e)
        {
            string foodName = FoodNameEntry.Text;
            string carbs = CarbsEntry.Text;
            string protein = ProteinEntry.Text;
            string fat = FatEntry.Text;
            string calories = CaloriesEntry.Text;
            string mealType = MealTypePicker.SelectedItem?.ToString();

            // Validate input fields
            if (string.IsNullOrWhiteSpace(foodName) ||
                string.IsNullOrWhiteSpace(carbs) ||
                string.IsNullOrWhiteSpace(protein) ||
                string.IsNullOrWhiteSpace(fat) ||
                string.IsNullOrWhiteSpace(mealType))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            // Create a new nutrition entry
            var entry = new NutritionEntry
            {
                FoodName = foodName,
                Carbs = double.Parse(carbs),
                Protein = double.Parse(protein),
                Fat = double.Parse(fat),
                Calories = double.Parse(calories),
                MealType = mealType
            };

            // Save the new entry to the database
            _database.SaveNutritionEntry(entry);
            LoadEntries(); // Refresh the entries list

            await DisplayAlert("Entry Added", "Your entry has been saved.", "OK");

            // Clear fields after saving
            FoodNameEntry.Text = string.Empty;
            CarbsEntry.Text = string.Empty;
            ProteinEntry.Text = string.Empty;
            FatEntry.Text = string.Empty;
            CaloriesEntry.Text = string.Empty;
            MealTypePicker.SelectedIndex = -1;

            // Navigate back to MainPage
            await Navigation.PopAsync();
        }

        // Event to handle property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
