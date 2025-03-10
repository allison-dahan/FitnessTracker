namespace FitnessTracker.Services;
using FitnessTracker.Models;

public interface INutritionRepository
{
    IEnumerable<Meal> GetAllMeals();
    IEnumerable<Meal> GetMealsByDate(DateTime date);
    IEnumerable<Meal> GetTodaysMeals();
    NutritionSummaryModel GetTodaysSummary();
    NutritionSummaryModel GetSummaryByDate(DateTime date);
    void AddMeal(Meal meal);
    void UpdateMeal(Meal meal);
    void DeleteMeal(int id);
    void AddWaterIntake(WaterIntake waterIntake);
    decimal GetTodaysWaterIntake();


}