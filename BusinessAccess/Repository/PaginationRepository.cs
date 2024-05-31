using BusinessAccess.Repository.IRepository;

namespace BusinessAccess.Repository
{
    public class PaginationRepository<T> : IPaginationRepository<T> where T : class
    {
        public List<T> GetPagedData(List<T> entity, int currentpage, int pagesize)
        {
            entity = entity.Skip((currentpage - 1) * pagesize).Take(pagesize).ToList();
            return entity;
        }
    }
}
