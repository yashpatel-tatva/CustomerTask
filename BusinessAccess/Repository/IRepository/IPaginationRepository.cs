using DataAccess.DTOs;

namespace BusinessAccess.Repository.IRepository
{
    public interface IPaginationRepository<T> where T : class
    {
        PageFilterResponseDTO<T> GetPagedData(List<T> entity, PageFilterRequestDTO<T> pageFilterDTO);
    }
}
