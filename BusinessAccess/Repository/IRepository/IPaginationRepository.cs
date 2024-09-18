using DataAccess.DTOs;

namespace BusinessAccess.Repository.IRepository
{
    public interface IPaginationRepository<T, U> where T : class where U : class
    {
        //PageFilterResponseDTO<T> GetPagedData(List<T> entity, PageFilterRequestDTO<U> pageFilterDTO);
        PageFilterResponseDTO<T> GetPagedDataTable(PageFilterRequestDTO<U> pageFilterDTO);
    }
}
