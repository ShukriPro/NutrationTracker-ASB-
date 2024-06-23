using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.Maui.Controls;
using ASB.Models;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace ASB
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalCarbs { get; set; }
        public double TotalFat { get; set; }

        public double BreakfastCalories { get; set; }
        public double LunchCalories { get; set; }
        public double DinnerCalories { get; set; }
        public double SnacksCalories { get; set; }

        public string BreakfastFoodNames { get; set; }
        public string LunchFoodNames { get; set; }
        public string DinnerFoodNames { get; set; }
        public string SnacksFoodNames { get; set; }

        private NutritionDatabase _database;

        public ICommand NavigateToFoodEntryCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            _database = new NutritionDatabase();
            NavigateToFoodEntryCommand = new Command<string>(NavigateToFoodEntryPage);
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateTotals();
        }

        private async void NavigateToFoodEntryPage(string mealType)
        {
            await Navigation.PushAsync(new FoodEntryPage(mealType));
        }

        private void UpdateTotals()
        {
            var entries = _database.GetNutritionEntries();
            TotalCalories = entries.Sum(e => e.Calories);
            TotalProtein = entries.Sum(e => e.Protein);
            TotalCarbs = entries.Sum(e => e.Carbs);
            TotalFat = entries.Sum(e => e.Fat);

            BreakfastCalories = entries.Where(e => e.MealType == "Breakfast").Sum(e => e.Calories);
            LunchCalories = entries.Where(e => e.MealType == "Lunch").Sum(e => e.Calories);
            DinnerCalories = entries.Where(e => e.MealType == "Dinner").Sum(e => e.Calories);
            SnacksCalories = entries.Where(e => e.MealType == "Snacks").Sum(e => e.Calories);

            // Capitalize the first letter of each food name for Breakfast, Lunch, Dinner, and Snacks
            BreakfastFoodNames = TruncateString(
                string.Join(", ",
                    entries.Where(e => e.MealType == "Breakfast")
                           .Select(e => char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower())),
                20);

            LunchFoodNames = TruncateString(
                string.Join(", ",
                    entries.Where(e => e.MealType == "Lunch")
                           .Select(e => char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower())),
                20);

            DinnerFoodNames = TruncateString(
                string.Join(", ",
                    entries.Where(e => e.MealType == "Dinner")
                           .Select(e => char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower())),
                20);

            SnacksFoodNames = TruncateString(
                string.Join(", ",
                    entries.Where(e => e.MealType == "Snacks")
                           .Select(e => char.ToUpper(e.FoodName[0]) + e.FoodName.Substring(1).ToLower())),
                20);

            OnPropertyChanged(nameof(TotalCalories));
            OnPropertyChanged(nameof(TotalProtein));
            OnPropertyChanged(nameof(TotalCarbs));
            OnPropertyChanged(nameof(TotalFat));
            OnPropertyChanged(nameof(BreakfastCalories));
            OnPropertyChanged(nameof(LunchCalories));
            OnPropertyChanged(nameof(DinnerCalories));
            OnPropertyChanged(nameof(SnacksCalories));
            OnPropertyChanged(nameof(BreakfastFoodNames));
            OnPropertyChanged(nameof(LunchFoodNames));
            OnPropertyChanged(nameof(DinnerFoodNames));
            OnPropertyChanged(nameof(SnacksFoodNames));
        }

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void OnBreakfastTapped(object sender, EventArgs e)
        {
            // Navigate to Breakfast Page
            await Navigation.PushAsync(new BreakfastPage());
        }

        private async void OnLunchTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LunchPage());
      
        }
        private async void OnAddFoodEntryClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FoodEntryPage());
            
        }
        private async void OnDinnerTapped(object sender, EventArgs e)
        {
            // Navigate to Dinner Page
            await Navigation.PushAsync(new DinnerPage());
        }

        private async void OnSnacksTapped(object sender, EventArgs e)
        {
            // Navigate to Snacks Page
            await Navigation.PushAsync(new SnackPage());
        }
    }
}
