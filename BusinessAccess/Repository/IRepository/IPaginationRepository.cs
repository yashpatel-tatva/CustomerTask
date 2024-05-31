namespace BusinessAccess.Repository.IRepository
{
    public interface IPaginationRepository<T> where T : class
    {
        List<T> GetPagedData(List<T> entity, int currentpage, int pagesize);
    }
}
