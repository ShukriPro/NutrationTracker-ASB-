using SQLite;

namespace ASB.Models
{
    public class NutritionEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FoodName { get; set; }
        public double Carbs { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double Calories { get; set; }
        public string MealType { get; set; }
        public string ServingSize { get; set; }

        [Ignore]
        public string DetailInfo => $"Carbs: {Carbs}g, Protein: {Protein}g, Fat: {Fat}g, Calories: {Calories}";
    }
}
