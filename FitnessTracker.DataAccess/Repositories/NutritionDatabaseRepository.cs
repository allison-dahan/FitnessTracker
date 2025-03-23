using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Repositories
{
    public class NutritionDatabaseRepository : INutritionRepository
    {
        private readonly FitnessTrackerContext _context;

        public NutritionDatabaseRepository(FitnessTrackerContext context)
        {
            _context = context;
        }

        public IEnumerable<MealModel> GetAllMeals()
        {
            return _context.Meals
                .OrderByDescending(m => m.Date)
                .ThenBy(m => m.MealType)
                .ToList();
        }

        public IEnumerable<MealModel> GetMealsByDate(DateTime date)
        {
            return _context.Meals
                .Where(m => m.Date.Date == date.Date)
                .OrderBy(m => m.MealType)
                .ToList();
        }

        public IEnumerable<MealModel> GetTodaysMeals()
        {
            return GetMealsByDate(DateTime.Now.Date);
        }

        public NutritionSummaryModel GetTodaysSummary()
        {
            return GetSummaryByDate(DateTime.Now.Date);
        }

        public NutritionSummaryModel GetSummaryByDate(DateTime date)
        {
            var meals = GetMealsByDate(date);
            return new NutritionSummaryModel
            {
                TotalCalories = meals.Sum(m => m.Calories),
                ProteinGrams = meals.Sum(m => m.Protein),
                CarbsGrams = meals.Sum(m => m.Carbs),
                FatGrams = meals.Sum(m => m.Fat),
                // You might want to fetch goals from a separate table or configuration
                CalorieGoal = 2000,
                ProteinGoal = 100,
                CarbsGoal = 300,
                FatGoal = 67
            };
        }

        public void AddMeal(MealModel meal)
        {
            _context.Meals.Add(meal);
            _context.SaveChanges();
        }

        public void UpdateMeal(MealModel meal)
        {
            var existingMeal = _context.Meals.Find(meal.Id);
            if (existingMeal != null)
            {
                existingMeal.Date = meal.Date;
                existingMeal.MealType = meal.MealType;
                existingMeal.Calories = meal.Calories;
                existingMeal.Protein = meal.Protein;
                existingMeal.Carbs = meal.Carbs;
                existingMeal.Fat = meal.Fat;
                existingMeal.Notes = meal.Notes;

                _context.SaveChanges();
            }
        }

        public void DeleteMeal(int id)
        {
            var meal = _context.Meals.Find(id);
            if (meal != null)
            {
                _context.Meals.Remove(meal);
                _context.SaveChanges();
            }
        }

        public void AddWaterIntake(WaterIntakeModel waterIntake)
        {
            _context.WaterIntakes.Add(waterIntake);
            _context.SaveChanges();
        }

        public decimal GetTodaysWaterIntake()
        {
            return GetWaterIntakeByDate(DateTime.Now.Date);
        }

        private decimal GetWaterIntakeByDate(DateTime date)
        {
            return _context.WaterIntakes
                .Where(w => w.Date.Date == date.Date)
                .Sum(w => w.Amount);
        }
    }
}