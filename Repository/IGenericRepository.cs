namespace Bus_Booking_System.Repository
{
    
        public interface IGenericRepository<T> where T : class
        {
            List<T> GetAll();
            IQueryable<T> GetAllQueryable();


			//T GetById(int id);
            T? GetById(int id);

            void Add(T entity);

            void Update(T entity);

            void Delete(int id);

            void Save();
        }
    
}
