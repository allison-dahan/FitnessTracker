using FitnessTracker.DataAccess.Interfaces;

namespace FitnessTracker.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private List<T> _items = new List<T>();

        public IEnumerable<T> GetAll() => _items;
        
        public T GetById(int id) 
        {
            var property = typeof(T).GetProperty("Id");
            return _items.FirstOrDefault(
                item => (int)property.GetValue(item) == id
            );
        }
        
        public void Add(T entity) => _items.Add(entity);
        public void Update(T entity) { /* Update logic */ }
        public void Delete(int id) 
        {
            var item = GetById(id);
            if (item != null) _items.Remove(item);
        }
    }
}