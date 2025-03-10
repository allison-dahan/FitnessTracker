using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public class MockNutritionRepository : INutritionRepository
    {
        private List<Meal> _meals = new List<Meal>
        {
            new Meal
            {
                Id = 1,
                UserId = 1, // Assuming default user ID
                Date = DateTime.Now.Date,
                MealType = "Breakfast",
                TotalCalories = 450,
                Protein = 25,
                Carbs = 60,
                Fat = 12,
                Notes = "Oatmeal with berries and protein powder"
            },
            new Meal
            {
                Id = 2,
                UserId = 1,
                Date = DateTime.Now.Date,
                MealType = "Lunch",
                TotalCalories = 650,
                Protein = 35,
                Carbs = 80,
                Fat = 15,
                Notes = "Chicken salad sandwich with whole grain bread"
            },
            new Meal
            {
                Id = 3,
                UserId = 1,
                Date = DateTime.Now.AddDays(-1),
                MealType = "Dinner",
                TotalCalories = 750,
                Protein = 45,
                Carbs = 65,
                Fat = 25,
                Notes = "Salmon with roasted vegetables and quinoa"
            }
        };

        private List<WaterIntake> _waterIntakes = new List<WaterIntake>
        {
            new WaterIntake
            {
                Id = 1,
                UserId = 1,
                Date = DateTime.Now.Date,
                Amount = 0.5m
            },
            new WaterIntake
            {
                Id = 2,
                UserId = 1,
                Date = DateTime.Now.Date,
                Amount = 0.5m
            },
            new WaterIntake
            {
                Id = 3,
                UserId = 1,
                Date = DateTime.Now.Date,
                Amount = 0.8m
            }
        };

        // nutrition goals
        private readonly NutritionSummaryModel _goals = new NutritionSummaryModel
        {
            CalorieGoal = 2000,
            ProteinGoal = 100,
            CarbsGoal = 300,
            FatGoal = 67,
            WaterGoal = 2.5m
        };

        public IEnumerable<Meal> GetAllMeals()
        {
            return _meals.OrderByDescending(m => m.Date).ThenBy(m => m.MealType);
        }

        public IEnumerable<Meal> GetMealsByDate(DateTime date)
        {
            return _meals.Where(m => m.Date.Date == date.Date)
                         .OrderBy(m => m.MealType);
        }

        public IEnumerable<Meal> GetTodaysMeals()
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
            var summary = new NutritionSummaryModel
            {
                TotalCalories = meals.Sum(m => m.TotalCalories),
                ProteinGrams = meals.Sum(m => m.Protein),
                CarbsGrams = meals.Sum(m => m.Carbs),
                FatGrams = meals.Sum(m => m.Fat),
                CalorieGoal = _goals.CalorieGoal,
                ProteinGoal = _goals.ProteinGoal,
                FatGoal = _goals.FatGoal,
                CarbsGoal = _goals.CarbsGoal,
                WaterIntake = GetWaterIntakeByDate(date),
                WaterGoal = _goals.WaterGoal
            };

            return summary;
        }

        public void AddMeal(Meal meal)
        {
            meal.Id = _meals.Any() ? _meals.Max(m => m.Id) + 1 : 1;
            meal.UserId = 1; // Set default user ID
            _meals.Add(meal);
        }

        public void UpdateMeal(Meal meal)
        {
            var existingMeal = _meals.FirstOrDefault(m => m.Id == meal.Id);
            if (existingMeal != null)
            {
                existingMeal.Date = meal.Date;
                existingMeal.MealType = meal.MealType;
                existingMeal.TotalCalories = meal.TotalCalories;
                existingMeal.Protein = meal.Protein;
                existingMeal.Carbs = meal.Carbs;
                existingMeal.Fat = meal.Fat;
                existingMeal.Notes = meal.Notes;
            }
        }

        public void DeleteMeal(int id)
        {
            var meal = _meals.FirstOrDefault(m => m.Id == id);
            if (meal != null)
            {
                _meals.Remove(meal);
            }
        }

        public void AddWaterIntake(WaterIntake waterIntake)
        {
            waterIntake.Id = _waterIntakes.Any() ? _waterIntakes.Max(w => w.Id) + 1 : 1;
            waterIntake.UserId = 1; // Set default user ID
            _waterIntakes.Add(waterIntake);
        }

        public decimal GetTodaysWaterIntake()
        {
            return GetWaterIntakeByDate(DateTime.Now.Date);
        }

        private decimal GetWaterIntakeByDate(DateTime date)
        {
            return _waterIntakes
                .Where(w => w.Date.Date == date.Date)
                .Sum(w => w.Amount);
        }
    }
}