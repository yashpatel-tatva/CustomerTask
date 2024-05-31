using BusinessAccess.Repository.IRepository;
using DataAccess.DTOs;

namespace BusinessAccess.Repository
{
    public class PaginationRepository<T> : IPaginationRepository<T> where T : class
    {
        public PageFilterResponseDTO<T> GetPagedData(List<T> entity, PageFilterRequestDTO<T> pageFilterDTO)
        {

            PageFilterResponseDTO<T> pageFilterResponseDTO = new PageFilterResponseDTO<T>();
            pageFilterResponseDTO.TotalColumn = typeof(T).GetProperties().Length;
            pageFilterResponseDTO.CurrentPage = pageFilterDTO.currentpage;
            pageFilterResponseDTO.PageSize = pageFilterDTO.pagesize;
            pageFilterResponseDTO.OrderColumnName = pageFilterDTO.OrderByColumnName;
            pageFilterResponseDTO.OrderBy = pageFilterDTO.OrderBy;



            /// Searching
            var searchColumns = pageFilterDTO.SearchByColumns;

            if (searchColumns != null)
            {
                var searchProperties = typeof(T).GetProperties();
                var columnProperties = searchColumns.GetType().GetProperties();

                foreach (var columnProperty in columnProperties)
                {
                    if (columnProperty.Name == "Id")
                        continue;
                    var searchValue = columnProperty.GetValue(searchColumns)?.ToString().ToLower();
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        var entityProperty = searchProperties.FirstOrDefault(p => p.Name == columnProperty.Name);
                        if (entityProperty != null)
                        {
                            entity = entity.Where(e => (bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(searchValue))).ToList();
                        }
                    }
                }
            }


            /// Info Of Pages
            var pages = (decimal)entity.Count / (decimal)pageFilterDTO.pagesize;
            pageFilterResponseDTO.TotalPage = (int)Math.Ceiling(pages);
            pageFilterResponseDTO.TotalRecords = entity.Count();




            /// Sorting
            if (pageFilterDTO.OrderByColumnName != null)
            {
                var propertyInfo = typeof(T).GetProperty(pageFilterDTO.OrderByColumnName);

                if (propertyInfo != null)
                {
                    entity = pageFilterDTO.OrderBy
                        ? entity.OrderBy(e => propertyInfo.GetValue(e, null)).ToList()
                        : entity.OrderByDescending(e => propertyInfo.GetValue(e, null)).ToList();
                }
            }




            /// Paging
            entity = entity.Skip((pageFilterDTO.currentpage - 1) * pageFilterDTO.pagesize).Take(pageFilterDTO.pagesize).ToList();




            ///Response
            pageFilterResponseDTO.Data = entity;
            return pageFilterResponseDTO;
        }
    }
}
