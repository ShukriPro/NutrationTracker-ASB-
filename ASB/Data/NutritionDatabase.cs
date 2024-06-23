using ASB.Models;
using SQLite;

public class NutritionDatabase
{
    private readonly SQLiteConnection _database;

    public NutritionDatabase()
    {
        var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "nutrition.db3");
        _database = new SQLiteConnection(dbPath);
        _database.CreateTable<NutritionEntry>();
    }

    public int SaveNutritionEntry(NutritionEntry entry)
    {
        if (entry.Id != 0)
        {
            return _database.Update(entry);
        }
        else
        {
            return _database.Insert(entry);
        }
    }

    public List<NutritionEntry> GetNutritionEntries()
    {
        return _database.Table<NutritionEntry>().ToList();
    }

    public int DeleteNutritionEntry(int id)
    {
        return _database.Delete<NutritionEntry>(id);
    }
}
