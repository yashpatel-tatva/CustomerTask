using BusinessAccess.Repository.IRepository;
using DataAccess.DTOs;

namespace BusinessAccess.Repository
{
    public class PaginationRepository<T, U> : IPaginationRepository<T, U> where T : class where U : class
    {
        public PageFilterResponseDTO<T> GetPagedData(List<T> entity, PageFilterRequestDTO<U> pageFilterDTO)
        {

            PageFilterResponseDTO<T> pageFilterResponseDTO = new PageFilterResponseDTO<T>();
            pageFilterResponseDTO.TotalColumn = typeof(T).GetProperties().Length;
            pageFilterResponseDTO.CurrentPage = pageFilterDTO.currentpage;
            pageFilterResponseDTO.PageSize = pageFilterDTO.pagesize;
            pageFilterResponseDTO.OrderColumnName = pageFilterDTO.OrderByColumnName;
            pageFilterResponseDTO.OrderBy = pageFilterDTO.OrderBy;

            var searchProperties = typeof(T).GetProperties();

            /// searching from key
            

            //string searchValue  = pageFilterDTO.search?.ToLower();

            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    entity = entity.Where(e =>
            //    {
            //        foreach (var entityProperty in searchProperties)
            //        {
            //            if ((bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(searchValue)))
            //            {
            //                return true;
            //            }
            //        }
            //        return false;
            //    }).ToList();

            //    foreach (var entityProperty in searchProperties)
            //    {
            //        entity = entity.OrderBy(e => entityProperty.GetValue(e)?.ToString()).ToList();
            //    }
            //}


            /// Searching from filter
            var searchColumns = pageFilterDTO.SearchByColumns;

            if (searchColumns != null)
            {
                var columnProperties = searchColumns.GetType().GetProperties();

                foreach (var columnProperty in columnProperties)
                {
                    var thissearchValue = columnProperty.GetValue(searchColumns)?.ToString().ToLower();
                    if (!string.IsNullOrEmpty(thissearchValue))
                    {
                        var entityProperty = searchProperties.FirstOrDefault(p => p.Name == columnProperty.Name);
                        if (entityProperty != null)
                        {
                            entity = entity
                                .Where(e => (bool)(entityProperty.GetValue(e)?.ToString().ToLower().Contains(thissearchValue)))
                                .OrderBy(e => entityProperty.GetValue(e)?.ToString().IndexOf(thissearchValue) ?? int.MaxValue)
                                .ThenBy(e => entityProperty.GetValue(e)?.ToString())
                                .ToList();
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
