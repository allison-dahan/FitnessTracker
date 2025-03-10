using System;
using System.Linq.Expressions;
using FitnessTracker.Models;
using FitnessTracker.Services;
using FitnessTracker.Repositories.Interfaces;

namespace FitnessTracker.Repositories.Implementations
{
    /// <summary>
    /// Wrapper for Nutrition Repository that implements both specific and generic interfaces
    /// </summary>
    public class NutritionRepositoryWrapper : INutritionRepository, IGenericRepository<Meal>
    {
        private readonly MockNutritionRepository _mockRepository;
        private List<Meal> _entities;

        public NutritionRepositoryWrapper(MockNutritionRepository mockRepository)
        {
            _mockRepository = mockRepository ?? throw new ArgumentNullException(nameof(mockRepository));
            _entities = _mockRepository.GetAllMeals().ToList();
        }

        // IGenericRepository<Meal> Implementation
        public IEnumerable<Meal> GetAll() => _mockRepository.GetAllMeals();

        public IEnumerable<Meal> Find(Expression<Func<Meal, bool>> predicate)
        {
            return _entities.AsQueryable().Where(predicate).ToList();
        }

        public Meal GetById(int id)
        {
            return _entities.FirstOrDefault(m => m.Id == id);
        }

        public void Add(Meal entity)
        {
            _mockRepository.AddMeal(entity);
            _entities = _mockRepository.GetAllMeals().ToList();
        }

        public void Update(Meal entity)
        {
            _mockRepository.UpdateMeal(entity);
            _entities = _mockRepository.GetAllMeals().ToList();
        }

        public void Delete(int id)
        {
            _mockRepository.DeleteMeal(id);
            _entities = _mockRepository.GetAllMeals().ToList();
        }

        // INutritionRepository Implementation
        public IEnumerable<Meal> GetAllMeals() => _mockRepository.GetAllMeals();
        public IEnumerable<Meal> GetMealsByDate(DateTime date) => _mockRepository.GetMealsByDate(date);
        public IEnumerable<Meal> GetTodaysMeals() => _mockRepository.GetTodaysMeals();
        public NutritionSummaryModel GetTodaysSummary() => _mockRepository.GetTodaysSummary();
        public NutritionSummaryModel GetSummaryByDate(DateTime date) => _mockRepository.GetSummaryByDate(date);
        public void AddMeal(Meal meal) => _mockRepository.AddMeal(meal);
        public void UpdateMeal(Meal meal) => _mockRepository.UpdateMeal(meal);
        public void DeleteMeal(int id) => _mockRepository.DeleteMeal(id);
        public void AddWaterIntake(WaterIntake waterIntake) => _mockRepository.AddWaterIntake(waterIntake);
        public decimal GetTodaysWaterIntake() => _mockRepository.GetTodaysWaterIntake();
    }
}