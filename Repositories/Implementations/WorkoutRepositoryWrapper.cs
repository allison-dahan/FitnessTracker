using System;
using System.Linq.Expressions;
using FitnessTracker.Models;
using FitnessTracker.Services;
using FitnessTracker.Repositories.Interfaces;

namespace FitnessTracker.Repositories.Implementations
{
    /// <summary>
    /// Wrapper for Workout Repository that implements both specific and generic interfaces
    /// </summary>
    public class WorkoutRepositoryWrapper : IWorkoutRepository, IGenericRepository<Workout>
    {
        private readonly MockWorkoutRepository _mockRepository;
        private List<Workout> _entities;

        public WorkoutRepositoryWrapper(MockWorkoutRepository mockRepository)
        {
            _mockRepository = mockRepository ?? throw new ArgumentNullException(nameof(mockRepository));
            _entities = _mockRepository.GetAllWorkouts().ToList();
        }

        // IGenericRepository<Workout> Implementation
        public IEnumerable<Workout> GetAll() => _mockRepository.GetAllWorkouts();

        public IEnumerable<Workout> Find(Expression<Func<Workout, bool>> predicate)
        {
            return _entities.AsQueryable().Where(predicate).ToList();
        }

        public Workout GetById(int id)
        {
            return _entities.FirstOrDefault(w => w.Id == id);
        }

        public void Add(Workout entity)
        {
            _mockRepository.AddWorkout(entity);
            _entities = _mockRepository.GetAllWorkouts().ToList();
        }

        public void Update(Workout entity)
        {
            _mockRepository.UpdateWorkout(entity);
            _entities = _mockRepository.GetAllWorkouts().ToList();
        }

        public void Delete(int id)
        {
            _mockRepository.DeleteWorkout(id);
            _entities = _mockRepository.GetAllWorkouts().ToList();
        }

        // IWorkoutRepository Implementation
        public Workout GetWorkoutById(int id) => _mockRepository.GetWorkoutById(id);
        public void AddWorkout(Workout workout) => _mockRepository.AddWorkout(workout);
        public void UpdateWorkout(Workout workout) => _mockRepository.UpdateWorkout(workout);
        public void DeleteWorkout(int id) => _mockRepository.DeleteWorkout(id);
        public IEnumerable<Workout> GetAllWorkouts() => _mockRepository.GetAllWorkouts();
        public IEnumerable<Exercise> GetAllExercises() => _mockRepository.GetAllExercises();
        public IEnumerable<Workout> GetRecentWorkouts(int count) => _mockRepository.GetRecentWorkouts(count);
    }
}