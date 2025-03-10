namespace FitnessTracker.Services;
using FitnessTracker.Models;

public interface INutritionRepository
{
    IEnumerable<MealModel> GetAllMeals();
    IEnumerable<MealModel> GetMealsByDate(DateTime date);
    IEnumerable<MealModel> GetTodaysMeals();
    NutritionSummaryModel GetTodaysSummary();
    NutritionSummaryModel GetSummaryByDate(DateTime date);
    void AddMeal(MealModel meal);
    void UpdateMeal(MealModel meal);
    void DeleteMeal(int id);
    void AddWaterIntake(WaterIntakeModel waterIntake);
    decimal GetTodaysWaterIntake();


}