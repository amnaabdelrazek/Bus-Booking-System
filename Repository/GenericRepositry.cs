
namespace Bus_Booking_System.Repository
{
	public class GenericRepositry<T> : IGenericRepository<T> where T : class
	{
		private readonly MyAppContext Context;

		public GenericRepositry(MyAppContext context)
		{
			Context = context;
			
			
		}



		public List<T> GetAll()
		{
			return Context.Set<T>().ToList();
		}

		public IQueryable<T> GetAllQueryable()
		{
			return Context.Set<T>().AsNoTracking();
		}

		public T GetById(int id)
		{
			return Context.Set<T>().Find(id);
		}

		public void Add(T entity)
		{
			Context.Add(entity);
		}

		public void Update(T entity)
		{
			Context.Update(entity);
		}

		public void Delete(int id)
		{
			var entity = GetById(id);
			if (entity != null)
				Context.Remove(entity);
		}

		public void Save()
		{
			Context.SaveChanges();
		}
	}
}
